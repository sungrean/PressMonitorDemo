using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PressMonitorDemo
{
    public partial class MachineDetail : Form
    {

        private bool flagMove = false;
        private Point mPoint;
        public MachineDetail()
        {
            InitializeComponent();
        }


        //下面的mouseDown配合MouseMove实现窗口拖动
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new Point(e.X, e.Y);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }
            //左键按下时，设置可移动
            private void splitterRight_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
            {
                this.flagMove = true;

            }
            //右边移动
            private void splitterRight_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
            {
                if (this.flagMove)
                {
                    this.Width = this.Width + e.X;
                }
            }
            //左键松开时，设置不可移动
            private void splitterRight_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
            {
                this.flagMove = false;
            }
        private void MachineDetail_Load(object sender, EventArgs e)
        {

        }

        private void splitter2_MouseDown(object sender, MouseEventArgs e)
        {

            this.flagMove = true;
        }

        private void splitter2_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void splitter2_MouseUp(object sender, MouseEventArgs e)
        {

            this.flagMove = false;
        }
    }
}
