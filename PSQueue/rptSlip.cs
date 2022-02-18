using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace PSQueue
{
    public partial class rptSlip : DevExpress.XtraReports.UI.XtraReport
    {
        public rptSlip()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GVar.gvCaneType == "1")
            {
                xrPictureBox1.Visible = true;
                xrPictureBox2.Visible = false;
            }
            else if (GVar.gvCaneType == "2")
            {
                xrPictureBox1.Visible = false;
                xrPictureBox2.Visible = true;
            }
            else
            {
                xrPictureBox1.Visible = false;
                xrPictureBox2.Visible = false;
            }

            if (GVar.gvCarryPriceStatus)
            {
                xrLabel4.Visible = true;
            }
            else
            {
                xrLabel4.Visible = false;
            }

            string lvCaneApproveChk = fncGetCaneApproveStatus(xrLabel22.Text);
            if(lvCaneApproveChk == "1")
            {
                xrLabel30.Visible = true;
            }
            else
            {
                xrLabel30.Visible = false;
            }
        }

        public string fncGetCaneApproveStatus(string lvQ)
        {
            string lvReturn = "";

            DataTable DT = new DataTable();
            string lvSQL = "Select Q_CaneApprove From Queue_Diary Where Q_No = '" + GVar.gvQNo + "' And Q_Year = '' ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumRow = DT.Rows.Count;

            for (int i = 0; i < lvNumRow; i++)
            {
                lvReturn = DT.Rows[i]["Q_CaneApprove"].ToString();
            }

            return lvReturn;
        }
    }
}
