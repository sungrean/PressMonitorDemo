using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PressMonitorDemo
{
    public partial class demo : Form
    {
        public  CProtocol cProtocol;     //通讯协议工具
        public tagCFG cFG;              //设备配置参数

        COMM_FRAME_T frameRX = new COMM_FRAME_T();  //保存串口收到的数据
        long openDevCfgDlgTimeOut;

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
            if(!(cProtocol==null))
            {
                textBox1.Text = cProtocol.currMode.ToString();
            }
            if(serialPort1.IsOpen)
            {
                btnOpenPort.Text = "脱机";
            } else
            {
                btnOpenPort.Text = "登陆";
            }
            long ms = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;     //时间戳;
            if (openDevCfgDlgTimeOut > 0)       // is waiting device response
            {
                if (ms > openDevCfgDlgTimeOut)  //timeout
                {
                    openDevCfgDlgTimeOut = 0;
                    MessageBox.Show("等待设备响应超时");
                   // OpenDevCfgDlg();
                }
            } 
            while (cProtocol!=null&&cProtocol.GetRxFrame(ref frameRX))
            {
                ushort type = frameRX.type;
                type -= 0x2020; //两个字符转为大写
                switch (type)
                {
                    case CProtocol.FRAME_TYPE_RP:
                        MessageBox.Show(frameRX.len + "：" + frameRX.data.Length + "：" + Marshal.SizeOf(typeof(tagRec)));       //sizeof（tagRecItem）=770-->768，tagRec1540，|| 1543 frameRX.len + "："+frameRX.data.Length  1535  + "：" + Marshal.SizeOf(typeof(tagRec)) 1540 1536
                        tagRec rec = (tagRec)cProtocol.BytesToStruct(frameRX.data, frameRX.data.Length,typeof(tagRec));
                        break;
                    case CProtocol.FRAME_TYPE_NK: MessageBox.Show("设备忙"); break;
                    case CProtocol.FRAME_TYPE_GC:                         
                        break;
                    case CProtocol.FRAME_TYPE_SC: MessageBox.Show("更新设备信息成功！"); break;                    
                    case CProtocol.FRAME_TYPE_GS:   ;
                        break; 
                    default: break;
                } 
            }

        }

        /*********************************************************************************************************
            Function: Deal frame from Device
        */
        private void COMM_frameDeal(COMM_FRAME_T frame)
        {
            ushort type = frame.type;
            type -= 0x2020; //两个字符转为大写
            switch (type)
            {
                case CProtocol.FRAME_TYPE_RP: cProtocol.queueRX.Enqueue(frame); break;
                //case CProtocol.FRAME_TYPE_LI: COMM_cmdLI(port); break;
                //case CProtocol.FRAME_TYPE_LO: COMM_cmdLO(port); break;
                //case CProtocol.FRAME_TYPE_SM: COMM_cmdSM(port, p); break;
                //case CProtocol.FRAME_TYPE_GC: COMM_cmdGC(port); break;
                //case CProtocol.FRAME_TYPE_SC: COMM_cmdSC(port, p); break;
                //case CProtocol.FRAME_TYPE_GR: COMM_cmdGR(port); break;
                //case CProtocol.FRAME_TYPE_CR: COMM_cmdCR(port); break;
                //case CProtocol.FRAME_TYPE_GS: COMM_cmdGS(); break;
                //case CProtocol.FRAME_TYPE_RA: COMM_cmdRA(port, p); break;
                //case CProtocol.FRAME_TYPE_RC: COMM_cmdRC(port); break;		//20180902 清除计数器
                //case CProtocol.FRAME_TYPE_ST: COMM_cmdST(port, p); break;	//20180902 同步系统时间
                //case CProtocol.FRAME_TYPE_MS: COMM_cmdMS(port, p); break;	//20190618 选择基准波形
                //case CProtocol.FRAME_TYPE_FS: COMM_cmdFS(port, p); break;	//20190722 厂家设置
                //case CProtocol.FRAME_TYPE_DT: COMM_cmdDT(); break;	//20190722 厂家设置

                default: break;
            }
        } 

        private void btnWork_Click(object sender, EventArgs e)
        {
            if (!(cProtocol == null))
            {
                byte[] cmd = cProtocol.SwitchMode(tagMode.WORK);
                serialPort1.Write(cmd,0,cmd.Length);
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
                serialPort1.Write(cmd, 0, cmd.Length); 
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
            ParamsSetings form = new ParamsSetings(this); 
            form.ShowDialog();
            MessageBox.Show("debuging！");

            byte[] frm = cProtocol.GetCfg(); 
            serialPort1.Write(frm, 0, frm.Length);
            openDevCfgDlgTimeOut = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;     //时间戳
            openDevCfgDlgTimeOut += 1000;  //timeout is 1000 ms later 
        } 

        private void btnChoseModel_Click(object sender, EventArgs e)
        {

        }

        private void btnClearnCount_Click(object sender, EventArgs e)
        {

        }
    }
}
