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
    public partial class frmCarBrowse : Form
    {
        public frmCarBrowse()
        {
            InitializeComponent();
        }

        private void frmCarBrowse_Load(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        public void LoadData(string lvSQL)
        {
            //ค้นหาข้อมูล
            DataTable DT = new DataTable();

            if (lvSQL == "")
            {
                lvSQL = "Select * from Queue_Diary Where ";
                lvSQL += "(Q_WeightIN = 0 OR Q_WeightOUT = 0) ";
                lvSQL += "And Q_CloseStatus = '' ";
            }
            else
            {
                lvSQL = "Select * from Queue_Diary Where Q_CarNum like '%" + txtSearch.Text + "%' ";
                lvSQL += "And (Q_WeightIN = 0 OR Q_WeightOUT = 0) ";
                lvSQL += "And Q_CloseStatus = '' ";
            }

            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumRow = DT.Rows.Count;
            sp1.ActiveSheet.RowCount = 0;
            sp1.ActiveSheet.RowCount = lvNumRow;

            for (int i = 0; i < sp1.ActiveSheet.RowCount; i++)
            {
                sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["Q_No"].ToString();
                sp1.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["Q_CarNum"].ToString();
            }
        }

        private void sp1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GVar.gvSCode = sp1.ActiveSheet.Cells[e.Row, 0].Text;

            this.Close();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(txtSearch.Text != "")
                {
                    LoadData(txtSearch.Text);
                }
                else
                {
                    LoadData("");
                }
               
            }
        }
    }
}
