using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSQueue
{
    public partial class frmPrint : Form
    {
        public frmPrint()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string lvPrinterSelect = "";
                bool lvChkPrinterStatus = false;

                string lvPrinter = "";

                if (documentViewer1.DocumentSource.ToString() == "PSQueue.rptQueue")
                {
                    lvPrinter = "XP-80";

                    //เช็ค Printer
                    foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                    {
                        if (printer.IndexOf(lvPrinter) != -1)
                        {
                            lvChkPrinterStatus = FncCheckPrinterStatus(printer);
                            if (lvChkPrinterStatus)
                            {
                                lvPrinterSelect = printer;
                                break;
                            }
                        }
                    }

                    rptQueue Report = new rptQueue();
                    Report.PrinterName = lvPrinterSelect;
                    Report.PrintingSystem.ShowMarginsWarning = false;
                    Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                    Report.CreateDocument();

                    using (ReportPrintTool printTool = new ReportPrintTool(Report))
                    {
                        printTool.PrinterSettings.PrinterName = lvPrinterSelect;
                        printTool.Print();

                        printTool.Dispose();
                    }

                    Report.Dispose();
                }
                else if (documentViewer1.DocumentSource.ToString() == "PSQueue.rptQueue3")
                {
                    lvPrinter = "XP-80";

                    //เช็ค Printer
                    foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                    {
                        if (printer.IndexOf(lvPrinter) != -1)
                        {
                            lvChkPrinterStatus = FncCheckPrinterStatus(printer);
                            if (lvChkPrinterStatus)
                            {
                                lvPrinterSelect = printer;
                                break;
                            }
                        }
                    }

                    rptQueue3 Report = new rptQueue3();
                    Report.PrinterName = lvPrinterSelect;
                    Report.PrintingSystem.ShowMarginsWarning = false;
                    Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                    Report.CreateDocument();

                    using (ReportPrintTool printTool = new ReportPrintTool(Report))
                    {
                        printTool.PrinterSettings.PrinterName = lvPrinterSelect;
                        printTool.Print();

                        printTool.Dispose();
                    }

                    Report.Dispose();
                }
                else if (documentViewer1.DocumentSource.ToString() == "PSQueue.rptDiaryReportAll")
                {
                    //lvPrinter = "XP-80";

                    rptDiaryReportAll Report = new rptDiaryReportAll();
                    //Report.PrinterName = lvPrinter;
                    Report.PrintingSystem.ShowMarginsWarning = false;
                    Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                    Report.CreateDocument();

                    using (ReportPrintTool printTool = new ReportPrintTool(Report))
                    {
                        printTool.PrinterSettings.PrinterName = lvPrinter;
                        printTool.PrintDialog();

                        printTool.Dispose();
                    }

                    Report.Dispose();
                }
                else if (documentViewer1.DocumentSource.ToString() == "PSQueue.rptReportHour")
                {
                    //lvPrinter = "XP-80";

                    rptReportHour Report = new rptReportHour();
                    //Report.PrinterName = lvPrinter;
                    Report.PrintingSystem.ShowMarginsWarning = false;
                    Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                    Report.CreateDocument();

                    using (ReportPrintTool printTool = new ReportPrintTool(Report))
                    {
                        printTool.PrinterSettings.PrinterName = lvPrinter;
                        printTool.PrintDialog();

                        printTool.Dispose();
                    }

                    Report.Dispose();
                }
                else if (documentViewer1.DocumentSource.ToString() == "PSQueue.rptReportHourSum")
                {
                    //lvPrinter = "XP-80";

                    rptReportHourSum Report = new rptReportHourSum();
                    //Report.PrinterName = lvPrinter;
                    Report.PrintingSystem.ShowMarginsWarning = false;
                    Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                    Report.CreateDocument();

                    using (ReportPrintTool printTool = new ReportPrintTool(Report))
                    {
                        printTool.PrinterSettings.PrinterName = lvPrinter;
                        printTool.PrintDialog();

                        printTool.Dispose();
                    }

                    Report.Dispose();
                }
                else if (documentViewer1.DocumentSource.ToString() == "PSQueue.rptSumHour")
                {
                    //lvPrinter = "XP-80";

                    rptSumHour Report = new rptSumHour();
                    //Report.PrinterName = lvPrinter;
                    Report.PrintingSystem.ShowMarginsWarning = false;
                    Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                    Report.CreateDocument();

                    using (ReportPrintTool printTool = new ReportPrintTool(Report))
                    {
                        printTool.PrinterSettings.PrinterName = lvPrinter;
                        printTool.PrintDialog();

                        printTool.Dispose();
                    }

                    Report.Dispose();
                }
                else if (documentViewer1.DocumentSource.ToString() == "PSQueue.rptSlip")
                {
                    lvPrinter = "LQ-310";

                    //เช็ค Printer
                    foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                    {
                        if (printer.IndexOf(lvPrinter) != -1)
                        {
                            lvChkPrinterStatus = FncCheckPrinterStatus(printer);
                            if (lvChkPrinterStatus)
                            {
                                lvPrinterSelect = printer;
                                break;
                            }
                        }
                    }

                    rptSlip Report = new rptSlip();
                    Report.PrinterName = lvPrinterSelect;
                    Report.PrintingSystem.ShowMarginsWarning = false;
                    Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                    Report.CreateDocument();

                    using (ReportPrintTool printTool = new ReportPrintTool(Report))
                    {
                        printTool.PrinterSettings.PrinterName = lvPrinterSelect;
                        printTool.Print();

                        printTool.Dispose();
                    }

                    Report.Dispose();
                }
                else if (documentViewer1.DocumentSource.ToString() == "PSQueue.rptCutCar")
                {

                    rptCutCar Report = new rptCutCar();
                    Report.PrintingSystem.ShowMarginsWarning = false;
                    Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                    Report.CreateDocument();

                    using (ReportPrintTool printTool = new ReportPrintTool(Report))
                    {
                        printTool.PrintDialog();

                        printTool.Dispose();
                    }

                    Report.Dispose();
                }
                else if (documentViewer1.DocumentSource.ToString() == "PSQueue.rptReportSumTypeCane")
                {

                    rptReportSumTypeCane Report = new rptReportSumTypeCane();
                    Report.PrintingSystem.ShowMarginsWarning = false;
                    Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                    Report.CreateDocument();

                    using (ReportPrintTool printTool = new ReportPrintTool(Report))
                    {
                        printTool.PrintDialog();

                        printTool.Dispose();
                    }

                    Report.Dispose();
                }

                else if (documentViewer1.DocumentSource.ToString() == "PSQueue.rptHourA")
                {

                    rptHourA Report = new rptHourA();
                    Report.PrintingSystem.ShowMarginsWarning = false;
                    Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                    Report.CreateDocument();

                    using (ReportPrintTool printTool = new ReportPrintTool(Report))
                    {
                        printTool.PrintDialog();

                        printTool.Dispose();
                    }

                    Report.Dispose();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ไม่พบเครื่องพิมพ์ หรือ เครื่องพิมพ์อยู่ในสถานะ OFFLINE กรุณาตรวจสอบ !!" + Environment.NewLine + ex.Message, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }
        }

        private bool FncCheckPrinterStatus(string lvPrinter)
        {
            bool lvReturn = false;

            // Set management scope
            ManagementScope scope = new ManagementScope(@"\root\cimv2");
            scope.Connect();

            // Select Printers from WMI Object Collections
            ManagementObjectSearcher searcher = new
             ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            string printerName = "";
            foreach (ManagementObject printer in searcher.Get())
            {
                printerName = printer["Name"].ToString().ToLower();
                if (printerName.Equals(lvPrinter.ToLower()))
                {
                    //Console.WriteLine("Printer = " + printer["Name"]);
                    if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                    {
                        // printer is offline by user
                        lvReturn = false;
                        break;
                        //Console.WriteLine("Your Plug-N-Play printer is not connected.");
                    }
                    else
                    {
                        // printer is not offline
                        lvReturn = true;
                        break;
                        //Console.WriteLine("Your Plug-N-Play printer is connected.");
                    }
                }
            }


            return lvReturn;
        }

        private void frmPrint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnPrint_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void frmPrint_Load(object sender, EventArgs e)
        {

        }
    }
}
