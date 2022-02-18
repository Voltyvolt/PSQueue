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
    public partial class frmQuotaSearch : Form
    {
        public frmQuotaSearch()
        {
            InitializeComponent();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }

        public void LoadData()
        {
            //ค้นหาข้อมูล
            DataTable DT = new DataTable();
            
            string lvSQL = "Select Q_ID, Q_Prefix + ' ' + Q_FirstName + ' ' + Q_LastName as Q_Name from Cane_Quota Where Q_FirstName like '%" + txtSearch.Text + "%' OR Q_LastName like '%" + txtSearch.Text + "%' OR Q_ID like '" + txtSearch.Text + "' ";

            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumRow = DT.Rows.Count;
            sp1.ActiveSheet.RowCount = 0;
            sp1.ActiveSheet.RowCount = lvNumRow;

            for (int i = 0; i < sp1.ActiveSheet.RowCount; i++)
            {
                sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["Q_ID"].ToString();
                sp1.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["Q_Name"].ToString();
            }
        }

        private void sp1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GVar.gvSCode = sp1.ActiveSheet.Cells[e.Row, 0].Text;
            GVar.gvSName = sp1.ActiveSheet.Cells[e.Row, 1].Text;

            this.Close();
        }

        private void frmQuotaSearch_Load(object sender, EventArgs e)
        {
            LoadData();
            txtSearch.Focus();
        }
    }
}
