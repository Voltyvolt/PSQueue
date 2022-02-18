namespace PSQueue
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.txtPass = new DevExpress.XtraEditors.TextEdit();
            this.txtUser = new DevExpress.XtraEditors.TextEdit();
            this.btnSetting = new DevExpress.XtraEditors.SimpleButton();
            this.btnExit = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Controls.Add(this.labelControl1);
            this.groupBox1.Controls.Add(this.labelControl9);
            this.groupBox1.Controls.Add(this.txtPass);
            this.groupBox1.Controls.Add(this.txtUser);
            this.groupBox1.Location = new System.Drawing.Point(33, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 242);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // btnLogin
            // 
            this.btnLogin.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnLogin.ImageOptions.Image")));
            this.btnLogin.Location = new System.Drawing.Point(24, 172);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(174, 55);
            this.btnLogin.TabIndex = 35;
            this.btnLogin.Text = "เข้าสู่ระบบ";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(70, 95);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(69, 24);
            this.labelControl1.TabIndex = 34;
            this.labelControl1.Text = "รหัสผ่าน";
            // 
            // labelControl9
            // 
            this.labelControl9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Location = new System.Drawing.Point(70, 17);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(69, 24);
            this.labelControl9.TabIndex = 33;
            this.labelControl9.Text = "รหัสผู้ใช้";
            // 
            // txtPass
            // 
            this.txtPass.EditValue = "";
            this.txtPass.Location = new System.Drawing.Point(24, 125);
            this.txtPass.Name = "txtPass";
            this.txtPass.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.txtPass.Properties.Appearance.Options.UseFont = true;
            this.txtPass.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Bisque;
            this.txtPass.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtPass.Properties.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(174, 30);
            this.txtPass.TabIndex = 1;
            this.txtPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPass_KeyDown);
            // 
            // txtUser
            // 
            this.txtUser.EditValue = "";
            this.txtUser.Location = new System.Drawing.Point(24, 47);
            this.txtUser.Name = "txtUser";
            this.txtUser.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.txtUser.Properties.Appearance.Options.UseFont = true;
            this.txtUser.Properties.AppearanceFocused.BackColor = System.Drawing.Color.Bisque;
            this.txtUser.Properties.AppearanceFocused.Options.UseBackColor = true;
            this.txtUser.Size = new System.Drawing.Size(174, 30);
            this.txtUser.TabIndex = 0;
            this.txtUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUser_KeyDown);
            // 
            // btnSetting
            // 
            this.btnSetting.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.btnSetting.Appearance.Options.UseFont = true;
            this.btnSetting.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnSetting.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSetting.ImageOptions.Image")));
            this.btnSetting.Location = new System.Drawing.Point(268, 257);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(24, 23);
            this.btnSetting.TabIndex = 20;
            this.btnSetting.Tag = "";
            this.btnSetting.ToolTip = "ตั้งค่าโปรแกรม";
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnExit
            // 
            this.btnExit.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.btnExit.Appearance.Options.UseFont = true;
            this.btnExit.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnExit.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.ImageOptions.Image")));
            this.btnExit.Location = new System.Drawing.Point(268, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(24, 23);
            this.btnExit.TabIndex = 21;
            this.btnExit.Tag = "";
            this.btnExit.ToolTip = "ปิดโปรแกรม";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(289, 282);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "เข้าสู่ระบบ";
            this.Load += new System.EventHandler(this.Login_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUser.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.TextEdit txtPass;
        private DevExpress.XtraEditors.TextEdit txtUser;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.SimpleButton btnSetting;
        private DevExpress.XtraEditors.SimpleButton btnExit;
    }
}