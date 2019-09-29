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
            if(!(cProtocol==null))
            {
                textBox1.Text = cProtocol.currMode.ToString();
            }
            if(serialPort1.IsOpen)
            {
                btnOpenPort.Text = "脱机";
            }
            else
            {
                btnOpenPort.Text = "登陆";
            }



        }

        /*********************************************************************************************************
            Function: Deal frame from Device
        */
        private void frameDeal(byte[] buf)
        {
            int i;
            char[][] cmdStr =
            {
            "li".ToCharArray(), "lo".ToCharArray(), "sm".ToCharArray(),
            "gc".ToCharArray(), "sc".ToCharArray(), "gr".ToCharArray(),
            "cr".ToCharArray(), "gs".ToCharArray(), "ra".ToCharArray(),
            "rp".ToCharArray(), "al".ToCharArray(), "nk".ToCharArray(),
            "rc".ToCharArray(), "st".ToCharArray(), "ms".ToCharArray()
        };
            for (i = 0; i < DefineConstants.PRO_CMD_NUM; i++)
            {
                if ((buf[3] == cmdStr[i][0]) && (buf[4] == cmdStr[i][1]))
                {
                    break;
                }
            }
            tagProCmd cmd = (tagProCmd)i; //获取命令字

            if ((int)cmd < DefineConstants.PRO_CMD_NUM)
            {
                switch (cmd)
                {
                    case tagProCmd.PRO_CMD_LOG_IN:  
                        break;
                    case tagProCmd.PRO_CMD_LOG_OFF:  
                        //ExitRxThread();	//接收进程终止
                        break;
                    case tagProCmd.PRO_CMD_SWITtagMode:
                        break;
                    case tagProCmd.PRO_CMD_GET_CFG:
                        break;
                    case tagProCmd.PRO_CMD_SET_CFG:
                        //global::PostMessageA(m_hParent, WM_USER + 505, 0, 0);
                        break;
                    case tagProCmd.PRO_CMD_GET_REC:
                        break;
                    case tagProCmd.PRO_CMD_CLR_REC:             //清除记录 
                        break;
                    case tagProCmd.PRO_CMD_GET_STATE:
                        break;
                    case tagProCmd.PRO_CMD_RST_ALM: 
                        break;
                    case tagProCmd.PRO_CMD_REPORT:
                        break;
                    case tagProCmd.PRO_CMD_ALARM:
                        break;
                    case tagProCmd.PRO_CMD_BUSY: 
                        break;
                    case tagProCmd.PRO_CMD_RESET_CNT: 
                        break;
                    case tagProCmd.PRO_CMD_SYNC_TIME: 
                        break;
                    case tagProCmd.PRO_CMD_MODEL_SEL:
                        break; //20190618 
                    default:
                        break;
                }
            }
        }
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
             
            byte[] frm = cProtocol.GetCmdFrm(DefineConstants.FRAME_TYPE_GC);
            serialPort1.Write(frm, 0, frm.Length);
            openDevCfgDlgTimeOut = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;     //时间戳
            openDevCfgDlgTimeOut += 1000;   //timeout is 1000 ms later 
        } 

        private void btnChoseModel_Click(object sender, EventArgs e)
        {

        }
    }
}
