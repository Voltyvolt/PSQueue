using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;

namespace PSQueue
{
    public partial class rptDiaryReport : DevExpress.XtraReports.UI.XtraReport
    {
        public rptDiaryReport()
        {
            InitializeComponent();
        }

        private void rptDiaryReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            fncGetSubWeight();
        }

        public void fncGetSubWeight()
        {
            
            string lvDateS = "20201214";
            string lvDateE = Gstr.fncChangeTDate(DateTime.Now.ToString("dd/MM/yyyy"));
            //string lvDateE = "20201219";
            double lvSubWeight = 0;

            if(GVar.gvBillE == "")
            {
                GVar.gvBillE = "100000";
            }

            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "select * ";
            lvSQL += "from Queue_Diary ";
            lvSQL += "where Q_Status = 'Active' ";
            lvSQL += "and Q_WeightOUTDate >= '" + lvDateS + "' and Q_WeightOUTDate <= '" + lvDateE + "' ";

            lvSQL += "and Q_Year = '' And Cast(Q_BillingNo as int) >= '1' And Cast(Q_BillingNo as int) <= '" + GVar.gvBillE + "' ";
            lvSQL += "Order by Q_BillingNo DESC ";

            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                double lvNet = (Gstr.fncToDouble(FncChangeKiloToTon(DT.Rows[i]["Q_WeightIN"].ToString())) - Gstr.fncToDouble(FncChangeKiloToTon(DT.Rows[i]["Q_WeightOUT"].ToString())));
                lvSubWeight += lvNet;
            }


            xrLabel39.Text = lvSubWeight.ToString("#,##0.000");
        }

        private string FncChangeKiloToTon(string lvWeight)
        {
            string lvReturn = "";

            lvReturn = (Gstr.fncToDouble(lvWeight) / 1000).ToString("#,##0.000");

            return lvReturn;
        }

        private void rptDiaryReport_AfterPrint(object sender, EventArgs e)
        {
            
        }
    }
}
