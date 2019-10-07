using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PressMonitorDemo
{
    public partial class demo : Form
    {
        public  CProtocol cProtocol;     //通讯协议工具
        public tagCFG cFG;              //设备配置参数

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
        public const UInt16 FRAME_TYPE_FS = 0x5346;	//"FS" Factory Setting 
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
        }

        private void demo_Load(object sender, EventArgs e)
        {
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
                        cProtocol.SwitchMode(tagMode.LOG_IN);
                    }
                    else
                    { 
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
                textBox1.Text = cProtocol.currMode.ToString();
            }
            else
            {
                textBox1.Text = "未启动";
                return ;
            }

            while (cProtocol.GetRxFrame(ref frame))                 //不断从帧队列中读取数据帧
            {
                ushort type = cProtocol.GetRxType();
                type -= 0x2020; //两个字符转为大写
                switch (type)
                {
                    case FRAME_TYPE_RP:
                        COMM_FRAME_RPT frameRX = (COMM_FRAME_RPT)frame;     //强转为检测记录数据帧
                        tagRec rec=(tagRec)cProtocol.BytesToStruct(frameRX.data, frameRX.data.Length,typeof(tagRec)) ;
                        break;
                    case FRAME_TYPE_NK: MessageBox.Show("设备忙");
                        break;
                    case FRAME_TYPE_GC: 
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
            cProtocol.LogIn();
        }

        private void btnWork_Click(object sender, EventArgs e)
        {
            if (!(cProtocol == null))
                cProtocol.SwitchMode(tagMode.WORK);
        }

        private void btnLogoff_Click(object sender, EventArgs e)
        {
        }

        private void btnLearn_Click(object sender, EventArgs e)
        {
            if (!(cProtocol == null))
                cProtocol.SwitchMode(tagMode.LEARN);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (!(cProtocol == null))
                cProtocol.SwitchMode(tagMode.STOP);
        }

        private void btnParamSet_Click(object sender, EventArgs e)
        {
            if(!serialPort1.IsOpen)
            {
                MessageBox.Show("请先打开串口");//
                return;
            }

            ParamsSetings form = new ParamsSetings(this); 
            form.ShowDialog();
            MessageBox.Show("debuging！");
             
            byte[] frm = cProtocol.GetCmdFrm(CProtocol.FRAME_TYPE_GC);
            serialPort1.Write(frm, 0, frm.Length);
            openDevCfgDlgTimeOut = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;     //时间戳
            openDevCfgDlgTimeOut += 1000;   //timeout is 1000 ms later 
        } 

        private void btnChoseModel_Click(object sender, EventArgs e)
        {

        }
    }
}
