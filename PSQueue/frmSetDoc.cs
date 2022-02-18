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
    public partial class frmSetDoc : Form
    {
        public frmSetDoc()
        {
            InitializeComponent();
        }

        private void frmSetDoc_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "select * ";
            lvSQL += "from SysDocNo ";
            lvSQL += "where S_MCode in ('Queue_01','Queue_02','Queue_03','Weight_Bill','Weight_RailA','Weight_RailB') ";      

            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumRow = DT.Rows.Count;
            sp1.ActiveSheet.RowCount = 0;
            sp1.ActiveSheet.RowCount = lvNumRow;

            for (int i = 0; i < lvNumRow; i++)
            {
                sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["S_Remark"].ToString();
                sp1.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["S_RunNo"].ToString();
                sp1.ActiveSheet.Cells[i, 2].Text = DT.Rows[i]["S_RunNo"].ToString();
                sp1.ActiveSheet.Cells[i, 3].Text = DT.Rows[i]["S_MCode"].ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //ยืนยัน
            string lvTxtAlert = "ยืนยันการบันทึกข้อมูล ?";
            string lvSQL = "";
            string lvResault = "";
            DialogResult dialogResult = MessageBox.Show(lvTxtAlert, "Confirm?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                this.Cursor = Cursors.Default;
                return;
            }

            for (int i = 0; i < sp1.ActiveSheet.RowCount; i++)
            {
                string lvNo = sp1.ActiveSheet.Cells[i, 1].Text;
                string lvNoOld = sp1.ActiveSheet.Cells[i, 2].Text;
                string lvMCode = sp1.ActiveSheet.Cells[i, 3].Text;

                if (lvNoOld != lvNo)
                {
                    //อัพเดทเลข
                    lvSQL = "update SysDocNo set S_RunNo = '"+ lvNo + "' where S_MCode = '" + lvMCode + "' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                }
            }

            ////อัพตะกาว
            //lvSQL = "update Queue_Diary set Q_TKNoCheck = '1' where Q_TKNo <> '' ";
            //lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

            MessageBox.Show("บันทึกข้อมูลเรียบร้อย", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }
    }
}
