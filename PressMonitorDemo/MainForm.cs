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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string str = "设备详情（压接设备：1520）    ";
            MachineDetail myForm = new MachineDetail();
            if (tabControlCheckHave(this.tabControl1, str))
            {
                return;
            }
            else
            {
                tabControl1.TabPages.Add(str);
                tabControl1.SelectTab(tabControl1.TabPages.Count - 1);

                myForm.FormBorderStyle = FormBorderStyle.None;
                myForm.Dock = DockStyle.Fill;
                myForm.TopLevel = false;
                myForm.Show();
                myForm.Parent = tabControl1.SelectedTab;
            } 
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            dgv_machine_crimping.Rows.Add("1520", "在线", "192.168.0.12", "设置" , "调试");
        }
        /// <summary>
        /// 判断窗体是否打开
        /// </summary>
        /// <param name="tab">tabControl1控件</param>
        /// <param name="tabName">窗体标题</param>
        /// <returns>false:没有打开 ；true:打开状态</returns>
        public bool tabControlCheckHave(TabControl tab, string tabName)
        {
            for (int i = 0; i < tab.TabCount; i++)
            {
                if (tab.TabPages[i].Text == tabName)
                {
                    tab.SelectedIndex = i;
                    return true;
                }
            }
            return false;
        } 
        //重新绘制page页
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            /*如果将 DrawMode 属性设置为 OwnerDrawFixed， 
           则每当 TabControl 需要绘制它的一个选项卡时，它就会引发 DrawItem 事件*/
            try
            {
                this.tabControl1.TabPages[e.Index].BackColor = Color.LightBlue;
                Rectangle tabRect = this.tabControl1.GetTabRect(e.Index);
                e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text, this.Font, SystemBrushes.ControlText, (float)(tabRect.X + 2), (float)(tabRect.Y + 2));

                if (e.Index < 4)            //前面的4个不画X
                {
                    return;
                }
                using (Pen pen = new Pen(Color.White))
                {
                    tabRect.Offset(tabRect.Width - 15, 2);
                    tabRect.Width = 15;
                    tabRect.Height = 15;
                    e.Graphics.DrawRectangle(pen, tabRect);
                }
                Color color = (e.State == DrawItemState.Selected) ? Color.LightBlue : Color.White;
                using (Brush brush = new SolidBrush(color))
                {
                    e.Graphics.FillRectangle(brush, tabRect);
                }
                using (Pen pen2 = new Pen(Color.Red))
                {
                    Point point = new Point(tabRect.X + 3, tabRect.Y + 3);
                    Point point2 = new Point((tabRect.X + tabRect.Width) - 3, (tabRect.Y + tabRect.Height) - 3);
                    e.Graphics.DrawLine(pen2, point, point2);
                    Point point3 = new Point(tabRect.X + 3, (tabRect.Y + tabRect.Height) - 3);
                    Point point4 = new Point((tabRect.X + tabRect.Width) - 3, tabRect.Y + 3);
                    e.Graphics.DrawLine(pen2, point3, point4);
                }
                e.Graphics.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            { 
                int x = e.X;
                int y = e.Y;

                Rectangle tabRect = this.tabControl1.GetTabRect(this.tabControl1.SelectedIndex);
                tabRect.Offset(tabRect.Width - 0x12, 2);
                tabRect.Width = 15;
                tabRect.Height = 15;
                if ((((x > tabRect.X) && (x < tabRect.Right)) && (y > tabRect.Y)) && (y < tabRect.Bottom))
                {
                    if (tabControl1.SelectedIndex  < 4)            //前面的4个不关闭
                    {
                        return;
                    }
                    this.tabControl1.TabPages.Remove(this.tabControl1.SelectedTab);
                }
            } 
        }
    }
}
