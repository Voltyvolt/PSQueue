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
    public partial class frmSend : Form
    {
        public frmSend()
        {
            InitializeComponent();
        }

        private void frmSend_Load(object sender, EventArgs e)
        {
            LoadData();

            sp2.ActiveSheet.RowCount = 0;
        }

        private void LoadData()
        {
            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "";
            lvSQL += "Select * from Queue_Diary Where 1=1 ";
            lvSQL += "Order by Q_No Desc";

            DT = GsysSQL.fncGetQueryData(lvSQL, DT, false);

            int lvNumRow = DT.Rows.Count;
            sp1.ActiveSheet.RowCount = 0;
            sp1.ActiveSheet.RowCount = lvNumRow;

            //Progressbar
            progressBar1.Visible = true;
            progressBar1.Maximum = lvNumRow;
            progressBar1.Value = 0;

            for (int i = 0; i < lvNumRow; i++)
            {
                sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["Q_No"].ToString();
                sp1.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["Q_Rail"].ToString();
                sp1.ActiveSheet.Cells[i, 2].Text = DT.Rows[i]["Q_Quota"].ToString();
                sp1.ActiveSheet.Cells[i, 3].Text = DT.Rows[i]["Q_CarNum"].ToString();
                sp1.ActiveSheet.Cells[i, 4].Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_Date"].ToString());
                sp1.ActiveSheet.Cells[i, 5].Text = DT.Rows[i]["Q_Time"].ToString();
                sp1.ActiveSheet.Cells[i, 6].Text = DT.Rows[i]["Q_CarType"].ToString();

                string lvCaneID = DT.Rows[i]["Q_CaneType"].ToString();
                string lvCaneName = GsysSQL.fncFindCaneTypeName(lvCaneID, GVar.gvOnline);

                if (lvCaneName != "")
                    sp1.ActiveSheet.Cells[i, 7].Text = lvCaneID + " : " + lvCaneName;
                else
                    sp1.ActiveSheet.Cells[i, 7].Text = "";

                progressBar1.Value += 1;
                Application.DoEvents();
            }

            progressBar1.Visible = false;

            DT.Dispose();

        }

        private void SendData()
        {
            //Progressbar
            progressBar1.Visible = true;
            progressBar1.Maximum = sp1.ActiveSheet.RowCount;
            progressBar1.Value = 0;

            for (int i = 0; i < sp1.ActiveSheet.RowCount; i++)
            {


                ////บันทึกข้อมูล
                //string lvSQL = "Insert into Queue_Diary (Q_No, Q_Rail, Q_CarNum, Q_CutDoc, Q_Quota, Q_CaneDoc, Q_CaneType, Q_CutCar, Q_MainCar, Q_Date, Q_Time, Q_CarType,Q_CutPrice,Q_CarryPrice,Q_Station, Q_User) ";
                //lvSQL += "Values ('" + lvQ + "', '" + lvRail + "', '" + lvCarNum + "', '" + lvCutDoc + "', '" + lvQuota + "', '" + lvCaneDoc + "', '" + lvCaneType + "', '" + lvCutCar + "', '" + lvMainCar + "', '" + lvDate + "', '" + lvTime + "', '" + lvCarType + "', '" + lvCutPrice + "', '" + lvCarryPrice + "', '" + lvStation + "', '" + lvUser + "')";

                progressBar1.Value += 1;
                Application.DoEvents();
            }
        }
    }
}
