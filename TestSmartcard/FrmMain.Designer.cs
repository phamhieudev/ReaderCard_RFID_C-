namespace TestSmartcard
{
    partial class FrmMain
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCardSerialNo = new System.Windows.Forms.TextBox();
            this.btnReadCardSerialNo = new System.Windows.Forms.Button();
            this.labelState = new System.Windows.Forms.Label();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(289, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Serial Number:";
            // 
            // txtCardSerialNo
            // 
            this.txtCardSerialNo.Location = new System.Drawing.Point(368, 199);
            this.txtCardSerialNo.Name = "txtCardSerialNo";
            this.txtCardSerialNo.Size = new System.Drawing.Size(148, 20);
            this.txtCardSerialNo.TabIndex = 2;
            // 
            // btnReadCardSerialNo
            // 
            this.btnReadCardSerialNo.Location = new System.Drawing.Point(523, 194);
            this.btnReadCardSerialNo.Name = "btnReadCardSerialNo";
            this.btnReadCardSerialNo.Size = new System.Drawing.Size(75, 25);
            this.btnReadCardSerialNo.TabIndex = 3;
            this.btnReadCardSerialNo.Text = "Read";
            this.btnReadCardSerialNo.UseVisualStyleBackColor = true;
            this.btnReadCardSerialNo.Click += new System.EventHandler(this.btnReadCardSerialNo_Click);
            // 
            // labelState
            // 
            this.labelState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelState.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelState.Font = new System.Drawing.Font("SimSun", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelState.Location = new System.Drawing.Point(0, 430);
            this.labelState.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(764, 40);
            this.labelState.TabIndex = 32;
            this.labelState.Text = "Thông Báo";
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(440, 280);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 33;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 470);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.btnReadCardSerialNo);
            this.Controls.Add(this.txtCardSerialNo);
            this.Controls.Add(this.label2);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đọc thẻ Tiện Ích";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCardSerialNo;
        private System.Windows.Forms.Button btnReadCardSerialNo;
        private System.Windows.Forms.Label labelState;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
    }
}

