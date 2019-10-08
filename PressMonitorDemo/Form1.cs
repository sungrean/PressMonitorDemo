using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PressMonitorDemo
{
    public partial class demo : Form
    {
        public static  CProtocol cProtocol;     //通讯协议工具
        public tagCFG cFG;              //设备配置参数
        public static byte[] data;
        object frame = new object();  //保存串口收到的数据
        long openDevCfgDlgTimeOut;
        #region 命令字 
        public const UInt16 FRAME_TYPE_LI = 0x494C;	//"LI" Login
        public const UInt16 FRAME_TYPE_LO = 0x4F4C;	//"LO" Logout
        public const UInt16 FRAME_TYPE_SM = 0x4D53;	//"SM" SetMode
        public const UInt16 FRAME_TYPE_GC = 0x4347;	//"GC" GetConfig
        public const UInt16 FRAME_TYPE_SC = 0x4353;	//"SC" SetConfig 
        public const UInt16 FRAME_TYPE_GR = 0x5247;	//"GR" Get record
        public const UInt16 FRAME_TYPE_CR = 0x5243;	//"CR" clr record
        public const UInt16 FRAME_TYPE_GS = 0x5347;	//"GS" Get state
        public const UInt16 FRAME_TYPE_RA = 0x4152;	//"RA" Reset Alarm
        public const UInt16 FRAME_TYPE_RP = 0x5052;	//"RP" Report
        public const UInt16 FRAME_TYPE_AL = 0x4C41;	//"AL" Alarm
        public const UInt16 FRAME_TYPE_NK = 0x4E4B;	//"NK" Alarm        

        public const UInt16 FRAME_TYPE_RC = 0x4352;	//"RC" Reset counter
        public const UInt16 FRAME_TYPE_ST = 0x5453;	//"ST" Set RTC Time
        public const UInt16 FRAME_TYPE_MS = 0x534D;	//"MS" Mode select
        public const UInt16 FRAME_TYPE_FS = 0x5346; //"FS" Factory Setting 
        #endregion

        #region modbus服务器相关
        private static Socket severSocket = null;

        #endregion


        public demo()
        {
            InitializeComponent();
            cFG = new tagCFG();
            cFG.errAdj = 11212f;
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int cnt = serialPort1.BytesToRead;
            byte[] rcvBuf = new byte[cnt];
            serialPort1.Read(rcvBuf, 0, cnt);
            cProtocol.RxBuffAdd(rcvBuf, cnt);
            txtComRecv.Text = tools.ConvertString(rcvBuf);
        }

        private void demo_Load(object sender, EventArgs e)
        {
            //MessageBox.Show((259/256) + ":"+ (259 % 256));
            Control.CheckForIllegalCrossThreadCalls = false;            //允许线程间控件调用
            #region
            string[] ports = SerialPort.GetPortNames();
            cBoxCOMPORT.Items.AddRange(ports);
            cBoxCOMPORT.Text = ports[ports.Length - 1];
            #endregion
            timer1.Interval = 500;
            timer1.Enabled = true;
        }

        private void btnOpenPort_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                serialPort1.PortName = cBoxCOMPORT.Text;
                serialPort1.BaudRate = 115200;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Parity = Parity.None;
                serialPort1.ReadTimeout = 50;
                serialPort1.WriteTimeout = 50;
                try
                { 
                    serialPort1.Open();
                    if (serialPort1.IsOpen) 
                    {
                        cProtocol = new CProtocol(this);
                        cProtocol.m_comm = serialPort1;
                        cProtocol.LogIn();
                    } 
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                } 
            }
            else
            {
                if (!(cProtocol == null))
                    cProtocol.SwitchMode(tagMode.LOG_OFF);
                serialPort1.Close();
                cProtocol = null;
            }
        } 
        private void timer1_Tick(object sender, EventArgs e)
        {
            #region  状态显示
            if(serialPort1.IsOpen)
            {
                btnOpenPort.Text = "脱机";
            }
            else
            {
                btnOpenPort.Text = "登陆";
            }

            if(!(cProtocol==null))
            {
                txtStatu.Text = cProtocol.currMode.ToString();
            }
            else
            {
                txtStatu.Text = "未启动";
                return ;
            }
            #endregion
            #region 读取设备配置超时
            long ms = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;     //时间戳;
            if (openDevCfgDlgTimeOut > 0)       // is waiting device response
            {
                if (ms > openDevCfgDlgTimeOut)  //timeout
                {
                    openDevCfgDlgTimeOut = 0;
                    MessageBox.Show("等待设备响应超时"); 
                }
            }
            #endregion
            while (cProtocol.GetRxFrame(ref frame))                 //不断从帧队列中读取数据帧
            {
                ushort type = cProtocol.GetRxType();
                type -= 0x2020; //两个字符转为大写
                switch (type)
                {
                    case FRAME_TYPE_RP:
                        COMM_FRAME_RPT frameRX = (COMM_FRAME_RPT)frame;     //强转为检测记录数据帧
                        tagRec rec=(tagRec)cProtocol.BytesToStruct(frameRX.data, frameRX.data.Length,typeof(tagRec)) ;
                        data = frameRX.data;
                        for(int i=0;i<0x600;i++)
                        {
                            dataGridView1.Rows.Add(i, String.Format("{0:X2}", data[i]));
                        }
                        break;
                    case FRAME_TYPE_NK: MessageBox.Show("设备忙");
                        break;
                    case FRAME_TYPE_GC:
                        MessageBox.Show("获取到配置信息");
                        openDevCfgDlgTimeOut = 0;
                        break;
                    case FRAME_TYPE_SC: MessageBox.Show("更新设备信息成功！"); break; 

                    default: break;
                } 
            }
        }

        /*********************************************************************************************************
            Function: Deal frame from Device
        */
        //private void frameDeal(byte[] buf)
        //{
        //    int i;
        //    char[][] cmdStr =
        //    {
        //    "li".ToCharArray(), "lo".ToCharArray(), "sm".ToCharArray(),
        //    "gc".ToCharArray(), "sc".ToCharArray(), "gr".ToCharArray(),
        //    "cr".ToCharArray(), "gs".ToCharArray(), "ra".ToCharArray(),
        //    "rp".ToCharArray(), "al".ToCharArray(), "nk".ToCharArray(),
        //    "rc".ToCharArray(), "st".ToCharArray(), "ms".ToCharArray()
        //};
        //    for (i = 0; i < DefineConstants.PRO_CMD_NUM; i++)
        //    {
        //        if ((buf[3] == cmdStr[i][0]) && (buf[4] == cmdStr[i][1]))
        //        {
        //            break;
        //        }
        //    }
        //    tagProCmd cmd = (tagProCmd)i; //获取命令字

        //    if ((int)cmd < DefineConstants.PRO_CMD_NUM)
        //    {
        //        switch (cmd)
        //        {
        //            case tagProCmd.PRO_CMD_LOG_IN:  
        //                break;
        //            case tagProCmd.PRO_CMD_LOG_OFF:  
        //                //ExitRxThread();	//接收进程终止
        //                break;
        //            case tagProCmd.PRO_CMD_SWITtagMode:
        //                break;
        //            case tagProCmd.PRO_CMD_GET_CFG:
        //                break;
        //            case tagProCmd.PRO_CMD_SET_CFG:
        //                //global::PostMessageA(m_hParent, WM_USER + 505, 0, 0);
        //                break;
        //            case tagProCmd.PRO_CMD_GET_REC:
        //                break;
        //            case tagProCmd.PRO_CMD_CLR_REC:             //清除记录 
        //                break;
        //            case tagProCmd.PRO_CMD_GET_STATE:
        //                break;
        //            case tagProCmd.PRO_CMD_RST_ALM: 
        //                break;
        //            case tagProCmd.PRO_CMD_REPORT:
        //                break;
        //            case tagProCmd.PRO_CMD_ALARM:
        //                break;
        //            case tagProCmd.PRO_CMD_BUSY: 
        //                break;
        //            case tagProCmd.PRO_CMD_RESET_CNT: 
        //                break;
        //            case tagProCmd.PRO_CMD_SYNC_TIME: 
        //                break;
        //            case tagProCmd.PRO_CMD_MODEL_SEL:
        //                break; //20190618 
        //            default:
        //                break;
        //        }
        //    }
        //}
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(!(cProtocol==null))
            {
                byte[] cmd = cProtocol.SwitchMode(tagMode.WORK);
                serialPort1.Write(cmd, 0, cmd.Length);
            }
            cProtocol.LogIn();
        }

        private void btnWork_Click(object sender, EventArgs e)
        {
            if (!(cProtocol == null))
            {
                byte[] cmd = cProtocol.SwitchMode(tagMode.WORK);
                serialPort1.Write(cmd, 0, cmd.Length);
            } 
        }

        private void btnLogoff_Click(object sender, EventArgs e)
        {
        }

        private void btnLearn_Click(object sender, EventArgs e)
        {
            if (!(cProtocol == null))
            {
                byte[] cmd = cProtocol.SwitchMode(tagMode.LEARN);
                serialPort1.Write(cmd,0,cmd.Length);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!(cProtocol == null))
            {
                byte[] cmd = cProtocol.SwitchMode(tagMode.STOP);
                serialPort1.Write(cmd, 0, cmd.Length); 
            }
        }

        private void btnParamSet_Click(object sender, EventArgs e)
        {
            if(!serialPort1.IsOpen)
            {
                MessageBox.Show("请先打开串口");//
                return;
            } 
            byte[] frm = cProtocol.GetCmdFrm(CProtocol.FRAME_TYPE_GC);
            serialPort1.Write(frm, 0, frm.Length);
            openDevCfgDlgTimeOut = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;     //时间戳
            openDevCfgDlgTimeOut += 1000;   //timeout is 1000 ms later 

            ParamsSetings form = new ParamsSetings(this); 
            form.ShowDialog(); 
        } 

        private void btnChoseModel_Click(object sender, EventArgs e)
        {

        }

        private void btnClearnCount_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("请先打开串口");//
                return;
            }
            byte[] frm = cProtocol.GetCmdFrm(CProtocol.FRAME_TYPE_CR);
            serialPort1.Write(frm, 0, frm.Length);
        }

        private void btnAlmReset_Click(object sender, EventArgs e)
        {

            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("请先打开串口");//
                return;
            }
            byte[] frm = cProtocol.GetCmdFrm(CProtocol.FRAME_TYPE_RA);
            serialPort1.Write(frm, 0, frm.Length);
        }

        private void btnModbusServer_Click(object sender, EventArgs e)
        {
                         severSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                         IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, int.Parse(txtPort.Text));      //Parse(txtIp.Text)
            severSocket.Bind(endPoint);                 // 绑定
                         severSocket.Listen(10);                     // 设置最大连接数
                         Console.WriteLine("开始监听");
                         Thread thread = new Thread(ListenClientConnect);        // 开启线程监听客户端连接
                         thread.Start();
        }
         /// <summary>
         /// 监听客户端连接
         /// </summary>
         private static void ListenClientConnect()      
         {
             Socket clientSocket = severSocket.Accept();         // 接收客户端连接
             Console.WriteLine("客户端连接成功 信息： " + clientSocket.AddressFamily.ToString());
             //clientSocket.Send(Encoding.Default.GetBytes("你连接成功了"));
             Thread revThread = new Thread(ReceiveClientManage);
             revThread.Start(clientSocket);
         }
        
        /// <summary>
        /// 响应客户端消息
        /// </summary>
        /// <param name="clientSocket"></param>
         private static void ReceiveClientManage(object clientSocket)
         {
             Socket socket = clientSocket as Socket;
             byte[] buffer = new byte[1024];             
            while(true)
            {
                 int length = socket.Receive(buffer);        // 从客户端接收消息

                byte[] cmd = new byte[12];                      //请求命令
                for(int i=0;i<12;i++)
                {
                    cmd[i] = buffer[i];
                }        
                byte[] result = new byte[9];                    //异常响应基本结构
                for (int i = 0; i < 7; i++)
                {
                    result[i] = cmd[i];
                }
                result[5] = 0x09;
                result[7] = 0x83;
                if (length != 12)    //如果接受的 数据长度 不等于12 则不处理，返回0x03异常
                {
                    result[8] = (byte)0x3;
                    socket.Send(result);
                    continue;
                }
                if(cProtocol==null)     //设备异常
                {
                    result[8] = (byte)0x2;
                    socket.Send(result);
                    continue;
                }
                if(data==null)        //如果数据为空，则返回      返回0x02异常
                {
                    result[8] = (byte)0x1;
                    socket.Send(result);
                    continue;
                }
                 socket.Send(readDataByCmd(cmd));
            } 
         } 
        private static byte[] readDataByCmd(byte[] cmd)
        {
            byte[] head = new byte[] {0x00,0x00,0x00,0x00,0x00,0x00,0xff,0x03 }; //1、2两个字节是消息码，3、4是协议标识，5、6是消息长度（后面部分的长度【大段的】），7站好（ff），8功能码（03）
            for(int i=0;i< head.Length;i++)     //前面8个字节与请求的一致（长度除外）
            {
                head[i] = cmd[i];
            }
            int address = cmd[8] * 256 + cmd[9];            //读取寄存器起始地址
            int dataNum = cmd[10] * 256 + cmd[11];          //读取寄存器个数

            byte[] targetData = new byte[2*dataNum+1];        //数据部分（第一个字符为数据长度【不包含长度】)
            for(int i=0;i<dataNum; i++)
            {
                Buffer.BlockCopy(wordAt(address+i), 0, targetData,1+i*2, 2);            //获取到指定位置的数据并填充到targetData里（从targetData[1] 开始）
            }
            targetData[0] = (byte)(targetData.Length-1);        //设置数据部分长度为实际数据长度 - 1 
            byte[] result = new byte[head.Length + targetData.Length];
            Buffer.BlockCopy(head, 0, result, 0, head.Length);
            Buffer.BlockCopy(targetData, 0, result, head.Length, targetData.Length);
            result[4] = (byte)((result.Length - 6)/ 256);
            result[5] = (byte)((result.Length - 6) % 256);
            return result;
        }
        /// <summary>
        /// 获取到指定位置的寄存器
        /// </summary>
        /// <param name="address">寄存器地址</param>
        /// <returns></returns>
        private static byte[] wordAt(int address)           //本机数据是小段的，modbus中数据是大段的，
        {
            byte[] word=new byte[2];
            word[0] = data[address*2+1];
            word[1] = data[address * 2];
            return word;
        }
    }
}
