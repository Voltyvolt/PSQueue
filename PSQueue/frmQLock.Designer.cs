namespace PSQueue
{
    partial class frmQLock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQLock));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbAlert = new System.Windows.Forms.Label();
            this.tgOnOff = new DevExpress.XtraEditors.ToggleSwitch();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtLockNoE = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtLoopE = new DevExpress.XtraEditors.SpinEdit();
            this.txtLoopS = new DevExpress.XtraEditors.SpinEdit();
            this.txtLockNoS = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtQ = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTime = new DevExpress.XtraEditors.TimeEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDate = new DevExpress.XtraEditors.DateEdit();
            this.tgLockAlert = new DevExpress.XtraEditors.ToggleSwitch();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkCar4 = new System.Windows.Forms.CheckBox();
            this.chkCar5 = new System.Windows.Forms.CheckBox();
            this.chkCar3 = new System.Windows.Forms.CheckBox();
            this.chkCar1 = new System.Windows.Forms.CheckBox();
            this.chkCar2 = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkCane16 = new System.Windows.Forms.CheckBox();
            this.chkCane15 = new System.Windows.Forms.CheckBox();
            this.chkCane9 = new System.Windows.Forms.CheckBox();
            this.chkCane12 = new System.Windows.Forms.CheckBox();
            this.chkCane14 = new System.Windows.Forms.CheckBox();
            this.chkCane10 = new System.Windows.Forms.CheckBox();
            this.chkCane13 = new System.Windows.Forms.CheckBox();
            this.chkCane11 = new System.Windows.Forms.CheckBox();
            this.tgLockCarRegis = new DevExpress.XtraEditors.ToggleSwitch();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tgButtonClear = new DevExpress.XtraEditors.ToggleSwitch();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgOnOff.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLockNoE.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoopE.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoopS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLockNoS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tgLockAlert.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgLockCarRegis.Properties)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgButtonClear.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbAlert);
            this.groupBox1.Controls.Add(this.tgOnOff);
            this.groupBox1.Location = new System.Drawing.Point(12, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(506, 134);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "สถานะ";
            // 
            // lbAlert
            // 
            this.lbAlert.AutoSize = true;
            this.lbAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lbAlert.ForeColor = System.Drawing.Color.Red;
            this.lbAlert.Location = new System.Drawing.Point(17, 86);
            this.lbAlert.Name = "lbAlert";
            this.lbAlert.Size = new System.Drawing.Size(455, 20);
            this.lbAlert.TabIndex = 2;
            this.lbAlert.Text = "** หมายเหตุ กรณีปิดการใช้งานจะทำให้ระบบ คิวล๊อก หยุดทำงานทั้งระบบ **";
            this.lbAlert.Visible = false;
            // 
            // tgOnOff
            // 
            this.tgOnOff.EditValue = true;
            this.tgOnOff.Location = new System.Drawing.Point(133, 29);
            this.tgOnOff.Name = "tgOnOff";
            this.tgOnOff.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 20F);
            this.tgOnOff.Properties.Appearance.ForeColor = System.Drawing.Color.Green;
            this.tgOnOff.Properties.Appearance.Options.UseFont = true;
            this.tgOnOff.Properties.Appearance.Options.UseForeColor = true;
            this.tgOnOff.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.tgOnOff.Properties.OffText = "ปิดใช้งาน";
            this.tgOnOff.Properties.OnText = "เปิดใช้งาน";
            this.tgOnOff.Size = new System.Drawing.Size(270, 43);
            this.tgOnOff.TabIndex = 1;
            this.tgOnOff.Toggled += new System.EventHandler(this.toggleSwitch1_Toggled);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.Location = new System.Drawing.Point(134, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = "ข้อมูลประกาศ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label2.Location = new System.Drawing.Point(71, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 46);
            this.label2.TabIndex = 3;
            this.label2.Text = "เริ่ม : ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.txtQ);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtTime);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtDate);
            this.groupBox2.Location = new System.Drawing.Point(12, 144);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(506, 427);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtLockNoE);
            this.groupBox6.Controls.Add(this.txtLoopE);
            this.groupBox6.Controls.Add(this.txtLoopS);
            this.groupBox6.Controls.Add(this.txtLockNoS);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.groupBox6.Location = new System.Drawing.Point(21, 66);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(467, 155);
            this.groupBox6.TabIndex = 21;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "รอบ / ล็อก";
            // 
            // txtLockNoE
            // 
            this.txtLockNoE.EditValue = "1";
            this.txtLockNoE.Location = new System.Drawing.Point(273, 86);
            this.txtLockNoE.Name = "txtLockNoE";
            this.txtLockNoE.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 30F);
            this.txtLockNoE.Properties.Appearance.Options.UseFont = true;
            this.txtLockNoE.Properties.Appearance.Options.UseTextOptions = true;
            this.txtLockNoE.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtLockNoE.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtLockNoE.Properties.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.txtLockNoE.Size = new System.Drawing.Size(126, 54);
            this.txtLockNoE.TabIndex = 15;
            this.txtLockNoE.SelectedIndexChanged += new System.EventHandler(this.txtLockNoE_SelectedIndexChanged);
            // 
            // txtLoopE
            // 
            this.txtLoopE.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtLoopE.Location = new System.Drawing.Point(167, 86);
            this.txtLoopE.Name = "txtLoopE";
            this.txtLoopE.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 30F);
            this.txtLoopE.Properties.Appearance.Options.UseFont = true;
            this.txtLoopE.Properties.Appearance.Options.UseTextOptions = true;
            this.txtLoopE.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtLoopE.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtLoopE.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.txtLoopE.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.txtLoopE.Size = new System.Drawing.Size(100, 54);
            this.txtLoopE.TabIndex = 14;
            this.txtLoopE.EditValueChanged += new System.EventHandler(this.txtLoopE_EditValueChanged);
            // 
            // txtLoopS
            // 
            this.txtLoopS.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtLoopS.Location = new System.Drawing.Point(167, 26);
            this.txtLoopS.Name = "txtLoopS";
            this.txtLoopS.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 30F);
            this.txtLoopS.Properties.Appearance.Options.UseFont = true;
            this.txtLoopS.Properties.Appearance.Options.UseTextOptions = true;
            this.txtLoopS.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtLoopS.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtLoopS.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.txtLoopS.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.txtLoopS.Size = new System.Drawing.Size(100, 54);
            this.txtLoopS.TabIndex = 11;
            this.txtLoopS.EditValueChanged += new System.EventHandler(this.txtLoopS_EditValueChanged);
            // 
            // txtLockNoS
            // 
            this.txtLockNoS.EditValue = "1";
            this.txtLockNoS.Location = new System.Drawing.Point(273, 26);
            this.txtLockNoS.Name = "txtLockNoS";
            this.txtLockNoS.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 30F);
            this.txtLockNoS.Properties.Appearance.Options.UseFont = true;
            this.txtLockNoS.Properties.Appearance.Options.UseTextOptions = true;
            this.txtLockNoS.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtLockNoS.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtLockNoS.Properties.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.txtLockNoS.Size = new System.Drawing.Size(126, 54);
            this.txtLockNoS.TabIndex = 12;
            this.txtLockNoS.SelectedIndexChanged += new System.EventHandler(this.txtLockNo_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label8.Location = new System.Drawing.Point(85, 90);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 46);
            this.label8.TabIndex = 13;
            this.label8.Text = "ถึง : ";
            // 
            // txtQ
            // 
            this.txtQ.EditValue = "1 - 50";
            this.txtQ.Enabled = false;
            this.txtQ.Location = new System.Drawing.Point(133, 237);
            this.txtQ.Name = "txtQ";
            this.txtQ.Properties.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.txtQ.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 30F);
            this.txtQ.Properties.Appearance.Options.UseBackColor = true;
            this.txtQ.Properties.Appearance.Options.UseFont = true;
            this.txtQ.Properties.Appearance.Options.UseTextOptions = true;
            this.txtQ.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtQ.Size = new System.Drawing.Size(339, 54);
            this.txtQ.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.Location = new System.Drawing.Point(22, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 46);
            this.label5.TabIndex = 9;
            this.label5.Text = "คิวที่ : ";
            // 
            // txtTime
            // 
            this.txtTime.EditValue = new System.DateTime(2019, 1, 10, 8, 0, 0, 0);
            this.txtTime.Location = new System.Drawing.Point(240, 357);
            this.txtTime.Name = "txtTime";
            this.txtTime.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 30F);
            this.txtTime.Properties.Appearance.Options.UseFont = true;
            this.txtTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtTime.Size = new System.Drawing.Size(183, 54);
            this.txtTime.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label4.Location = new System.Drawing.Point(23, 363);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(211, 46);
            this.label4.TabIndex = 7;
            this.label4.Text = "เวลาเริ่มใช้ : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(22, 303);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(212, 46);
            this.label3.TabIndex = 5;
            this.label3.Text = "วันที่เริ่มใช้ : ";
            // 
            // txtDate
            // 
            this.txtDate.EditValue = new System.DateTime(2019, 1, 14, 0, 0, 0, 0);
            this.txtDate.Location = new System.Drawing.Point(240, 297);
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 30F);
            this.txtDate.Properties.Appearance.Options.UseFont = true;
            this.txtDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDate.Properties.DisplayFormat.FormatString = "";
            this.txtDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtDate.Properties.EditFormat.FormatString = "";
            this.txtDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtDate.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Buffered;
            this.txtDate.Size = new System.Drawing.Size(232, 54);
            this.txtDate.TabIndex = 6;
            // 
            // tgLockAlert
            // 
            this.tgLockAlert.Location = new System.Drawing.Point(532, 12);
            this.tgLockAlert.Name = "tgLockAlert";
            this.tgLockAlert.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.tgLockAlert.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.tgLockAlert.Properties.Appearance.Options.UseFont = true;
            this.tgLockAlert.Properties.Appearance.Options.UseForeColor = true;
            this.tgLockAlert.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.tgLockAlert.Properties.OffText = "ไม่บังคับใช้คิวล๊อก";
            this.tgLockAlert.Properties.OnText = "บังคับใช้คิวล็อก";
            this.tgLockAlert.Size = new System.Drawing.Size(252, 27);
            this.tgLockAlert.TabIndex = 12;
            this.tgLockAlert.Toggled += new System.EventHandler(this.tgLockAlert_Toggled);
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.ImageOptions.Image")));
            this.btnSave.Location = new System.Drawing.Point(12, 577);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(506, 72);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "บันทึก (F5)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkCar4);
            this.groupBox3.Controls.Add(this.chkCar5);
            this.groupBox3.Controls.Add(this.chkCar3);
            this.groupBox3.Controls.Add(this.chkCar1);
            this.groupBox3.Controls.Add(this.chkCar2);
            this.groupBox3.Location = new System.Drawing.Point(524, 45);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(115, 174);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ประเภทรถ";
            // 
            // chkCar4
            // 
            this.chkCar4.AutoSize = true;
            this.chkCar4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCar4.Location = new System.Drawing.Point(15, 139);
            this.chkCar4.Name = "chkCar4";
            this.chkCar4.Size = new System.Drawing.Size(58, 29);
            this.chkCar4.TabIndex = 20;
            this.chkCar4.Text = "พ่วง";
            this.chkCar4.UseVisualStyleBackColor = true;
            // 
            // chkCar5
            // 
            this.chkCar5.AutoSize = true;
            this.chkCar5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCar5.Location = new System.Drawing.Point(15, 79);
            this.chkCar5.Name = "chkCar5";
            this.chkCar5.Size = new System.Drawing.Size(69, 29);
            this.chkCar5.TabIndex = 21;
            this.chkCar5.Text = "สิบล้อ";
            this.chkCar5.UseVisualStyleBackColor = true;
            // 
            // chkCar3
            // 
            this.chkCar3.AutoSize = true;
            this.chkCar3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCar3.Location = new System.Drawing.Point(15, 109);
            this.chkCar3.Name = "chkCar3";
            this.chkCar3.Size = new System.Drawing.Size(80, 29);
            this.chkCar3.TabIndex = 19;
            this.chkCar3.Text = "บรรทุก";
            this.chkCar3.UseVisualStyleBackColor = true;
            // 
            // chkCar1
            // 
            this.chkCar1.AutoSize = true;
            this.chkCar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCar1.Location = new System.Drawing.Point(15, 19);
            this.chkCar1.Name = "chkCar1";
            this.chkCar1.Size = new System.Drawing.Size(90, 29);
            this.chkCar1.TabIndex = 17;
            this.chkCar1.Text = "สาวแต๋น";
            this.chkCar1.UseVisualStyleBackColor = true;
            // 
            // chkCar2
            // 
            this.chkCar2.AutoSize = true;
            this.chkCar2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCar2.Location = new System.Drawing.Point(15, 49);
            this.chkCar2.Name = "chkCar2";
            this.chkCar2.Size = new System.Drawing.Size(92, 29);
            this.chkCar2.TabIndex = 18;
            this.chkCar2.Text = "เทนเลอร์";
            this.chkCar2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkCane16);
            this.groupBox4.Controls.Add(this.chkCane15);
            this.groupBox4.Controls.Add(this.chkCane9);
            this.groupBox4.Controls.Add(this.chkCane12);
            this.groupBox4.Controls.Add(this.chkCane14);
            this.groupBox4.Controls.Add(this.chkCane10);
            this.groupBox4.Controls.Add(this.chkCane13);
            this.groupBox4.Controls.Add(this.chkCane11);
            this.groupBox4.Location = new System.Drawing.Point(647, 45);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(477, 174);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "ประเภทอ้อย";
            // 
            // chkCane16
            // 
            this.chkCane16.AutoSize = true;
            this.chkCane16.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCane16.Location = new System.Drawing.Point(16, 133);
            this.chkCane16.Name = "chkCane16";
            this.chkCane16.Size = new System.Drawing.Size(237, 29);
            this.chkCane16.TabIndex = 31;
            this.chkCane16.Text = "อ้อยไฟไหม้รถตัดนอก กาบใบ";
            this.chkCane16.UseVisualStyleBackColor = true;
            // 
            // chkCane15
            // 
            this.chkCane15.AutoSize = true;
            this.chkCane15.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCane15.Location = new System.Drawing.Point(273, 98);
            this.chkCane15.Name = "chkCane15";
            this.chkCane15.Size = new System.Drawing.Size(165, 29);
            this.chkCane15.TabIndex = 32;
            this.chkCane15.Text = "อ้อยไฟไหม้-สะอาด";
            this.chkCane15.UseVisualStyleBackColor = true;
            // 
            // chkCane9
            // 
            this.chkCane9.AutoSize = true;
            this.chkCane9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCane9.Location = new System.Drawing.Point(15, 28);
            this.chkCane9.Name = "chkCane9";
            this.chkCane9.Size = new System.Drawing.Size(112, 29);
            this.chkCane9.TabIndex = 25;
            this.chkCane9.Text = "อ้อยไฟไหม้";
            this.chkCane9.UseVisualStyleBackColor = true;
            // 
            // chkCane12
            // 
            this.chkCane12.AutoSize = true;
            this.chkCane12.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCane12.Location = new System.Drawing.Point(15, 63);
            this.chkCane12.Name = "chkCane12";
            this.chkCane12.Size = new System.Drawing.Size(242, 29);
            this.chkCane12.TabIndex = 26;
            this.chkCane12.Text = "อ้อยไฟไหม้ยอดยาว+ปนเปื้อน";
            this.chkCane12.UseVisualStyleBackColor = true;
            // 
            // chkCane14
            // 
            this.chkCane14.AutoSize = true;
            this.chkCane14.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCane14.Location = new System.Drawing.Point(15, 98);
            this.chkCane14.Name = "chkCane14";
            this.chkCane14.Size = new System.Drawing.Size(205, 29);
            this.chkCane14.TabIndex = 30;
            this.chkCane14.Text = "อ้อยไฟไหม้รถตัด กาบใบ";
            this.chkCane14.UseVisualStyleBackColor = true;
            // 
            // chkCane10
            // 
            this.chkCane10.AutoSize = true;
            this.chkCane10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCane10.Location = new System.Drawing.Point(133, 28);
            this.chkCane10.Name = "chkCane10";
            this.chkCane10.Size = new System.Drawing.Size(132, 29);
            this.chkCane10.TabIndex = 27;
            this.chkCane10.Text = "อ้อยไฟไหม้ C";
            this.chkCane10.UseVisualStyleBackColor = true;
            // 
            // chkCane13
            // 
            this.chkCane13.AutoSize = true;
            this.chkCane13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCane13.Location = new System.Drawing.Point(273, 63);
            this.chkCane13.Name = "chkCane13";
            this.chkCane13.Size = new System.Drawing.Size(152, 29);
            this.chkCane13.TabIndex = 29;
            this.chkCane13.Text = "อ้อยไฟไหม้รถตัด";
            this.chkCane13.UseVisualStyleBackColor = true;
            // 
            // chkCane11
            // 
            this.chkCane11.AutoSize = true;
            this.chkCane11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCane11.Location = new System.Drawing.Point(273, 28);
            this.chkCane11.Name = "chkCane11";
            this.chkCane11.Size = new System.Drawing.Size(184, 29);
            this.chkCane11.TabIndex = 28;
            this.chkCane11.Text = "อ้อยไฟไหม้รถตัดนอก";
            this.chkCane11.UseVisualStyleBackColor = true;
            // 
            // tgLockCarRegis
            // 
            this.tgLockCarRegis.Location = new System.Drawing.Point(8, 19);
            this.tgLockCarRegis.Name = "tgLockCarRegis";
            this.tgLockCarRegis.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.tgLockCarRegis.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.tgLockCarRegis.Properties.Appearance.Options.UseFont = true;
            this.tgLockCarRegis.Properties.Appearance.Options.UseForeColor = true;
            this.tgLockCarRegis.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.tgLockCarRegis.Properties.OffText = "ปิด";
            this.tgLockCarRegis.Properties.OnText = "เปิด";
            this.tgLockCarRegis.Size = new System.Drawing.Size(121, 27);
            this.tgLockCarRegis.TabIndex = 18;
            this.tgLockCarRegis.Toggled += new System.EventHandler(this.tgLockCarRegis_Toggled);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.tgButtonClear);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.tgLockCarRegis);
            this.groupBox5.Location = new System.Drawing.Point(524, 225);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(600, 424);
            this.groupBox5.TabIndex = 19;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "อื่นๆ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label7.Location = new System.Drawing.Point(118, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(18, 25);
            this.label7.TabIndex = 20;
            this.label7.Text = ":";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label6.Location = new System.Drawing.Point(142, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(389, 25);
            this.label6.TabIndex = 19;
            this.label6.Text = "ล๊อคทะเบียนรถต้องตรงกับที่ลงทะเบียน คิวล็อก เท่านั้น";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label9.Location = new System.Drawing.Point(116, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 25);
            this.label9.TabIndex = 23;
            this.label9.Text = ":";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label10.Location = new System.Drawing.Point(140, 51);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(219, 25);
            this.label10.TabIndex = 22;
            this.label10.Text = "แสดงปุ่ม Clear ข้อมูล คิวล็อก";
            // 
            // tgButtonClear
            // 
            this.tgButtonClear.Location = new System.Drawing.Point(8, 52);
            this.tgButtonClear.Name = "tgButtonClear";
            this.tgButtonClear.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.tgButtonClear.Properties.Appearance.ForeColor = System.Drawing.Color.Red;
            this.tgButtonClear.Properties.Appearance.Options.UseFont = true;
            this.tgButtonClear.Properties.Appearance.Options.UseForeColor = true;
            this.tgButtonClear.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Default;
            this.tgButtonClear.Properties.OffText = "ปิด";
            this.tgButtonClear.Properties.OnText = "เปิด";
            this.tgButtonClear.Size = new System.Drawing.Size(121, 27);
            this.tgButtonClear.TabIndex = 21;
            this.tgButtonClear.Toggled += new System.EventHandler(this.tgButtonClear_Toggled);
            // 
            // frmQLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1136, 660);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.tgLockAlert);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmQLock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ตั้งค่า คิว ล๊อก";
            this.Load += new System.EventHandler(this.frmQLock_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgOnOff.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLockNoE.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoopE.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoopS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLockNoS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tgLockAlert.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgLockCarRegis.Properties)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgButtonClear.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.ToggleSwitch tgOnOff;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.TimeEdit txtTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.DateEdit txtDate;
        private DevExpress.XtraEditors.TextEdit txtQ;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private System.Windows.Forms.Label lbAlert;
        private DevExpress.XtraEditors.ToggleSwitch tgLockAlert;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkCar4;
        private System.Windows.Forms.CheckBox chkCar5;
        private System.Windows.Forms.CheckBox chkCar3;
        private System.Windows.Forms.CheckBox chkCar1;
        private System.Windows.Forms.CheckBox chkCar2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkCane16;
        private System.Windows.Forms.CheckBox chkCane15;
        private System.Windows.Forms.CheckBox chkCane9;
        private System.Windows.Forms.CheckBox chkCane12;
        private System.Windows.Forms.CheckBox chkCane14;
        private System.Windows.Forms.CheckBox chkCane10;
        private System.Windows.Forms.CheckBox chkCane13;
        private System.Windows.Forms.CheckBox chkCane11;
        private DevExpress.XtraEditors.ComboBoxEdit txtLockNoS;
        private DevExpress.XtraEditors.SpinEdit txtLoopS;
        private DevExpress.XtraEditors.ToggleSwitch tgLockCarRegis;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.ComboBoxEdit txtLockNoE;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.SpinEdit txtLoopE;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.ToggleSwitch tgButtonClear;
    }
}