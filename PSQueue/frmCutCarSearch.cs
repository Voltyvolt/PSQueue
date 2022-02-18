using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSQueue
{
    public partial class frmCutCarSearch : Form
    {
        public frmCutCarSearch()
        {
            InitializeComponent();
        }

        private void frmCutCarSearch_Load(object sender, EventArgs e)
        {
            DataTable DT = new DataTable();
            var lvSQL = "Select * From Cane_NoCar";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumrow = DT.Rows.Count;

            sp1.ActiveSheet.Rows.Count = lvNumrow;

            for (int i = 0; i < lvNumrow; i++)
            {
                sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["N_NO"].ToString();
                sp1.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["N_Type"].ToString();
            }
        }

        private void sp1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GVar.gvCutcarNo = sp1.ActiveSheet.Cells[e.Row, 0].Text;

            this.Close();
        }
    }
}
