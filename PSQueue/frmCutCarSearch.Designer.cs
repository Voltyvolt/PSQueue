namespace PSQueue
{
    partial class frmCutCarSearch
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.sp1 = new FarPoint.Win.Spread.FpSpread();
            this.sp1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sp1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // sp1
            // 
            this.sp1.AccessibleDescription = "sp1, Sheet1, Row 0, Column 0, ";
            this.sp1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.sp1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.sp1.Location = new System.Drawing.Point(12, 12);
            this.sp1.Name = "sp1";
            this.sp1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.sp1_Sheet1});
            this.sp1.Size = new System.Drawing.Size(358, 295);
            this.sp1.TabIndex = 55;
            this.sp1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.sp1_CellDoubleClick);
            // 
            // sp1_Sheet1
            // 
            this.sp1_Sheet1.Reset();
            sp1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.sp1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            sp1_Sheet1.ColumnCount = 4;
            sp1_Sheet1.RowCount = 5;
            this.sp1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "เลขรถตัด";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "ชื่อรถตัด";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "ลำดับ เก่า";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "M_Code";
            this.sp1_Sheet1.Columns.Get(0).CellType = textCellType3;
            this.sp1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(0).Label = "เลขรถตัด";
            this.sp1_Sheet1.Columns.Get(0).Locked = true;
            this.sp1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(0).Width = 155F;
            this.sp1_Sheet1.Columns.Get(1).CellType = textCellType4;
            this.sp1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(1).Label = "ชื่อรถตัด";
            this.sp1_Sheet1.Columns.Get(1).Locked = true;
            this.sp1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(1).Width = 183F;
            this.sp1_Sheet1.Columns.Get(2).Label = "ลำดับ เก่า";
            this.sp1_Sheet1.Columns.Get(2).Visible = false;
            this.sp1_Sheet1.Columns.Get(2).Width = 92F;
            this.sp1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(3).Label = "M_Code";
            this.sp1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(3).Visible = false;
            this.sp1_Sheet1.Columns.Get(3).Width = 79F;
            this.sp1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.sp1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.sp1_Sheet1.RowHeader.Columns.Get(0).Width = 0F;
            this.sp1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 312);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(295, 24);
            this.label1.TabIndex = 56;
            this.label1.Text = "* Double Click เลือกเลขรถตัดที่ต้องการ";
            // 
            // frmCutCarSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 345);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sp1);
            this.Name = "frmCutCarSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "เลขรถตัด";
            this.Load += new System.EventHandler(this.frmCutCarSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sp1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp1_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private FarPoint.Win.Spread.FpSpread sp1;
        private FarPoint.Win.Spread.SheetView sp1_Sheet1;
        private System.Windows.Forms.Label label1;
    }
}