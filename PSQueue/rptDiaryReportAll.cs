using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace PSQueue
{
    public partial class rptDiaryReportAll : DevExpress.XtraReports.UI.XtraReport
    {
        public rptDiaryReportAll()
        {
            InitializeComponent();

            //xrSubreport1.ReportSourceUrl = PSQueue.rptDiaryReport;
        }

    }
}
