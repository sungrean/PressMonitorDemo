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
        CProtocol cProtocol;
        public demo()
        {
            InitializeComponent();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

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
            if (!(cProtocol == null))
                cProtocol.SwitchMode(tagMode.LOG_OFF);
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

        }
    }
}
