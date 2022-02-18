using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSQueue
{
    public partial class frmShowReport : Form
    {
        public frmShowReport()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            pnStatus.Visible = true;
            if (ChkShowCutCar.Checked)
            {
                LoadDataCutCar();
            }
            else
            {
                #region ข้อมูลรายชั่วโมง
                string[] lvKa1 = new string[] { "07.01-08.00", "08.01-09.00", "09.01-10.00", "10.01-11.00", "11.01-12.00", "12.01-13.00", "13.01-14.00", "14.01-15.00", "15.01-16.00", "16.01-17.00", "17.01-18.00", "18.01-19.00" };
                string[] lvKa2 = new string[] { "19.01-20.00", "20.01-21.00", "21.01-22.00", "22.01-23.00", "23.01-00.00", "00.01-01.00", "01.01-02.00", "02.01-03.00", "03.01-04.00", "04.01-05.00", "05.01-06.00", "06.01-07.00" };
                string[] lvKaALL = new string[] { "15.01-16.00", "16.01-17.00", "17.01-18.00", "18.01-19.00", "19.01-20.00", "20.01-21.00", "21.01-22.00", "22.01-23.00", "23.01-00.00", "00.01-01.00", "01.01-02.00", "02.01-03.00", "03.01-04.00", "04.01-05.00", "05.01-06.00", "06.01-07.00", "07.01-08.00", "08.01-09.00", "09.01-10.00", "10.01-11.00", "11.01-12.00", "12.01-13.00", "13.01-14.00", "14.01-15.00" };
                //string[] lvKaALL = new string[] { "00.01-01.00", "01.01-02.00", "02.01-03.00", "03.01-04.00", "04.01-05.00", "05.01-06.00", "06.01-07.00", "07.01-08.00", "08.01-09.00", "09.01-10.00", "10.01-11.00", "11.01-12.00", "12.01-13.00", "13.01-14.00", "14.01-15.00", "15.01-16.00", "16.01-17.00", "17.01-18.00", "18.01-19.00", "19.01-20.00", "20.01-21.00", "21.01-22.00", "22.01-23.00", "23.01-00.00" };
                string[] lvKa;

                string lvDateS = Gstr.fncChangeTDate(txtDateS.Text);
                string lvDateE = lvDateS;

                //string lvDateE = Gstr.fncChangeTDate(txtDateE.Text);
                string lvTimeS = txtTimeS.Text;
                string lvTimeE = txtTimeE.Text;

                string lvBillS = txtBillS.Text;
                GVar.gvBillS = lvBillS;
                string lvBillE = txtBillE.Text;
                GVar.gvBillE = lvBillE;

                string lvRail = "";
                //if (ChkA.Checked) lvRail = "A";
                //else if (ChkB.Checked) lvRail = "B";

                sp1.Sheets[0].RowCount = 0;
                sp1.Sheets[1].RowCount = 0;
                sp1.Sheets[2].RowCount = 0;

                if (rdKa1.Checked)
                    lvKa = lvKa1;
                else if (rdKaAll.Checked)
                    lvKa = lvKaALL;
                else
                    lvKa = lvKa2;

                lvRail = "A";

                string lvShowClose = "";
                if (chkShowClose.Checked)
                {
                    lvShowClose = "1";
                }

                double lvTotalCount1 = 0;
                double lvTotalCount2 = 0;
                double lvTotalSum1 = 0;
                double lvTotalSum2 = 0;

                ////Progressbar
                //pnStatus.Visible = true;

                //RailA
                if (ChkA.Checked || ChkAll.Checked)
                {
                    for (int i = 0; i < lvKa.Count(); i++)
                    {
                        sp1.Sheets[0].RowCount += 1;
                        sp1.Sheets[0].Cells[i, 0].Text = lvKa[i];

                        //ภ้าข้ามวัน
                        if (lvKa[i] == "23.01-00.00")
                        {
                            DateTime DT = DateTime.Parse(txtDateS.Text + " 00:00:00");
                            DT = DT.AddDays(1);

                            lvDateE = Gstr.fncChangeTDate(DT.ToString("dd/MM/yyyy"));
                        }
                        else if (lvKa[i] == "00.01-01.00" || lvKa[i] == "01.01-02.00" || lvKa[i] == "02.01-03.00" || lvKa[i] == "03.01-04.00" || lvKa[i] == "04.01-05.00" || lvKa[i] == "05.01-06.00" || lvKa[i] == "06.01-07.00")
                        {
                            DateTime DT = DateTime.Parse(txtDateS.Text + " 00:00:00");
                            DT = DT.AddDays(1);

                            lvDateS = Gstr.fncChangeTDate(DT.ToString("dd/MM/yyyy"));
                            lvDateE = lvDateS;
                        }

                        string[] lvArr = lvKa[i].Split('-');
                        lvTimeS = FncChangeFormatTime(lvArr[0], false);
                        lvTimeE = FncChangeFormatTime(lvArr[1], true);

                        if (lvTimeS == "11:01:00")
                        {

                        }

                        //อ้อยสด + อ้อยสด C + อ้อยสดยอดยาวปนเปื้อน
                        GsysSQL.fncGetDataCane(lvRail, "1','3','7", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[0].Cells[i, 1].Text = GVar.gvDataCount;
                        sp1.Sheets[0].Cells[i, 2].Text = GVar.gvDataSum;

                        //อ้อยสด C โหลดเก็บไว้แสดงรายงาน
                        GsysSQL.fncGetDataCane(lvRail, "3", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[0].Cells[i, 23].Text = GVar.gvDataCount;
                        sp1.Sheets[0].Cells[i, 24].Text = GVar.gvDataSum;

                        //อ้อยสดยอดยาวปนเปื้อนโหลดเก็บไว้แสดงรายงาน
                        GsysSQL.fncGetDataCane(lvRail, "7", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[0].Cells[i, 25].Text = GVar.gvDataCount;
                        sp1.Sheets[0].Cells[i, 26].Text = GVar.gvDataSum;

                        //สด-สะอาด
                        GsysSQL.fncGetDataCane(lvRail, "17", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[0].Cells[i, 3].Text = GVar.gvDataCount;
                        sp1.Sheets[0].Cells[i, 4].Text = GVar.gvDataSum;

                        //สดรถตัดใน
                        GsysSQL.fncGetDataCane(lvRail, "11", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[0].Cells[i, 5].Text = GVar.gvDataCount;
                        sp1.Sheets[0].Cells[i, 6].Text = GVar.gvDataSum;

                        //สดรถตัดนอก
                        GsysSQL.fncGetDataCane(lvRail, "5", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[0].Cells[i, 7].Text = GVar.gvDataCount;
                        sp1.Sheets[0].Cells[i, 8].Text = GVar.gvDataSum;

                        //สดไฟไหม้
                        GsysSQL.fncGetDataCane(lvRail, "2", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[0].Cells[i, 11].Text = GVar.gvDataCount;
                        sp1.Sheets[0].Cells[i, 12].Text = GVar.gvDataSum;

                        //สดไฟไหม้ - สะอาด
                        GsysSQL.fncGetDataCane(lvRail, "18", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[0].Cells[i, 13].Text = GVar.gvDataCount;
                        sp1.Sheets[0].Cells[i, 14].Text = GVar.gvDataSum;

                        //สดไฟไหม้ - รถตัดใน
                        GsysSQL.fncGetDataCane(lvRail, "12", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[0].Cells[i, 15].Text = GVar.gvDataCount;
                        sp1.Sheets[0].Cells[i, 16].Text = GVar.gvDataSum;

                        //สดไฟไหม้ - รถตัดนอก
                        GsysSQL.fncGetDataCane(lvRail, "6", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[0].Cells[i, 17].Text = GVar.gvDataCount;
                        sp1.Sheets[0].Cells[i, 18].Text = GVar.gvDataSum;

                        //ถ้าเป็น 0 ให้แสดงช่องว่าง
                        int lvSheetIndex = 0;
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 1].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 1].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 2].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 2].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 3].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 3].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 4].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 4].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 5].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 5].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 6].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 6].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 7].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 7].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 8].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 8].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 11].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 11].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 12].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 12].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 13].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 13].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 14].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 14].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 15].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 15].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 16].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 16].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 17].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 17].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 18].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 18].Text = "";

                        Application.DoEvents();
                    }

                    //สรุปยอด Sum
                    FncSumData(0);
                }

                //RailB
                lvRail = "B";
                lvDateS = Gstr.fncChangeTDate(txtDateS.Text);
                lvDateE = lvDateS;
                if (ChkB.Checked || ChkAll.Checked)
                {
                    for (int i = 0; i < lvKa.Count(); i++)
                    {
                        sp1.Sheets[1].RowCount += 1;
                        sp1.Sheets[1].Cells[i, 0].Text = lvKa[i];

                        //ภ้าข้ามวัน
                        if (lvKa[i] == "23.01-00.00")
                        {
                            DateTime DT = DateTime.Parse(txtDateS.Text + " 00:00:00");
                            DT = DT.AddDays(1);

                            lvDateE = Gstr.fncChangeTDate(DT.ToString("dd/MM/yyyy"));
                            //lvDateE = DT.ToString("yyyyMMdd");
                        }
                        else if (lvKa[i] == "00.01-01.00" || lvKa[i] == "01.01-02.00" || lvKa[i] == "02.01-03.00" || lvKa[i] == "03.01-04.00" || lvKa[i] == "04.01-05.00" || lvKa[i] == "05.01-06.00" || lvKa[i] == "06.01-07.00")
                        {
                            DateTime DT = DateTime.Parse(txtDateS.Text + " 00:00:00");
                            DT = DT.AddDays(1);

                            lvDateS = Gstr.fncChangeTDate(DT.ToString("dd/MM/yyyy"));
                            lvDateE = lvDateS;
                        }

                        string[] lvArr = lvKa[i].Split('-');
                        lvTimeS = FncChangeFormatTime(lvArr[0], false);
                        lvTimeE = FncChangeFormatTime(lvArr[1], true);

                        //อ้อยสด + อ้อยสด C + อ้อยสดยอดยาวปนเปื้อน
                        GsysSQL.fncGetDataCane(lvRail, "1','3','7", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[1].Cells[i, 1].Text = GVar.gvDataCount;
                        sp1.Sheets[1].Cells[i, 2].Text = GVar.gvDataSum;

                        //อ้อยสด C โหลดเก็บไว้แสดงรายงาน
                        GsysSQL.fncGetDataCane(lvRail, "3", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[1].Cells[i, 23].Text = GVar.gvDataCount;
                        sp1.Sheets[1].Cells[i, 24].Text = GVar.gvDataSum;

                        //อ้อยสดยอดยาวปนเปื้อนโหลดเก็บไว้แสดงรายงาน
                        GsysSQL.fncGetDataCane(lvRail, "7", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[1].Cells[i, 25].Text = GVar.gvDataCount;
                        sp1.Sheets[1].Cells[i, 26].Text = GVar.gvDataSum;

                        //สด-สะอาด
                        GsysSQL.fncGetDataCane(lvRail, "17", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[1].Cells[i, 3].Text = GVar.gvDataCount;
                        sp1.Sheets[1].Cells[i, 4].Text = GVar.gvDataSum;

                        //สดรถตัดใน
                        GsysSQL.fncGetDataCane(lvRail, "11", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[1].Cells[i, 5].Text = GVar.gvDataCount;
                        sp1.Sheets[1].Cells[i, 6].Text = GVar.gvDataSum;

                        //สดรถตัดนอก
                        GsysSQL.fncGetDataCane(lvRail, "5", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[1].Cells[i, 7].Text = GVar.gvDataCount;
                        sp1.Sheets[1].Cells[i, 8].Text = GVar.gvDataSum;

                        //สดไฟไหม้
                        GsysSQL.fncGetDataCane(lvRail, "2", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[1].Cells[i, 11].Text = GVar.gvDataCount;
                        sp1.Sheets[1].Cells[i, 12].Text = GVar.gvDataSum;

                        //สดไฟไหม้ - สะอาด
                        GsysSQL.fncGetDataCane(lvRail, "18", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[1].Cells[i, 13].Text = GVar.gvDataCount;
                        sp1.Sheets[1].Cells[i, 14].Text = GVar.gvDataSum;

                        //สดไฟไหม้ - รถตัดใน
                        GsysSQL.fncGetDataCane(lvRail, "12", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[1].Cells[i, 15].Text = GVar.gvDataCount;
                        sp1.Sheets[1].Cells[i, 16].Text = GVar.gvDataSum;

                        //สดไฟไหม้ - รถตัดนอก
                        GsysSQL.fncGetDataCane(lvRail, "6", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[1].Cells[i, 17].Text = GVar.gvDataCount;
                        sp1.Sheets[1].Cells[i, 18].Text = GVar.gvDataSum;

                        //ถ้าเป็น 0 ให้แสดงช่องว่าง
                        int lvSheetIndex = 1;
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 1].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 1].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 2].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 2].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 3].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 3].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 4].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 4].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 5].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 5].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 6].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 6].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 7].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 7].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 8].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 8].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 11].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 11].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 12].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 12].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 13].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 13].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 14].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 14].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 15].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 15].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 16].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 16].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 17].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 17].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 18].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 18].Text = "";

                        Application.DoEvents();
                    }

                    //สรุปยอด Sum
                    FncSumData(1);
                }

                //RailALL
                lvRail = "";
                lvDateS = Gstr.fncChangeTDate(txtDateS.Text);
                lvDateE = lvDateS;
                if (ChkAll.Checked)
                {
                    for (int i = 0; i < lvKa.Count(); i++)
                    {
                        sp1.Sheets[2].RowCount += 1;
                        sp1.Sheets[2].Cells[i, 0].Text = lvKa[i];

                        //ภ้าข้ามวัน
                        if (lvKa[i] == "23.01-00.00")
                        {
                            DateTime DT = DateTime.Parse(txtDateS.Text + " 00:00:00");
                            DT = DT.AddDays(1);

                            lvDateE = Gstr.fncChangeTDate(DT.ToString("dd/MM/yyyy"));
                            //lvDateE = DT.ToString("yyyyMMdd");
                        }
                        else if (lvKa[i] == "00.01-01.00" || lvKa[i] == "01.01-02.00" || lvKa[i] == "02.01-03.00" || lvKa[i] == "03.01-04.00" || lvKa[i] == "04.01-05.00" || lvKa[i] == "05.01-06.00" || lvKa[i] == "06.01-07.00")
                        {
                            DateTime DT = DateTime.Parse(txtDateS.Text + " 00:00:00");
                            DT = DT.AddDays(1);

                            lvDateS = Gstr.fncChangeTDate(DT.ToString("dd/MM/yyyy"));
                            lvDateE = lvDateS;
                        }

                        string[] lvArr = lvKa[i].Split('-');
                        lvTimeS = FncChangeFormatTime(lvArr[0], false);
                        lvTimeE = FncChangeFormatTime(lvArr[1], true);

                        //อ้อยสด + อ้อยสด C + อ้อยสดยอดยาวปนเปื้อน
                        GsysSQL.fncGetDataCane(lvRail, "1','3','7", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[2].Cells[i, 1].Text = GVar.gvDataCount;
                        sp1.Sheets[2].Cells[i, 2].Text = GVar.gvDataSum;

                        //อ้อยสด C โหลดเก็บไว้แสดงรายงาน
                        GsysSQL.fncGetDataCane(lvRail, "3", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[2].Cells[i, 23].Text = GVar.gvDataCount;
                        sp1.Sheets[2].Cells[i, 24].Text = GVar.gvDataSum;

                        //อ้อยสดยอดยาวปนเปื้อนโหลดเก็บไว้แสดงรายงาน
                        GsysSQL.fncGetDataCane(lvRail, "7", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[2].Cells[i, 25].Text = GVar.gvDataCount;
                        sp1.Sheets[2].Cells[i, 26].Text = GVar.gvDataSum;

                        //สด-สะอาด
                        GsysSQL.fncGetDataCane(lvRail, "17", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[2].Cells[i, 3].Text = GVar.gvDataCount;
                        sp1.Sheets[2].Cells[i, 4].Text = GVar.gvDataSum;

                        //สดรถตัดใน
                        GsysSQL.fncGetDataCane(lvRail, "11", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[2].Cells[i, 5].Text = GVar.gvDataCount;
                        sp1.Sheets[2].Cells[i, 6].Text = GVar.gvDataSum;

                        //สดรถตัดนอก
                        GsysSQL.fncGetDataCane(lvRail, "5", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[2].Cells[i, 7].Text = GVar.gvDataCount;
                        sp1.Sheets[2].Cells[i, 8].Text = GVar.gvDataSum;

                        //สดไฟไหม้
                        GsysSQL.fncGetDataCane(lvRail, "2", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[2].Cells[i, 11].Text = GVar.gvDataCount;
                        sp1.Sheets[2].Cells[i, 12].Text = GVar.gvDataSum;

                        //สดไฟไหม้ - สะอาด
                        GsysSQL.fncGetDataCane(lvRail, "18", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[2].Cells[i, 13].Text = GVar.gvDataCount;
                        sp1.Sheets[2].Cells[i, 14].Text = GVar.gvDataSum;

                        //สดไฟไหม้ - รถตัดใน
                        GsysSQL.fncGetDataCane(lvRail, "12", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[2].Cells[i, 15].Text = GVar.gvDataCount;
                        sp1.Sheets[2].Cells[i, 16].Text = GVar.gvDataSum;

                        //สดไฟไหม้ - รถตัดนอก
                        GsysSQL.fncGetDataCane(lvRail, "6", lvDateS, lvDateE, lvTimeS, lvTimeE, lvShowClose, lvBillS, lvBillE, GVar.gvOnline);
                        sp1.Sheets[2].Cells[i, 17].Text = GVar.gvDataCount;
                        sp1.Sheets[2].Cells[i, 18].Text = GVar.gvDataSum;

                        //ถ้าเป็น 0 ให้แสดงช่องว่าง
                        int lvSheetIndex = 2;
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 1].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 1].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 2].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 2].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 3].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 3].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 4].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 4].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 5].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 5].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 6].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 6].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 7].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 7].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 8].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 8].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 11].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 11].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 12].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 12].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 13].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 13].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 14].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 14].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 15].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 15].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 16].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 16].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 17].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 17].Text = "";
                        if (Gstr.fncToDouble(sp1.Sheets[lvSheetIndex].Cells[i, 18].Text) == 0) sp1.Sheets[lvSheetIndex].Cells[i, 18].Text = "";

                        Application.DoEvents();

                    }

                    //สรุปยอด Sum
                    FncSumData(2);
                }
                #endregion

                pnTotalCut.Visible = false;
            }

            //ซ่อน แสดง Sheet
            fncShowSheet(true);

            pnStatus.Visible = false;
            this.Cursor = Cursors.Default;
        }

        private void LoadDataCutCar()
        {
            pnStatus.Visible = true;
            sp1.Enabled = false;

            #region ข้อมูลรถตัด
            DataTable DT1 = new DataTable();
            string lvSQL = "select Q_No, Q_Quota, Q_CarNum, Q_BillingNo, Q_CutCar, Q_CutDoc, Q_WeightIN, Q_WeightOUT, (Q_WeightIN - Q_WeightOUT) as NetWeight, Q_WeightOUTTime,Q_CaneType ";
            lvSQL += "from Queue_Diary HD ";
            lvSQL += "where Q_CaneType in ('11','12') and Q_Year = ''";

            if (ChkA.Checked)
                lvSQL += "and Q_Rail = 'A' ";
            else if (ChkB.Checked)
                lvSQL += "and Q_Rail = 'B' ";

            if (chkShowClose.Checked)
                lvSQL += "and Q_CloseStatus = '1' ";
            else
                lvSQL += "and Q_CloseStatus = '' ";

            if (txtBillS.Text != "" && txtBillE.Text != "")
            {
                lvSQL += "and (Cast(Q_BillingNo as int) >= '" + txtBillS.Text + "' and Cast(Q_BillingNo as int) <= '" + txtBillE.Text + "') ";
                //lvSQL += "union ";
                //lvSQL += "select (Q_No + '.1') as Q_No, Q_Quota, Q_CarNum, Q_BillingNo, Q_CutCar, Q_CutDoc, Q_WeightIN, Q_WeightOUT, (Q_WeightIN - Q_WeightOUT) as NetWeight, Q_WeightOUTTime,Q_CaneType ";
                //lvSQL += "from Queue_Diary ";
                //lvSQL += "where Q_CaneType in ('11','12')  and Q_Year = '' ";
                //lvSQL += "and (Cast(Q_BillingNo as int) >= '66059' and Cast(Q_BillingNo as int) <= '66981') and Q_No = CAST(Q_No as int) ";
            }
            else
            {

            }

            lvSQL += "order by Q_No ";
            DT1 = GsysSQL.fncGetQueryData(lvSQL, DT1, GVar.gvOnline);

            int lvNumRow = DT1.Rows.Count;
            if (lvNumRow > 0)
            {
                sp1.Sheets["รถตัด"].RowCount = 0;

                double lvCut1 = 0;
                double lvCut2 = 0;
                double lvCountCar = 0;

                for (int i = 0; i < lvNumRow; i++)
                {
                    //เอามาเฉพาะ คิวที่ชั่งเสร็จแล้ว
                    bool lvChkQFinish = GsysSQL.fncCheckQueueFinish(DT1.Rows[i]["Q_No"].ToString());
                    if (lvChkQFinish)
                    {
                        sp1.Sheets["รถตัด"].RowCount += 1;
                        int lvIndex = sp1.Sheets["รถตัด"].RowCount - 1;
                        sp1.Sheets["รถตัด"].Cells[lvIndex, 0].Text = DT1.Rows[i]["Q_Quota"].ToString();
                        sp1.Sheets["รถตัด"].Cells[lvIndex, 1].Text = DT1.Rows[i]["Q_CarNum"].ToString();
                        sp1.Sheets["รถตัด"].Cells[lvIndex, 2].Text = DT1.Rows[i]["Q_BillingNo"].ToString();
                        sp1.Sheets["รถตัด"].Cells[lvIndex, 3].Text = DT1.Rows[i]["Q_CutCar"].ToString();
                        sp1.Sheets["รถตัด"].Cells[lvIndex, 4].Text = DT1.Rows[i]["Q_CutDoc"].ToString();
                        sp1.Sheets["รถตัด"].Cells[lvIndex, 5].Text = FncChangeTonToKg(DT1.Rows[i]["Q_WeightIN"].ToString());
                        sp1.Sheets["รถตัด"].Cells[lvIndex, 6].Text = FncChangeTonToKg(DT1.Rows[i]["Q_WeightOUT"].ToString());
                        sp1.Sheets["รถตัด"].Cells[lvIndex, 7].Text = FncChangeTonToKg(DT1.Rows[i]["NetWeight"].ToString());
                        sp1.Sheets["รถตัด"].Cells[lvIndex, 8].Text = DT1.Rows[i]["Q_WeightOUTTime"].ToString();
                        sp1.Sheets["รถตัด"].Cells[lvIndex, 9].Text = DT1.Rows[i]["Q_CaneType"].ToString() + " : " + GsysSQL.fncFindCaneTypeName(DT1.Rows[i]["Q_CaneType"].ToString(), true);
                        sp1.Sheets["รถตัด"].Cells[lvIndex, 10].Text = DT1.Rows[i]["Q_No"].ToString();

                        if (DT1.Rows[i]["Q_CaneType"].ToString() == "11")
                            lvCut1 += Gstr.fncToDouble(sp1.Sheets["รถตัด"].Cells[lvIndex, 7].Text);
                        else
                            lvCut2 += Gstr.fncToDouble(sp1.Sheets["รถตัด"].Cells[lvIndex, 7].Text);

                        //เก็บเลขที่บิลไว้เรียงข้อมูล
                        if (DT1.Rows[i]["Q_BillingNo"].ToString() == "")
                        {
                            string lvQNew = (Math.Floor(Gstr.fncToDouble(DT1.Rows[i]["Q_No"].ToString()))).ToString();
                            sp1.Sheets["รถตัด"].Cells[lvIndex, 12].Text = GsysSQL.FindBillNoByQueue(lvQNew, GVar.gvOnline);
                        }
                        else
                        {
                            sp1.Sheets["รถตัด"].Cells[lvIndex, 12].Text = DT1.Rows[i]["Q_BillingNo"].ToString();
                        }

                        if (DT1.Rows[i]["Q_BillingNo"].ToString() != "")
                            lvCountCar += 1;

                        //ถ้าเป็นชั่งรวมให้แสดงคิวลูกด้วย และ แสดงข้อมูลย้อนหลัง และ มีการกรอกเลขที่บิล
                        if (txtBillS.Text != "" && txtBillE.Text != "" && chkShowClose.Checked && !DT1.Rows[i]["Q_No"].ToString().Contains("."))
                        {
                            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                            SqlCommand cmd = new SqlCommand();
                            SqlDataReader dr;

                            string lvQlook = DT1.Rows[i]["Q_No"].ToString() + ".1";

                            cmd.Connection = con;
                            con.Open();
                            cmd.CommandText = "SELECT Q_No, Q_Quota, Q_CarNum, Q_BillingNo, Q_CutCar, Q_CutDoc, Q_WeightIN, Q_WeightOUT, (Q_WeightIN - Q_WeightOUT) as NetWeight, Q_WeightOUTTime,Q_CaneType FROM Queue_Diary WHERE Q_No = '" + lvQlook + "' and Q_Year = '' and Q_BillingNo = '' ";
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    sp1.Sheets["รถตัด"].RowCount += 1;
                                    lvIndex = sp1.Sheets["รถตัด"].RowCount - 1;
                                    sp1.Sheets["รถตัด"].Cells[lvIndex, 0].Text = dr["Q_Quota"].ToString();
                                    sp1.Sheets["รถตัด"].Cells[lvIndex, 1].Text = dr["Q_CarNum"].ToString();
                                    sp1.Sheets["รถตัด"].Cells[lvIndex, 2].Text = dr["Q_BillingNo"].ToString();
                                    sp1.Sheets["รถตัด"].Cells[lvIndex, 3].Text = dr["Q_CutCar"].ToString();
                                    sp1.Sheets["รถตัด"].Cells[lvIndex, 4].Text = dr["Q_CutDoc"].ToString();
                                    sp1.Sheets["รถตัด"].Cells[lvIndex, 5].Text = FncChangeTonToKg(dr["Q_WeightIN"].ToString());
                                    sp1.Sheets["รถตัด"].Cells[lvIndex, 6].Text = FncChangeTonToKg(dr["Q_WeightOUT"].ToString());
                                    sp1.Sheets["รถตัด"].Cells[lvIndex, 7].Text = FncChangeTonToKg(dr["NetWeight"].ToString());
                                    sp1.Sheets["รถตัด"].Cells[lvIndex, 8].Text = dr["Q_WeightOUTTime"].ToString();
                                    sp1.Sheets["รถตัด"].Cells[lvIndex, 9].Text = dr["Q_CaneType"].ToString() + " : " + GsysSQL.fncFindCaneTypeName(dr["Q_CaneType"].ToString(), true);
                                    sp1.Sheets["รถตัด"].Cells[lvIndex, 10].Text = dr["Q_No"].ToString();

                                    if (dr["Q_CaneType"].ToString() == "11")
                                        lvCut1 += Gstr.fncToDouble(sp1.Sheets["รถตัด"].Cells[lvIndex, 7].Text);
                                    else
                                        lvCut2 += Gstr.fncToDouble(sp1.Sheets["รถตัด"].Cells[lvIndex, 7].Text);

                                    //เก็บเลขที่บิลไว้เรียงข้อมูล
                                    if (dr["Q_BillingNo"].ToString() == "")
                                    {
                                        string lvQNew = (Math.Floor(Gstr.fncToDouble(dr["Q_No"].ToString()))).ToString();
                                        sp1.Sheets["รถตัด"].Cells[lvIndex, 12].Text = GsysSQL.FindBillNoByQueue(lvQNew, GVar.gvOnline);
                                    }
                                    else
                                    {
                                        sp1.Sheets["รถตัด"].Cells[lvIndex, 12].Text = dr["Q_BillingNo"].ToString();
                                    }

                                    if (dr["Q_BillingNo"].ToString() != "")
                                        lvCountCar += 1;
                                }
                            }
                            else
                            {

                            }

                            dr.Close();
                            con.Close();
                        }
                    }
                    Application.DoEvents();
                }

                txtCountCar.Text = lvCountCar.ToString("#,##0");
                txtCut1.Text = lvCut1.ToString("#,##0.00");
                txtCut2.Text = lvCut2.ToString("#,##0.00");
                txtCutAll.Text = (lvCut1 + lvCut2).ToString("#,##0.00");
            }

            DT1.Dispose();

            #endregion

            sp1.Enabled = true;
            pnStatus.Visible = false;
            pnTotalCut.Visible = true;
        }

        private string FncChangeTonToKg(string lvWeight)
        {
            string lvReturn = "";
            double lvWeightChange = 0;

            lvWeightChange = Gstr.fncToDouble(lvWeight);
            lvWeightChange = lvWeightChange / 1000;
            lvReturn = lvWeightChange.ToString("#,##0.00");

            return lvReturn;
        }

        private void FncSumData(int lvSheet)
        {
            //รวม
            sp1.Sheets[lvSheet].RowCount += 1;
            int lvLast = sp1.Sheets[lvSheet].RowCount - 1;
            sp1.Sheets[lvSheet].Cells[lvLast, 0].Text = "รวม";
            sp1.Sheets[lvSheet].Cells[lvLast, 1].Formula = "Sum(B1:B" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 2].Formula = "Sum(C1:C" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 3].Formula = "Sum(D1:D" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 4].Formula = "Sum(E1:E" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 5].Formula = "Sum(F1:F" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 6].Formula = "Sum(G1:G" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 7].Formula = "Sum(H1:H" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 8].Formula = "Sum(I1:I" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 11].Formula = "Sum(L1:L" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 12].Formula = "Sum(M1:M" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 13].Formula = "Sum(N1:N" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 14].Formula = "Sum(O1:O" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 15].Formula = "Sum(P1:P" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 16].Formula = "Sum(Q1:Q" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 17].Formula = "Sum(R1:R" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 18].Formula = "Sum(S1:S" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 23].Formula = "Sum(X1:X" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 24].Formula = "Sum(Y1:Y" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 25].Formula = "Sum(Z1:Z" + lvLast + ")";
            sp1.Sheets[lvSheet].Cells[lvLast, 26].Formula = "Sum(AA1:AA" + lvLast + ")";
            sp1.Sheets[lvSheet].Rows[lvLast].BackColor = Color.LightGray;
        }

        private void fncShowSheet(bool lvSelectIndex)
        {
            if (ChkShowCutCar.Checked)
            {
                if (lvSelectIndex) sp1.ActiveSheetIndex = 4;
                sp1.Sheets["A"].Visible = false;
                sp1.Sheets["B"].Visible = false;
                sp1.Sheets["รวม"].Visible = false;
                sp1.Sheets["รายละเอียด"].Visible = false;
                sp1.Sheets["รถตัด"].Visible = true;
            }
            else if (ChkA.Checked)
            {
                if (lvSelectIndex) sp1.ActiveSheetIndex = 0;
                sp1.Sheets["A"].Visible = true;
                sp1.Sheets["B"].Visible = false;
                sp1.Sheets["รวม"].Visible = false;
                sp1.Sheets["รายละเอียด"].Visible = false;
                sp1.Sheets["รถตัด"].Visible = false;
            }
            else if (ChkB.Checked)
            {
                if (lvSelectIndex) sp1.ActiveSheetIndex = 1;
                sp1.Sheets["A"].Visible = false;
                sp1.Sheets["B"].Visible = true;
                sp1.Sheets["รวม"].Visible = false;
                sp1.Sheets["รายละเอียด"].Visible = false;
                sp1.Sheets["รถตัด"].Visible = false;
            }
            else if (ChkAll.Checked)
            {
                if (lvSelectIndex) sp1.ActiveSheetIndex = 2;
                sp1.Sheets["A"].Visible = false;
                sp1.Sheets["B"].Visible = false;
                sp1.Sheets["รวม"].Visible = true;
                sp1.Sheets["รายละเอียด"].Visible = false;
                sp1.Sheets["รถตัด"].Visible = false;
            }

        }

        private string FncChangeFormatTime(string lvData, bool lvTEnd)
        {
            string lvReturn = "";

            if (lvTEnd)
                lvData = lvData + ".59";
            else
                lvData = lvData + ".00";

            lvData = lvData.Replace('.', ':');

            lvReturn = lvData;

            return lvReturn;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            sp1.ActiveSheet.RowCount = 0;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            pnPrintMenu.ExpandAll();
            pnMenuPrint.Visible = true;
            //CPrintMenu.Show(Cursor.Position);
        }

        private void sp1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            try
            {
                int lvColAct = e.Column;
                string lvTime = sp1.ActiveSheet.Cells[e.Row, 0].Text;
                string[] lvArr = lvTime.Split('-');
                string lvTimeS = FncChangeFormatTime(lvArr[0], false);
                string lvTimeE = FncChangeFormatTime(lvArr[1], true);

                string lvDateS = Gstr.fncChangeTDate(txtDateS.Text);
                string lvDateE = "";

                if (lvArr[0] == "00.01" || lvArr[0] == "01.01" || lvArr[0] == "02.01" || lvArr[0] == "03.01" || lvArr[0] == "04.01" || lvArr[0] == "05.01" || lvArr[0] == "06.01" || lvArr[0] == "07.01" || lvArr[0] == "08.01" || lvArr[0] == "09.01" || lvArr[0] == "10.01" || lvArr[0] == "11.01" || lvArr[0] == "12.01" || lvArr[0] == "13.01" || lvArr[0] == "14.01")
                {
                    DateTime DTS = DateTime.Parse(txtDateS.Text + " 00:00:00");
                    lvDateS = Gstr.fncChangeTDate(DTS.AddDays(1).ToString("dd/MM/yyyy"));
                    lvDateE = lvDateS;
                }
                else if (lvArr[0] == "23.01")
                {
                    DateTime DTS = DateTime.Parse(txtDateS.Text + " 00:00:00");
                    lvDateS = Gstr.fncChangeTDate(DTS.ToString("dd/MM/yyyy"));
                    lvDateE = Gstr.fncChangeTDate(DTS.AddDays(1).ToString("dd/MM/yyyy"));
                }
                else
                {
                    lvDateE = lvDateS;
                }

                string lvRail = "";
                if (ChkA.Checked) lvRail = "A";
                else if (ChkB.Checked) lvRail = "B";

                string lvCaneType = "";
                if (lvColAct == 1 || lvColAct == 2)
                {
                    lvCaneType = "1','3','7";
                }
                else if (lvColAct == 3 || lvColAct == 4)
                {
                    lvCaneType = "17";
                }
                else if (lvColAct == 5 || lvColAct == 6)
                {
                    lvCaneType = "11";
                }
                else if (lvColAct == 7 || lvColAct == 8)
                {
                    lvCaneType = "5";
                }
                else if (lvColAct == 11 || lvColAct == 12)
                {
                    lvCaneType = "2";
                }
                else if (lvColAct == 13 || lvColAct == 14)
                {
                    lvCaneType = "18";
                }
                else if (lvColAct == 15 || lvColAct == 16)
                {
                    lvCaneType = "12";
                }
                else if (lvColAct == 17 || lvColAct == 18)
                {
                    lvCaneType = "6";
                }

                //Get Data
                DataTable DT = new DataTable();

                string lvSQL = "select Q_No,Q_Rail,Q_CarNum,Q_Quota,Q_CaneType,Q_WeightOUTDate,Q_WeightOUTTime,Q_WeightIN,Q_WeightOUT,Q_BillingNo ";
                lvSQL += "from Queue_Diary ";
                lvSQL += "where 1=1 And Q_CaneType in ('" + lvCaneType + "') And Q_WeightOUT > 0 ";//
                if (lvRail != "") lvSQL += "And Q_Rail = '" + lvRail + "' ";
                if (chkShowClose.Checked) lvSQL += "And Q_CloseStatus = '1' "; else lvSQL += "And Q_CloseStatus = '' ";
                lvSQL += "And CONVERT(datetime, Q_WeightOUTDate + ' ' + Q_WeightOUTTime, 103) >= CONVERT(datetime, '" + lvDateS + "' + ' ' + '" + lvTimeS + "', 103) ";
                lvSQL += "And CONVERT(datetime, Q_WeightOUTDate + ' ' + Q_WeightOUTTime, 103) <= CONVERT(datetime, '" + lvDateE + "' + ' ' + '" + lvTimeE + "', 103) ";
                lvSQL += "Order by Q_Rail,Q_WeightOUTDate,Q_WeightOUTTime ";
                DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

                int lvNumRow = DT.Rows.Count;

                frmShowDetail frm = new frmShowDetail();
                frm.sp1.ActiveSheet.RowCount = 0;
                for (int i = 0; i < lvNumRow; i++)
                {
                    frm.sp1.ActiveSheet.RowCount += 1;
                    frm.sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["Q_BillingNo"].ToString();
                    frm.sp1.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["Q_No"].ToString();
                    frm.sp1.ActiveSheet.Cells[i, 2].Text = DT.Rows[i]["Q_Rail"].ToString();
                    frm.sp1.ActiveSheet.Cells[i, 3].Text = DT.Rows[i]["Q_CarNum"].ToString();
                    frm.sp1.ActiveSheet.Cells[i, 4].Text = DT.Rows[i]["Q_Quota"].ToString();
                    frm.sp1.ActiveSheet.Cells[i, 5].Text = GsysSQL.fncFindCaneTypeName(DT.Rows[i]["Q_CaneType"].ToString(), GVar.gvOnline);
                    frm.sp1.ActiveSheet.Cells[i, 6].Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightOUTDate"].ToString());
                    frm.sp1.ActiveSheet.Cells[i, 7].Text = DT.Rows[i]["Q_WeightOUTTime"].ToString();
                    frm.sp1.ActiveSheet.Cells[i, 8].Text = (Gstr.fncToDouble(DT.Rows[i]["Q_WeightIN"].ToString()) / 1000).ToString("#,##0.000"); // 
                    frm.sp1.ActiveSheet.Cells[i, 9].Text = (Gstr.fncToDouble(DT.Rows[i]["Q_WeightOUT"].ToString()) / 1000).ToString("#,##0.000"); // 
                }

                //รวม
                frm.sp1.ActiveSheet.RowCount += 1;
                int lvLast = frm.sp1.ActiveSheet.RowCount - 1;
                frm.sp1.ActiveSheet.Cells[lvLast, 0].Text = "รวม";
                frm.sp1.ActiveSheet.Cells[lvLast, 8].Formula = "Sum(I1:I" + lvLast + ")";
                frm.sp1.ActiveSheet.Cells[lvLast, 9].Formula = "Sum(J1:J" + lvLast + ")";
                frm.sp1.ActiveSheet.Cells[lvLast, 10].Formula = "Sum(K1:K" + lvLast + ")";
                frm.sp1.ActiveSheet.Rows[lvLast].BackColor = Color.LightGray;

                if (lvNumRow > 0)
                    frm.ShowDialog();
            }
            catch (Exception ex)
            {
                sp1.ActiveSheetIndex = 0;
            }
        }

        private void แบบรวมToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FncPrintReportALL("ALL");
        }

        private void FncPrintReportALL(string lvKa)
        {
            this.Cursor = Cursors.WaitCursor;
            //ลบข้อมูล เก่า
            string lvSQL = "Delete From SysTemp "; //HD
            string lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);

            //เวลาแต่ละกะ
            string[] lvKa1 = new string[] { "07.01-08.00", "08.01-09.00", "09.01-10.00", "10.01-11.00", "11.01-12.00", "12.01-13.00", "13.01-14.00", "14.01-15.00", "15.01-16.00", "16.01-17.00", "17.01-18.00", "18.01-19.00" };
            string[] lvKa2 = new string[] { "19.01-20.00", "20.01-21.00", "21.01-22.00", "22.01-23.00", "23.01-00.00", "00.01-01.00", "01.01-02.00", "02.01-03.00", "03.01-04.00", "04.01-05.00", "05.01-06.00", "06.01-07.00" };

            //Sheet 0 = A , 1 = B , 2 = รวม
            for (int i = 0; i < sp1.ActiveSheet.RowCount; i++)
            {
                string lvField1 = sp1.ActiveSheet.Cells[i, 0].Text;
                string lvField2 = txtDatePrint.Text;
                double lvNum1 = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 21].Text); //จำนวนรถ A
                double lvNum2 = Gstr.fncToDouble(sp1.Sheets[1].Cells[i, 21].Text); //จำนวนรถ B
                double lvNum3 = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 22].Text); //ตันอ้อยรวม A
                double lvNum4 = Gstr.fncToDouble(sp1.Sheets[1].Cells[i, 22].Text); //ตันอ้อยรวม B
                double lvNum5 = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 10].Text); //อ้อยสด A
                double lvNum6 = Gstr.fncToDouble(sp1.Sheets[1].Cells[i, 10].Text); // อ้อยสด B
                double lvNum7 = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 20].Text); //อ้อยไฟไหม้ A
                double lvNum8 = Gstr.fncToDouble(sp1.Sheets[1].Cells[i, 20].Text); //อ้อยไฟไหม้ B
                double lvNum9 = i; //ลำดับ

                bool lvInsert = false;
                if (lvKa == "Night")
                {
                    foreach (var item in lvKa2)
                    {
                        if (item == lvField1)
                        {
                            if (lvField1 == "19.01-20.00") lvNum9 = 1;
                            else if (lvField1 == "20.01-21.00") lvNum9 = 2;
                            else if (lvField1 == "21.01-22.00") lvNum9 = 3;
                            else if (lvField1 == "22.01-23.00") lvNum9 = 4;
                            else if (lvField1 == "23.01-00.00") lvNum9 = 5;
                            else if (lvField1 == "00.01-01.00") lvNum9 = 6;
                            else if (lvField1 == "01.01-02.00") lvNum9 = 7;
                            else if (lvField1 == "02.01-03.00") lvNum9 = 8;
                            else if (lvField1 == "03.01-04.00") lvNum9 = 9;
                            else if (lvField1 == "04.01-05.00") lvNum9 = 10;
                            else if (lvField1 == "05.01-06.00") lvNum9 = 11;
                            else if (lvField1 == "06.01-07.00") lvNum9 = 12;

                            lvInsert = true;
                        }
                    }
                }
                else if (lvKa == "Day")
                {
                    foreach (var item in lvKa1)
                    {
                        if (item == lvField1)
                        {
                            if (lvField1 == "07.01-08.00") lvNum9 = 1;
                            else if (lvField1 == "08.01-09.00") lvNum9 = 2;
                            else if (lvField1 == "09.01-10.00") lvNum9 = 3;
                            else if (lvField1 == "10.01-11.00") lvNum9 = 4;
                            else if (lvField1 == "11.01-12.00") lvNum9 = 5;
                            else if (lvField1 == "12.01-13.00") lvNum9 = 6;
                            else if (lvField1 == "13.01-14.00") lvNum9 = 7;
                            else if (lvField1 == "14.01-15.00") lvNum9 = 8;
                            else if (lvField1 == "15.01-16.00") lvNum9 = 9;
                            else if (lvField1 == "16.01-17.00") lvNum9 = 10;
                            else if (lvField1 == "17.01-18.00") lvNum9 = 11;
                            else if (lvField1 == "18.01-19.00") lvNum9 = 12;
                            lvInsert = true;
                        }
                    }
                }
                else if (lvKa == "ALL")
                {
                    lvInsert = true;
                }

                if (lvField1 != "รวม")
                {
                    lvSQL = "Insert into SysTemp (Field1, Field2, Num1, Num2, Num3, Num4, Num5, Num6, Num7, Num8, Num9) ";//
                    lvSQL += "Values ('" + lvField1 + "', '" + lvField2 + "', '" + lvNum1 + "', '" + lvNum2 + "', '" + lvNum3 + "', '" + lvNum4 + "', '" + lvNum5 + "', '" + lvNum6 + "', '" + lvNum7 + "', '" + lvNum8 + "', '" + lvNum9 + "') ";

                    if (lvInsert)
                        lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
                }
            }

            //แสดงก่อนพิมพ์
            frmPrint frm = new frmPrint();
            frm.documentViewer1.DocumentSource = typeof(PSQueue.rptReportHour);
            frm.ShowDialog();

            this.Cursor = Cursors.Default;
        }

        private void แบบละเอยดToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //ลบข้อมูล เก่า
            string lvSQL = "Delete From SysTemp "; //HD
            string lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);

            int lvSheet = 2;
            string lvRail = "A-B";
            if (ChkA.Checked)
            {
                lvSheet = 0;
                lvRail = "A";
            }
            else if (ChkB.Checked)
            {
                lvSheet = 1;
                lvRail = "B";
            }

            for (int i = 0; i < sp1.Sheets[lvSheet].RowCount; i++)
            {
                string lvField1 = sp1.Sheets[lvSheet].Cells[i, 0].Text;
                string lvField2 = txtDatePrint.Text;
                string lvField3 = lvRail;
                double lvNum1 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 1].Text); //
                double lvNum2 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 2].Text); //
                double lvNum3 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 3].Text); //
                double lvNum4 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 4].Text); //
                double lvNum5 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 5].Text); //
                double lvNum6 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 6].Text); //
                double lvNum7 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 7].Text); //
                double lvNum8 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 8].Text); //

                double lvNum11 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 11].Text); //
                double lvNum12 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 12].Text); //
                double lvNum13 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 13].Text); //
                double lvNum14 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 14].Text); //
                double lvNum15 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 15].Text); //
                double lvNum16 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 16].Text); //
                double lvNum17 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 17].Text); //
                double lvNum18 = Gstr.fncToDouble(sp1.Sheets[lvSheet].Cells[i, 18].Text); //

                if (lvField1 != "รวม")
                {
                    lvSQL = "Insert into SysTemp (Field1, Field2, Field3, Num1, Num2, Num3, Num4, Num5, Num6, Num7, Num8, Num9, Num10, Num11, Num12, Num13, Num14, Num15, Num16) ";//
                    lvSQL += "Values ('" + lvField1 + "', '" + lvField2 + "', '" + lvField3 + "', '" + lvNum1 + "', '" + lvNum2 + "', '" + lvNum3 + "', '" + lvNum4 + "', '" + lvNum5 + "', '" + lvNum6 + "', '" + lvNum7 + "', '" + lvNum8 + "' ";
                    lvSQL += ", '" + lvNum11 + "', '" + lvNum12 + "', '" + lvNum13 + "', '" + lvNum14 + "', '" + lvNum15 + "', '" + lvNum16 + "', '" + lvNum17 + "', '" + lvNum18 + "') ";
                    lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
                }
            }

            //แสดงก่อนพิมพ์
            frmPrint frm = new frmPrint();
            frm.documentViewer1.DocumentSource = typeof(PSQueue.rptReportHourSum);
            frm.ShowDialog();

            this.Cursor = Cursors.Default;
        }

        private void fncPrintReport(string lvTypeReport)
        {
            if (lvTypeReport == "รายงานการหีบอ้อยประจำวัน")
            {
                #region รายงานการหีบอ้อยประจำวัน
                this.Cursor = Cursors.WaitCursor;
                //ลบข้อมูล เก่า
                string lvSQL = "Delete From SysTemp "; //HD
                string lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);

                int lvSheet = 2;

                for (int i = 0; i < 2; i++)
                {
                    string lvField1 = txtDatePrint.Text;
                    string lvField2 = "";

                    string lvField3 = "";
                    if (i == 0) lvField3 = "อ้อยสด";
                    else if (i == 1) lvField3 = "อ้อยไฟไหม้";

                    int lvSheetSelect = 0;
                    int lvColCar = 0;
                    int lvColTon = 0;
                    if (lvField3 == "อ้อยสด")
                    {
                        lvSheetSelect = 0;
                        lvColCar = 9;
                        lvColTon = 10;
                    }
                    else if (lvField3 == "อ้อยไฟไหม้")
                    {
                        lvSheetSelect = 1;
                        lvColCar = 19;
                        lvColTon = 20;
                    }

                    double lvNum1 = fncSumDataByColumns(0, lvColCar); //จำนวนคัน ราง A
                    double lvNum2 = fncSumDataByColumns(0, lvColTon); //จำนวนตัน ราง A
                    double lvNum3 = fncSumDataByColumns(1, lvColCar); //จำนวนคัน ราง B
                    double lvNum4 = fncSumDataByColumns(1, lvColTon); //จำนวนตัน ราง B
                    double lvNum5 = lvNum1 + lvNum3; //รวม คัน
                    double lvNum6 = lvNum2 + lvNum4; //รวม ตัน

                    lvSQL = "Insert into SysTemp (Field1, Field2, Field3, Num1, Num2, Num3, Num4, Num5, Num6) ";//
                    lvSQL += "Values ('" + lvField1 + "', '" + lvField2 + "', '" + lvField3 + "', '" + lvNum1 + "', '" + lvNum2 + "', '" + lvNum3 + "', '" + lvNum4 + "', '" + lvNum5 + "', '" + lvNum6 + "') ";
                    lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
                }
                
                #endregion

                #region รายงานการหีบอ้อยประจำวัน (แยกประเภท)
                
                //string[] lvType = new string[] { "อ้อยสดรถตัด", "อ้อยสดรถตัดนอก", "อ้อยไฟไหม้รถตัด", "อ้อยไฟไหม้รถตัดนอก" };

                //DateTime DTS = DateTime.Parse(txtDateS.Text + " " + DateTime.Now.ToString("HH:mm:ss"));
                ////DTS = DTS.AddDays(1);

                //for (int i = 0; i < lvType.Count(); i++)
                //{
                //    string lvField1 = DTS.ToString();
                //    string lvField2 = lvType[i];
                //    string lvField3 = "";

                //    int lvColCar = 0;
                //    int lvColTon = 0;

                //    if (lvType[i] == "อ้อยสด")
                //    {
                //        lvColCar = 1;
                //        lvColTon = 2;
                //        lvField3 = "1";
                //    }
                //    if (lvType[i] == "อ้อยสด-สะอาด")
                //    {
                //        lvColCar = 3;
                //        lvColTon = 4;
                //        lvField3 = "1";
                //    }
                //    if (lvType[i] == "อ้อยสดรถตัด")
                //    {
                //        lvColCar = 5;
                //        lvColTon = 6;
                //        lvField3 = "1";
                //    }
                //    if (lvType[i] == "อ้อยสดรถตัดนอก")
                //    {
                //        lvColCar = 7;
                //        lvColTon = 8;
                //        lvField3 = "1";
                //    }
                //    if (lvType[i] == "อ้อยไฟไหม้")
                //    {
                //        lvColCar = 11;
                //        lvColTon = 12;
                //        lvField3 = "2";
                //    }
                //    if (lvType[i] == "อ้อยไฟไหม้-สะอาด")
                //    {
                //        lvColCar = 13;
                //        lvColTon = 14;
                //        lvField3 = "2";
                //    }
                //    if (lvType[i] == "อ้อยไฟไหม้รถตัด")
                //    {
                //        lvColCar = 15;
                //        lvColTon = 16;
                //        lvField3 = "2";
                //    }
                //    if (lvType[i] == "อ้อยไฟไหม้รถตัดนอก")
                //    {
                //        lvColCar = 17;
                //        lvColTon = 18;
                //        lvField3 = "2";
                //    }

                //    int lvLastA = sp1.Sheets[0].RowCount - 1;
                //    double lvNum1 = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, lvColCar].Text); //จำนวนคัน ราง A
                //    double lvNum2 = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, lvColTon].Text); //จำนวนตัน ราง A

                //    int lvLastB = sp1.Sheets[1].RowCount - 1;
                //    double lvNum3 = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastA, lvColCar].Text); //จำนวนคัน ราง B
                //    double lvNum4 = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastA, lvColTon].Text); //จำนวนตัน ราง B
                //    double lvNum5 = lvNum1 + lvNum3; //รวม คัน
                //    double lvNum6 = lvNum2 + lvNum4; //รวม ตัน

                //    lvSQL = "Insert into SysTemp (Field4, Field5, Field6, Num7, Num8, Num9, Num10, Num11, Num12) ";//
                //    lvSQL += "Values ('" + lvField1 + "', '" + lvField2 + "', '" + lvField3 + "', '" + lvNum1 + "', '" + lvNum2 + "', '" + lvNum3 + "', '" + lvNum4 + "', '" + lvNum5 + "', '" + lvNum6 + "') ";
                //    if (lvNum5 + lvNum6 > 0) lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);//

                //    ////ทำตัวรวมแยกอ้อยสด - ไฟไหม้
                //    //if (lvType[i] == "อ้อยสดรถตัดนอก")
                //    //{
                //    //    lvField2 = "รวมอ้อยสด";
                //    //    lvNum1 = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, 9].Text); //จำนวนคัน รวมสด ราง A
                //    //    lvNum2 = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, 10].Text); //จำนวนตัน รวมไฟไหม้ ราง A

                //    //    lvNum3 = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastA, 19].Text); //จำนวนคัน รวมสด ราง B
                //    //    lvNum4 = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastA, 20].Text); //จำนวนตัน รวมไฟไหม้ ราง B

                //    //    lvSQL = "Insert into SysTemp (Field1, Field2, Num1, Num2, Num3, Num4, Num5, Num6) ";//
                //    //    lvSQL += "Values ('" + lvField1 + "', '" + lvField2 + "', '" + lvNum1 + "', '" + lvNum2 + "', '" + lvNum3 + "', '" + lvNum4 + "', '" + lvNum5 + "', '" + lvNum6 + "') ";
                //    //    lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
                //    //}
                //    //else if (lvType[i] == "อ้อยไฟไหม้รถตัดนอก")
                //    //{
                //    //    lvField2 = "รวมอ้อยไฟไหม้";
                //    //    lvNum1 = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, 9].Text); //จำนวนคัน รวมสด ราง A
                //    //    lvNum2 = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, 10].Text); //จำนวนตัน รวมไฟไหม้ ราง A

                //    //    lvNum3 = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastA, 19].Text); //จำนวนคัน รวมสด ราง B
                //    //    lvNum4 = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastA, 20].Text); //จำนวนตัน รวมไฟไหม้ ราง B

                //    //    lvSQL = "Insert into SysTemp (Field1, Field2, Num1, Num2, Num3, Num4, Num5, Num6) ";//
                //    //    lvSQL += "Values ('" + lvField1 + "', '" + lvField2 + "', '" + lvNum1 + "', '" + lvNum2 + "', '" + lvNum3 + "', '" + lvNum4 + "', '" + lvNum5 + "', '" + lvNum6 + "') ";
                //    //    lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
                //    //}
                //}
                
                //แสดงก่อนพิมพ์
                frmPrint frm = new frmPrint();
                frm.documentViewer1.DocumentSource = typeof(PSQueue.rptDiaryReportAll);
                frm.ShowDialog();

                this.Cursor = Cursors.Default;
                #endregion
            }

            else if (lvTypeReport == "รายงานการหีบอ้อยประจำวัน (แยกประเภท)")
            {
                #region รายงานการหีบอ้อยประจำวัน (แยกประเภท)
                this.Cursor = Cursors.WaitCursor;
                //ลบข้อมูล เก่า
                string lvSQL = "Delete From SysTemp "; //HD
                string lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);

                string[] lvType = new string[] { "อ้อยสด", "อ้อยสด C", "อ้อยสดยอดยาว+ปนเปื้อน", "อ้อยสด-สะอาด", "อ้อยสดรถตัด", "อ้อยสดรถตัดนอก", "อ้อยไฟไหม้", "อ้อยไฟไหม้-สะอาด", "อ้อยไฟไหม้รถตัด", "อ้อยไฟไหม้รถตัดนอก" };

                DateTime DTS = DateTime.Parse(txtDateS.Text + " " + DateTime.Now.ToString("HH:mm:ss"));
                //DTS = DTS.AddDays(1);

                for (int i = 0; i < lvType.Count(); i++)
                {
                    string lvField1 = DTS.ToString();
                    string lvField2 = lvType[i];
                    string lvField3 = "";

                    int lvColCar = 0;
                    int lvColTon = 0;

                    if (lvType[i] == "อ้อยสด")
                    {
                        lvColCar = 1;
                        lvColTon = 2;
                        lvField3 = "1";
                    }
                    if (lvType[i] == "อ้อยสด C")
                    {
                        lvColCar = 23;
                        lvColTon = 24;
                        lvField3 = "1";
                    }
                    if (lvType[i] == "อ้อยสดยอดยาว+ปนเปื้อน")
                    {
                        lvColCar = 25;
                        lvColTon = 26;
                        lvField3 = "1";
                    }
                    if (lvType[i] == "อ้อยสด-สะอาด")
                    {
                        lvColCar = 3;
                        lvColTon = 4;
                        lvField3 = "1";
                    }
                    if (lvType[i] == "อ้อยสดรถตัด")
                    {
                        lvColCar = 5;
                        lvColTon = 6;
                        lvField3 = "1";
                    }
                    if (lvType[i] == "อ้อยสดรถตัดนอก")
                    {
                        lvColCar = 7;
                        lvColTon = 8;
                        lvField3 = "1";
                    }
                    if (lvType[i] == "อ้อยไฟไหม้")
                    {
                        lvColCar = 11;
                        lvColTon = 12;
                        lvField3 = "2";
                    }
                    if (lvType[i] == "อ้อยไฟไหม้-สะอาด")
                    {
                        lvColCar = 13;
                        lvColTon = 14;
                        lvField3 = "2";
                    }
                    if (lvType[i] == "อ้อยไฟไหม้รถตัด")
                    {
                        lvColCar = 15;
                        lvColTon = 16;
                        lvField3 = "2";
                    }
                    if (lvType[i] == "อ้อยไฟไหม้รถตัดนอก")
                    {
                        lvColCar = 17;
                        lvColTon = 18;
                        lvField3 = "2";
                    }

                    int lvLastA = sp1.Sheets[0].RowCount - 1;
                    double lvNum1 = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, lvColCar].Text); //จำนวนคัน ราง A
                    double lvNum2 = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, lvColTon].Text); //จำนวนตัน ราง A

                    //ราง A ถ้าเป็นช่องอ้อยสด ต้องลบยอด อ้อยสด C ออกไปด้วยเนื่องจากในรายงาน อ้อยสด C มีช่องของตัวเอง
                    if (lvType[i] == "อ้อยสด")
                    {
                        double lvNumCaneC = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, 23].Text);
                        double lvSumCaneC = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, 24].Text);
                        double lvNumCaneY = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, 25].Text);
                        double lvSumCaneY = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, 26].Text);

                        lvNum1 = lvNum1 - lvNumCaneC - lvNumCaneY;
                        lvNum2 = lvNum2 - lvSumCaneC - lvSumCaneY;
                    }

                    int lvLastB = sp1.Sheets[1].RowCount - 1;
                    double lvNum3 = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastA, lvColCar].Text); //จำนวนคัน ราง B
                    double lvNum4 = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastA, lvColTon].Text); //จำนวนตัน ราง B

                    //ราง B ถ้าเป็นช่องอ้อยสด ต้องลบยอด อ้อยสด C ออกไปด้วยเนื่องจากในรายงาน อ้อยสด C มีช่องของตัวเอง
                    if (lvType[i] == "อ้อยสด")
                    {
                        double lvNumCaneC = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastB, 23].Text);
                        double lvSumCaneC = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastB, 24].Text);
                        double lvNumCaneY = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastB, 25].Text);
                        double lvSumCaneY = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastB, 26].Text);

                        lvNum3 = lvNum3 - lvNumCaneC - lvNumCaneY;
                        lvNum4 = lvNum4 - lvSumCaneC - lvSumCaneY;
                    }

                    double lvNum5 = lvNum1 + lvNum3; //รวม คัน
                    double lvNum6 = lvNum2 + lvNum4; //รวม ตัน



                    lvSQL = "Insert into SysTemp (Field1, Field2, Field3, Num1, Num2, Num3, Num4, Num5, Num6) ";//
                    lvSQL += "Values ('" + lvField1 + "', '" + lvField2 + "', '" + lvField3 + "', '" + lvNum1 + "', '" + lvNum2 + "', '" + lvNum3 + "', '" + lvNum4 + "', '" + lvNum5 + "', '" + lvNum6 + "') ";
                    if (lvNum5 + lvNum6 > 0) lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);//

                    ////ทำตัวรวมแยกอ้อยสด - ไฟไหม้
                    //if (lvType[i] == "อ้อยสดรถตัดนอก")
                    //{
                    //    lvField2 = "รวมอ้อยสด";
                    //    lvNum1 = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, 9].Text); //จำนวนคัน รวมสด ราง A
                    //    lvNum2 = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, 10].Text); //จำนวนตัน รวมไฟไหม้ ราง A

                    //    lvNum3 = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastA, 19].Text); //จำนวนคัน รวมสด ราง B
                    //    lvNum4 = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastA, 20].Text); //จำนวนตัน รวมไฟไหม้ ราง B

                    //    lvSQL = "Insert into SysTemp (Field1, Field2, Num1, Num2, Num3, Num4, Num5, Num6) ";//
                    //    lvSQL += "Values ('" + lvField1 + "', '" + lvField2 + "', '" + lvNum1 + "', '" + lvNum2 + "', '" + lvNum3 + "', '" + lvNum4 + "', '" + lvNum5 + "', '" + lvNum6 + "') ";
                    //    lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
                    //}
                    //else if (lvType[i] == "อ้อยไฟไหม้รถตัดนอก")
                    //{
                    //    lvField2 = "รวมอ้อยไฟไหม้";
                    //    lvNum1 = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, 9].Text); //จำนวนคัน รวมสด ราง A
                    //    lvNum2 = Gstr.fncToDouble(sp1.Sheets[0].Cells[lvLastA, 10].Text); //จำนวนตัน รวมไฟไหม้ ราง A

                    //    lvNum3 = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastA, 19].Text); //จำนวนคัน รวมสด ราง B
                    //    lvNum4 = Gstr.fncToDouble(sp1.Sheets[1].Cells[lvLastA, 20].Text); //จำนวนตัน รวมไฟไหม้ ราง B

                    //    lvSQL = "Insert into SysTemp (Field1, Field2, Num1, Num2, Num3, Num4, Num5, Num6) ";//
                    //    lvSQL += "Values ('" + lvField1 + "', '" + lvField2 + "', '" + lvNum1 + "', '" + lvNum2 + "', '" + lvNum3 + "', '" + lvNum4 + "', '" + lvNum5 + "', '" + lvNum6 + "') ";
                    //    lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
                    //}
                }

                //แสดงก่อนพิมพ์
                frmPrint frm = new frmPrint();
                frm.documentViewer1.DocumentSource = typeof(PSQueue.rptReportSumTypeCane);
                frm.ShowDialog();

                this.Cursor = Cursors.Default;
                #endregion
            }
            else if (lvTypeReport == "รายงานรถตัดประจำวัน")
            {
                #region รายงานการหีบอ้อยรถตัดประจำวัน
                this.Cursor = Cursors.WaitCursor;
                //ลบข้อมูล เก่า
                string lvSQL = "Delete From SysTemp "; //HD
                string lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);

                for (int i = 0; i < sp1.Sheets["รถตัด"].RowCount; i++)
                {
                    string lvField1 = sp1.Sheets["รถตัด"].Cells[i, 0].Text; //โควต้า
                    string lvField2 = sp1.Sheets["รถตัด"].Cells[i, 1].Text; //ทะเบียนรถ
                    string lvField3 = sp1.Sheets["รถตัด"].Cells[i, 2].Text; //เลขที่บิล
                    string lvField4 = sp1.Sheets["รถตัด"].Cells[i, 3].Text; //เลขที่คิวรถตัด
                    double lvNum1 = Gstr.fncToDouble(sp1.Sheets["รถตัด"].Cells[i, 5].Text); //นน.รวม
                    double lvNum2 = Gstr.fncToDouble(sp1.Sheets["รถตัด"].Cells[i, 6].Text); //นน.รถ
                    double lvNum3 = Gstr.fncToDouble(sp1.Sheets["รถตัด"].Cells[i, 7].Text); //นน.สุทธิ
                    double lvNum4 = Gstr.fncToDouble(sp1.Sheets["รถตัด"].Cells[i, 12].Text); //เลขที่บิลเอาไว้เรียงข้อมูล
                    string lvField5 = sp1.Sheets["รถตัด"].Cells[i, 8].Text; //เวลาออก
                    string lvField6 = txtDateS.Text; //วันที่
                    string lvField7 = sp1.Sheets["รถตัด"].Cells[i, 4].Text; //ใบนำตัด
                    string lvField8 = "รวมทั้งหมด " + txtCountCar.Text + " คัน"; //จำนวนคัน


                    lvSQL = "Insert into SysTemp (Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Num1, Num2, Num3, Num4) ";//
                    lvSQL += "Values ('" + lvField1 + "', '" + lvField2 + "', '" + lvField3 + "', '" + lvField4 + "', '" + lvField5 + "', '" + lvField6 + "', '" + lvField7 + "', '" + lvField8 + "', '" + lvNum1 + "', '" + lvNum2 + "', '" + lvNum3 + "', '" + lvNum4 + "') ";
                    lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
                }

                //แสดงก่อนพิมพ์
                frmPrint frm = new frmPrint();
                frm.documentViewer1.DocumentSource = typeof(PSQueue.rptCutCar);
                frm.ShowDialog();

                this.Cursor = Cursors.Default;
                #endregion
            }
            else if (lvTypeReport == "รายงานโควต้าที่ไม่ได้ลงทะเบียนชาวไร่")
            {
                #region รายงานโควต้าที่ไม่ได้ลงทะเบียนชาวไร่
                this.Cursor = Cursors.WaitCursor;

                string lvSQL = "Delete From SysTemp "; //HD
                string lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
                DataTable DT = new DataTable();
                string lvDateS = Gstr.fncChangeTDate(txtDateS.Text); 
                string lvDateE = Gstr.fncChangeTDate(txtDatePrint.Text); 
               
                lvSQL = "Select * From Queue_Diary Where Q_SubQuota in ( '2312', '6678', '7092', '7112', '7478', '7644', '7657', '7756', '7757', '7793', '7817', '7903', '8409', '8439', '8443', '8490', '8502', '8525', '8869', '9099', '9116', '9132') And CONVERT(datetime, Q_WeightOUTDate + ' ' + Q_WeightOUTTime, 103) >= CONVERT(datetime, '" + (Gstr.fncToInt(lvDateS) - 1).ToString() + "' + ' 15:00:00', 103) and CONVERT(datetime, Q_WeightOUTDate + ' ' + Q_WeightOUTTime, 103) < CONVERT(datetime, '" + lvDateS + "' + ' 15:00:00', 103) And Q_CloseStatus = '1' ";
                DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    string lvQ_BillingNo = DT.Rows[i]["Q_BillingNo"].ToString(); //เลขที่บิล
                    string lvQuota = DT.Rows[i]["Q_SubQuota"].ToString(); //โควต้า
                    string lvQ_SampleNo = DT.Rows[i]["Q_SampleNo"].ToString(); //เลขที่ตัวอย่าง
                    string lvQ_Date = Gstr.fncChangeSDate(DT.Rows[i]["Q_Date"].ToString()); //วันที่
                    string lvQ_Time = DT.Rows[i]["Q_Time"].ToString(); //เวลา
                    string lvDateTime = lvQ_Date + " " + lvQ_Time;
                    string lvQ_No = DT.Rows[i]["Q_No"].ToString(); //คิวที่
                    string lvQ_TKNo = DT.Rows[i]["Q_TKNo"].ToString(); //ตะกาว
                    string lvQ_CarNum = DT.Rows[i]["Q_CarNum"].ToString(); //ทะเบียนรถ
                    string lvQ_CarNum2 = DT.Rows[i]["Q_CarNum2"].ToString(); //ทะเบียนรถพ่วง
                    string lvQ_CarAll = "";

                    if (lvQ_CarNum2 != "" && lvQ_CarNum2 != null)
                    {
                        lvQ_CarAll = lvQ_CarNum + " - " + lvQ_CarNum2;
                    }
                    else
                    {
                        lvQ_CarAll = lvQ_CarNum;
                    }

                    string lvQ_CarType = DT.Rows[i]["Q_CarType"].ToString(); //ประเภทรถ
                    string lvQ_WeightIn = Gstr.fncToInt(DT.Rows[i]["Q_WeightIN"].ToString()).ToString("#,##0.00"); //น้ำหนักชั่งเข้า
                    string lvQ_WeightInDate = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightINDate"].ToString());
                    string lvQ_WeightOut = Gstr.fncToInt(DT.Rows[i]["Q_WeightOUT"].ToString()).ToString("#,##0.00"); //น้ำหนักชั่งออก
                    string lvQ_WeightOutDate = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightOUTDate"].ToString());
                    string lvQ_WeightAll = (Gstr.fncToInt(lvQ_WeightIn) - Gstr.fncToInt(lvQ_WeightOut)).ToString("#,##0.00"); //สุทธิ
                    string lvQ_CloseStatus = DT.Rows[i]["Q_CloseStatus"].ToString(); //สถานะ

                    lvSQL = "Insert Into SysTemp (Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14)" +
                        " Values ('" + lvQ_BillingNo + "', '" + lvQuota + "', '" + lvQ_SampleNo + "', '" + lvDateTime + "', '" + lvQ_No + "', '" + lvQ_TKNo + "', '" + lvQ_CarAll + "', " +
                        "'" + lvQ_CarType + "', '" + lvQ_WeightIn + "', '" + lvQ_WeightOut + "', '" + lvQ_WeightAll + "', '" + lvQ_CloseStatus + "', '" + lvQ_WeightInDate + "', '" + lvQ_WeightOutDate + "')";
                    lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
                }

                //แสดงก่อนพิมพ์
                frmSubQuotaPrint frm = new frmSubQuotaPrint();
                frm.ShowDialog();

                this.Cursor = Cursors.Default;
                #endregion
            }
        }

        private double fncSumDataByColumns(int sheet, int lvCol)
        {
            double lvReturn = 0;

            for (int i = 0; i < sp1.Sheets[sheet].RowCount; i++)
            {
                if (sp1.Sheets[sheet].Cells[i, 0].Text != "รวม")
                    lvReturn += Gstr.fncToDouble(sp1.Sheets[sheet].Cells[i, lvCol].Text);
            }

            return lvReturn;
        }

        private void pnPrintMenu_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Text == "- แบบรวม (ราง A และ ราง B)")
            {
                FncPrintReportALL("ALL");
                pnMenuPrint.Visible = false;
            }
            else if (e.Node.Text == "- กะดึก")
            {
                FncPrintReportALL("Night");
                pnMenuPrint.Visible = false;
            }
            else if (e.Node.Text == "- กะเช้า")
            {
                FncPrintReportALL("Day");
                pnMenuPrint.Visible = false;
            }
            else if (e.Node.Text == "- แบบละเอียด (A3)")
            {
                แบบละเอยดToolStripMenuItem_Click(sender, e);
                pnMenuPrint.Visible = false;
            }
            else if (e.Node.Text == "รายงานหีบอ้อยประจำวัน")
            {
                fncPrintReport("รายงานการหีบอ้อยประจำวัน");
                pnMenuPrint.Visible = false;
            }
            else if (e.Node.Text == "รายงานหีบอ้อยประจำวัน (แยกประเภท)")
            {
                if (!ChkAll.Checked)
                {
                    MessageBox.Show("กรุณาเลือกดูแบบทั้งหมด ราง A และ ราง B", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                fncPrintReport("รายงานการหีบอ้อยประจำวัน (แยกประเภท)");
                pnMenuPrint.Visible = false;
            }
            else if (e.Node.Text == "รายงานรถตัด ประจำวัน")
            {
                fncPrintReport("รายงานรถตัดประจำวัน");
                pnMenuPrint.Visible = false;
            }
            else if (e.Node.Text == "รายงานโควต้าที่ไม่ได้ลงทะเบียนชาวไร่")
            {
                fncPrintReport("รายงานโควต้าที่ไม่ได้ลงทะเบียนชาวไร่");
                pnMenuPrint.Visible = false;
            }
            else if (e.Node.Text == "ตันอ้อยรวม (กะเช้า)")
            {
                //แสดงก่อนพิมพ์
                GVar.gvPrintMode = "Day";

                frmPrint frm = new frmPrint();
                frm.documentViewer1.DocumentSource = typeof(PSQueue.rptHourA);
                frm.ShowDialog();
                pnMenuPrint.Visible = false;
            }
            else if (e.Node.Text == "ตันอ้อยรวม (กะดึก)")
            {
                //แสดงก่อนพิมพ์
                GVar.gvPrintMode = "Night";

                frmPrint frm = new frmPrint();
                frm.documentViewer1.DocumentSource = typeof(PSQueue.rptHourA);
                frm.ShowDialog();
                pnMenuPrint.Visible = false;
            }

        }

        private void btnClosePrintMenu_Click(object sender, EventArgs e)
        {
            pnMenuPrint.Visible = false;
        }

        private void frmShowReport_Load(object sender, EventArgs e)
        {
            //เช็ค Desktop Size
            int lvDeskWidth = SystemInformation.VirtualScreen.Width;
            int lvDeskHeight = SystemInformation.VirtualScreen.Height;

            if (lvDeskWidth == 1280)// && lvDeskHeight >= 1024
            {
                this.WindowState = FormWindowState.Maximized;
                this.MaximumSize = new Size(1280, 1024);
                this.MinimumSize = new Size(1280, 1024);
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.MaximumSize = new Size(1280, 1024);
                this.MinimumSize = new Size(1280, 1024);
                this.StartPosition = FormStartPosition.CenterScreen;
            }
        }

        private void ChkA_CheckedChanged(object sender, EventArgs e)
        {
            //btnSearch_Click(sender, e);
        }

        private void ChkB_CheckedChanged(object sender, EventArgs e)
        {
            //btnSearch_Click(sender, e);
        }

        private void ChkAll_CheckedChanged(object sender, EventArgs e)
        {
            //btnSearch_Click(sender, e);
        }

        private void txtDateS_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime DTNew = DateTime.Parse(txtDateS.Text);
                DTNew = DTNew.AddDays(1);

                txtDatePrint.Text = DTNew.ToString("dd/MM/yyyy");
            }
            catch
            {

            }
        }

        private void rdKa1_CheckedChanged(object sender, EventArgs e)
        {
            //btnSearch_Click(sender, e);
        }

        private void lvKa2_CheckedChanged(object sender, EventArgs e)
        {
            //btnSearch_Click(sender, e);
        }

        private void rdKaAll_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ChkShowCutCar_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkShowCutCar.Checked)
            {
                ChkAll.Checked = true;
                rdKaAll.Checked = true;
            }
        }

        private void frmShowReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (GVar.gvUser == "psview" || GVar.gvUser == "Noo")
                  Application.Exit();
        }
    }
}
