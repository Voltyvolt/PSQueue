namespace PSQueue
{
    partial class frmCarBrowse
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.sp1 = new FarPoint.Win.Spread.FpSpread();
            this.sp1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.sp1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // sp1
            // 
            this.sp1.AccessibleDescription = "sp1, Sheet1, Row 0, Column 0, ";
            this.sp1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.sp1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.sp1.Location = new System.Drawing.Point(12, 48);
            this.sp1.Name = "sp1";
            this.sp1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.sp1_Sheet1});
            this.sp1.Size = new System.Drawing.Size(299, 285);
            this.sp1.TabIndex = 38;
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
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "คิวที่";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "ทะเบียนรถ";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "ลำดับ เก่า";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "M_Code";
            this.sp1_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.sp1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(0).Label = "คิวที่";
            this.sp1_Sheet1.Columns.Get(0).Locked = true;
            this.sp1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(0).Width = 97F;
            this.sp1_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.sp1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(1).Label = "ทะเบียนรถ";
            this.sp1_Sheet1.Columns.Get(1).Locked = true;
            this.sp1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(1).Width = 154F;
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
            this.sp1_Sheet1.RowHeader.Columns.Get(0).Width = 28F;
            this.sp1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.txtSearch.Location = new System.Drawing.Point(12, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(299, 30);
            this.txtSearch.TabIndex = 53;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // frmCarBrowse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 345);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.sp1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmCarBrowse";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ค้นหารถ";
            this.Load += new System.EventHandler(this.frmCarBrowse_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sp1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp1_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread sp1;
        private FarPoint.Win.Spread.SheetView sp1_Sheet1;
        private System.Windows.Forms.TextBox txtSearch;
    }
}