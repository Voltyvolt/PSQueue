namespace PSQueue
{
    partial class frmSetDoc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetDoc));
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType1 = new FarPoint.Win.Spread.CellType.NumberCellType();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.sp1 = new FarPoint.Win.Spread.FpSpread();
            this.sp1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.sp1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.ImageOptions.Image")));
            this.btnSave.Location = new System.Drawing.Point(12, 207);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(260, 42);
            this.btnSave.TabIndex = 36;
            this.btnSave.Text = "บันทึก";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // sp1
            // 
            this.sp1.AccessibleDescription = "sp1, Sheet1, Row 0, Column 0, ";
            this.sp1.EditModeReplace = true;
            this.sp1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.sp1.Location = new System.Drawing.Point(3, 4);
            this.sp1.Name = "sp1";
            this.sp1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.sp1_Sheet1});
            this.sp1.Size = new System.Drawing.Size(277, 197);
            this.sp1.TabIndex = 37;
            // 
            // sp1_Sheet1
            // 
            this.sp1_Sheet1.Reset();
            sp1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.sp1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            sp1_Sheet1.ColumnCount = 4;
            sp1_Sheet1.RowCount = 5;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "รายการ";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "ลำดับ";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "ลำดับ เก่า";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "M_Code";
            this.sp1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(0).Label = "รายการ";
            this.sp1_Sheet1.Columns.Get(0).Locked = true;
            this.sp1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(0).Width = 136F;
            this.sp1_Sheet1.Columns.Get(1).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            numberCellType1.DecimalPlaces = 0;
            numberCellType1.MaximumValue = 10000000D;
            numberCellType1.MinimumValue = -10000000D;
            numberCellType1.ShowSeparator = true;
            this.sp1_Sheet1.Columns.Get(1).CellType = numberCellType1;
            this.sp1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(1).Label = "ลำดับ";
            this.sp1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(1).Width = 92F;
            this.sp1_Sheet1.Columns.Get(2).Label = "ลำดับ เก่า";
            this.sp1_Sheet1.Columns.Get(2).Width = 92F;
            this.sp1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(3).Label = "M_Code";
            this.sp1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(3).Width = 79F;
            this.sp1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.sp1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.sp1_Sheet1.RowHeader.Columns.Get(0).Width = 28F;
            this.sp1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmSetDoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.sp1);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSetDoc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ตั้งค่าเอกสาร";
            this.Load += new System.EventHandler(this.frmSetDoc_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sp1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSave;
        private FarPoint.Win.Spread.FpSpread sp1;
        private FarPoint.Win.Spread.SheetView sp1_Sheet1;
    }
}