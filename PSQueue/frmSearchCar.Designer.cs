namespace PSQueue
{
    partial class frmSearchCar
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
            this.components = new System.ComponentModel.Container();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.sp1 = new FarPoint.Win.Spread.FpSpread();
            this.sp1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmb_select = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSearchDoc = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp1_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(156, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(220, 38);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // sp1
            // 
            this.sp1.AccessibleDescription = "sp1, Sheet1, Row 0, Column 0, ";
            this.sp1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sp1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.sp1.Location = new System.Drawing.Point(0, 0);
            this.sp1.Name = "sp1";
            this.sp1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.sp1_Sheet1});
            this.sp1.Size = new System.Drawing.Size(1492, 625);
            this.sp1.TabIndex = 56;
            this.sp1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.sp1_CellDoubleClick);
            // 
            // sp1_Sheet1
            // 
            this.sp1_Sheet1.Reset();
            sp1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.sp1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            sp1_Sheet1.ColumnCount = 18;
            sp1_Sheet1.ColumnHeader.RowCount = 2;
            sp1_Sheet1.RowCount = 5;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 0).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "เลขใบนำตัด";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 1).ColumnSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "แปลง";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 3).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "โควต้า";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 4).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "ประเภทอ้อย";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 5).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "ทะเบียนรถแม่";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 6).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "ลูกพ่วง";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 7).ColumnSpan = 3;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "เหมาตัด";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 10).ColumnSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "เหมาคีบ";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 12).ColumnSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "เหมาบรรทุก";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 14).ColumnSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "เหมารวม";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 16).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "สถานะเรียกเก็บค่าบรรทุก";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 17).RowSpan = 2;
            this.sp1_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "สถานะการตัด";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 1).Value = "เลขที่แปลง";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 2).Value = "ชื่อแปลง";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 7).Value = "ผู้รับเหมา";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 8).Value = "เลขรถตัด";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 9).Value = "ค่าตัด";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 10).Value = "ผู้รับเหมา";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 11).Value = "ค่าคีบ";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 12).Value = "ผู้รับเหมา";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 13).Value = "ค่าบรรทุก";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 14).Value = "ผู้รับเหมา";
            this.sp1_Sheet1.ColumnHeader.Cells.Get(1, 15).Value = "ค่ารับเหมา";
            this.sp1_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.sp1_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.sp1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(0).Locked = true;
            this.sp1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(0).Width = 79F;
            this.sp1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(1).Label = "เลขที่แปลง";
            this.sp1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(1).Width = 72F;
            this.sp1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(2).Label = "ชื่อแปลง";
            this.sp1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(2).Width = 124F;
            this.sp1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.sp1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(3).Width = 170F;
            this.sp1_Sheet1.Columns.Get(4).Width = 132F;
            this.sp1_Sheet1.Columns.Get(5).CellType = textCellType2;
            this.sp1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(5).Locked = true;
            this.sp1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(5).Width = 100F;
            this.sp1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(6).Width = 100F;
            this.sp1_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(7).Label = "ผู้รับเหมา";
            this.sp1_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(7).Width = 100F;
            this.sp1_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(8).Label = "เลขรถตัด";
            this.sp1_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(8).Width = 100F;
            this.sp1_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(9).Label = "ค่าตัด";
            this.sp1_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(9).Width = 100F;
            this.sp1_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(10).Label = "ผู้รับเหมา";
            this.sp1_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(10).Width = 100F;
            this.sp1_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(11).Label = "ค่าคีบ";
            this.sp1_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(11).Width = 100F;
            this.sp1_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(12).Label = "ผู้รับเหมา";
            this.sp1_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(12).Width = 100F;
            this.sp1_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(13).Label = "ค่าบรรทุก";
            this.sp1_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(13).Width = 100F;
            this.sp1_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(14).Label = "ผู้รับเหมา";
            this.sp1_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(14).Width = 100F;
            this.sp1_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(15).Label = "ค่ารับเหมา";
            this.sp1_Sheet1.Columns.Get(15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(15).Width = 100F;
            this.sp1_Sheet1.Columns.Get(16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(16).Width = 84F;
            this.sp1_Sheet1.Columns.Get(17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(17).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.sp1_Sheet1.Columns.Get(17).Width = 109F;
            this.sp1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.sp1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.sp1_Sheet1.RowHeader.Columns.Get(0).Width = 21F;
            this.sp1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(14, 559);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 24);
            this.label1.TabIndex = 57;
            this.label1.Text = "* Double Click เพื่อเลือกรายการที่ต้องการ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 31);
            this.label2.TabIndex = 58;
            this.label2.Text = "ทะเบียนรถ : ";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmb_select);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.sp1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1492, 625);
            this.panel1.TabIndex = 59;
            // 
            // cmb_select
            // 
            this.cmb_select.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.cmb_select.FormattingEnabled = true;
            this.cmb_select.Items.AddRange(new object[] {
            "50 รายการ",
            "100 รายการ",
            "ทั้งหมด"});
            this.cmb_select.Location = new System.Drawing.Point(1344, 577);
            this.cmb_select.Name = "cmb_select";
            this.cmb_select.Size = new System.Drawing.Size(121, 26);
            this.cmb_select.TabIndex = 58;
            this.cmb_select.SelectedIndexChanged += new System.EventHandler(this.cmb_select_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(1234, 577);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 24);
            this.label3.TabIndex = 57;
            this.label3.Text = "* เลือกข้อมูล: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(394, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 31);
            this.label4.TabIndex = 58;
            this.label4.Text = "เลขใบนำตัด : ";
            // 
            // txtSearchDoc
            // 
            this.txtSearchDoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchDoc.Location = new System.Drawing.Point(546, 12);
            this.txtSearchDoc.Name = "txtSearchDoc";
            this.txtSearchDoc.Size = new System.Drawing.Size(220, 38);
            this.txtSearchDoc.TabIndex = 0;
            this.txtSearchDoc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchDoc_KeyDown);
            // 
            // frmSearchCar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1492, 681);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtSearchDoc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSearchCar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ค้นหาทะเบียนรถ";
            this.Load += new System.EventHandler(this.frmSearchCar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sp1_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.BindingSource bindingSource1;
        private FarPoint.Win.Spread.FpSpread sp1;
        private FarPoint.Win.Spread.SheetView sp1_Sheet1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cmb_select;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSearchDoc;
    }
}