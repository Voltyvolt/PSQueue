namespace PSQueue
{
    partial class frmShowDetail
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType1 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType2 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType3 = new FarPoint.Win.Spread.CellType.NumberCellType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShowDetail));
            this.sp1 = new FarPoint.Win.Spread.FpSpread();
            this.sp1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            ((System.ComponentModel.ISupportInitialize)(this.sp1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // sp1
            // 
            this.sp1.AccessibleDescription = "sp1, รายละเอียด, Row 0, Column 0, ";
            this.sp1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sp1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.sp1.Location = new System.Drawing.Point(0, 0);
            this.sp1.Name = "sp1";
            this.sp1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.sp1_Sheet1});
            this.sp1.Size = new System.Drawing.Size(1143, 606);
            this.sp1.TabIndex = 18;
            // 
            // sp1_Sheet1
            // 
            this.sp1_Sheet1.Reset();
            sp1_Sheet1.SheetName = "รายละเอียด";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.sp1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            sp1_Sheet1.ColumnCount = 11;
            sp1_Sheet1.ColumnHeader.RowCount = 2;
            sp1_Sheet1.RowCount = 10;
            this.sp1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 0).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "เลขที่บิล";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 1).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "คิวที่";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 2).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "ราง";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 3).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "ทะเบียนรถ";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 4).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "โควต้า";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 5).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "ประเภทอ้อย";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 6).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "วันที่";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 7).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "เวลา";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 8).ColumnSpan = 3;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "น้ำหนัก";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 8).Value = "ขาเข้า";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 9).Value = "ขาออก";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 10).Value = "สุทธิ";
            this.sp1_Sheet1.ColumnHeader.Rows.Get(0).Height = 22F;
            this.sp1_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.sp1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(0).Width = 117F;
            this.sp1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(3).Width = 100F;
            this.sp1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(4).Width = 80F;
            this.sp1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.sp1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(5).Width = 170F;
            this.sp1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(6).Width = 100F;
            this.sp1_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(7).Width = 100F;
            numberCellType1.DecimalPlaces = 3;
            numberCellType1.ShowSeparator = true;
            this.sp1_Sheet1.Columns.Get(8).CellType = numberCellType1;
            this.sp1_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.sp1_Sheet1.Columns.Get(8).Label = "ขาเข้า";
            this.sp1_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(8).Width = 100F;
            numberCellType2.DecimalPlaces = 3;
            numberCellType2.ShowSeparator = true;
            this.sp1_Sheet1.Columns.Get(9).CellType = numberCellType2;
            this.sp1_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.sp1_Sheet1.Columns.Get(9).Label = "ขาออก";
            this.sp1_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(9).Width = 100F;
            this.sp1_Sheet1.Columns.Get(10).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(238)))), ((int)(((byte)(188)))));
            numberCellType3.DecimalPlaces = 3;
            numberCellType3.ShowSeparator = true;
            this.sp1_Sheet1.Columns.Get(10).CellType = numberCellType3;
            this.sp1_Sheet1.Columns.Get(10).Formula = "RC[-2]-RC[-1]";
            this.sp1_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.sp1_Sheet1.Columns.Get(10).Label = "สุทธิ";
            this.sp1_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(10).Width = 100F;
            this.sp1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ExtendedSelect;
            this.sp1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.sp1_Sheet1.SelectionPolicy = FarPoint.Win.Spread.Model.SelectionPolicy.MultiRange;
            this.sp1_Sheet1.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            this.sp1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmShowDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 606);
            this.Controls.Add(this.sp1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmShowDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "แสดงข้อมูลบิล";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmShowDetail_KeyDown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.frmShowDetail_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.sp1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private FarPoint.Win.Spread.SheetView sp1_Sheet1;
        public FarPoint.Win.Spread.FpSpread sp1;
    }
}