using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace PSQueue
{
    public partial class rptHourA : DevExpress.XtraReports.UI.XtraReport
    {
        public rptHourA()
        {
            InitializeComponent();
        }

        private void rptHourA_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if(GVar.gvPrintMode == "Day")
            {
                xrLabel17.Text = "07.01-08.00";
                xrLabel21.Text = "08.01-09.00";
                xrLabel24.Text = "09.01-10.00";
                xrLabel28.Text = "10.01-11.00";
                xrLabel32.Text = "11.01-12.00";
                xrLabel36.Text = "12.01-13.00";
                xrLabel39.Text = "13.01-14.00";
                xrLabel40.Text = "14.01-15.00";
                xrLabel42.Text = "15.01-16.00";
                xrLabel44.Text = "16.01-17.00";
                xrLabel45.Text = "17.01-18.00";
                xrLabel46.Text = "18.01-19.00";

                fncLoadData("07.01-08.00", xrLabel18, xrLabel19, xrLabel20, xrLabel22, xrLabel23, xrLabel25);
                fncLoadData("08.01-09.00", xrLabel47, xrLabel60, xrLabel71, xrLabel103, xrLabel114, xrLabel115);
                fncLoadData("09.01-10.00", xrLabel48, xrLabel61, xrLabel72, xrLabel102, xrLabel113, xrLabel116);
                fncLoadData("10.01-11.00", xrLabel51, xrLabel64, xrLabel75, xrLabel99, xrLabel110, xrLabel119);
                fncLoadData("11.01-12.00", xrLabel52, xrLabel63, xrLabel74, xrLabel100, xrLabel111, xrLabel118);
                fncLoadData("12.01-13.00", xrLabel53, xrLabel62, xrLabel73, xrLabel101, xrLabel112, xrLabel117);
                fncLoadData("13.01-14.00", xrLabel54, xrLabel70, xrLabel81, xrLabel93, xrLabel104, xrLabel125);
                fncLoadData("14.01-15.00", xrLabel55, xrLabel69, xrLabel80, xrLabel94, xrLabel105, xrLabel124);
                fncLoadData("15.01-16.00", xrLabel56, xrLabel68, xrLabel79, xrLabel95, xrLabel106, xrLabel123);
                fncLoadData("16.01-17.00", xrLabel59, xrLabel65, xrLabel76, xrLabel98, xrLabel109, xrLabel120);
                fncLoadData("17.01-18.00", xrLabel58, xrLabel66, xrLabel77, xrLabel97, xrLabel108, xrLabel121);
                fncLoadData("18.01-19.00", xrLabel57, xrLabel67, xrLabel78, xrLabel96, xrLabel107, xrLabel122);
                

            }
            else
            {
                xrLabel17.Text = "19.01-20.00";
                xrLabel21.Text = "20.01-21.00";
                xrLabel24.Text = "21.01-22.00";
                xrLabel28.Text = "22.01-23.00";
                xrLabel32.Text = "23.01-00.00";
                xrLabel36.Text = "00.01-01.00";
                xrLabel39.Text = "01.01-02.00";
                xrLabel40.Text = "02.01-03.00";
                xrLabel42.Text = "03.01-04.00";
                xrLabel44.Text = "04.01-05.00";
                xrLabel45.Text = "05.01-06.00";
                xrLabel46.Text = "06.01-07.00";

                fncLoadData("19.01-20.00", xrLabel18, xrLabel19, xrLabel20, xrLabel22, xrLabel23, xrLabel25);
                fncLoadData("20.01-21.00", xrLabel47, xrLabel60, xrLabel71, xrLabel103, xrLabel114, xrLabel115);
                fncLoadData("21.01-22.00", xrLabel48, xrLabel61, xrLabel72, xrLabel102, xrLabel113, xrLabel116);
                fncLoadData("22.01-23.00", xrLabel51, xrLabel64, xrLabel75, xrLabel99, xrLabel110, xrLabel119);
                fncLoadData("23.01-00.00", xrLabel52, xrLabel63, xrLabel74, xrLabel100, xrLabel111, xrLabel118);
                fncLoadData("00.01-01.00", xrLabel53, xrLabel62, xrLabel73, xrLabel101, xrLabel112, xrLabel117);
                fncLoadData("01.01-02.00", xrLabel54, xrLabel70, xrLabel81, xrLabel93, xrLabel104, xrLabel125);
                fncLoadData("02.01-03.00", xrLabel55, xrLabel69, xrLabel80, xrLabel94, xrLabel105, xrLabel124);
                fncLoadData("03.01-04.00", xrLabel56, xrLabel68, xrLabel79, xrLabel95, xrLabel106, xrLabel123);
                fncLoadData("04.01-05.00", xrLabel59, xrLabel65, xrLabel76, xrLabel98, xrLabel109, xrLabel120);
                fncLoadData("05.01-06.00", xrLabel58, xrLabel66, xrLabel77, xrLabel97, xrLabel108, xrLabel121);
                fncLoadData("06.01-07.00", xrLabel57, xrLabel67, xrLabel78, xrLabel96, xrLabel107, xrLabel122);
            }
            

            fncSumCar();
            fncSumAll();
            fncSumNetweight();
        }

        private void fncLoadData(string lvTime, XRLabel lbText1, XRLabel lbText2, XRLabel lbText3, XRLabel lbText4, XRLabel lbText5, XRLabel lbText6)
        {
            //โหลดข้อมูล
            DataTable DT = new DataTable();
            string lvSQL = "Select * From SysTemp Where Field1 = '" + lvTime + "' ";
            DT = GsysSQL.fncGetQueryDataAccess(lvSQL, DT);

            int lvNumRow = DT.Rows.Count;
            
            for (int i = 0; i < lvNumRow; i++)
            {
                string lvNum1 = DT.Rows[i]["Num1"].ToString();
                string lvNum2 = DT.Rows[i]["Num2"].ToString();
                string lvNum3 = DT.Rows[i]["Num3"].ToString();
                string lvNum4 = DT.Rows[i]["Num4"].ToString();
                string lvNum5 = DT.Rows[i]["Num5"].ToString();
                string lvNum6 = DT.Rows[i]["Num6"].ToString();
                string lvNum7 = DT.Rows[i]["Num7"].ToString();
                string lvNum8 = DT.Rows[i]["Num8"].ToString();
                string lvNum9 = DT.Rows[i]["Num9"].ToString();


                xrLabel50.Text = DT.Rows[i]["Field2"].ToString();
                lbText1.Text = lvNum1;
                lbText2.Text = lvNum2;
                lbText3.Text = (Gstr.fncToDouble(lbText1.Text) + Gstr.fncToDouble(lbText2.Text)).ToString("#,##0");
                lbText4.Text = lvNum3;
                lbText5.Text = lvNum4;
                lbText6.Text = (Gstr.fncToDouble(lbText4.Text) + Gstr.fncToDouble(lbText5.Text)).ToString("#,##0.00");
            }
        }

        private void fncSumCar()
        {
            string lv1516 = fncFindCarCount("15.01-16.00");
            string lv1617 = fncFindCarCount("16.01-17.00");
            string lv1718 = fncFindCarCount("17.01-18.00");
            string lv1819 = fncFindCarCount("18.01-19.00");
            string lv1920 = fncFindCarCount("19.01-20.00");
            string lv2021 = fncFindCarCount("20.01-21.00");
            string lv2122 = fncFindCarCount("21.01-22.00");
            string lv2223 = fncFindCarCount("22.01-23.00");
            string lv2300 = fncFindCarCount("23.01-00.00");
            string lv0001 = fncFindCarCount("00.01-01.00");
            string lv0102 = fncFindCarCount("01.01-02.00");
            string lv0203 = fncFindCarCount("02.01-03.00");
            string lv0304 = fncFindCarCount("03.01-04.00");
            string lv0405 = fncFindCarCount("04.01-05.00");
            string lv0506 = fncFindCarCount("05.01-06.00");
            string lv0607 = fncFindCarCount("06.01-07.00");
            string lv0708 = fncFindCarCount("07.01-08.00");
            string lv0809 = fncFindCarCount("08.01-09.00");
            string lv0910 = fncFindCarCount("09.01-10.00");
            string lv1011 = fncFindCarCount("10.01-11.00");
            string lv1112 = fncFindCarCount("11.01-12.00");
            string lv1213 = fncFindCarCount("12.01-13.00");
            string lv1314 = fncFindCarCount("13.01-14.00");
            string lv1415 = fncFindCarCount("14.01-15.00");

            if(GVar.gvPrintMode == "Day")
            {
                if
                    (xrLabel79.Text == "0") xrLabel90.Text = "0";
                else
                    xrLabel90.Text = lv1516;

                if
                    (xrLabel76.Text == "0") xrLabel87.Text = "0";
                else
                    xrLabel87.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617)).ToString();

                if
                   (xrLabel77.Text == "0") xrLabel88.Text = "0";
                else
                    xrLabel88.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718)).ToString();

                if
                  (xrLabel78.Text == "0") xrLabel89.Text = "0";
                else
                    xrLabel89.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819)).ToString();

                if
                  (xrLabel20.Text == "0") xrLabel13.Text = "0";
                else
                    xrLabel13.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToInt(lv2122) + Gstr.fncToInt(lv2223) + Gstr.fncToInt(lv2300) + Gstr.fncToInt(lv0001) + Gstr.fncToInt(lv0102) + Gstr.fncToInt(lv0203) + Gstr.fncToInt(lv0304) + Gstr.fncToInt(lv0405) + Gstr.fncToInt(lv0506) + Gstr.fncToInt(lv0607) + Gstr.fncToInt(lv0708)).ToString();

                if
                    (xrLabel71.Text == "0") xrLabel82.Text = "0";
                else
                    xrLabel82.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToInt(lv2122) + Gstr.fncToInt(lv2223) + Gstr.fncToInt(lv2300) + Gstr.fncToInt(lv0001) + Gstr.fncToInt(lv0102) + Gstr.fncToInt(lv0203) + Gstr.fncToInt(lv0304) + Gstr.fncToInt(lv0405) + Gstr.fncToInt(lv0506) + Gstr.fncToInt(lv0607) + Gstr.fncToInt(lv0708) + Gstr.fncToInt(lv0809)).ToString();

                if
                    (xrLabel72.Text == "0") xrLabel83.Text = "0";
                else
                    xrLabel83.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToInt(lv2122) + Gstr.fncToInt(lv2223) + Gstr.fncToInt(lv2300) + Gstr.fncToInt(lv0001) + Gstr.fncToInt(lv0102) + Gstr.fncToInt(lv0203) + Gstr.fncToInt(lv0304) + Gstr.fncToInt(lv0405) + Gstr.fncToInt(lv0506) + Gstr.fncToInt(lv0607) + Gstr.fncToInt(lv0708) + Gstr.fncToInt(lv0809) + Gstr.fncToInt(lv0910)).ToString();

                if
                    (xrLabel75.Text == "0") xrLabel86.Text = "0";
                else
                    xrLabel86.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToInt(lv2122) + Gstr.fncToInt(lv2223) + Gstr.fncToInt(lv2300) + Gstr.fncToInt(lv0001) + Gstr.fncToInt(lv0102) + Gstr.fncToInt(lv0203) + Gstr.fncToInt(lv0304) + Gstr.fncToInt(lv0405) + Gstr.fncToInt(lv0506) + Gstr.fncToInt(lv0607) + Gstr.fncToInt(lv0708) + Gstr.fncToInt(lv0809) + Gstr.fncToInt(lv0910) + Gstr.fncToInt(lv1011)).ToString();

                if
                    (xrLabel74.Text == "0") xrLabel85.Text = "0";
                else
                    xrLabel85.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToInt(lv2122) + Gstr.fncToInt(lv2223) + Gstr.fncToInt(lv2300) + Gstr.fncToInt(lv0001) + Gstr.fncToInt(lv0102) + Gstr.fncToInt(lv0203) + Gstr.fncToInt(lv0304) + Gstr.fncToInt(lv0405) + Gstr.fncToInt(lv0506) + Gstr.fncToInt(lv0607) + Gstr.fncToInt(lv0708) + Gstr.fncToInt(lv0809) + Gstr.fncToInt(lv0910) + Gstr.fncToInt(lv1011) + Gstr.fncToInt(lv1112)).ToString();

                if
                    (xrLabel73.Text == "0") xrLabel84.Text = "0";
                else
                    xrLabel84.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToInt(lv2122) + Gstr.fncToInt(lv2223) + Gstr.fncToInt(lv2300) + Gstr.fncToInt(lv0001) + Gstr.fncToInt(lv0102) + Gstr.fncToInt(lv0203) + Gstr.fncToInt(lv0304) + Gstr.fncToInt(lv0405) + Gstr.fncToInt(lv0506) + Gstr.fncToInt(lv0607) + Gstr.fncToInt(lv0708) + Gstr.fncToInt(lv0809) + Gstr.fncToInt(lv0910) + Gstr.fncToInt(lv1011) + Gstr.fncToInt(lv1112) + Gstr.fncToInt(lv1213)).ToString();

                if
                    (xrLabel81.Text == "0") xrLabel92.Text = "0";
                else
                    xrLabel92.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToInt(lv2122) + Gstr.fncToInt(lv2223) + Gstr.fncToInt(lv2300) + Gstr.fncToInt(lv0001) + Gstr.fncToInt(lv0102) + Gstr.fncToInt(lv0203) + Gstr.fncToInt(lv0304) + Gstr.fncToInt(lv0405) + Gstr.fncToInt(lv0506) + Gstr.fncToInt(lv0607) + Gstr.fncToInt(lv0708) + Gstr.fncToInt(lv0809) + Gstr.fncToInt(lv0910) + Gstr.fncToInt(lv1011) + Gstr.fncToInt(lv1112) + Gstr.fncToInt(lv1213) + Gstr.fncToInt(lv1314)).ToString();

                if
                   (xrLabel80.Text == "0") xrLabel91.Text = "0";
                else
                    xrLabel91.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToInt(lv2122) + Gstr.fncToInt(lv2223) + Gstr.fncToInt(lv2300) + Gstr.fncToInt(lv0001) + Gstr.fncToInt(lv0102) + Gstr.fncToInt(lv0203) + Gstr.fncToInt(lv0304) + Gstr.fncToInt(lv0405) + Gstr.fncToInt(lv0506) + Gstr.fncToInt(lv0607) + Gstr.fncToInt(lv0708) + Gstr.fncToInt(lv0809) + Gstr.fncToInt(lv0910) + Gstr.fncToInt(lv1011) + Gstr.fncToInt(lv1112) + Gstr.fncToInt(lv1213) + Gstr.fncToInt(lv1314) + Gstr.fncToInt(lv1415)).ToString();
            }
            else
            {
                if
                  (xrLabel20.Text == "0") xrLabel13.Text = "0";
                else
                    xrLabel13.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819) + Gstr.fncToInt(lv1920)).ToString();

                if
                 (xrLabel71.Text == "0") xrLabel82.Text = "0";
                else
                    xrLabel82.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToInt(lv2021)).ToString();

                if
                 (xrLabel72.Text == "0") xrLabel83.Text = "0";
                else
                    xrLabel83.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToInt(lv2021) + Gstr.fncToInt(lv2122)).ToString();

                if
                    (xrLabel72.Text == "0") xrLabel83.Text = "0";
                else
                    xrLabel83.Text = (Gstr.fncToInt(lv1516) + Gstr.fncToInt(lv1617) + Gstr.fncToInt(lv1718) + Gstr.fncToInt(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToInt(lv2021) + Gstr.fncToInt(lv2122)).ToString();
            }
        }

        private void fncSumAll()
        {
            //รวมและรวมทั้งหมด

            //รถออกราง A
            double lvLA1 = Gstr.fncToDouble(xrLabel18.Text);
            double lvLA2 = Gstr.fncToDouble(xrLabel47.Text);
            double lvLA3 = Gstr.fncToDouble(xrLabel48.Text);
            double lvLA4 = Gstr.fncToDouble(xrLabel51.Text);
            double lvLA5 = Gstr.fncToDouble(xrLabel52.Text);
            double lvLA6 = Gstr.fncToDouble(xrLabel53.Text);
            double lvLA7 = Gstr.fncToDouble(xrLabel54.Text);
            double lvLA8 = Gstr.fncToDouble(xrLabel55.Text);
            double lvLA9 = Gstr.fncToDouble(xrLabel56.Text);
            double lvLA10 = Gstr.fncToDouble(xrLabel59.Text);
            double lvLA11 = Gstr.fncToDouble(xrLabel58.Text);
            double lvLA12 = Gstr.fncToDouble(xrLabel57.Text);
            xrLabel26.Text = (lvLA1 + lvLA2 + lvLA3 + lvLA4 + lvLA5 + lvLA6 + lvLA7 + lvLA8 + lvLA9 + lvLA10 + lvLA11 + lvLA12).ToString("#,##0");

            //รถออกราง B
            double lvLB1 = Gstr.fncToDouble(xrLabel19.Text);
            double lvLB2 = Gstr.fncToDouble(xrLabel60.Text);
            double lvLB3 = Gstr.fncToDouble(xrLabel61.Text);
            double lvLB4 = Gstr.fncToDouble(xrLabel64.Text);
            double lvLB5 = Gstr.fncToDouble(xrLabel63.Text);
            double lvLB6 = Gstr.fncToDouble(xrLabel62.Text);
            double lvLB7 = Gstr.fncToDouble(xrLabel70.Text);
            double lvLB8 = Gstr.fncToDouble(xrLabel69.Text);
            double lvLB9 = Gstr.fncToDouble(xrLabel68.Text);
            double lvLB10 = Gstr.fncToDouble(xrLabel65.Text);
            double lvLB11 = Gstr.fncToDouble(xrLabel66.Text);
            double lvLB12 = Gstr.fncToDouble(xrLabel67.Text);
            xrLabel27.Text = (lvLB1 + lvLB2 + lvLB3 + lvLB4 + lvLB5 + lvLB6 + lvLB7 + lvLB8 + lvLB9 + lvLB10 + lvLB11 + lvLB12).ToString("#,##0");

            //รวมทั้งหมดรถออก A,B
            xrLabel43.Text = (Gstr.fncToInt(xrLabel26.Text) + Gstr.fncToInt(xrLabel27.Text)).ToString("#,##0");

            //รวมรถออกสะสมราง A,B
            xrLabel16.Text = xrLabel43.Text;

            //ตันอ้อยรวท/ชม. ราง A
            double lvTA1 = Gstr.fncToDouble(xrLabel22.Text);
            double lvTA2 = Gstr.fncToDouble(xrLabel103.Text);
            double lvTA3 = Gstr.fncToDouble(xrLabel102.Text);
            double lvTA4 = Gstr.fncToDouble(xrLabel99.Text);
            double lvTA5 = Gstr.fncToDouble(xrLabel100.Text);
            double lvTA6 = Gstr.fncToDouble(xrLabel101.Text);
            double lvTA7 = Gstr.fncToDouble(xrLabel93.Text);
            double lvTA8 = Gstr.fncToDouble(xrLabel94.Text);
            double lvTA9 = Gstr.fncToDouble(xrLabel95.Text);
            double lvTA10 = Gstr.fncToDouble(xrLabel98.Text);
            double lvTA11 = Gstr.fncToDouble(xrLabel97.Text);
            double lvTA12 = Gstr.fncToDouble(xrLabel96.Text);
            xrLabel31.Text = (lvTA1 + lvTA2 + lvTA3 + lvTA4 + lvTA5 + lvTA6 + lvTA7 + lvTA8 + lvTA9 + lvTA10 + lvTA11 + lvTA12).ToString("#,##0.00");

            //ตันอ้อยรวท/ชม. ราง B
            double lvTB1 = Gstr.fncToDouble(xrLabel23.Text);
            double lvTB2 = Gstr.fncToDouble(xrLabel114.Text);
            double lvTB3 = Gstr.fncToDouble(xrLabel113.Text);
            double lvTB4 = Gstr.fncToDouble(xrLabel110.Text);
            double lvTB5 = Gstr.fncToDouble(xrLabel111.Text);
            double lvTB6 = Gstr.fncToDouble(xrLabel112.Text);
            double lvTB7 = Gstr.fncToDouble(xrLabel104.Text);
            double lvTB8 = Gstr.fncToDouble(xrLabel105.Text);
            double lvTB9 = Gstr.fncToDouble(xrLabel106.Text);
            double lvTB10 = Gstr.fncToDouble(xrLabel109.Text);
            double lvTB11 = Gstr.fncToDouble(xrLabel108.Text);
            double lvTB12 = Gstr.fncToDouble(xrLabel107.Text);
            xrLabel30.Text = (lvTB1 + lvTB2 + lvTB3 + lvTB4 + lvTB5 + lvTB6 + lvTB7 + lvTB8 + lvTB9 + lvTB10 + lvTB11 + lvTB12).ToString("#,##0.00");

            //รวมทั้งหมดตันอ้อย A,B
            xrLabel38.Text = (Gstr.fncToDouble(xrLabel31.Text) + Gstr.fncToDouble(xrLabel30.Text)).ToString("#,##0.00");
            xrLabel127.Text = xrLabel38.Text;
        }

        private void fncSumNetweight()
        {
            string lv1516 = fncFindNetWeight("15.01-16.00");
            string lv1617 = fncFindNetWeight("16.01-17.00");
            string lv1718 = fncFindNetWeight("17.01-18.00");
            string lv1819 = fncFindNetWeight("18.01-19.00");
            string lv1920 = fncFindNetWeight("19.01-20.00");
            string lv2021 = fncFindNetWeight("20.01-21.00");
            string lv2122 = fncFindNetWeight("21.01-22.00");
            string lv2223 = fncFindNetWeight("22.01-23.00");
            string lv2300 = fncFindNetWeight("23.01-00.00");
            string lv0001 = fncFindNetWeight("00.01-01.00");
            string lv0102 = fncFindNetWeight("01.01-02.00");
            string lv0203 = fncFindNetWeight("02.01-03.00");
            string lv0304 = fncFindNetWeight("03.01-04.00");
            string lv0405 = fncFindNetWeight("04.01-05.00");
            string lv0506 = fncFindNetWeight("05.01-06.00");
            string lv0607 = fncFindNetWeight("06.01-07.00");
            string lv0708 = fncFindNetWeight("07.01-08.00");
            string lv0809 = fncFindNetWeight("08.01-09.00");
            string lv0910 = fncFindNetWeight("09.01-10.00");
            string lv1011 = fncFindNetWeight("10.01-11.00");
            string lv1112 = fncFindNetWeight("11.01-12.00");
            string lv1213 = fncFindNetWeight("12.01-13.00");
            string lv1314 = fncFindNetWeight("13.01-14.00");
            string lv1415 = fncFindNetWeight("14.01-15.00");

            if(GVar.gvPrintMode == "Day")
            {
                if
                   (xrLabel120.Text == "0.00") xrLabel136.Text = "0.00";
                else
                    xrLabel136.Text = lv1516;

                if
                   (xrLabel120.Text == "0.00") xrLabel133.Text = "0.00";
                else
                    xrLabel133.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617)).ToString("#,###.#0");

                if
                   (xrLabel121.Text == "0.00") xrLabel134.Text = "0.00";
                else
                    xrLabel134.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718)).ToString("#,###.#0");

                if
                   (xrLabel122.Text == "0.00") xrLabel135.Text = "0.00";
                else
                    xrLabel135.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819)).ToString("#,###.#0");

                if 
                    (xrLabel25.Text == "0.00") xrLabel139.Text = "0.00";
                else
                    xrLabel139.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203) + Gstr.fncToDouble(lv0304) + Gstr.fncToDouble(lv0405) + Gstr.fncToDouble(lv0506) + Gstr.fncToDouble(lv0607) + Gstr.fncToDouble(lv0708)).ToString("#,###.#0");

                if
                    (xrLabel115.Text == "0.00") xrLabel29.Text = "0.00";
                else
                    xrLabel29.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203) + Gstr.fncToDouble(lv0304) + Gstr.fncToDouble(lv0405) + Gstr.fncToDouble(lv0506) + Gstr.fncToDouble(lv0607) + Gstr.fncToDouble(lv0708) + Gstr.fncToDouble(lv0809)).ToString("#,###.#0");

                if
                    (xrLabel116.Text == "0.00") xrLabel33.Text = "0.00";
                else
                    xrLabel33.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203) + Gstr.fncToDouble(lv0304) + Gstr.fncToDouble(lv0405) + Gstr.fncToDouble(lv0506) + Gstr.fncToDouble(lv0607) + Gstr.fncToDouble(lv0708) + Gstr.fncToDouble(lv0809) + Gstr.fncToDouble(lv0910)).ToString("#,###.#0");

                if
                    (xrLabel119.Text == "0.00") xrLabel132.Text = "0.00";
                else
                    xrLabel132.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203) + Gstr.fncToDouble(lv0304) + Gstr.fncToDouble(lv0405) + Gstr.fncToDouble(lv0506) + Gstr.fncToDouble(lv0607) + Gstr.fncToDouble(lv0708) + Gstr.fncToDouble(lv0809) + Gstr.fncToDouble(lv0910) + Gstr.fncToDouble(lv1011)).ToString("#,###.#0");

                if
                    (xrLabel118.Text == "0.00") xrLabel41.Text = "0.00";
                else
                    xrLabel41.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203) + Gstr.fncToDouble(lv0304) + Gstr.fncToDouble(lv0405) + Gstr.fncToDouble(lv0506) + Gstr.fncToDouble(lv0607) + Gstr.fncToDouble(lv0708) + Gstr.fncToDouble(lv0809) + Gstr.fncToDouble(lv0910) + Gstr.fncToDouble(lv1011) + Gstr.fncToDouble(lv1112)).ToString("#,###.#0");

                if
                    (xrLabel117.Text == "0.00") xrLabel37.Text = "0.00";
                else
                    xrLabel37.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203) + Gstr.fncToDouble(lv0304) + Gstr.fncToDouble(lv0405) + Gstr.fncToDouble(lv0506) + Gstr.fncToDouble(lv0607) + Gstr.fncToDouble(lv0708) + Gstr.fncToDouble(lv0809) + Gstr.fncToDouble(lv0910) + Gstr.fncToDouble(lv1011) + Gstr.fncToDouble(lv1112) + Gstr.fncToDouble(lv1213)).ToString("#,###.##");

                if
                   (xrLabel125.Text == "0.00") xrLabel138.Text = "0.00";
                else
                    xrLabel138.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203) + Gstr.fncToDouble(lv0304) + Gstr.fncToDouble(lv0405) + Gstr.fncToDouble(lv0506) + Gstr.fncToDouble(lv0607) + Gstr.fncToDouble(lv0708) + Gstr.fncToDouble(lv0809) + Gstr.fncToDouble(lv0910) + Gstr.fncToDouble(lv1011) + Gstr.fncToDouble(lv1112) + Gstr.fncToDouble(lv1213) + Gstr.fncToDouble(lv1314)).ToString("#,###.##");

                if
                   (xrLabel124.Text == "0.00") xrLabel137.Text = "0.00";
                else
                    xrLabel137.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToInt(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203) + Gstr.fncToDouble(lv0304) + Gstr.fncToDouble(lv0405) + Gstr.fncToDouble(lv0506) + Gstr.fncToDouble(lv0607) + Gstr.fncToDouble(lv0708) + Gstr.fncToDouble(lv0809) + Gstr.fncToDouble(lv0910) + Gstr.fncToDouble(lv1011) + Gstr.fncToDouble(lv1112) + Gstr.fncToDouble(lv1213) + Gstr.fncToDouble(lv1314) + Gstr.fncToDouble(lv1415)).ToString("#,###.##");
            }
            else
            {
                if
                    (xrLabel25.Text == "0.00") xrLabel139.Text = "0.00";
                else
                    xrLabel139.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToDouble(lv1920)).ToString("#,##0.00");

                if
                   (xrLabel115.Text == "0.00") xrLabel29.Text = "0.00";
                else
                    xrLabel29.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToDouble(lv1920) + Gstr.fncToDouble(lv2021)).ToString("#,##0.00");

                if
                    (xrLabel116.Text == "0.00") xrLabel33.Text = "0.00";
                else
                    xrLabel33.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToDouble(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122)).ToString("#,##0.00");

                if
                    (xrLabel119.Text == "0.00") xrLabel132.Text = "0.00";
                else
                    xrLabel132.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToDouble(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223)).ToString("#,##0.00");

                if
                    (xrLabel118.Text == "0.00") xrLabel41.Text = "0.00";
                else
                    xrLabel41.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToDouble(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300)).ToString("#,##0.00");

                if
                   (xrLabel117.Text == "0.00") xrLabel37.Text = "0.00";
                else
                    xrLabel37.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToDouble(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001)).ToString("#,##0.00");

                if
                   (xrLabel125.Text == "0.00") xrLabel138.Text = "0.00";
                else
                    xrLabel138.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToDouble(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102)).ToString("#,##0.00");

                if
                   (xrLabel124.Text == "0.00") xrLabel137.Text = "0.00";
                else
                    xrLabel137.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToDouble(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203)).ToString("#,##0.00");

                if
                   (xrLabel120.Text == "0.00") xrLabel136.Text = "0.00";
                else
                    xrLabel136.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToDouble(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203) + Gstr.fncToDouble(lv0304)).ToString("#,##0.00");

                if
                   (xrLabel120.Text == "0.00") xrLabel133.Text = "0.00";
                else
                    xrLabel133.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToDouble(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203) + Gstr.fncToDouble(lv0304) + Gstr.fncToDouble(lv0405)).ToString("#,##0.00");

                if
                   (xrLabel121.Text == "0.00") xrLabel134.Text = "0.00";
                else
                    xrLabel134.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToDouble(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203) + Gstr.fncToDouble(lv0304) + Gstr.fncToDouble(lv0405) + Gstr.fncToDouble(lv0506)).ToString("#,##0.00");

                if
                   (xrLabel122.Text == "0.00") xrLabel135.Text = "0.00";
                else
                    xrLabel135.Text = (Gstr.fncToDouble(lv1516) + Gstr.fncToDouble(lv1617) + Gstr.fncToDouble(lv1718) + Gstr.fncToDouble(lv1819) + Gstr.fncToDouble(lv1920) + Gstr.fncToDouble(lv2021) + Gstr.fncToDouble(lv2122) + Gstr.fncToDouble(lv2223) + Gstr.fncToDouble(lv2300) + Gstr.fncToDouble(lv0001) + Gstr.fncToDouble(lv0102) + Gstr.fncToDouble(lv0203) + Gstr.fncToDouble(lv0304) + Gstr.fncToDouble(lv0405) + Gstr.fncToDouble(lv0506) + Gstr.fncToDouble(lv0607)).ToString("#,##0.00");
            }
        }

        private string fncFindNetWeight(string lvTime)
        {
            #region //Connect Database 
            OleDbConnection con = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataReader dr;
            #endregion

            string lvReturn = "";

            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "SELECT * From Systemp Where Field1 = '" + lvTime + "' ";
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string lvNum3 = dr["Num3"].ToString();
                    string lvNum4 = dr["Num4"].ToString();

                    lvReturn = (Gstr.fncToDouble(lvNum3) + Gstr.fncToDouble(lvNum4)).ToString("#,##0.00");
                }
            }

            dr.Close();
            con.Close();

            return lvReturn;
        }

        private string fncFindCarCount(string lvTime)
        {
            #region //Connect Database 
            OleDbConnection con = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataReader dr;
            #endregion

            string lvReturn = "";

            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "SELECT * From Systemp Where Field1 = '" + lvTime + "' ";
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string lvNum1 = dr["Num1"].ToString();
                    string lvNum2 = dr["Num2"].ToString();

                    lvReturn = (Gstr.fncToDouble(lvNum1) + Gstr.fncToDouble(lvNum2)).ToString();
                }
            }

            dr.Close();
            con.Close();

            return lvReturn;
        }
    }
}
