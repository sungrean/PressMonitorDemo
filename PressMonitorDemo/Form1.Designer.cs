namespace PressMonitorDemo
{
    partial class demo
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnParamSet = new System.Windows.Forms.Button();
            this.btnClearnCount = new System.Windows.Forms.Button();
            this.btnChoseModel = new System.Windows.Forms.Button();
            this.btnAlmReset = new System.Windows.Forms.Button();
            this.btnDelAlm = new System.Windows.Forms.Button();
            this.btnGetAlm = new System.Windows.Forms.Button();
            this.btnLearn = new System.Windows.Forms.Button();
            this.btnWork = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.btnOpenPort = new System.Windows.Forms.Button();
            this.cBoxCOMPORT = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnParamSet
            // 
            this.btnParamSet.Location = new System.Drawing.Point(394, 68);
            this.btnParamSet.Name = "btnParamSet";
            this.btnParamSet.Size = new System.Drawing.Size(87, 23);
            this.btnParamSet.TabIndex = 2;
            this.btnParamSet.Text = "参数设置";
            this.btnParamSet.UseVisualStyleBackColor = true;
            this.btnParamSet.Click += new System.EventHandler(this.btnParamSet_Click);
            // 
            // btnClearnCount
            // 
            this.btnClearnCount.Location = new System.Drawing.Point(394, 107);
            this.btnClearnCount.Name = "btnClearnCount";
            this.btnClearnCount.Size = new System.Drawing.Size(87, 23);
            this.btnClearnCount.TabIndex = 3;
            this.btnClearnCount.Text = "清除计数器";
            this.btnClearnCount.UseVisualStyleBackColor = true;
            this.btnClearnCount.Click += new System.EventHandler(this.btnClearnCount_Click);
            // 
            // btnChoseModel
            // 
            this.btnChoseModel.Location = new System.Drawing.Point(394, 150);
            this.btnChoseModel.Name = "btnChoseModel";
            this.btnChoseModel.Size = new System.Drawing.Size(87, 23);
            this.btnChoseModel.TabIndex = 4;
            this.btnChoseModel.Text = "选择基准波形";
            this.btnChoseModel.UseVisualStyleBackColor = true;
            this.btnChoseModel.Click += new System.EventHandler(this.btnChoseModel_Click);
            // 
            // btnAlmReset
            // 
            this.btnAlmReset.Location = new System.Drawing.Point(529, 150);
            this.btnAlmReset.Name = "btnAlmReset";
            this.btnAlmReset.Size = new System.Drawing.Size(87, 23);
            this.btnAlmReset.TabIndex = 7;
            this.btnAlmReset.Text = "复位报警记录";
            this.btnAlmReset.UseVisualStyleBackColor = true;
            // 
            // btnDelAlm
            // 
            this.btnDelAlm.Location = new System.Drawing.Point(529, 107);
            this.btnDelAlm.Name = "btnDelAlm";
            this.btnDelAlm.Size = new System.Drawing.Size(87, 23);
            this.btnDelAlm.TabIndex = 6;
            this.btnDelAlm.Text = "删除报警记录";
            this.btnDelAlm.UseVisualStyleBackColor = true;
            // 
            // btnGetAlm
            // 
            this.btnGetAlm.Location = new System.Drawing.Point(529, 68);
            this.btnGetAlm.Name = "btnGetAlm";
            this.btnGetAlm.Size = new System.Drawing.Size(87, 23);
            this.btnGetAlm.TabIndex = 5;
            this.btnGetAlm.Text = "读取报警记录";
            this.btnGetAlm.UseVisualStyleBackColor = true;
            // 
            // btnLearn
            // 
            this.btnLearn.Font = new System.Drawing.Font("宋体", 12F);
            this.btnLearn.Location = new System.Drawing.Point(155, 133);
            this.btnLearn.Name = "btnLearn";
            this.btnLearn.Size = new System.Drawing.Size(83, 49);
            this.btnLearn.TabIndex = 9;
            this.btnLearn.Text = "学习模式";
            this.btnLearn.UseVisualStyleBackColor = true;
            this.btnLearn.Click += new System.EventHandler(this.btnLearn_Click);
            // 
            // btnWork
            // 
            this.btnWork.Font = new System.Drawing.Font("宋体", 12F);
            this.btnWork.Location = new System.Drawing.Point(66, 133);
            this.btnWork.Name = "btnWork";
            this.btnWork.Size = new System.Drawing.Size(83, 49);
            this.btnWork.TabIndex = 8;
            this.btnWork.Text = "工作模式";
            this.btnWork.UseVisualStyleBackColor = true;
            this.btnWork.Click += new System.EventHandler(this.btnWork_Click);
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // btnOpenPort
            // 
            this.btnOpenPort.Location = new System.Drawing.Point(172, 12);
            this.btnOpenPort.Name = "btnOpenPort";
            this.btnOpenPort.Size = new System.Drawing.Size(75, 23);
            this.btnOpenPort.TabIndex = 10;
            this.btnOpenPort.Text = "登陆";
            this.btnOpenPort.UseVisualStyleBackColor = true;
            this.btnOpenPort.Click += new System.EventHandler(this.btnOpenPort_Click);
            // 
            // cBoxCOMPORT
            // 
            this.cBoxCOMPORT.FormattingEnabled = true;
            this.cBoxCOMPORT.Location = new System.Drawing.Point(83, 12);
            this.cBoxCOMPORT.Name = "cBoxCOMPORT";
            this.cBoxCOMPORT.Size = new System.Drawing.Size(83, 20);
            this.cBoxCOMPORT.TabIndex = 11;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 194);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(685, 192);
            this.textBox1.TabIndex = 12;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("宋体", 12F);
            this.btnStop.Location = new System.Drawing.Point(244, 133);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(83, 49);
            this.btnStop.TabIndex = 13;
            this.btnStop.Text = "停机模式";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 398);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cBoxCOMPORT);
            this.Controls.Add(this.btnOpenPort);
            this.Controls.Add(this.btnLearn);
            this.Controls.Add(this.btnWork);
            this.Controls.Add(this.btnAlmReset);
            this.Controls.Add(this.btnDelAlm);
            this.Controls.Add(this.btnGetAlm);
            this.Controls.Add(this.btnChoseModel);
            this.Controls.Add(this.btnClearnCount);
            this.Controls.Add(this.btnParamSet);
            this.Name = "demo";
            this.ShowIcon = false;
            this.Text = "demo";
            this.Load += new System.EventHandler(this.demo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnParamSet;
        private System.Windows.Forms.Button btnClearnCount;
        private System.Windows.Forms.Button btnChoseModel;
        private System.Windows.Forms.Button btnAlmReset;
        private System.Windows.Forms.Button btnDelAlm;
        private System.Windows.Forms.Button btnGetAlm;
        private System.Windows.Forms.Button btnLearn;
        private System.Windows.Forms.Button btnWork;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button btnOpenPort;
        private System.Windows.Forms.ComboBox cBoxCOMPORT;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnStop;
    }
}

