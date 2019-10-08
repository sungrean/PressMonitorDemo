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
            this.txtStatu = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnStop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnModbusServer = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtComRecv = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnParamSet
            // 
            this.btnParamSet.Location = new System.Drawing.Point(439, 24);
            this.btnParamSet.Name = "btnParamSet";
            this.btnParamSet.Size = new System.Drawing.Size(87, 23);
            this.btnParamSet.TabIndex = 2;
            this.btnParamSet.Text = "参数设置";
            this.btnParamSet.UseVisualStyleBackColor = true;
            this.btnParamSet.Click += new System.EventHandler(this.btnParamSet_Click);
            // 
            // btnClearnCount
            // 
            this.btnClearnCount.Location = new System.Drawing.Point(439, 63);
            this.btnClearnCount.Name = "btnClearnCount";
            this.btnClearnCount.Size = new System.Drawing.Size(87, 23);
            this.btnClearnCount.TabIndex = 3;
            this.btnClearnCount.Text = "清除计数器";
            this.btnClearnCount.UseVisualStyleBackColor = true;
            this.btnClearnCount.Click += new System.EventHandler(this.btnClearnCount_Click);
            // 
            // btnChoseModel
            // 
            this.btnChoseModel.Location = new System.Drawing.Point(439, 106);
            this.btnChoseModel.Name = "btnChoseModel";
            this.btnChoseModel.Size = new System.Drawing.Size(87, 23);
            this.btnChoseModel.TabIndex = 4;
            this.btnChoseModel.Text = "选择基准波形";
            this.btnChoseModel.UseVisualStyleBackColor = true;
            this.btnChoseModel.Click += new System.EventHandler(this.btnChoseModel_Click);
            // 
            // btnAlmReset
            // 
            this.btnAlmReset.Location = new System.Drawing.Point(574, 106);
            this.btnAlmReset.Name = "btnAlmReset";
            this.btnAlmReset.Size = new System.Drawing.Size(87, 23);
            this.btnAlmReset.TabIndex = 7;
            this.btnAlmReset.Text = "复位报警记录";
            this.btnAlmReset.UseVisualStyleBackColor = true;
            this.btnAlmReset.Click += new System.EventHandler(this.btnAlmReset_Click);
            // 
            // btnDelAlm
            // 
            this.btnDelAlm.Location = new System.Drawing.Point(574, 63);
            this.btnDelAlm.Name = "btnDelAlm";
            this.btnDelAlm.Size = new System.Drawing.Size(87, 23);
            this.btnDelAlm.TabIndex = 6;
            this.btnDelAlm.Text = "删除报警记录";
            this.btnDelAlm.UseVisualStyleBackColor = true;
            // 
            // btnGetAlm
            // 
            this.btnGetAlm.Location = new System.Drawing.Point(574, 24);
            this.btnGetAlm.Name = "btnGetAlm";
            this.btnGetAlm.Size = new System.Drawing.Size(87, 23);
            this.btnGetAlm.TabIndex = 5;
            this.btnGetAlm.Text = "读取报警记录";
            this.btnGetAlm.UseVisualStyleBackColor = true;
            // 
            // btnLearn
            // 
            this.btnLearn.Font = new System.Drawing.Font("宋体", 12F);
            this.btnLearn.Location = new System.Drawing.Point(160, 53);
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
            this.btnWork.Location = new System.Drawing.Point(71, 53);
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
            this.btnOpenPort.Location = new System.Drawing.Point(177, 24);
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
            this.cBoxCOMPORT.Location = new System.Drawing.Point(88, 24);
            this.cBoxCOMPORT.Name = "cBoxCOMPORT";
            this.cBoxCOMPORT.Size = new System.Drawing.Size(83, 20);
            this.cBoxCOMPORT.TabIndex = 11;
            // 
            // txtStatu
            // 
            this.txtStatu.Location = new System.Drawing.Point(23, 244);
            this.txtStatu.Name = "txtStatu";
            this.txtStatu.Size = new System.Drawing.Size(154, 21);
            this.txtStatu.TabIndex = 12;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("宋体", 12F);
            this.btnStop.Location = new System.Drawing.Point(249, 53);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(83, 49);
            this.btnStop.TabIndex = 13;
            this.btnStop.Text = "停机模式";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.btnModbusServer);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Controls.Add(this.txtIp);
            this.groupBox1.Location = new System.Drawing.Point(17, 269);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(673, 221);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "modbus tcp服务";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.address,
            this.value});
            this.dataGridView1.Location = new System.Drawing.Point(404, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(263, 195);
            this.dataGridView1.TabIndex = 3;
            // 
            // address
            // 
            this.address.HeaderText = "地址";
            this.address.Name = "address";
            this.address.ReadOnly = true;
            this.address.Width = 150;
            // 
            // value
            // 
            this.value.HeaderText = "值";
            this.value.Name = "value";
            this.value.ReadOnly = true;
            this.value.Width = 150;
            // 
            // btnModbusServer
            // 
            this.btnModbusServer.Location = new System.Drawing.Point(292, 18);
            this.btnModbusServer.Name = "btnModbusServer";
            this.btnModbusServer.Size = new System.Drawing.Size(75, 23);
            this.btnModbusServer.TabIndex = 2;
            this.btnModbusServer.Text = "启动modbus服务";
            this.btnModbusServer.UseVisualStyleBackColor = true;
            this.btnModbusServer.Click += new System.EventHandler(this.btnModbusServer_Click);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(177, 20);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(93, 21);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "502";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(6, 20);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(148, 21);
            this.txtIp.TabIndex = 0;
            this.txtIp.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "串口接收到的数据";
            // 
            // txtComRecv
            // 
            this.txtComRecv.Location = new System.Drawing.Point(23, 126);
            this.txtComRecv.Multiline = true;
            this.txtComRecv.Name = "txtComRecv";
            this.txtComRecv.Size = new System.Drawing.Size(309, 112);
            this.txtComRecv.TabIndex = 16;
            // 
            // demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 502);
            this.Controls.Add(this.txtComRecv);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.txtStatu);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
        private System.Windows.Forms.TextBox txtStatu;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn address;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
        private System.Windows.Forms.Button btnModbusServer;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtComRecv;
    }
}

