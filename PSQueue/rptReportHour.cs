using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace PSQueue
{
    public partial class rptReportHour : DevExpress.XtraReports.UI.XtraReport
    {
        public rptReportHour()
        {
            InitializeComponent();
        }

        private string fncChangeZero(string lvTXT)
        {
            string lvReturn = "";

            if (lvTXT == "0.00")
                lvTXT = "";

            lvReturn = lvTXT;
            return lvReturn;
        }

        private void xrLabel20_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel20.Text = fncChangeZero(xrLabel20.Text);
        }

        private void xrLabel21_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel21.Text = fncChangeZero(xrLabel21.Text);
        }

        private void xrLabel22_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel22.Text = fncChangeZero(xrLabel22.Text);
        }

        private void xrLabel23_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel23.Text = fncChangeZero(xrLabel23.Text);
        }

        private void xrLabel25_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel25.Text = fncChangeZero(xrLabel25.Text);
        }

        private void xrLabel24_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrLabel24.Text = fncChangeZero(xrLabel24.Text);
        }
    }
}
