namespace PSQueue
{
    partial class frmLoginOption
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkIn = new System.Windows.Forms.RadioButton();
            this.chkOut = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkWeight = new System.Windows.Forms.RadioButton();
            this.chkQ = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cmbQ = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkOut);
            this.groupBox1.Controls.Add(this.chkIn);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IN - OUT";
            // 
            // chkIn
            // 
            this.chkIn.AutoSize = true;
            this.chkIn.Location = new System.Drawing.Point(39, 19);
            this.chkIn.Name = "chkIn";
            this.chkIn.Size = new System.Drawing.Size(52, 17);
            this.chkIn.TabIndex = 2;
            this.chkIn.TabStop = true;
            this.chkIn.Text = "ขาเข้า";
            this.chkIn.UseVisualStyleBackColor = true;
            this.chkIn.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // chkOut
            // 
            this.chkOut.AutoSize = true;
            this.chkOut.Location = new System.Drawing.Point(124, 19);
            this.chkOut.Name = "chkOut";
            this.chkOut.Size = new System.Drawing.Size(55, 17);
            this.chkOut.TabIndex = 2;
            this.chkOut.TabStop = true;
            this.chkOut.Text = "ขาออก";
            this.chkOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkOut.UseVisualStyleBackColor = true;
            this.chkOut.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkWeight);
            this.groupBox2.Controls.Add(this.chkQ);
            this.groupBox2.Location = new System.Drawing.Point(12, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(216, 51);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "โหมด";
            // 
            // chkWeight
            // 
            this.chkWeight.AutoSize = true;
            this.chkWeight.Location = new System.Drawing.Point(124, 19);
            this.chkWeight.Name = "chkWeight";
            this.chkWeight.Size = new System.Drawing.Size(55, 17);
            this.chkWeight.TabIndex = 2;
            this.chkWeight.TabStop = true;
            this.chkWeight.Text = "ห้องชั่ง";
            this.chkWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkWeight.UseVisualStyleBackColor = true;
            this.chkWeight.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // chkQ
            // 
            this.chkQ.AutoSize = true;
            this.chkQ.Location = new System.Drawing.Point(39, 19);
            this.chkQ.Name = "chkQ";
            this.chkQ.Size = new System.Drawing.Size(58, 17);
            this.chkQ.TabIndex = 2;
            this.chkQ.TabStop = true;
            this.chkQ.Text = "ป้อมคิว";
            this.chkQ.UseVisualStyleBackColor = true;
            this.chkQ.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmbPort);
            this.groupBox3.Location = new System.Drawing.Point(12, 183);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(216, 51);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Ports";
            // 
            // cmbPort
            // 
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10"});
            this.cmbPort.Location = new System.Drawing.Point(39, 19);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(140, 21);
            this.cmbPort.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cmbQ);
            this.groupBox4.Location = new System.Drawing.Point(12, 126);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(216, 51);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "ป้อมคิวที่";
            // 
            // cmbQ
            // 
            this.cmbQ.FormattingEnabled = true;
            this.cmbQ.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cmbQ.Location = new System.Drawing.Point(39, 19);
            this.cmbQ.Name = "cmbQ";
            this.cmbQ.Size = new System.Drawing.Size(140, 21);
            this.cmbQ.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 240);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(215, 29);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "บันทึก";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmLoginOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 281);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLoginOption";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ตั้งค่า";
            this.Load += new System.EventHandler(this.frmLoginOption_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton chkOut;
        private System.Windows.Forms.RadioButton chkIn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton chkWeight;
        private System.Windows.Forms.RadioButton chkQ;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cmbQ;
        private System.Windows.Forms.Button btnSave;
    }
}