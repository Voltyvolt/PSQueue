using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PSQueue
{
    public partial class frmMain : Form
    {
        public static string SetValueForCarType = "";
        public static string SetValueForCaneType = "";
        public static string SetValueForQuota = "";
        public static string SetValueForName = "";
        public static string SetValueForCarNum1 = "";
        public static string SetValueForCarNum2 = "";

        string pvmode = "";
        int pvCountWeight = 0;
        string pvLastWeight = "";

        public frmMain()
        {
            InitializeComponent();
        }

        //private static Version version = new Version(Application.ProductVersion);

        //public static Version Version
        //{
        //    get
        //    {
        //        return version;
        //    }
        //}

        public string VersionLabel
        {
            get
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    Version ver = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    return string.Format("Product Name: PSWeight Cane, Version: 2.0", ver.Major, ver.Minor, "0", "0", Assembly.GetEntryAssembly().GetName().Name);
                }
                else
                {
                    var ver = Assembly.GetExecutingAssembly().GetName().Version;
                    return string.Format("Product Name: PSWeight Cane, Version: 2.0", ver.Major, ver.Minor, "0", "0", Assembly.GetEntryAssembly().GetName().Name);
                }
            }
        }

        private void frmMain2_Load(object sender, EventArgs e)
        {
            //this.Text = "โปรแกรมคิวรถบรรทุก" + "  V." + ;
            //this.Text = String.Format("โปรแกรมคิวรถบรรทุก V. {0}.{1}.{2}.{3}", Version.Major.ToString(), Version.Minor.ToString(), Version.Build.ToString(), Version.Revision.ToString());
            this.Text = VersionLabel + " **** Server : " + GVar.gvConnectionDB + "  | Database : " + GVar.gvDBConnect + " **** ";

            //เช็คสถานะ INOUT
            if (GVar.gvINOUT == "OUT")
            {
                lbIN.Visible = false;
                lbOUT.Visible = true;
                pnIN.Enabled = false;
                pnOut.Enabled = true;
            }
            else
            {
                lbIN.Visible = true;
                lbOUT.Visible = false;
                pnIN.Enabled = true;
                pnOut.Enabled = false;
            }

            //เช็คสถานะว่า Online อยู่หรือไม่
            //fncCheckONLINE();
            GVar.gvOnline = true;

            //เช็คสิทธิ์การเข้าใช้งาน
            GsysSQL.fncFindPermission(GVar.gvUser, "Queue_00", GVar.gvOnline);

            CheckDatetimeFormat();

            lbUser.Text = GVar.gvUser;
            lbState.Text = "-";
            lbCarName.Text = "";
            lbCaneName.Text = "";

            btnClear_Click(sender, e);

            //LoadDataQLock();

            //เชื่อมต่อเครื่องชั่ง
            if (GVar.gvTypeProgram == "W")
            {
                btnConnect_Click(sender, e);
            }
            else
            {

            }
            
            //Default Printer
            PrinterSettings settings = new PrinterSettings();
            cmbPrinter.Text = settings.PrinterName;

            lbComport.Text = GVar.gvComport;

            if(lbUser.Text == "jum")
            {
                GVar.gvPermitDel = "1";
            }
        }
        

        private void fncCheckONLINE()
        {
            ////เช็คสถานะว่า Online อยู่หรือไม่
            //bool lvStatus = GsysSQL.fncChkOnline(GVar.gvServerIP);

            //if (lvStatus)
            //{
            //    lbStatus.Text = "ONLINE";
            //    lbStatus.ForeColor = Color.Green;
            //}
            //else
            //{
            //    lbStatus.Text = "OFFLINE";
            //    lbStatus.ForeColor = Color.Red;
            //}

            ////ถ้า OFFLINE
            //if (!lvStatus)
            //{
            //    //อ่าน TXT
            //    string[] lvArr = System.IO.File.ReadAllText(Application.StartupPath + "\\System_Data.dll").Split('/');

            //    if (lvArr[5] == "ONLINE")
            //    {
            //        GVar.gvCountOFFLINE += 1;

            //        if (GVar.gvCountOFFLINE >= 5)
            //        {
            //            MessageBox.Show("ไม่สามารถติดต่อกับ Server ได้ โปรแกรมจะเข้าสู่ Mode OFFLINE" + Environment.NewLine + "กรุณา ปิดและเปิดใหม่ โปรแกรมอีกครั้ง", "แจ้งเตือน OFFLINE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            string text = lvArr[0] + "/" + lvArr[1] + "/" + lvArr[2] + "/" + lvArr[3] + "/" + lvArr[4] + "/OFFLINE";
            //            System.IO.File.WriteAllText(Application.StartupPath + "\\System_Data.dll", text);
            //            GVar.gvCountOFFLINE = 0;
            //        }
            //    }
            //}
            //else
            //{
            //    //ถ้า ONLINE
            //    //อ่าน TXT
            //    string[] lvArr = System.IO.File.ReadAllText(Application.StartupPath + "\\System_Data.dll").Split('/');

            //    if (lvArr[5] == "OFFLINE")
            //    {
            //        MessageBox.Show("เชื่อมต่อ Server ได้สำเร็จ" + Environment.NewLine + "โปรแกรม ปิดและเปิดใหม่ อีกครั้ง", "แจ้งเตือน ONLINE", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        string text = lvArr[0] + "/" + lvArr[1] + "/" + lvArr[2] + "/" + lvArr[3] + "/" + lvArr[4] + "/ONLINE";
            //        System.IO.File.WriteAllText(Application.StartupPath + "\\System_Data.dll", text);
            //        Application.Restart();
            //    }
            //}
        }

        private void CheckDatetimeFormat()
        {
            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            if (sysFormat != "dd/MM/yyyy")
            {
                MessageBox.Show("รูปแบบวันที่เครื่องไม่ถูกต้อง กรุณาตรวจสอบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var cplPath = System.IO.Path.Combine(Environment.SystemDirectory, "control.exe");
                System.Diagnostics.Process.Start(cplPath, "/name Microsoft.DateAndTime");
                Application.Exit();
            }
        }

        private void LoadData()
        {
            this.Cursor = Cursors.WaitCursor;
            //Get Data
            DataTable DT = new DataTable();
            ///

            string lvSQL = "";
            string lvCondition = "";
        string lvRecord = txtRecord.Text;
            if (lvRecord != "ALL")
                lvSQL += "Select Top " + lvRecord + " * from Queue_Diary Where (Q_Year = '' ";
            else
                lvSQL += "Select * from Queue_Diary Where (Q_Year = '' ";

            //วันที่และเวลา
            if (chkSearchDate.Checked)
            {
                string lvDateS = Gstr.fncChangeTDate(txtDateS.Text);
                string lvDateE = Gstr.fncChangeTDate(txtDateE.Text);

                lvSQL += "And Q_Date >= '" + lvDateS + "' And Q_Date <= '" + lvDateE + "' ";
                lvCondition += "And Q_Date >= '" + lvDateS + "' And Q_Date <= '" + lvDateE + "' ";  
            }
            if (chkSearchTime.Checked)
            {
                string lvTimeS = txtTimeS.Text;
                string lvTimeE = txtTimeE.Text;

                lvSQL += "And Q_Time >= '" + lvTimeS + "' And Q_Time <= '" + lvTimeE + "' ";
                lvCondition += "And Q_Time >= '" + lvTimeS + "' And Q_Time <= '" + lvTimeE + "' ";
            }

            //ประเภท
            if (GVar.gvTypeProgram == "Q")
            {
                string lvType = "";
                //if (ChkShow0.Checked) lvType = "-";
                if (ChkShowA.Checked)
                {
                    if (lvType != "") lvType += "','";
                    lvType += "A";
                }

                if (ChkShowB.Checked)
                {
                    if (lvType != "") lvType += "','";
                    lvType += "B";
                }

                if (ChkShow0.Checked)
                {
                    if (lvType != "") lvType += "','";
                    lvType += "-";
                }

                if (lvType != "" && GVar.gvTypeProgram == "Q")
                {
                    lvSQL += "And Q_Rail in ('" + lvType + "') ";
                    lvCondition += "And Q_Rail in ('" + lvType + "') ";
                }
            }
            else
            {
                //แสดงรถ ราง A และ B
                string lvRail = "";
                if (ChkShowA_1.Checked)
                {
                    if (lvRail != "") lvRail += "','";
                    lvRail += "A";
                }

                if (ChkShowB_1.Checked)
                {
                    if (lvRail != "") lvRail += "','";
                    lvRail += "B";
                }

                if (ChkShowA_1.Checked && ChkShowB_1.Checked)
                {
                    if (lvRail != "") lvRail += "','";
                    lvRail += "-";
                }

                if (lvRail != "")
                {
                    lvSQL += "And Q_Rail in ('" + lvRail + "') ";
                    lvCondition += "And Q_Rail in ('" + lvRail + "') ";
                }
            }

            //แสดงเฉพาะรายการที่ยังไม่ปิดยอด
            lvSQL += "And Q_CloseStatus = '' ";
            lvCondition += "And Q_CloseStatus = '' ";

            //เอาเฉพาะเอกสารที่ไม่ยกเลิก
            lvSQL += "And Q_Status = 'Active' ";
            lvCondition += "And Q_Status = 'Active' ";

            if (ChkShowOut.Checked || ChkShowOut_1.Checked)
            {
                lvSQL += "And Q_WeightINDate != '' ";
                lvCondition += "And Q_WeightINDate != '' ";
            }
            else if (ChkShowIN.Checked || ChkShowIN_1.Checked)
            {
                lvSQL += "And Q_WeightINDate = '' ";
                lvCondition += "And Q_WeightINDate = '' ";
            }

            //เอาเฉพาะเอกสารยังไม่ชั่งออก
            if (!chkShowFinish.Checked && !chkShowFinish_1.Checked)
            {
                lvSQL += "And Q_WeightOUTDate = '' ";
                lvCondition += "And Q_WeightOUTDate = '' ";

                //ตัดชั่งพ่วงที่แม่ชั่งเสร็จแล้วออก
                lvSQL += "And Q_WeightFinish = '' ";
                lvCondition += "And Q_WeightFinish = '' ";
            }
            else
            {
                lvSQL += "And(Q_WeightINDate != '' And Q_WeightOUTDate != '') OR Q_WeightFinish = '1' ";
                lvCondition += "And (Q_WeightINDate != '' And Q_WeightOUTDate != '') OR Q_WeightFinish = '1' ";
            }

            if (GVar.gvTypeProgram == "Q")
            {
                //กรองตาม Station
                if (chkShowPOM.Checked)
                {
                    lvSQL += "And Q_Station = '" + GVar.gvStation + "' ";
                    lvCondition += "And Q_Station = '" + GVar.gvStation + "' ";
                }

                if (!GVar.gvOnline)
                {
                    //ถ้าเป็น OFFLINE ไม่ต้องแสดง Status 3
                    lvSQL += "And Q_SyncStatus <> '3' ";
                    lvCondition += "And Q_SyncStatus <> '3' ";
                }
                /*if(GVar.gvQNo != "")
                {
                    lvSQL += "And Q_No = '" + GVar.gvQNo + "' ";
                }*/
            }
            else if (GVar.gvTypeProgram == "W")
            {
                //กรองตาม Station
                if (chkShowPOM1.Checked)
                {
                    lvSQL += "And Q_Station = '" + GVar.gvStation + "' ";
                    lvCondition += "And Q_Station = '" + GVar.gvStation + "' ";
                }
            }

            lvSQL += ") and Q_Year = '' ";

            if (GVar.gvTypeProgram == "W")
                lvSQL += "Order by Q_No ";
            else
                lvSQL += "Order by Q_No Desc";


            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumRow = DT.Rows.Count;
            sp1.ActiveSheet.RowCount = 0;
            sp1.ActiveSheet.RowCount = lvNumRow;

            for (int i = 0; i < lvNumRow; i++)
            {
                if (DT.Rows[i]["Q_No"].ToString() == "20034.1")
                {

                }

                sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["Q_Station"].ToString();
                sp1.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["Q_No"].ToString();
                sp1.ActiveSheet.Cells[i, 2].Text = DT.Rows[i]["Q_Rail"].ToString();
                sp1.ActiveSheet.Cells[i, 3].Text = DT.Rows[i]["Q_Quota"].ToString();
                sp1.ActiveSheet.Cells[i, 4].Text = DT.Rows[i]["Q_CarNum"].ToString();
                sp1.ActiveSheet.Cells[i, 5].Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_Date"].ToString());
                sp1.ActiveSheet.Cells[i, 6].Text = DT.Rows[i]["Q_Time"].ToString();
                sp1.ActiveSheet.Cells[i, 7].Text = DT.Rows[i]["Q_CarType"].ToString();

                double lvRowH = sp1.ActiveSheet.Rows[i].Height;

                string lvCaneID = DT.Rows[i]["Q_CaneType"].ToString();
                string lvCaneName = GsysSQL.fncFindCaneTypeName(lvCaneID, GVar.gvOnline);

                if (lvCaneName != "")
                    sp1.ActiveSheet.Cells[i, 8].Text = lvCaneID + " : " + lvCaneName;
                else
                    sp1.ActiveSheet.Cells[i, 8].Text = "";

                //เช็คสถานะชั่งรวมคิวแม่
                string[] lvArr = DT.Rows[i]["Q_No"].ToString().Split('.');
                string lvChkAll = GsysSQL.fncCheckWeightALL(lvArr[0], GVar.gvOnline);

                //สถานะ
                if (DT.Rows[i]["Q_WeightFinish"].ToString() == "1")
                { 
                    sp1.ActiveSheet.Cells[i, 9].Text = "ชั่งครบแล้ว";
                    sp1.ActiveSheet.Cells[i, 9].ForeColor = Color.Blue;
                }
                else if (DT.Rows[i]["Q_No"].ToString().Contains('.') && DT.Rows[i]["Q_WeightINDate"].ToString() != "")
                {
                    sp1.ActiveSheet.Cells[i, 9].Text = "รอชั่งออก";
                    sp1.ActiveSheet.Cells[i, 9].ForeColor = Color.Red;
                }
                else if (DT.Rows[i]["Q_WeightIN"].ToString() == "0" && DT.Rows[i]["Q_WeightOUT"].ToString() == "0" )//&& lvChkAll != "1"
                {
                    sp1.ActiveSheet.Cells[i, 9].Text = "รอชั่งเข้า";
                    sp1.ActiveSheet.Cells[i, 9].ForeColor = Color.Green;
                }
                else if (DT.Rows[i]["Q_WeightIN"].ToString() != "0" && DT.Rows[i]["Q_WeightOUT"].ToString() != "0")
                {
                    sp1.ActiveSheet.Cells[i, 9].Text = "ชั่งครบแล้ว";
                    sp1.ActiveSheet.Cells[i, 9].ForeColor = Color.Blue;
                }
                else if ((DT.Rows[i]["Q_WeightIN"].ToString() != "0" && DT.Rows[i]["Q_WeightOUT"].ToString() == "0"))
                {
                    sp1.ActiveSheet.Cells[i, 9].Text = "รอชั่งออก";
                    sp1.ActiveSheet.Cells[i, 9].ForeColor = Color.Red;
                }
                

                sp1.ActiveSheet.Cells[i, 10].Text = DT.Rows[i]["Q_BillingNo"].ToString();
                sp1.ActiveSheet.Cells[i, 11].Text = DT.Rows[i]["Q_TKNo"].ToString();

                //ถ้าติ๊กแสดงเฉพาะชั่งเข้า
                if ((ChkShowIN_1.Checked || ChkShowIN.Checked) && sp1.ActiveSheet.Cells[i, 9].Text == "รอชั่งออก")
                {
                    sp1.ActiveSheet.Rows[i].Height = 0;
                }
                else
                {
                    sp1.ActiveSheet.Rows[i].Height = 30;
                }
            }

            DT.Dispose();


            //แสดงตัวเลขท้ายหน้าจอ
            FncRefreshCountQ(lvCondition);

            pnRecord.Visible = false;

            this.Cursor = Cursors.Default;
        }

        private void FncRefreshCountQ(string lvCondition)
        {
            //แสดงตัวเลขท้ายหน้าจอ
            int lvCount0 = 0;
            int lvCountA = 0;
            int lvCountB = 0;
            
            //ชั่ง
            lbCount0_1.Text = GsysSQL.fncCountW("IN", false, GVar.gvOnline); //ชั่งเข้าแล้ว
            lbCountA_1.Text = GsysSQL.fncCountW("OUT", false, GVar.gvOnline); //ชั่งออกแล้ว
            lbCountB_1.Text = GsysSQL.fncCountW("SUM", true, GVar.gvOnline);  //น้ำหนักรวม

            //lvCount0 = Gstr.fncToInt(lbCount0_1.Text);
            lvCountA = Gstr.fncToInt(lbCountA_1.Text);
            lvCountB = Gstr.fncToInt(lbCountB_1.Text);
            
            //คิว
            lbCount0.Text = GsysSQL.fncCountQ("-", lvCondition, GVar.gvOnline);//ไม่ระบุลาน
            lbCountA.Text = GsysSQL.fncCountQ("A", lvCondition, GVar.gvOnline);//รอคิวราง A
            lbCountB.Text = GsysSQL.fncCountQ("B", lvCondition, GVar.gvOnline);//รอคิวราง B

            lvCount0 = Gstr.fncToInt(lbCount0.Text); 
            lvCountA = Gstr.fncToInt(lbCountA.Text);
            lvCountB = Gstr.fncToInt(lbCountB.Text);

            lbCountAll.Text = (lvCountA + lvCountB + lvCount0).ToString("#,###"); //รถรอคิวทั้งหมด
        }

        private void txtQuota_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                #region Code เก่า Ment ไว้ก่อน
                //var lvBarCode = txtQuota.Text;
                //var lvBarCodeSplit = lvBarCode.Split('P');
                //GVar.gvQuota = "";
                //GVar.gvSubQuota = "";
                //var lvQuota = "";
                //var lvPlanId = "";
                //string[] lvQuotachk = { "2312", "6678", "7092", "7112", "7478", "7644", "7657", "7756", "7757", "7793", "7817", "7903", "8409", "8439", "8443", "8490", "8502", "8525", "8869", "9099", "9116", "9132" }; //22

                ////ถ้ามีแปลงให้ Split ออกด้วย
                //if (lvBarCodeSplit.Length > 1)
                //{
                //    lvQuota = lvBarCodeSplit[0];
                //    foreach (string x in lvQuotachk)
                //    {
                //        if (lvQuota.Contains(x))
                //        {
                //            GVar.gvSubQuota = x;
                //            lvQuota = "9999";
                //            MessageBox.Show("โควต้านี้ไม่ได้ลงทะเบียนชาวไร่ อ้อยจะถูกโอนไปยังโควต้า 9999", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            break;
                //        }
                //        else
                //        {

                //        }
                //    }

                //    lvPlanId = lvBarCodeSplit[1];
                //}
                ////ถ้าไม่มีแปลงให้ระบุแค่โควต้า
                //else
                //{
                //    lvQuota = lvBarCodeSplit[0];

                //    foreach (string x in lvQuotachk)
                //    {
                //        if (lvQuota.Contains(x))
                //        {
                //            GVar.gvSubQuota = x;
                //            lvQuota = "9999";
                //            MessageBox.Show("โควต้านี้ไม่ได้ลงทะเบียนชาวไร่ อ้อยจะถูกโอนไปยังโควต้า 9999", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            break;
                //        }
                //        else
                //        {

                //        }
                //    }
                //}

                ////เช็คโควต้าว่ามีหรือไม่ ?
                //GVar.gvQuota = lvQuota;
                //string lvQuotaCheckActive = GsysSQL.fncGetQuotaActive(GVar.gvQuota);
                //    if (lvQuotaCheckActive == "")
                //    {
                //        MessageBox.Show("โควต้านี้ไม่ได้ลงทะเบียนชาวไร่", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        return;
                //    }

                //string[] lvQuotaWchk = { "3595", "3597", "3600", "7699", "8181", "9157", "9158", "9161", "9176", "9177", "9179", "9181", "9182", "9189", "9190", "9191", "9194", "9195", "9200", "9214", "9999" };
                //foreach(string x in lvQuotaWchk)
                //{
                //    if (lvQuota.Contains(x))
                //    {
                //        label48.Visible = true;
                //    }
                //}
                #endregion

                ChkBonsugo.Checked = false;


                //ค้นหาข้อมูลโควต้าจากรหัส
                GVar.gvQuota = "";
                string lvQuota = txtQuota.Text;
                string lvName = GsysSQL.fncFindQuotaName(lvQuota, GVar.gvOnline);

                if (lvName != "")
                {
                    txtQuota.Text = lvQuota;
                    GVar.gvQuota = txtQuota.Text;

                    //if(GVar.gvSubQuota != "")
                    //{
                    //    txtQuota.Text = lvQuota + "(" + GVar.gvSubQuota + ")";
                    //}

                    txtQuotaName.Text = lvName;
                    if (GVar.gvBonsucroStatus == "1") ChkBonsugo.Checked = true;
                    else ChkBonsugo.Checked = false;
                    txtCarNum.Focus();
                }
                else
                {
                    MessageBox.Show("ไม่พบข้อมูลโควต้า กรุณาตรวจสอบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQuota.Text = "";
                    txtQuota.Focus();
                }

                //ส่งข้อมูลไป Monitor
                fncRefreshDataMonitor();

                //โหลดข้อมูลสายจาก MCSS
                fncLoadMCSSName();
            }
        }

        private void txtCarNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !txtQ.ReadOnly && GVar.gvTypeProgram == "W")
            {
                //ปิดการใช้งานเนื่องจาก ใช้ปุ๋มค้นหาของระบบ ใบนำตัดออนไลน์

                if (txtCarNum.Text != "" && pvmode == "")
                {
                    //ค้นหาทะเบียนรถ
                    DataTable DT = new DataTable();

                    string lvSQL = "Select * from Queue_Diary Where Q_CarNum like '%" + txtCarNum.Text + "%' and Q_Year = '' ";
                    lvSQL += "And (Q_WeightOUT = 0) ";//Q_WeightIN = 0 OR 
                    lvSQL += "And Q_CloseStatus = '' ";
                    DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

                    int lvNumRow = DT.Rows.Count;

                    string lvQ = "";
                    if (lvNumRow > 0)
                    {
                        if (lvNumRow == 1)
                        {
                            lvQ = DT.Rows[0]["Q_No"].ToString();
                            txtCutDoc.Text = GsysSQL.fncGetCutdoc(lvQ);
                           
                            if (!txtCutDoc.Text.Contains('/'))
                            {
                                LoadQueueData(lvQ, GVar.gvStation, false);
                                LoadCutDocData();
                            }
                            else
                            {
                                DataTable DTs = new DataTable();
                                string SQL = "Select * From  Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_Year = '' ";
                                DTs = GsysSQL.fncGetQueryData(SQL, DTs, true);

                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    txtCutDoc.Text = DTs.Rows[i]["Q_CutDoc"].ToString();
                                    txtCutCar.Text = DTs.Rows[i]["Q_CutCar"].ToString();
                                    string CutPrice = DTs.Rows[i]["Q_CutPrice"].ToString();
                                    string CarryPrice = DTs.Rows[i]["Q_CarryPrice"].ToString();
                                    if (CarryPrice != "0" || CarryPrice != "")
                                    {
                                        txtCarryPrice.Text = CarryPrice;
                                        chkCarryPrice.Checked = true;
                                    }
                                    if (CutPrice != "0" || CutPrice != "")
                                    {
                                        txtCutPrice.Text = CutPrice;
                                    }
                                }
                            }
                            LoadQueueData(lvQ, GVar.gvStation, true);
                        }
                        else if (lvNumRow > 1)
                        {
                            GVar.gvSCode = "";
                            frmCarBrowse frm = new frmCarBrowse();
                            frm.LoadData(lvSQL);
                            frm.ShowDialog();

                            if (GVar.gvSCode != "")
                            {
                                txtCutDoc.Text = GsysSQL.fncGetCutdoc(lvQ);
                                if (!txtCutDoc.Text.Contains('/'))
                                {
                                    LoadQueueData(lvQ, GVar.gvStation, false);
                                    LoadCutDocData();
                                }
                                else
                                {
                                    DataTable DTs = new DataTable();
                                    string SQL = "Select * From  Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_Year = '' ";
                                    DTs = GsysSQL.fncGetQueryData(SQL, DTs, true);

                                    for (int i = 0; i < DT.Rows.Count; i++)
                                    {
                                        txtCutDoc.Text = DTs.Rows[i]["Q_CutDoc"].ToString();
                                        txtCutCar.Text = DTs.Rows[i]["Q_CutCar"].ToString();
                                        string CutPrice = DTs.Rows[i]["Q_CutPrice"].ToString();
                                        string CarryPrice = DTs.Rows[i]["Q_CarryPrice"].ToString();
                                        if (CarryPrice != "0" || CarryPrice != "")
                                        {
                                            txtCarryPrice.Text = CarryPrice;
                                            chkCarryPrice.Checked = true;
                                        }
                                        if (CutPrice != "0" || CutPrice != "")
                                        {
                                            txtCutPrice.Text = CutPrice;
                                        }
                                    }
                                }
                                LoadQueueData(lvQ, GVar.gvStation, true);
                               
                            }
                        }
                        fncEnableBtnWhenClick(lvQ);
                    }
                    else
                    {
                        txtCarNum.Text = "";
                        MessageBox.Show("แจ้งเตือน...", "ไม่พบข้อมูลทะเบียนรถในระบบกรุณาตรวจสอบอีกครั้ง...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtCarNum.Focus();
                    }
                }
            }
            else if (e.KeyCode == Keys.Enter && GVar.gvTypeProgram == "Q")
            {
                //ถอดรหัสทะเบียนรถ
                //เปลี่ยนภาษาไทยเป็น Eng
                string lvCarBar = FncChangeEngToThaiCar(txtCarNum.Text);
                txtCarNum.Text = lvCarBar;

                if (txtCarNum.Text != "") txtCutCar.Focus();
                //FncCheckQLockStatus();
            }
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCutCar.Focus();
                }
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void txtCutDoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCutDoc.Text != "")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCutPrice.Focus();
                }
            }
        }

        private void txtCaneDoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCaneDoc.Text != "")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtSampleNo.Focus();
                }
            }
        }

        private void txtMainCar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSave_Click(sender, e);
        }

        private string FncCheckQLockCarAndCane()
        {
            string lvPass = "Yes";

            //เช็คว่าจำเป็นต้องทำคิวล็อกหรือไม่ 
            //สาวแต๋น พ่วง  ฟม.รถตัด  ฟม.สะอาด ไม่ต้องแจ้งเตือนทำคิวล็อก
            string lvData = GsysSQL.fncFindQLockAlert();
            string[] lvArrData = lvData.Split(',');
            string lvLockAlert = lvArrData[0];
            string lvChkCar1 = lvArrData[1];
            string lvChkCar2 = lvArrData[2];
            string lvChkCar3 = lvArrData[3];
            string lvChkCar4 = lvArrData[4];
            string lvChkCar5 = lvArrData[5];
            string lvChkCane9 = lvArrData[6];
            string lvChkCane10 = lvArrData[7];
            string lvChkCane11 = lvArrData[8];
            string lvChkCane12 = lvArrData[9];
            string lvChkCane13 = lvArrData[10];
            string lvChkCane14 = lvArrData[11];
            string lvChkCane15 = lvArrData[12];
            string lvChkCane16 = lvArrData[13];
            string lvChkLoopNoS = lvArrData[14]; //loop Now S
            string lvChkLockNoS = lvArrData[15]; //LockNo Now S
            string lvChkLoopNoE = lvArrData[16]; //loop Now E
            string lvChkLockNoE = lvArrData[17]; //LockNo Now E

            //เช็ครถก่อน
            for (int i = 0; i < 5; i++)
            {
                if (lvChkCar1 == "1" && chkCar1.Checked) //สาวเเต๋น
                {
                    //เช็คอ้อย
                    for (int l = 0; l < 8; l++)
                    {
                        if (lvChkCane9 == "1" && chkCane9.Checked) lvPass = "No";
                        else if (lvChkCane10 == "1" && chkCane10.Checked) lvPass = "No";
                        else if (lvChkCane11 == "1" && chkCane11.Checked) lvPass = "No";
                        else if (lvChkCane12 == "1" && chkCane12.Checked) lvPass = "No";
                        else if (lvChkCane13 == "1" && chkCane13.Checked) lvPass = "No";
                        else if (lvChkCane14 == "1" && chkCane14.Checked) lvPass = "No";
                        else if (lvChkCane15 == "1" && chkCane15.Checked) lvPass = "No";
                        else if (lvChkCane16 == "1" && chkCane16.Checked) lvPass = "No";
                    }
                }
                else if (lvChkCar2 == "1" && chkCar2.Checked) //
                {
                    //เช็คอ้อย
                    for (int l = 0; l < 8; l++)
                    {
                        if (lvChkCane9 == "1" && chkCane9.Checked) lvPass = "No";
                        else if (lvChkCane10 == "1" && chkCane10.Checked) lvPass = "No";
                        else if (lvChkCane11 == "1" && chkCane11.Checked) lvPass = "No";
                        else if (lvChkCane12 == "1" && chkCane12.Checked) lvPass = "No";
                        else if (lvChkCane13 == "1" && chkCane13.Checked) lvPass = "No";
                        else if (lvChkCane14 == "1" && chkCane14.Checked) lvPass = "No";
                        else if (lvChkCane15 == "1" && chkCane15.Checked) lvPass = "No";
                        else if (lvChkCane16 == "1" && chkCane16.Checked) lvPass = "No";
                    }
                }
                else if (lvChkCar3 == "1" && chkCar3.Checked) //
                {
                    //เช็คอ้อย
                    for (int l = 0; l < 8; l++)
                    {
                        if (lvChkCane9 == "1" && chkCane9.Checked) lvPass = "No";
                        else if (lvChkCane10 == "1" && chkCane10.Checked) lvPass = "No";
                        else if (lvChkCane11 == "1" && chkCane11.Checked) lvPass = "No";
                        else if (lvChkCane12 == "1" && chkCane12.Checked) lvPass = "No";
                        else if (lvChkCane13 == "1" && chkCane13.Checked) lvPass = "No";
                        else if (lvChkCane14 == "1" && chkCane14.Checked) lvPass = "No";
                        else if (lvChkCane15 == "1" && chkCane15.Checked) lvPass = "No";
                        else if (lvChkCane16 == "1" && chkCane16.Checked) lvPass = "No";
                    }
                }
                else if (lvChkCar4 == "1" && chkCar4.Checked) //
                {
                    //เช็คอ้อย
                    for (int l = 0; l < 8; l++)
                    {
                        if (lvChkCane9 == "1" && chkCane9.Checked) lvPass = "No";
                        else if (lvChkCane10 == "1" && chkCane10.Checked) lvPass = "No";
                        else if (lvChkCane11 == "1" && chkCane11.Checked) lvPass = "No";
                        else if (lvChkCane12 == "1" && chkCane12.Checked) lvPass = "No";
                        else if (lvChkCane13 == "1" && chkCane13.Checked) lvPass = "No";
                        else if (lvChkCane14 == "1" && chkCane14.Checked) lvPass = "No";
                        else if (lvChkCane15 == "1" && chkCane15.Checked) lvPass = "No";
                        else if (lvChkCane16 == "1" && chkCane16.Checked) lvPass = "No";
                    }
                }
                else if (lvChkCar5 == "1" && chkCar5.Checked) //
                {
                    //เช็คอ้อย
                    for (int l = 0; l < 8; l++)
                    {
                        if (lvChkCane9 == "1" && chkCane9.Checked) lvPass = "No";
                        else if (lvChkCane10 == "1" && chkCane10.Checked) lvPass = "No";
                        else if (lvChkCane11 == "1" && chkCane11.Checked) lvPass = "No";
                        else if (lvChkCane12 == "1" && chkCane12.Checked) lvPass = "No";
                        else if (lvChkCane13 == "1" && chkCane13.Checked) lvPass = "No";
                        else if (lvChkCane14 == "1" && chkCane14.Checked) lvPass = "No";
                        else if (lvChkCane15 == "1" && chkCane15.Checked) lvPass = "No";
                        else if (lvChkCane16 == "1" && chkCane16.Checked) lvPass = "No";
                    }
                }
            }

            return lvPass;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            
            //เก็บตัวแปร
            GVar.gvStatusFail = "";
            GVar.gvCarryPriceStatus = false;
            if (chkCarryPrice.Checked) GVar.gvCarryPriceStatus = true;

            string lvRail = "";
            if (ChkA.Checked) lvRail = "A"; else if (ChkB.Checked) lvRail = "B"; else lvRail = "-";
            string lvQ = txtQ.Text;
            GVar.gvQNo = lvQ;
            string lvCarNumFirst = (txtCarNum.Text).Replace(" ", " ");
            string lvCarNum = lvCarNumFirst;
            
            string lvCarNumSecond = "";
            string lvCarNum2 = "";

            if (!lvQ.Contains('.'))
            {
                lvCarNumSecond = (txtCarNum2.Text).Replace(" ", " ");
                lvCarNum2 = lvCarNumSecond;
            }
            else
            {
                lvCarNumFirst = (txtCarNum.Text).Replace(" ", " ");
                lvCarNum = lvCarNumFirst;

                lvCarNumSecond = GsysSQL.fncGetCarnum1(Gstr.Left(lvQ, 6));
                lvCarNum2 = lvCarNumSecond;
            }
            string lvCutDoc = txtCutDoc.Text;
            string lvCutDoc2 = txtCutDoc2.Text;
            string lvQuota = txtQuota.Text;
            string lvSubQuota = GVar.gvSubQuota;
            string lvCaneDoc = Gstr.fncGetDataCode(txtCaneDoc.Text, ":");
            string lvCutCar = Gstr.fncGetDataCode(txtCutCar.Text, ":");
            string lvCutCar2 = Gstr.fncGetDataCode(txtCutCar2.Text, ":");
            string lvMainCar = txtMainCar.Text;
            string lvDate = Gstr.fncChangeTDate(lbDate.Text);
            string lvTime = lbTime.Text;
            string lvCarType = fncGetDataCheck("Car", "");
            string lvCaneType = fncGetDataCheck("Cane", "");
            double lvCutPrice = Gstr.fncToDouble(txtCutPrice.Text);

            string lvChkCarryPrice = "";
            if (chkCarryPrice.Checked) lvChkCarryPrice = "1";
            double lvCarryPrice = Gstr.fncToDouble(txtCarryPrice.Text);

            string lvUser = GVar.gvUser;
            string lvBonsugo = "";
            if (ChkBonsugo.Checked) lvBonsugo = "1";
            string lvWeightAll = "";
            if (ChkWeightAll.Checked) lvWeightAll = "1";

            string lvStation = Gstr.Left(lvQ,1);

            string lvChkMain = GsysSQL.fncCheckMainQuota(lvMainCar, lvQuota, GVar.gvOnline);
            string lvSampleNo = txtSampleNo.Text.Trim();
            string lvSampleNo2 = "";

            string lvBilling = txtBillNo.Text;
            string lvBillingOld = GVar.gvBillingOld;
            string lvTKNo = txtTakaoNo.Text;
            string lvTKNoOld = GVar.gvTKNoOld;

            //ข้อมูลใบนำตัด
            //ข้อมูลผู้รับเหมาคีบ
            string CutOwner = txtCutOwner.Text;
            string[] lvCarcutRmsplit = { };
            string lvCarcutRmid = "";
            string lvCarcutRmname = "";
            string lvCarcutRmPrice = "";

            string TruckOwner = txtCarOwner.Text;
            string[] lvTruckRmsplit = { };
            string lvTruckRmid = ""; //รหัส 23
            string lvTruckRmname = ""; //ชื่อ 24
            string lvTruckRmprice = ""; //ราคา 25

            string KeebOwner = txtKeepOwner.Text;
            string[] lvKeebRmsplit = { };
            string lvKeebRmid = ""; //รหัส 26
            string lvKeebRmname = ""; //ชื่อ 27
            string lvKeebRmprice = ""; //ราคา 28

            string AllOwner = txtContractAll.Text;
            string[] lvAllRmsplit = { };
            string lvAllRmid = ""; //รหัส 29
            string lvAllRmname = ""; //ชื่อ 30
            string lvAllRmprice = ""; //ราคา 31

            string C_ContracType = "";
            if (lvCaneType == "5" || lvCaneType == "6")
            {
                C_ContracType = "รถตัดนอก";
            }
            else if(lvCaneType == "11" || lvCaneType == "12")
            {
                C_ContracType = "รถตัดใน";
            }
            else
            {
                C_ContracType = "คนตัด";
            }
            
            #region เช็คข้อมูลต่างๆ ระบบคิว

            string lvQuotaCheckActive = GsysSQL.fncGetQuotaActive(GVar.gvQuota);
           
            if (lvQuotaCheckActive == "")
            {
                MessageBox.Show("โควต้านี้ไม่ได้ลงทะเบียนชาวไร่", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtQuota.Focus();
                txtQuota.SelectAll();

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                return;
            }

            if (!ChkA.Checked && !ChkB.Checked && GVar.gvTypeProgram == "W")
            {
                MessageBox.Show("กรุณาระบุ ลาน A หรือ ลาน B", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                return;
            }
            else if (txtQ.Text == "")
            {
                MessageBox.Show("กรุณาระบุคิว", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtQ.Focus();

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                return;
            }
            else if (txtCarNum.Text == "" || txtCarNum.Text.Trim() == "-")//
            {
                if (GVar.gvTypeProgram == "W")
                {
                    MessageBox.Show("กรุณาระบุเลขทะเบียนรถ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCarNum.Focus();

                    this.Cursor = Cursors.Default;
                    btnSave.Enabled = true;
                    btnClear.Enabled = true;
                    return;
                }
                else if (GVar.gvTypeProgram == "Q" && txtCarNum.Text == "")
                {
                    MessageBox.Show("กรุณาระบุเลขทะเบียนรถ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCarNum.Focus();

                    this.Cursor = Cursors.Default;
                    btnSave.Enabled = true;
                    btnClear.Enabled = true;
                    return;
                }
            }
            else if (txtCarNum2.Visible && (txtCarNum2.Text == ""))
            {
                MessageBox.Show("กรุณาระบุเลขทะเบียนรถลูก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCarNum2.Focus();

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                return;
            }
            else if (txtQuota.Text == "")
            {
                MessageBox.Show("กรุณาระบุโควต้า", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtQuota.Focus();

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                return;
            }
            else if (lvCarType == "")
            {
                MessageBox.Show("กรุณาระบุประเภท รถอ้อย", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                return;
            }
            else if (lvCaneType == "")
            {
                MessageBox.Show("กรุณาระบุประเภท อ้อย", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                return;
            }

            if ((chkCane7.Checked == true && chk_CutdocA.Checked == true && txtCutOwner.Text == "") || (chkCane13.Checked == true && chk_CutdocA.Checked == true && txtCutOwner.Text == ""))
            {
                MessageBox.Show("กรุณากรอกผู้รับเหมาตัดโรงงาน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                txtCutCar.Focus();
                return;
            }

            //ถ้าเป็นตัวลูกมา เช็คประเภทอ้อยกับเลขโควต้าว่าตรงกับตัวแม่รึเปล่า
            if (lvQ.Contains('.') && GVar.gvTypeProgram == "Q")
            {
                //Split เลขที่คิวด้วย .
                string lvQSplit = "";
                string[] lvQMomSplit = lvQ.Split('.');
                lvQSplit = lvQMomSplit[0];

                //ค้นหาโควต้าจากเลขตัวแม่
                string lvQuotaMom = GsysSQL.fncFindQuotaByQ(lvQSplit, true);

                //ถ้าเลขโควต้าตอนนี้กับเลขโควต้าตัวแม่ไม่ตรงกัน
                if (lvQuotaMom.Trim() != lvQuota.Trim())
                {
                    MessageBox.Show("ถ้าเป็นคนละโควต้า ให้เปิดคิวใหม่", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Cursor = Cursors.Default;
                    btnSave.Enabled = true;
                    btnClear.Enabled = true;
                    return;
                }

                string lvCaneMom = GsysSQL.fncGetCaneTypeDetail(lvQSplit);
                if (lvCaneMom.Trim() != lvCaneType.Trim())
                {
                    MessageBox.Show("ถ้าเป็นอ้อยคนละประเภท ให้เปิดคิวใหม่", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Cursor = Cursors.Default;
                    btnSave.Enabled = true;
                    btnClear.Enabled = true;
                    return;
                }
            }

            if (GVar.gvTypeProgram == "W" && lvSampleNo == "" && lbOUT.Visible)// && !chkCar1.Checked
            {
                MessageBox.Show("กรุณาระบุเลขตัวอย่าง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                txtSampleNo.Focus();
                return;
            }
            
            //else if (txtCutPrice.Text == "" && txtCutOwner.Text != "" && chk_CutdocA.Checked == true || txtCutPrice.Text == "0" && txtCutOwner.Text != "" && chk_CutdocA.Checked == true)
            //{
            //    MessageBox.Show("ถ้าเปิดใบนำตัด กรุณาระบุราคาผู้รับเหมาตัด", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    this.Cursor = Cursors.Default;
            //    btnSave.Enabled = true;
            //    btnClear.Enabled = true;
            //    txtSampleNo.Focus();
            //    return;
            //}

            //else if(txtCarryPrice.Text == "" && chkCarryPrice.Checked == true && chk_CutdocA.Checked == true || txtCarryPrice.Text == "0" && chkCarryPrice.Checked == true && chk_CutdocA.Checked == true)
            //{
            //    MessageBox.Show("ถ้าเปิดใบนำตัด กรุณาระบุราคาผู้รับเหมาบรรทุก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    this.Cursor = Cursors.Default;
            //    btnSave.Enabled = true;
            //    btnClear.Enabled = true;
            //    txtSampleNo.Focus();
            //    return;
            //}

            else if(txtKeepPrice.Text == "" && txtKeepOwner.Text != "" && chk_CutdocA.Checked == true || txtKeepPrice.Text == "0" && txtKeepOwner.Text != "" && chk_CutdocA.Checked == true)
            {
                MessageBox.Show("ถ้าเปิดใบนำตัด กรุณาระบุราคาผู้รับเหมาคีบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                txtSampleNo.Focus();
                return;
            }

            else if (txtContractAllPrice.Text == "" && txtContractAll.Text != "" && chk_CutdocA.Checked == true || txtContractAllPrice.Text == "0" && txtContractAll.Text != "" && chk_CutdocA.Checked == true)
            {
                MessageBox.Show("ถ้าเปิดใบนำตัด กรุณาระบุราคาผู้รับเหมารวม", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                txtSampleNo.Focus();
                return;
            }

            //เช็คโควต้าผู้รับเหมาตัดกับโควต้าคนส่งอ้อย
            string[] txtcutrmsplit = txtCutOwner.Text.Split(':');
            string cutrmidchk = txtcutrmsplit[0].Trim();
            if (lvQuota == cutrmidchk && chk_CutdocA.Checked == true)
            {
                MessageBox.Show("ถ้าเปิดใบนำตัด ผู้รับเหมาตัดห้ามเป็นโควต้าเดียวกับโควต้าส่งอ้อย", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                txtSampleNo.Focus();
                return;
            }

            //เช็คโควต้าผู้รับเหมาบรรทุกกับโควต้าคนส่งอ้อย
            string[] txtcarrmsplit = txtCarOwner.Text.Split(':');
            string carrmidchk = txtcarrmsplit[0].Trim();
            if(lvQuota == carrmidchk && chk_CutdocA.Checked == true)
            {
                MessageBox.Show("ถ้าเปิดใบนำตัด ผู้รับเหมาบรรทุกห้ามเป็นโควต้าเดียวกับโควต้าส่งอ้อย", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                txtSampleNo.Focus();
                return;
            }

            //เช็คโควต้าผู้รับเหมาคีบกัยโควต้าคนส่งอ้อย
            string[] txtkeeprmsplit = txtKeepOwner.Text.Split(':');
            string keebrmchk = txtkeeprmsplit[0].Trim();
            if(lvQuota == keebrmchk && chk_CutdocA.Checked == true)
            {
                MessageBox.Show("ถ้าเปิดใบนำตัด ผู้รับเหมาคีบห้ามเป็นโควต้าเดียวกับโควต้าส่งอ้อย", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                txtSampleNo.Focus();
                return;
            }

            //เช็คโควต้าผู้รับเหมาคีบกัยโควต้าคนส่งอ้อย
            string[] txtallrmsplit = txtContractAll.Text.Split(':');
            string allrmchk = txtallrmsplit[0].Trim();
            if (lvQuota == allrmchk && chk_CutdocA.Checked == true)
            {
                MessageBox.Show("ถ้าเปิดใบนำตัด ผู้รับเหมารวมห้ามเป็นโควต้าเดียวกับโควต้าส่งอ้อย", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                txtSampleNo.Focus();
                return;
            }

            //เช็คทะเบียนรถว่ามี อยู่ในลานหรือไม่
            string lvChkDuptCar = GsysSQL.fncCheckCarNum(txtCarNum.Text, GVar.gvOnline);
            if (lvChkDuptCar != "" && pvmode == "" && pvmode != "Add Truck")
            {
                string lvChkStatus = GsysSQL.fncCheckCarNumStatus(txtCarNum.Text, GVar.gvOnline);

                MessageBox.Show("ทะเบียนรถซ้ำ กับคิวที่ " + lvChkDuptCar + " " + lvChkStatus + Environment.NewLine + Environment.NewLine + "กรุณาตรวจสอบข้อมูลดูอีกครั้ง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCarNum.Text = "";
                txtCarNum.Focus();

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                return;
            }
            else if (lvCarNum == lvCarNum2)
            {
                MessageBox.Show("ทะเบียนแม่ กับ ลูก ต้องไม่ซ้ำกัน กรุณาตรวจสอบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //txtCarNum.Text = "";
                txtCarNum.Focus();

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                return;
            }

           /* string lvCarnumbyCutdoc = GsysSQL.fncCheckCarNumByCutDocID(txtCutDoc.Text); //เลือกทะเบียนจากใบนำตัด
            if (lvCarNum != lvCarnumbyCutdoc && lvCutDoc != "" && !lvQ.Contains(".") && GVar.gvTypeProgram == "Q") //ถ้ามีใบนำตัดและทะเบียนไม่ตรงกัน
            {
                MessageBox.Show("ทะเบียนรถที่มาแจ้งที่ป้อมคิวไม่ตรงกับทะเบียนรถในใบนำตัดที่เลือกมา" + Environment.NewLine + "กรุณาตรวจสอบอีกครั้ง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //txtCarNum.Text = "";
                txtCarNum.Focus();

                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                return;
            }*/

            //string lvCarnumbyCutdoc2 = GsysSQL.fncCheckCarNumByCutDocID2(txtCutDoc.Text);
            //if (lvCarNum != lvCarnumbyCutdoc2 && lvCutDoc != "" && lvQ.Contains(".") && GVar.gvTypeProgram == "Q") //ถ้ามีใบนำตัดและทะเบียนไม่ตรงกัน
            //{
            //    MessageBox.Show("ทะเบียนรถพ่วงที่มาแจ้งที่ป้อมคิวไม่ตรงกับทะเบียนรถในใบนำตัดที่เลือกมา" + Environment.NewLine + "กรุณาตรวจสอบอีกครั้ง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    //txtCarNum.Text = "";
            //    txtCarNum.Focus();

            //    this.Cursor = Cursors.Default;
            //    btnSave.Enabled = true;
            //    btnClear.Enabled = true;
            //    return;
            //}

            //เช็คเลขตัวอย่าง
            if (lvSampleNo != "")
            {
                if ((lvSampleNo.Length > 4 && lvSampleNo.Length < 8) || lvSampleNo.Length < 4)
                {
                    MessageBox.Show("กรุณาระบุเลขตัวอย่างให้ถูกต้อง และ ครบถ้วน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    this.Cursor = Cursors.Default;
                    btnSave.Enabled = true;
                    btnClear.Enabled = true;
                    return;
                }
                else if (lvSampleNo.Length == 4)
                {
                    string lvChkSampleNo = "";

                    if (pvmode == "Edit")
                    {
                        //ถ้ามีการแก้ไขให้เช็คซ้ำ
                        if (GVar.gvSampleNoOld != lvSampleNo && lvSampleNo != "")
                        {
                            lvChkSampleNo = GsysSQL.fncCheckSampleNo(lvSampleNo, lvQ, GVar.gvOnline);
                        }
                    }
                    else
                    {
                        if (lvSampleNo != "")
                        {
                            //ถ้าเป็นโหมดอื่นๆ ให้เช็คตามปกติ
                            lvChkSampleNo = GsysSQL.fncCheckSampleNo(lvSampleNo, lvQ, GVar.gvOnline);
                        }
                    }

                    if (lvChkSampleNo != "")
                    {
                        MessageBox.Show("เลขตัวอย่าง " + lvSampleNo + " ซ้ำ กับคิวที่ " + lvChkSampleNo + Environment.NewLine + Environment.NewLine + "กรุณาตรวจสอบข้อมูลดูอีกครั้ง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }

                    //เช็คว่าเลขตัวอย่างถูกกับรางหรือไม่
                    string lvChkRailSample = FncCheckSampleNo(lvSampleNo, lvRail);
                    if (lvChkRailSample != "")
                    {
                        MessageBox.Show(lvChkRailSample, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }
                }
                else if (lvSampleNo.Length == 8)
                {
                    string lvSam1 = Gstr.Left(lvSampleNo, 4);
                    string lvSam2 = Gstr.Right(lvSampleNo, 4);
                    
                    string lvChkSam1 = Gstr.Left(GVar.gvSampleNoOld, 4);
                    string lvChkSam2 = Gstr.Right(GVar.gvSampleNoOld, 4);

                    string lvChkSampleNo = "";

                    if (pvmode == "Edit")
                    {
                        //ถ้ามีการแก้ไขให้เช็คซ้ำ
                        if (lvChkSam1 != lvSam1 && lvSam1 != "")
                        {
                            lvChkSampleNo = GsysSQL.fncCheckSampleNo(lvSam1, lvQ, GVar.gvOnline);
                        }
                    }
                    else
                    {
                        if (lvSam1 != "")
                        {
                            //ถ้าเป็นโหมดอื่นๆ ให้เช็คตามปกติ
                            lvChkSampleNo = GsysSQL.fncCheckSampleNo(lvSam1, lvQ, GVar.gvOnline);
                        }
                    }

                    if (lvChkSampleNo != "")
                    {
                        MessageBox.Show("เลขตัวอย่าง " + lvSam1 + " ซ้ำ กับคิวที่ " + lvChkSampleNo + Environment.NewLine + Environment.NewLine + "กรุณาตรวจสอบข้อมูลดูอีกครั้ง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }

                    //เช็คว่าเลขตัวอย่างถูกกับรางหรือไม่
                    string lvChkRailSample = FncCheckSampleNo(lvSam1, lvRail);
                    if (lvChkRailSample != "")
                    {
                        MessageBox.Show(lvChkRailSample, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }
                    
                    //เลขตัวอย่างชุด 2
                    lvChkSampleNo = "";
                    if (pvmode == "Edit")
                    {
                        //ถ้ามีการแก้ไขให้เช็คซ้ำ
                        if (lvChkSam2 != lvSam2 && lvSam2 != "")
                        {
                            lvChkSampleNo = GsysSQL.fncCheckSampleNo(lvSam2, lvQ, GVar.gvOnline);
                        }
                    }
                    else
                    {
                        if (lvSam2 != "")
                        {
                            //ถ้าเป็นโหมดอื่นๆ ให้เช็คตามปกติ
                            lvChkSampleNo = GsysSQL.fncCheckSampleNo(lvSam2, lvQ, GVar.gvOnline);
                        }
                    }

                    if (lvChkSampleNo != "")
                    {
                        MessageBox.Show("เลขตัวอย่าง " + lvSam2 + " ซ้ำ กับคิวที่ " + lvChkSampleNo + Environment.NewLine + Environment.NewLine + "กรุณาตรวจสอบข้อมูลดูอีกครั้ง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }

                    //เช็คว่าเลขตัวอย่างถูกกับรางหรือไม่
                    lvChkRailSample = FncCheckSampleNo(lvSam2, lvRail);
                    if (lvChkRailSample != "")
                    {
                        MessageBox.Show(lvChkRailSample, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }

                    //เลขที่ตัวอย่างแม่ลูกต้องไม่ซ้ำกัน
                    if (lvSam1 == lvSam2 && lvSampleNo.Length == 8)
                    {
                        MessageBox.Show("เลขตัวอย่าง รถแม่ กับ ลูก ต้องไม่ซ้ำกัน" + Environment.NewLine + Environment.NewLine + "กรุณาตรวจสอบข้อมูลดูอีกครั้ง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }
                }
            }

            //เช็คเลขที่บิลซ้ำ
            string lvChkBill = GsysSQL.FindBillNo(lvBilling, GVar.gvOnline);
            if (pvmode == "Edit" && GVar.gvNewBill && lvChkBill != "" && GVar.gvTypeProgram == "W")
            {
                MessageBox.Show("เลขที่บิล " + lvBilling + " ซ้ำ" + Environment.NewLine + Environment.NewLine + "กรุณาตรวจสอบข้อมูลดูอีกครั้ง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;

                //หน่วงเวลาสุ่ม 1-5 วิ
                Random rnd = new Random();
                int lvSleepTime = rnd.Next(1000, 2000);
                System.Threading.Thread.Sleep(lvSleepTime);

                //เช็คบิลก่อนหน้า
                string lvChkJumpBill = FncCheckJumpBill();
                lvBilling = lvChkJumpBill;
                lvChkBill = GsysSQL.FindBillNo(lvBilling, GVar.gvOnline);

                if (lvChkBill != "")
                    lvBilling = GsysSQL.fncGetLastBillNo(GVar.gvOnline);

                Rechk:
                string lvChkBill2 = GsysSQL.FindBillNo(lvBilling, GVar.gvOnline);
                if (lvChkBill2 != "")
                {
                    //อัพเดทบิล
                    string lvSQL2 = "Update SysDocNo Set S_RunNo += 1 Where S_MCode = 'Weight_Bill' ";
                    string lvResault2 = GsysSQL.fncExecuteQueryData(lvSQL2, GVar.gvOnline);

                    lvBilling = GsysSQL.fncGetLastBillNo(GVar.gvOnline);
                    goto Rechk;
                }

                if (lvChkJumpBill == "")
                {
                    txtBillNo.Text = GsysSQL.fncGetLastBillNo(GVar.gvOnline);
                    GVar.gvChangeBill = false;
                }
                else
                {
                    GVar.gvChangeBill = true;
                    txtBillNo.Text = lvChkJumpBill;
                }

                chkChangeBill.Checked = false;

                return;
            }

            //เช็คถ้าเป็นพ่วงต้องมาจากคิวแม่เท่านั้น
            if (pvmode != "Add Truck" && pvmode != "Edit" && chkCar4.Checked)
            {
                MessageBox.Show("ประเภทรถพ่วงต้อง คลิ๊กขวาเพิ่มจากคิวแม่เท่านั้น", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                txtBillNo.Text = GVar.gvBillingOld;
                return;
            }

            //Genใหม่อีกครั้ง
            if (pvmode == "" && pvmode != "Add Truck")
            {
                lvQ = GsysSQL.fncGenQueueNo(GVar.gvOnline);
                
                //Gen คิวตะกาว
                if (ChkA.Checked)
                    txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo_New("A", GVar.gvTypeDocTakao, GVar.gvOnline);
                else if (ChkB.Checked)
                    txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo_New("B", GVar.gvTypeDocTakao, GVar.gvOnline);
            }
            else if (pvmode == "Edit" && lbOUT.Visible)
            {                
                if (GVar.gvNewBill && !chkChangeBill.Checked)
                {
                    //เช็คเลขที่บิลอีกรอบ
                    //lvBilling = GsysSQL.fncGetLastBillNo(GVar.gvOnline);

                    Rechk:
                    string lvChkBill2 = GsysSQL.FindBillNo(lvBilling, GVar.gvOnline);
                    if (lvChkBill2 != "")
                    {

                    }
                }
            }

            txtQ.Text = lvQ;
            txtBillNo.Text = lvBilling;

            //เช็คว่ากดดึงน้ำหนักแล้วหรือยัง
            if (GVar.gvTypeProgram == "W")
            {
                string[] lvArr = lvQ.Split('.');
                //เช็คว่าคิวแม่เป็นชั่งรวมหรือไม่ ถ้าเป็นชั่งรวมไม่ต้องเช็คนำหนัก
                string lvWeightAllStatus = GsysSQL.fncCheckWeightALL(lvArr[0], GVar.gvOnline);

                if (GVar.gvINOUT == "IN")
                {
                    if (lvWeightAllStatus != "1" && Gstr.fncToDouble(txtWeightIN.Text) <= 0 && pvmode != "Add Truck")
                    {
                        MessageBox.Show("กรุณาโหลดข้อมูลน้ำหนัก ก่อนบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }

                    if (lvWeightAllStatus != "1" && txtWeightIN.Text == "" && pvmode != "Add Truck")
                    {
                        MessageBox.Show("กรุณาโหลดข้อมูลน้ำหนัก ก่อนบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }

                    //เช็คว่าถ้าเป็นชั่งรวมให้ใส่ทะเบียนรถพ่วงด้วย
                    if (ChkWeightAll.Checked)
                    {
                        if (txtCarNum2.Text == "" && txtCarNum2.Visible)
                        {
                            MessageBox.Show("กรุณาระบุ ทะเบียนรถลูก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtCarNum2.Focus();
                            this.Cursor = Cursors.Default;
                            btnSave.Enabled = true;
                            btnClear.Enabled = true;
                            return;
                        }
                    }
                }
                else if (GVar.gvINOUT == "OUT")
                {
                    if (lvWeightAllStatus != "1" && Gstr.fncToDouble(txtWeightIN.Text) <= 0 && pvmode != "Add Truck")
                    {
                        MessageBox.Show("ไม่พบน้ำหนักขาเข้า กรุณาชั่งน้ำหนักขาเข้าก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }
                    if (lvWeightAllStatus != "1" && txtWeightIN.Text == "" && pvmode != "Add Truck")
                    {
                        MessageBox.Show("ไม่พบน้ำหนักขาเข้า กรุณาชั่งน้ำหนักขาเข้าก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }
                    else if (lvWeightAllStatus != "1" && Gstr.fncToDouble(txtWeightOut.Text) == 0)
                    {
                        MessageBox.Show("กรุณาโหลดข้อมูลน้ำหนัก ก่อนบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }
                    else if (lvWeightAllStatus != "1" && txtWeightOut.Text == "")
                    {
                        MessageBox.Show("กรุณาโหลดข้อมูลน้ำหนัก ก่อนบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }
                    if (lvWeightAllStatus == "1" && Gstr.fncToDouble(txtWeightIN.Text) <= 0 && pvmode != "Add Truck")
                    {
                        MessageBox.Show("ไม่พบน้ำหนักขาเข้า กรุณาชั่งน้ำหนักขาเข้าก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }
                    if (lvWeightAllStatus == "1" && txtWeightIN.Text == "" && pvmode != "Add Truck")
                    {
                        MessageBox.Show("ไม่พบน้ำหนักขาเข้า กรุณาชั่งน้ำหนักขาเข้าก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }
                    else if(lvWeightAllStatus == "1" && !lvQ.Contains(".") && Gstr.fncToDouble(txtWeightOut.Text) <= 0 && pvmode != "Add Truck")
                    {
                        MessageBox.Show("กรุณาโหลดข้อมูลน้ำหนัก ก่อนบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }
                    else if (lvWeightAllStatus == "1" && !lvQ.Contains(".") && txtWeightOut.Text == "" && pvmode != "Add Truck")
                    {
                        MessageBox.Show("กรุณาโหลดข้อมูลน้ำหนัก ก่อนบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }
                    else if (lvWeightAllStatus == "1" && !lvQ.Contains(".") && Gstr.fncToDouble(txtWeightOut.Text) >= 0 && txtSampleNo.Text.Length != 8)
                    {
                        MessageBox.Show("กรณีชั่งรวม กรุณาระบุเลขที่ตัวอย่างให้ครบถ้วน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     
                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        txtSampleNo.Focus();
                        return;
                    }
                   
                }
            }
            else if (GVar.gvTypeProgram == "Q" && GVar.gvUser != "admin" && pvmode == "Edit")
            {
                if (Gstr.fncToDouble(txtWeightIN.Text) > 0 || txtWeightINDate.Text != "  /  /")
                {
                    MessageBox.Show("ไม่สามารถแก้ไขข้อมูลได้ เนื่องจาก คิว " + lvQ + " ได้ชั่งเข้าแล้ว" + Environment.NewLine + Environment.NewLine + "กรุณาตรวจสอบข้อมูลดูอีกครั้ง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Cursor = Cursors.Default;
                    btnSave.Enabled = true;
                    btnClear.Enabled = true;
                    return;
                }
            }

            //ถ้าคิวแม่เป็นชั่งรวม จะต้องชั่งน้ำหนักที่ คิวแม่เท่านั้น
            if (lvQ.Contains("."))
            {
                string[] lvArr = lvQ.Split('.');
                //เช็คว่าคิวแม่เป็นชั่งรวมหรือไม่ ถ้าเป็นชั่งรวมไม่ต้องเช็คนำหนัก
                string lvWeightAllStatus = GsysSQL.fncCheckWeightALL(lvArr[0], GVar.gvOnline);

                if (GVar.gvTypeProgram == "W" && lvWeightAllStatus == "1" && Gstr.fncToDouble(txtWeightIN.Text) > 0)
                {
                    MessageBox.Show("ไม่สามารถบันทึกได้ เนื่องจาก คิวแม่เป็นชั่งรวม  กรุณาทำรายการที่คิวแม่", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Cursor = Cursors.Default;
                    btnSave.Enabled = true;
                    btnClear.Enabled = true;
                    return;
                }
            }

            /*if (txtCutDoc.Text == "" && chkCane7.Checked == true)
            {
                MessageBox.Show("ต้องเปิดจากใบนำตัดเท่านั้น...", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chkCane7.Checked = false;
            }

            if (txtCutDoc.Text == "" && chkCane13.Checked == true)
            {
                MessageBox.Show("ต้องเปิดจากใบนำตัดเท่านั้น...", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chkCane7.Checked = false;
            }*/

            #endregion

            #region เช็คข้อมูล ระบบคิวล็อก ยังไม่ได้ใช้ ปี 6465
            ////ถ้าไม่คีย์ เลขคิวล็อก ก็ให้ปิด Tab QLock เหมือนเดิม
            //if (txtQLockNo.Text == "" && txtQLockQNo.Text == "") pnShowQLock.Visible = true;

            ////เช็คว่าจำเป็นต้องทำคิวล็อกหรือไม่ 
            ////สาวแต๋น พ่วง  ฟม.รถตัด  ฟม.สะอาด ไม่ต้องแจ้งเตือนทำคิวล็อก
            //string lvData = GsysSQL.fncFindQLockAlert();
            //string[] lvArrData = lvData.Split(',');
            //string lvLockAlert = lvArrData[0];
            //string lvChkCar1 = lvArrData[1];
            //string lvChkCar2 = lvArrData[2];
            //string lvChkCar3 = lvArrData[3];
            //string lvChkCar4 = lvArrData[4];
            //string lvChkCar5 = lvArrData[5];
            //string lvChkCane9 = lvArrData[6];
            //string lvChkCane10 = lvArrData[7];
            //string lvChkCane11 = lvArrData[8];
            //string lvChkCane12 = lvArrData[9];
            //string lvChkCane13 = lvArrData[10];
            //string lvChkCane14 = lvArrData[11];
            //string lvChkCane15 = lvArrData[12];
            //string lvChkCane16 = lvArrData[13];
            //string lvChkLoopNoS = lvArrData[14]; //loop Now S
            //string lvChkLockNoS = lvArrData[15]; //LockNo Now S
            //string lvChkLoopNoE = lvArrData[16]; //loop Now E
            //string lvChkLockNoE = lvArrData[17]; //LockNo Now E

            //GVar.gvLockAlert = lvLockAlert;

            //if (GVar.gvTypeProgram == "Q" && lvLockAlert == "1" && pnShowQLock.Visible) 
            //{
            //    string lvPass = FncCheckQLockCarAndCane();

            //    if (lvPass == "No")
            //    {
            //        MessageBox.Show("กรุณา ลงทะเบียนคิวล็อก เนื่องจากเป็นอ้อยไฟไหม้", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //        this.Cursor = Cursors.Default;
            //        btnSave.Enabled = true;
            //        btnClear.Enabled = true;
            //        return;
            //    }                                
            //}
            //else if (GVar.gvTypeProgram == "Q" && lvLockAlert == "1" && !pnShowQLock.Visible && GVar.gvLockBar != "")
            //{
            //    //รอบประกาศ
            //    int LoopNowS = Gstr.fncToInt(lvChkLoopNoS);
            //    int LockNowS = Gstr.fncToInt(lvChkLockNoS);
            //    int LoopNowE = Gstr.fncToInt(lvChkLoopNoE);
            //    int LockNowE = Gstr.fncToInt(lvChkLockNoE);

            //    //ถ้ามารอบถูก (เช็คประวัติแล้ว) Pass
            //    //string lvLoopChk = GsysSQL.fncFindMaxLoopByLockQNo(txtQLockQNo.Text); //loop จาก ตารางประวัติ

            //    //แยก PK To Loop lock Q
            //    string lvLoop = Gstr.Left(GVar.gvLockBar, 2);
            //    string lvLock = Gstr.Mid(GVar.gvLockBar, 3, 2);
            //    string lvLockQ = Gstr.Right(GVar.gvLockBar, 4);

            //    //มาจริง
            //    int LoopCheck = (Gstr.fncToInt(lvLoop));
            //    int LockCheck = (Gstr.fncToInt(lvLock));

            //    if (txtQLockNo.Text == "")
            //    {
            //        MessageBox.Show("กรุณาระบุ เลขที่ล็อก ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        this.Cursor = Cursors.Default;
            //        btnSave.Enabled = true;
            //        btnClear.Enabled = true;
            //        txtQLockNo.Focus();
            //        return;
            //    }
            //    else if (txtQLockQNo.Text == "")
            //    {
            //        MessageBox.Show("กรุณาระบุ เลขที่คิวล็อก ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        this.Cursor = Cursors.Default;
            //        btnSave.Enabled = true;
            //        btnClear.Enabled = true;
            //        txtQLockQNo.Focus();
            //        return;
            //    }

            //    int lvChkNowS = Gstr.fncToInt(LoopNowS.ToString() + LockNowS.ToString("00"));
            //    int lvChkNowE = Gstr.fncToInt(LoopNowE.ToString() + LockNowE.ToString("00"));
            //    int lvChkCome = Gstr.fncToInt(LoopCheck.ToString() + LockCheck.ToString("00"));

            //    if (lvChkCome >= lvChkNowS && lvChkCome <= lvChkNowE) //เช็คว่ามาทันช่วงที่ประกาศหรือไม่?
            //    {
            //        //ถ้าอยู่ในช่วง Pass
            //    }
            //    else
            //    {
            //        if (lvChkCome > lvChkNowE) //มาก่อนเวลา
            //        {
            //            //ถ้าไม่อยู่ -- Lock ที่มา > ช่วงประกาศ  แจ้งเตือน ยังไม่ถึงคิวเรียก ให้ไปรอ ลาน 5 
            //            MessageBox.Show("รอบที่ " + lvLoop + " ล็อกที่ " + lvLock + " คิวล็อกที่ " + lvLockQ + " ทะเบียน " + txtCarNum.Text + " ยังไม่ถึงคิวเรียก "+ Environment.NewLine +"ให้รอเรียกคิว ที่ลาน 5", "แจ้งเตือนยังไม่ถึงคิว", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            this.Cursor = Cursors.Default;
            //            btnSave.Enabled = true;
            //            btnClear.Enabled = true;
            //            return;                  
            //        }
            //        else if (lvChkCome < lvChkNowS) // มาไม่ทันเวลา ตกรอบ
            //        {
            //            //ถ้าไม่อยู่ -- Lock ที่มา < ช่วงประกาศ  แจ้งเตือน ไม่ทันรอบ บันทึก แล้วให้ไปรอ ลาน 5
            //            if (pvmode != "Edit")
            //                MessageBox.Show("รอบที่ " + lvLoop + " ล็อกที่ " + lvLock + " คิวล็อกที่ " + lvLockQ + " ทะเบียน " + txtCarNum.Text + " มาไม่ทันรอบปัจจุบัน " + Environment.NewLine + Environment.NewLine + "ให้บันทึกข้อมูลตามปกติ " + Environment.NewLine + "แล้วแจ้งรถให้ไปรอที่ ลาน 5", "แจ้งเตือนตกรอบ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //            GVar.gvStatusFail = "1";
            //        }
            //    }
            //}

            #endregion

            //ยืนยัน
            string lvTxtAlert = "ยืนยันการบันทึก คิวที่ " + lvQ;
            //if (GVar.gvDateBill != Gstr.fncChangeTDate(txtDate.Text)) lvTxtAlert = "  **วันที่ไม่ตรงกับใบเสร็จ**"+ Environment.NewLine + Environment.NewLine + "ยืนยันการทำรายการต่อหรือไม่?";
            DialogResult dialogResult = MessageBox.Show(lvTxtAlert, "Confirm?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                this.Cursor = Cursors.Default;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                return;
            }

            string lvSQL = "";
            string lvResault = "";
            string lvPaystatus = "";

            if (chk_CutdocA.Checked == true)
            {
                //ข้อมูลผู้รับเหมาตัด
                if (CutOwner != "")
                {
                    if (txtCutOwner.Text.Contains(':'))
                    {
                        lvCarcutRmsplit = CutOwner.Split(':');
                        lvCarcutRmid = lvCarcutRmsplit[0]; //รหัส 20
                        lvCarcutRmname = lvCarcutRmsplit[1]; //ชื่อ 21
                    }
                    else
                    {
                        lvCarcutRmid = txtCutOwner.Text; //รหัส 20
                    }

                    lvCarcutRmPrice = txtCutPrice.Text; //ราคา 22
                }


                //ข้อมูลผู้รับเหมาบรรทุก
                if (TruckOwner != "")
                {
                    if (txtCarOwner.Text.Contains(':'))
                    {
                        lvTruckRmsplit = TruckOwner.Split(':');
                        lvTruckRmid = lvTruckRmsplit[0]; //รหัส 23
                        lvTruckRmname = lvTruckRmsplit[1]; //ชื่อ 24

                    }
                    else
                    {
                        lvTruckRmid = txtCarOwner.Text;
                    }
                    lvTruckRmprice = txtCarryPrice.Text; //ราคา 25
                }

                //ข้อมูลผู้รับเหมาคีบ
                if (KeebOwner != "")
                {
                    if (txtKeepOwner.Text.Contains(':'))
                    {
                        lvKeebRmsplit = KeebOwner.Split(':');
                        lvKeebRmid = lvKeebRmsplit[0]; //รหัส 26
                        lvKeebRmname = lvKeebRmsplit[1]; //ชื่อ 27

                    }
                    else
                    {
                        lvKeebRmid = txtKeepOwner.Text;
                    }
                    lvKeebRmprice = txtKeepPrice.Text; //ราคา 28
                }

                //ข้อมูลผู้รับเหมารวม
                if (AllOwner != "")
                {
                    if (txtContractAll.Text.Contains(':'))
                    {
                        lvAllRmsplit = AllOwner.Split(':');
                        lvAllRmid = lvAllRmsplit[0]; //รหัส 29
                        lvAllRmname = lvAllRmsplit[1]; //ชื่อ 30
                    }
                    else
                    {
                        lvAllRmid = txtContractAll.Text;
                    }

                    lvAllRmprice = txtContractAllPrice.Text; //ราคา 31
                }
                
                if(chkCarryPrice.Checked == true)
                {
                    lvPaystatus = "ชำระ";
                }
                else
                {
                    lvPaystatus = "ไม่ชำระ";
                }

                if (pvmode == "Edit")
                {
                    //*วันที่ 30
                    //เช็คซ้ำใบนำตัด
                    string lvQCheck = GsysSQL.fncGetQByCutDocID(lvCutDoc); //เช็คว่าคิวที่ได้ใช่คิวตัวเองไหม
                    if(lvQ == lvQCheck)
                    {
                        //ถ้าคิวที่ได้เป็นคิวตัวเองไม่ต้องทำอะไร
                    }
                    else
                    {
                        //*วันที่ 30 ถ้าไม่ใช่คิวตัวเองให้ Insert เข้าไปใหม่ก่อน
                        lvSQL = "Insert Into Cane_QueueData (C_Quota, C_FullName, C_CutContactorId, C_CarcutNumber, C_Price, C_KeebContractorId, C_KeebContractorPrice, C_ContractorId, C_BoxTruckId, C_TruckPrice, " +
                          "C_TruckCarnum, C_TruckCarnum2, C_AllContractor, C_AllPrice, C_Queue, C_QueueStatus, C_UserId, C_Date, C_Time, C_PayStatus, C_TruckType, C_ContracType, Q_UserUpd, C_CaneType) Values ('" + lvQuota + "', '" + txtQuotaName.Text + "', '" + lvCarcutRmid + "', '" + lvCutCar + "', '" + lvCarcutRmPrice + "', '" + lvKeebRmid + "', " +
                          "'" + lvKeebRmprice + "', '" + lvTruckRmid + "', '" + lvCarNum + "', '" + lvTruckRmprice + "', '" + lvCarNum + "', '" + txtCarNum2.Text + "', '" + lvAllRmid + "', " +
                          "'" + lvAllRmprice + "', '" + txtQ.Text + "', 'รับข้อมูลเข้า', '" + GVar.gvUser + "', '" + lvDate + "', '" + lvTime + "', '" + lvPaystatus + "', '" + lvCarType + "', '" + C_ContracType + "', '" + GVar.gvUser + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "', '" + lvCaneType + "')";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, true);

                        lvCutDoc = GsysSQL.fncGetLastCutdocIDByQ(lvQ); //ไปเลือกใบนำตัดใบใหม่ที่ได้มาจากการ Insert
                    }

                    //อัพเดทข้อมูลใบนำตัด
                    lvSQL = "Update Cane_QueueData SET C_Quota = '" + lvQuota + "' ,C_CutContactorId = '" + lvCarcutRmid + "', C_CarcutNumber = '" + lvCutCar + "', C_Price = '" + lvCarcutRmPrice + "', " +
                       "C_KeebContractorId = '" + lvKeebRmid + "', C_KeebContractorPrice = '" + lvKeebRmprice + "', C_ContractorId = '" + lvTruckRmid + "', C_BoxTruckId = '" + lvCarNum + "', " +
                       "C_TruckPrice = '" + lvTruckRmprice + "', C_TruckCarnum = '" + lvCarNum + "', C_TruckCarnum2 = '" + txtCarNum2.Text + "', C_AllContractor = '" + lvAllRmid + "', " +
                       "C_AllPrice = '" + lvAllRmprice + "', C_TruckType = '" + lvCarType + "', C_PayStatus = '" + lvPaystatus + "', C_ContracType = '" + C_ContracType + "', Q_UserUpd = '" + GVar.gvUser + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "', C_CaneType = '" + lvCaneType + "'  WHERE C_ID = '" + lvCutDoc + "'  ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, true);

                    //*วันที่ 30 อัพเดทใบนำตัดใน Queue_Diary
                    lvSQL = "Update Queue_Diary SET Q_CutCar = '" + lvCutCar + "', Q_CaneDoc = '" + txtCaneDoc.Text + "', Q_CutPrice = '" + lvCarcutRmPrice + "', Q_CarryPrice = '" + lvTruckRmprice + "' " +
                       "WHERE Q_CutDoc = '" + lvCutDoc + "' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, true);


                }
                else
                {
                    //ถ้าไม่ใช่การ Edit และไม่ได้ Add คิวลูก
                    if(GVar.gvAddCarnum2 == false)
                    {
                        //*วันที่ 30
                        lvSQL = "Insert Into Cane_QueueData (C_Quota, C_FullName, C_CutContactorId, C_CarcutNumber, C_Price, C_KeebContractorId, C_KeebContractorPrice, C_ContractorId, C_BoxTruckId, C_TruckPrice, " +
                          "C_TruckCarnum, C_TruckCarnum2, C_AllContractor, C_AllPrice, C_Queue, C_QueueStatus, C_UserId, C_Date, C_Time, C_PayStatus, C_TruckType, C_ContracType, Q_UserUpd, C_CaneType) Values ('" + lvQuota + "', '" + txtQuotaName.Text + "', '" + lvCarcutRmid + "', '" + lvCutCar + "', '" + lvCarcutRmPrice + "', '" + lvKeebRmid + "', " +
                          "'" + lvKeebRmprice + "', '" + lvTruckRmid + "', '" + lvCarNum + "', '" + lvTruckRmprice + "', '" + lvCarNum + "', '" + txtCarNum2.Text + "', '" + lvAllRmid + "', " +
                          "'" + lvAllRmprice + "', '" + txtQ.Text + "', 'รับข้อมูลเข้า', '" + GVar.gvUser + "', '" + lvDate + "', '" + lvTime + "', '" + lvPaystatus + "', '" + lvCarType + "', '" + C_ContracType + "', '" + GVar.gvUser + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "', '" + lvCaneType + "')";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, true);
                    }
                    else
                    {
                        //*วันที่ 30 ถ้า Add คิวลูกให้อัพเดททะเบียนลูกเข้าไปเฉยๆ
                        lvSQL = "Update Cane_QueueData SET C_TruckCarnum2 = '" + txtCarNum.Text + "', Q_UserUpd = '" + GVar.gvUser + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "' WHERE C_ID = '" + lvCutDoc + "' ";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, true);
                    }
                    
                }
            }

            //*วันที่ 30 เก็บ Log ไว้จับใบนำตัดตอนบันทึกข้อมูลใบนำตัดเสร็จ
            GsysSQL.fncKeepLogData(GVar.gvUser, "ป้องกันใบนำตัด หลัง Insert เสร็จ", "เลขที่คิว : " + lvQ + "เลขที่โควต้า : " + lvQuota + "เลขใบนำตัด : " + lvCutDoc + "ผู้รับเหมาตัด : " + lvCarcutRmid + "ชื่อผู้รับเหมาตัด" + lvCarcutRmname + "ราคาผู้รับเหมาตัด : " + lvCarcutRmPrice + "ผู้รับเหมาบรรทุก : " + lvTruckRmid + "ชื่อผู้รับเหมาบรรทุก : " + lvTruckRmname +
                "ราคาผู้รับเหมาบรรทุก : " + lvTruckRmprice + "ผู้รับเหมาคีบ : " + lvKeebRmid + "ชื่อรับเหมาคีบ : " + lvKeebRmname + "ราคาผู้รับเหมาคีบ : " + lvKeebRmprice + "ผู้รับเหมารวม : " + lvAllRmid + "ชื่อผู้รับเหมารวม : " + lvAllRmname + "ราคาผู้รับเหมารวม : " + lvAllRmprice +
                 "วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "User ที่อัพเดท : " + GVar.gvUser + "Mode : " + pvmode + "Type ที่กด" + GVar.gvTypeProgram + "สถานะเรียกเก็บ : " + lvPaystatus);

            //ถ้าเป็นชั่งรวมให้เอาเฉพาะ 4 หลักแรกพอ
            if (ChkWeightAll.Checked)
            {
                if (lvSampleNo.Length == 8)
                    lvSampleNo2 = Gstr.Right(lvSampleNo, 4);
                else
                    lvSampleNo2 = "";

                lvSampleNo = Gstr.Left(lvSampleNo, 4);
            }

            //ถ้าเป็นโปรแรกมคิว pvmode = "" || ถ้าเป็นห้องชั่งกดปุ่มเพิ่ม pvmode = Add Truck
            if (pvmode == "" || pvmode == "Add Truck")
            {
                if (GVar.gvPermitNew != "1")
                {
                    MessageBox.Show("คุณไม่มีสิทธิ์ในการใช้งาน ฟังก์ชันนี้ กรุณาติดต่อผู้ดูแล !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Cursor = Cursors.Default;
                    btnSave.Enabled = true;
                    btnClear.Enabled = true;
                    return;
                }

                //Genใหม่อีกครั้ง
                if (pvmode == "" && pvmode != "Add Truck")
                {
                    lvQ = GsysSQL.fncGenQueueNo(GVar.gvOnline);
                }

                txtQ.Text = lvQ;
                txtBillNo.Text = lvBilling;

                string lvCutDocChk = GsysSQL.fncGetLastCutdocIDByQ(lvQ); //ไปเลือกใบนำตัดของคิวตัวเองมา
                if (lvCutDocChk != "")
                {
                    lvCutDoc = lvCutDocChk; //ถ้าเกิดมีใบนำตัดของคิวตัวเองให้ใช้ค่านี้แทนเพราะเวลาอัพเดทค่าบรรทุกโปรแกรมคิวกับใบนำตัดจะได้ไม่ซ้ำกัน กรณีกดพร้อมกัน 2 คิว
                }
                else
                {
                    //ถ้าไม่มี Insert ใบที่ SQL Generate ให้ เข้าไปตรงๆ
                }
                
                //บันทึกข้อมูล
                lvSQL = "Insert into Queue_Diary (Q_No, Q_Rail, Q_CarNum, Q_CutDoc, Q_Quota, Q_CaneDoc, Q_CaneType, Q_CutCar, Q_MainCar, Q_Date, Q_Time, Q_CarType,Q_CutPrice,Q_CarryPrice,Q_Station, Q_User, Q_Bonsugo, Q_WeightALLStatus, Q_CarryPriceStatus, Q_SubQuota) ";
                lvSQL += "Values ('" + lvQ + "', '" + lvRail + "', '" + lvCarNum + "', '" + lvCutDoc + "', '" + lvQuota + "', '" + lvCaneDoc + "', '" + lvCaneType + "', '" + lvCutCar + "', '" + lvMainCar + "', '" + lvDate + "', '" + lvTime + "', '" + lvCarType + "', '" + lvCutPrice + "', '" + lvCarryPrice + "', '" + lvStation + "', '" + lvUser + "', '" + lvBonsugo + "', '" + lvWeightAll + "', '" + lvChkCarryPrice + "', '" + lvSubQuota + "')";
                lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                //ถ้าเช็ครวมให้ Add Q ลูกอัตโนมัติโดยใช้รายละเอียดเดิม แต่เปลี่ยนเลขทะเบียน
                if (GVar.gvTypeProgram == "W" && GVar.gvINOUT == "IN" && ChkWeightAll.Checked)
                {
                    string lvQNew = lvQ + ".1";

                    //เช็คคิวเดิมก่อน ถ้ามีไม่ต้อง Add
                    string lvChkQ = GsysSQL.fncCheckQueue(lvQNew, GVar.gvOnline);

                    if (lvChkQ == "")
                    {
                        //บันทึกข้อมูล
                        lvSQL = "Insert into Queue_Diary (Q_No, Q_Rail, Q_CarNum, Q_CutDoc, Q_Quota, Q_CaneDoc, Q_CaneType, Q_CutCar, Q_MainCar, Q_Date, Q_Time, Q_CarType,Q_CutPrice,Q_CarryPrice,Q_Station, Q_User, Q_Bonsugo, Q_WeightALLStatus, Q_SubQuota) ";
                        lvSQL += "Values ('" + lvQNew + "', '" + lvRail + "', '" + lvCarNum2 + "', '" + lvCutDoc + "', '" + lvQuota + "', '" + lvCaneDoc + "', '" + lvCaneType + "', '" + lvCutCar + "', '" + lvMainCar + "', '" + lvDate + "', '" + lvTime + "', '" + lvCarType + "', '" + lvCutPrice + "', '" + lvCarryPrice + "', '" + lvStation + "', '" + lvUser + "', '" + lvBonsugo + "', '" + lvWeightAll + "', '" + lvSubQuota + "')";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                        //อัพเดททะเบียนลูกให้คิวแม่
                        lvSQL = "Update Queue_Diary set Q_CarNum2 = '"+ lvCarNum2 +"' Where Q_No = " + lvQ + " and Q_CloseStatus = '' and Q_Year = ''  ";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                    }
                }

                //ถ้าเป็นข้อมูลที่มาจากใบนำตัดให้แอดข้อมูลทะเบียนลูกไปเลย
                if (txtCutDoc.Text != "" && ChkWeightAll.Checked)
                {
                    string lvQNew = lvQ + ".1";

                    //เช็คคิวเดิมก่อน ถ้ามีไม่ต้อง Add
                    string lvChkQ = GsysSQL.fncCheckQueue(lvQNew, GVar.gvOnline);

                    if (lvQNew.Contains("."))
                    {
                        if (lvCarType != "พ่วง")
                        {
                            lvCarType = "พ่วง";
                        }
                    }

                    if (lvChkQ == "")
                    {
                        //บันทึกข้อมูล
                        lvSQL = "Insert into Queue_Diary (Q_No, Q_Rail, Q_CarNum, Q_CutDoc, Q_Quota, Q_CaneDoc, Q_CaneType, Q_CutCar, Q_MainCar, Q_Date, Q_Time, Q_CarType,Q_CutPrice,Q_CarryPrice,Q_Station, Q_User, Q_Bonsugo, Q_WeightALLStatus, Q_SubQuota) ";
                        lvSQL += "Values ('" + lvQNew + "', '" + lvRail + "', '" + lvCarNum2 + "', '" + lvCutDoc + "', '" + lvQuota + "', '" + lvCaneDoc + "', '" + lvCaneType + "', '" + lvCutCar + "', '" + lvMainCar + "', '" + lvDate + "', '" + lvTime + "', '" + lvCarType + "', '" + lvCutPrice + "', '" + lvCarryPrice + "', '" + lvStation + "', '" + lvUser + "', '" + lvBonsugo + "', '" + lvWeightAll + "', '" + lvSubQuota + "')";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                        //อัพเดททะเบียนลูกให้คิวแม่
                        lvSQL = "Update Queue_Diary set Q_CarNum2 = '" + lvCarNum2 + "' Where Q_No = " + lvQ + " and Q_CloseStatus = '' and Q_Year = ''  ";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                    }
                }

                //Update SysDoc
                if (GVar.gvStation == "1" && pvmode != "Add Truck")
                {
                    //อัพเดทป้อม 1
                    lvSQL = "Update SysDocNo Set S_RunNo += 1 Where S_MCode = 'Queue_01' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                }
                else if (GVar.gvStation == "2" && pvmode != "Add Truck")
                {
                    //อัพเดทป้อม 2
                    lvSQL = "Update SysDocNo Set S_RunNo += 1 Where S_MCode = 'Queue_02' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                }
                else if (GVar.gvStation == "3" && pvmode != "Add Truck")
                {
                    //อัพเดทป้อม 3
                    lvSQL = "Update SysDocNo Set S_RunNo += 1 Where S_MCode = 'Queue_03' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                }
            }
            else //ถ้าเป็นโหมด Edit
            {
                if (GVar.gvPermitEdit != "1")
                {
                    MessageBox.Show("คุณไม่มีสิทธิ์ในการใช้งาน ฟังก์ชันนี้ กรุณาติดต่อผู้ดูแล !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Cursor = Cursors.Default;
                    btnSave.Enabled = true;
                    btnClear.Enabled = true;
                    return;
                }

                //อัพเดทข้อมูล Queue_Diary
                lvSQL = "Update Queue_Diary Set Q_Rail = '" + lvRail + "', Q_CarNum = '" + lvCarNum + "', Q_CutDoc = '" + lvCutDoc + "', Q_Quota = '" + lvQuota + "', Q_CaneDoc = '" + lvCaneDoc + "', Q_CaneType = '" + lvCaneType + "', ";
                lvSQL += "Q_CutCar = '" + lvCutCar + "', Q_MainCar = '" + lvMainCar + "', Q_Date = '" + lvDate + "', Q_Time = '" + lvTime + "', Q_CarType = '" + lvCarType + "',Q_CutPrice = '" + lvCutPrice + "',Q_CarryPrice = '" + lvCarryPrice + "', ";
                lvSQL += "Q_Station = '" + lvStation + "', Q_User = '" + lvUser + "', Q_Bonsugo = '" + lvBonsugo + "' , Q_WeightALLStatus = '" + lvWeightAll + "', Q_CarryPriceStatus = '"+ lvChkCarryPrice +"', Q_SubQuota = '" + lvSubQuota + "' ";
                lvSQL += "Where Q_No = '" + lvQ + "' and Q_Year = '' ";

                //ทำเฉพาะรายการที่ยังไม่ปิดยอด
                lvSQL += "And Q_CloseStatus = '' ";
                lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                
                //ถ้าเป็นชั่งรวมให้อัพเดทรางให้เหมือนกับตัวแม่ด้วย
                if (lvWeightAll == "1")
                {
                    //อัพเดททะเบียนลูกให้คิวแม่
                     lvSQL = "Update Queue_Diary set Q_CarNum2 = '" + lvCarNum2 + "' Where Q_No = " + lvQ + " and Q_CloseStatus = '' and Q_Year = '' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                    //ถ้าเช็ครวมให้ Add Q ลูกอัตโนมัติโดยใช้รายละเอียดเดิม แต่เปลี่ยนเลขทะเบียน
                    if (GVar.gvTypeProgram == "W" && GVar.gvINOUT == "IN" && ChkWeightAll.Checked)
                    {
                        string lvQNew = lvQ + ".1";

                        //เช็คคิวเดิมก่อน ถ้ามีไม่ต้อง Add
                        string lvChkQ = GsysSQL.fncCheckQueue(lvQNew, true);
                        if (lvChkQ == "")
                        {
                            //บันทึกข้อมูล
                            lvSQL = "Insert into Queue_Diary (Q_No, Q_Rail, Q_CarNum, Q_CutDoc, Q_Quota, Q_CaneDoc, Q_CaneType, Q_CutCar, Q_MainCar, Q_Date, Q_Time, Q_CarType,Q_CutPrice,Q_CarryPrice,Q_Station, Q_User, Q_Bonsugo, Q_WeightALLStatus, Q_SubQuota) ";
                            lvSQL += "Values ('" + lvQNew + "', '" + lvRail + "', '" + lvCarNum2 + "', '" + lvCutDoc2 + "', '" + lvQuota + "', '" + lvCaneDoc + "', '" + lvCaneType + "', '" + lvCutCar2 + "', '" + lvMainCar + "', '" + lvDate + "', '" + lvTime + "', '" + lvCarType + "', '" + lvCutPrice + "', '" + lvCarryPrice + "', '" + lvStation + "', '" + lvUser + "', '" + lvBonsugo + "', '" + lvWeightAll + "', '" + lvSubQuota + "')";
                            lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                            //อัพเดททะเบียนลูกให้คิวแม่
                            lvSQL = "Update Queue_Diary set Q_CarNum2 = '" + lvCarNum2 + "' Where Q_No = " + lvQ + " and Q_CloseStatus = '' and Q_Year = '' ";
                            lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                        }
                       
                    }
                }

                //อัพเดททะรางให้คิวลูก 
                string lvQNew2 = "";
                if (lvQ.Contains("."))
                {
                    lvQNew2 = lvQ;
                }
                else
                {
                    if(lvWeightAll == "1")
                    {
                        lvQNew2 = lvQ + ".1";
                        lvSQL = "Update Queue_Diary Set Q_Rail = '" + lvRail + "', Q_CaneType = '" + lvCaneType + "', Q_Carnum = '" + lvCarNum2 + "' Where Q_No = '" + lvQNew2 + "' And Q_CloseStatus = '' and Q_Year = '' ";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                    }
                    else
                    {
                        lvQNew2 = lvQ + ".1";
                        string lvCarCum2Tw = GsysSQL.fncCheckOldCarnum2(lvQNew2, true);
                        lvSQL = "Update Queue_Diary Set Q_Rail = '" + lvRail + "', Q_CaneType = '" + lvCaneType + "', Q_Carnum = '" + lvCarCum2Tw + "' Where Q_No = '" + lvQNew2 + "' And Q_CloseStatus = '' and Q_Year = '' ";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                    }
                    
                }

                lvSQL = "Update Queue_Diary Set Q_Rail = '" + lvRail + "' Where Q_No = '" + lvQNew2 + "' And Q_CloseStatus = '' and Q_Year = '' ";
                lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
            }

            bool lvPrint = false;
            string lvTKNew2 = "";

            //ข้อมูลน้ำหนัก บันทึกเฉพาะโปรแกรมห้องชั่งเท่านั้น
            if (GVar.gvTypeProgram == "W")
            {
                if (GVar.gvINOUT != "OUT" && GVar.gvTKNew && !chkChangeTK.Checked)
                {
                    FncGenTKNo_New(lvRail, lvQ);
                }
                else if (chkChangeTK.Checked)
                {
                    //หา Last TypeDoc ของรางนั้นๆ มา + 1 แล้วเริ่มนับ 1 ใหม่
                    string lvLastTypeDoc = GsysSQL.fncGetLastTakaoTypeDoc(lvRail);
                    int lvLastNo = Gstr.fncToInt(lvLastTypeDoc) + 1;
                    GVar.gvTypeDocTakao = lvLastNo.ToString();
                    string GetWeightIN = GsysSQL.fncGetWeightIN(lvQ, GVar.gvOnline);
                    if(GetWeightIN == "")
                    {
                        lvSQL = "Insert into Queue_TableNumber(Q_Runno, Q_TypeDoc, Q_Rail, Q_Year, Q_QNo, Q_Status) Values ('" + txtTakaoNo.Text + "','" + lvLastNo + "','" + lvRail + "','','" + lvQ + "','ChangeTKNo') ";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                    }
                }

                double lvWeightIN = Gstr.fncToDouble(txtWeightIN.Text);

                string lvWeightINDate = Gstr.fncChangeTDate(txtWeightINDate.Text);
                if (lvWeightINDate == "  /  /") lvWeightINDate =  "";

                string lvWeoghtINTime = txtWeightINTime.Text;
                if (lvWeoghtINTime == "  :  :") lvWeoghtINTime = "";

                double lvWeightOUT = Gstr.fncToDouble(txtWeightOut.Text);

                string lvWeightOUTDate = Gstr.fncChangeTDate(txtWeightOutDate.Text);
                if (lvWeightOUTDate == "  /  /") lvWeightOUTDate = "";

                string lvWeoghtOUTTime = txtWeightOutTime.Text;
                if (lvWeoghtOUTTime == "  :  :") lvWeoghtOUTTime = "";

                string lvUserWeight = lbUser.Text;
                //string lvSampleNo = txtSampleNo.Text;
                lvTKNo = txtTakaoNo.Text;
                lvSampleNo = Gstr.Left(txtSampleNo.Text, 4);

            Rechk:

                if (GVar.gvNewBill && !chkChangeBill.Checked)
                {
                    //เช็คเลขที่บิลอีกรอบ
                    lvBilling = GsysSQL.fncGetLastBillNo(GVar.gvOnline);

                    string lvChkBill2 = GsysSQL.FindBillNo(lvBilling, GVar.gvOnline);
                    if (lvChkBill2 != "")
                    {
                        //อัพเดทบิล
                        lvSQL = "Update SysDocNo Set S_RunNo += 1 Where S_MCode = 'Weight_Bill' ";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                        lvBilling = GsysSQL.fncGetLastBillNo(GVar.gvOnline);
                        goto Rechk;
                    }
                }
                else
                {

                }

                try
                {
                    //lvBilling = "000011";
                    if(GVar.gvTypeProgram == "W" && lbOUT.Visible == true)
                    {

                    }
                    else
                    {
                        string mode = pvmode;
                        if(chkChangeTK.Checked == false)
                        {
                         string lvLastDocType = GVar.gvTypeDocTakao;
                        //อัพเดทราง A
                        //lvSQL = "Update SysDocNo Set S_RunNo += 1 Where S_MCode = 'Weight_RailA' ";
                        reins:
                            string GetWeightIN = GsysSQL.fncGetWeightIN(lvQ, GVar.gvOnline); //เลขตะกาว
                            if(GetWeightIN == "")
                            {
                                lvSQL = "Insert into Queue_TableNumber(Q_Runno,Q_TypeDoc,Q_Rail,Q_Year,Q_QNo) Values ('" + lvTKNo + "','" + GVar.gvTypeDocTakao + "','" + lvRail + "','" + "" + "','" + lvQ + "') ";
                                string lvChkTK = GsysSQL.fncCheckTKNo_ByTK(lvTKNo, lvLastDocType, lvRail);
                                if (lvChkTK == "")
                                {
                                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                                }
                                else
                                {
                                    lvTKNo = (Gstr.fncToInt(lvTKNo) + 1).ToString();
                                    goto reins;
                                }
                            }
                        }
                    }

                    //ข้อมูลน้ำหนัก
                    lvSQL = "Update Queue_Diary Set ";
                    lvSQL += "Q_WeightIN = '" + lvWeightIN + "', Q_WeightINDate = '" + lvWeightINDate + "', Q_WeightINTime = '" + lvWeoghtINTime + "', ";
                    lvSQL += "Q_WeightOUT = '" + lvWeightOUT + "', Q_WeightOUTDate = '" + lvWeightOUTDate + "', Q_WeightOUTTime = '" + lvWeoghtOUTTime + "', ";
                    lvSQL += "Q_TKNo = '" + lvTKNo + "', Q_WeightALLStatus = '" + lvWeightAll + "' ";

                    //lvSQL += "Q_SampleNo = '" + lvSampleNo + "', Q_BillingNo = '" + lvBilling + "', Q_TKNo = '" + lvTKNo + "', Q_WeightALLStatus = '" + lvWeightAll + "' ";

                    if (lvSampleNo != "") lvSQL += ", Q_SampleNo = '" + lvSampleNo + "' ";
                    if (lvBilling != "") lvSQL += ", Q_BillingNo = '" + lvBilling + "' ";

                    if (GVar.gvINOUT == "IN")
                        lvSQL += ", Q_UserWeightIN = '" + lvUserWeight + "'";
                    else
                        lvSQL += ", Q_UserWeightOUT = '" + lvUserWeight + "'";

                    lvSQL += "Where Q_No = '" + lvQ + "' and Q_Year = '' ";

                    //ทำเฉพาะรายการที่ยังไม่ปิดยอด
                    lvSQL += "And Q_CloseStatus = '' ";

                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                    
                    if(lbOUT.Visible == true)
                    {
                        //ส่งตันให้ลูกไร่แต่ละคนไม่เกิน 5000 ตัน
                        //DataTable DT = new DataTable();
                        //lvSQL = "Select Top 1 * From Cane_QuotaHeadUse Where Cane_QuotaHead = '" + lvQuota + "' And Cane_QuotaUse < '5000' ";
                        //DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

                        //for (int i = 0; i < DT.Rows.Count; i++)
                        //{
                        //    var SubQuota = DT.Rows[i]["Cane_Quota"].ToString();
                        //    lvSubQuotaName = GsysSQL.fncFindQuotaName(SubQuota, GVar.gvOnline);
                        //    GVar.gvSubQuota = SubQuota;
                        //    var lvWeightUpdate = lvWeightIN - lvWeightOUT;

                        //    //Update ตันที่ใช้ของลูกไร่
                        //    //lvSQL = "Update Cane_QuotaHeadUse SET Cane_QuotaUse = Cane_QuotaUse + '" + lvWeightUpdate + "' Where Cane_Quota = '" + SubQuota + "'";
                        //    //lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                        //    //Update ลูกไร่ใน Queue_Diary
                        //    lvSQL = "Update Queue_Diary SET Q_SubQuota = '" + SubQuota + "' Where Q_No = '" + lvQ + "' And Q_Year = ''";
                        //    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                        //}
                    }
                    
                    //ถ้า PK ซ้ำสั่งให้ไป Gen ใหม่ SQL จะเป็นคนเตือนเพราะตั้ง Unique Key เอาไว้ เคยเกิดกรณีคิวซ้ำแล้วห้องชั่งขาออกบันทึกไม่ได้ให้ไปดูตรงนี้ก่อน (Volt เตือนแล้วนะ)
                    if (lvResault.Contains("duplicate key"))
                    {
                        MessageBox.Show("เลขที่บิล " + lvBilling + " ซ้ำ กรุณาบันทึกใหม่อีกครั้ง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;

                        //เช็คเลขที่บิลอีกรอบ
                        chkChangeBill.Checked = false;
                        txtBillNo.Text = GsysSQL.fncGetLastBillNo(GVar.gvOnline);
                        return;

                        //if (GVar.gvNewBill && !chkChangeBill.Checked)
                        //{
                        //    goto Rechk;
                        //}
                    }
                    else if (lvResault.Contains("deadlocked")) //ยังไม่เคยเจอ deadlocked
                    {
                        MessageBox.Show("ไม่สามารถบันทึกข้อมูลได้ กรุณากดบันทึกใหม่อีกครั้ง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Cursor = Cursors.Default;
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                        return;
                    }

                    //เช็คความผิดปกติ
                    if (lvResault != "Success")
                    {
                        lineNotify(lvResault + " คิวที่ " + lvQ);
                        //lineNotify2(lvResault + " คิวที่ " + lvQ);
                    }
                }
                catch (Exception ex)
                {
                    lineNotify(ex.Message);
                    //lineNotify2(ex.Message);
                }

                //if (lvWeightOUT > 0) lvPrint = true;
                lvPrint = true;
                string lvQ2 = "";
                //ถ้าเป็นชั่งรวมให้อัพเดทรางให้เหมือนกับตัวแม่ด้วย
                string lvQNew2 = "";
                if (lvWeightAll == "1")
                {
                    
                    lvQNew2 = lvQ + ".1";
                    
                reGen:
                    lvTKNew2 = "";
                    //if (lvQNew2.Contains('.') && pvmode == "Edit")
                    //{
                    //    lvTKNew2 = GsysSQL.fncGetTakaoNo2(lvQNew2, GVar.gvOnline);
                    //}
                    
                    //หน่วงเวลาสุ่ม 1-5 วิ
                    Random rnd = new Random();
                    int lvSleepTime = rnd.Next(500, 1000);
                    System.Threading.Thread.Sleep(lvSleepTime);

                    if (ChkA.Checked)
                    {
                        string GetWeightIN = GsysSQL.fncGetWeightIN(lvQNew2, GVar.gvOnline);
                        //string lvTKNO2 = GsysSQL.fncGetLastTakaoNo_New(lvRail, GVar.gvTypeDocTakao, GVar.gvOnline);
                        //lvTKNew2 = (Gstr.fncToInt(lvTKNO2) + 1).ToString();
                        if(GetWeightIN == "")
                        {
                            lvTKNew2 = GsysSQL.fncGetLastTakaoNo_New(lvRail, GVar.gvTypeDocTakao, GVar.gvOnline).ToString();
                        }
                        else
                        {
                            lvTKNew2 = GsysSQL.fncGetWeightIN(lvQNew2, GVar.gvOnline);
                        }
                       
                    }
                  
                    else if (ChkB.Checked)
                    {
                        string GetWeightIN = GsysSQL.fncGetWeightIN(lvQNew2, GVar.gvOnline);
                        //string lvTKNO2 = GsysSQL.fncGetLastTakaoNo_New(lvRail, GVar.gvTypeDocTakao, GVar.gvOnline);
                        //lvTKNew2 = (Gstr.fncToInt(lvTKNO2) + 1).ToString();

                        if(GetWeightIN == "")
                        {
                            lvTKNew2 = GsysSQL.fncGetLastTakaoNo_New(lvRail, GVar.gvTypeDocTakao, GVar.gvOnline).ToString();
                        }
                        else
                        {
                            lvTKNew2 = GsysSQL.fncGetWeightIN(lvQNew2, GVar.gvOnline);
                        }

                    }

                    #region////รีเซ็ตถ้ามากกว่า 1000
                    ////รีเซ็ตถ้ามากกว่า 1000
                    //if (Gstr.fncToInt(lvTKNew2) > 200)
                    //{
                    //    //อัพเดทราง A
                    //    lvSQL = "Update Queue_Diary Set Q_TKNoCheck = '1' Where Q_Rail = '" + lvRail + "' And Q_TKNo <> '' and Q_Year = '' ";
                    //    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                    //    goto reGen;
                    //}
                    #endregion

                    //เช็คว่าตะกาวที่ Gen ได้ซ้ำไหม ถ้าไม่ใช่โหมด Edit
                    if (pvmode != "Edit")
                    {
                        string lvChkTK = GsysSQL.fncCheckTKNo_New(lvTKNew2, lvRail, GVar.gvOnline);
                        string lvLastDocType = GsysSQL.fncGetLastTakaoTypeDoc(lvRail);
                        if (lvChkTK != "")
                        {
                            if (ChkA.Checked)
                                lvTKNew2 = GsysSQL.fncGetLastTakaoNo_New(lvRail, lvLastDocType, GVar.gvOnline);
                            else if (ChkB.Checked)
                                lvTKNew2 = GsysSQL.fncGetLastTakaoNo_New(lvRail, lvLastDocType, GVar.gvOnline);

                            goto reGen;
                        }
                    }

                    if (ChkA.Checked)
                    {
                        lvQNew2 = lvQ;
                        string lvLastDocType = GVar.gvTypeDocTakao;

                        ////อัพเดทราง A
                        ////lvSQL = "Update SysDocNo Set S_RunNo += 1 Where S_MCode = 'Weight_RailA' ";
                        //reins:
                        //lvSQL = "Insert into Queue_TableNumber(Q_Runno,Q_TypeDoc,Q_Rail,Q_Year,Q_QNo) Values ('" + lvTKNo + "','" + GVar.gvTypeDocTakao + "','" + lvRail + "','" + "" + "','" + lvQ + "') ";
                        //string lvChkTK = GsysSQL.fncCheckTKNo_ByTK(lvTKNo, lvLastDocType, lvRail);
                        //if(lvChkTK == "")
                        //{
                        //    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                        //}
                        //else
                        //{
                        //    lvTKNo = (Gstr.fncToInt(lvTKNo) + 1).ToString();
                        //    goto reins;
                        //}

                        if (ChkWeightAll.Checked)
                        {
                        //lvTKNew2 = (Gstr.fncToInt(lvTKNo) + 1).ToString();
                        rechk:

                            
                            if (lvQ.Contains("."))
                            {
                                lvQ2 = lvQNew2;
                            }
                            else
                            {
                                lvQ2 = lvQNew2 + ".1";
                            }

                            string GetWeightIN = GsysSQL.fncGetWeightIN(lvQ2, GVar.gvOnline); //น้ำหนักขาเข้า
                            if (GetWeightIN == "")
                            {
                                lvTKNew2 = GsysSQL.fncGetLastTakaoNo_New(lvRail, GVar.gvTypeDocTakao, GVar.gvOnline);
                                lvSQL = "Insert into Queue_TableNumber(Q_Runno,Q_TypeDoc,Q_Rail,Q_Year,Q_QNo) Values ('" + lvTKNew2 + "','" + GVar.gvTypeDocTakao + "','" + lvRail + "','" + "" + "','" + lvQ2 + "') ";

                                string lvChkTK = GsysSQL.fncCheckTKNo_ByTK(lvTKNew2, lvLastDocType, lvRail);
                                if (lvChkTK == "")
                                {
                                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                                }
                                else
                                {
                                    if (lvTKNew2 == "502")
                                    {
                                        lvTKNew2 = "1";
                                        GVar.gvTypeDocTakao = (Gstr.fncToInt(GVar.gvTypeDocTakao) + 1).ToString();
                                    }
                                    else
                                    {
                                        lvTKNew2 = (Gstr.fncToInt(lvTKNew2) + 1).ToString();
                                        goto rechk;
                                    }

                                }
                            }
                            else
                            {
                                lvTKNew2 = GetWeightIN;
                            }

                        }
                    }
                    else if (ChkB.Checked)
                    {
                        lvQNew2 = lvQ;
                        string lvLastDocType = GVar.gvTypeDocTakao;
                        //อัพเดทราง B
                        //lvSQL = "Update SysDocNo Set S_RunNo += 1 Where S_MCode = 'Weight_RailB' ";
                        //reins:
                        //    lvSQL = "Insert into Queue_TableNumber(Q_Runno,Q_TypeDoc,Q_Rail,Q_Year,Q_QNo) Values ('" + lvTKNo + "','" + GVar.gvTypeDocTakao + "','" + lvRail + "','" + "" + "','" + lvQ + "') ";
                        //    string lvChkTK = GsysSQL.fncCheckTKNo_ByTK(lvTKNo, lvLastDocType, lvRail);
                        //    if (lvChkTK == "")
                        //    {
                        //        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                        //    }
                        //    else
                        //    {
                        //        lvTKNo = (Gstr.fncToInt(lvTKNo) + 1).ToString();
                        //        goto reins;
                        //    }

                       
                        if (ChkWeightAll.Checked)
                        {
                        rechk:

                            
                            if (lvQ.Contains("."))
                            {
                                lvQ2 = lvQNew2;
                            }
                            else
                            {
                                lvQ2 = lvQNew2 + ".1";
                            }
                            //lvTKNew2 = (Gstr.fncToInt(lvTKNew2) + 1).ToString();
                            string GetWeightIN = GsysSQL.fncGetWeightIN(lvQ2, GVar.gvOnline);
                            if (GetWeightIN == "")
                            {
                                lvTKNew2 = GsysSQL.fncGetLastTakaoNo_New(lvRail, GVar.gvTypeDocTakao, GVar.gvOnline);
                                lvSQL = "Insert into Queue_TableNumber(Q_Runno,Q_TypeDoc,Q_Rail,Q_Year,Q_QNo) Values ('" + lvTKNew2 + "','" + GVar.gvTypeDocTakao + "','" + lvRail + "','" + "" + "','" + lvQ2 + "') ";

                                string lvChkTK = GsysSQL.fncCheckTKNo_ByTK(lvTKNew2, lvLastDocType, lvRail);
                                if (lvChkTK == "")
                                {
                                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                                }

                                else
                                {
                                    if (lvTKNew2 == "502")
                                    {
                                        lvTKNew2 = "1";
                                        GVar.gvTypeDocTakao = (Gstr.fncToInt(GVar.gvTypeDocTakao) + 1).ToString();
                                    }
                                    else
                                    {
                                        lvTKNew2 = (Gstr.fncToInt(lvTKNew2) + 1).ToString();
                                        goto rechk;
                                    }
                                }
                            }
                            else
                            {
                                lvTKNew2 = GetWeightIN;
                            }
                        }
                    }

                    lvSQL = "Update Queue_Diary Set Q_Rail = '" + lvRail + "', Q_TKNo = '" + lvTKNew2 + "', Q_WeightINDate = '" + lvWeightINDate + "', Q_WeightINTime = '" + lvWeoghtINTime + "', Q_SampleNo = '" + lvSampleNo2 + "'  Where Q_No = '" + lvQ2 + "' And Q_CloseStatus = '' and Q_Year = '' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                    //อัพเดททะรางให้คิวลูก และ ก็อัพทะเบียนรถให้คิวลูก กรณีแก้ที่คิวแม่
                    //lvSQL = "Update Queue_Diary Set Q_Rail = '" + lvRail + "', Q_CarNum = '" + lvCarNum2 + "', Q_CutCar = '" + lvCutCar2 + "', Q_CutDoc = '" + lvCutDoc2 + "' Where Q_No = '" + lvQNew2 + "' And Q_CloseStatus = '' and Q_Year = '' ";
                    //lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                    //อัพเดทสถานะว่าสมบูรณ์ที่ พ่วงแล้ว เพื่อจะได้ไม่ต้องแสดง
                    if (lvWeightOUT > 0)
                    {
                        string lvCar2 = GsysSQL.fncFindCarNum2(lvQ2, GVar.gvOnline);
                        lvSQL = "Update Queue_Diary Set Q_WeightFinish = '1' Where Q_No = '" + lvQ2 + "' And Q_CloseStatus = '' and Q_Year = '' ";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                    }
                }
                

                //Update SysDoc
                //อัพเดทบิล
                lvSQL = "Update SysDocNo Set S_RunNo += 1 Where S_MCode = 'Weight_Bill' ";
                if (lvBillingOld != lvBilling && !chkChangeBill.Checked && !GVar.gvChangeBill) lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                #region //ไม่ใช้
                /*if (GVar.gvTypeProgram == "W" && lbOUT.Visible == true)
                {

                }
                else
                {

                    if(ChkWeightAll.Checked == true)
                    {
                       
                        if (lvQNew2.Contains("."))
                        {
                            lvQ2 = lvQNew2;
                        }
                        else
                        {
                            lvQ2 = lvQNew2 + ".1";
                        }
                       
                        lvSQL = "Update Queue_Diary Set Q_Rail = '" + lvRail + "', Q_TKNo = '" + lvTKNew2 + "', Q_WeightINDate = '" + lvWeightINDate + "', Q_WeightINTime = '" + lvWeoghtINTime + "', Q_SampleNo = '" + lvSampleNo2 + "'  Where Q_No = '" + lvQ2 + "' And Q_CloseStatus = '' and Q_Year = '' ";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                    }
                    else
                    {

                    }
                   
                }*/
                #endregion

                //อัพเดทข้อมูลไปที่ลานย่อย
                string lvChkCarLan = GsysSQL.fncCheckCarLan(lvCarNum, true, GVar.gvOnline);
                if (lvChkCarLan != "")
                {
                    //อัพเลขเลขบิลย่อยให้บิลโรงงาน
                    lvSQL = "Update Queue_Diary set Q_BillLan = '" + lvChkCarLan + "' where Q_No = '" + lvQ + "' and Q_CloseStatus = '' and Q_Year = '' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                    //อัพเลขเลขบิลโรงงานให้บิลย่อย
                    lvSQL = "Update MiniCane_BillHD set M_BillNo = '" + lvBilling + "' where M_DocNo = '" + lvChkCarLan + "' and M_Year = '63/64' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                }

                #region //แถวๆนี้น่าจะเป็น Q Lock
                string lvChkQLockRegis = FncCheckQLockCarAndCane();
                GVar.gvPrintQLock = "No";
                //ต้องเป็นขาออกเท่านั้น ระบบคิวล็อกต้องเปิด ต้องเป็นคิวที่ทำคิวล็อก มา
                if (GVar.gvINOUT == "OUT" && GVar.gvLockAlert == "1" && lvChkQLockRegis == "No")
                {
                    //ระบบคิวล็อก Gen BarCode ตอนออก
                    string lvPK = GsysSQL.fncFindLastLockPK();

                ReChkPK:

                    string lvChkDuptPK = GsysSQL.fncFindLockPK(lvPK);
                    if (lvChkDuptPK != "")
                    {
                        //หน่วงเวลาสุ่ม 1-2 วิ
                        Random rnd = new Random();
                        int lvSleepTime = rnd.Next(1000, 2000);
                        System.Threading.Thread.Sleep(lvSleepTime);

                        lvPK = GsysSQL.fncFindLastLockPK();

                        goto ReChkPK;
                    }

                    //lvPK = "03010001";

                    //แยก PK To Loop lock Q
                    string lvLoop = Gstr.Left(lvPK, 2);
                    string lvLock = Gstr.Mid(lvPK, 3, 2);
                    string lvLockQ = Gstr.Right(lvPK, 4);

                    //ถ้าเป็นการแก้ไข หรือ มีข้อมูลแล้วไม่ต้อง Add
                    string lvChkQLock = GsysSQL.fncFindLockPKByQ(lvQ);

                    if (lvChkQLock == "")
                    {
                        //บันทึกลงตาราง Diary เก็บข้อมูลแต่ละรอบ
                        lvSQL = "Insert into Queue_LockDiary (L_PK, L_Loop, L_LockNo, L_LockQNo, L_LockCarNum, L_LockDate, L_LockTime, L_QMain) ";
                        lvSQL += "Values ('" + lvPK + "', '" + lvLoop + "', '" + lvLock + "', '" + lvLockQ + "', '" + lvCarNum + "', '" + lvWeightOUTDate + "', '" + lvWeoghtOUTTime + "', '" + lvQ + "')";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                        if (lvResault.Contains("PRIMARY KEY"))
                        {
                            lvPK = GsysSQL.fncFindLastLockPK();
                            goto ReChkPK;
                        }

                        GVar.gvLockBar = lvPK;
                    }
                    else
                    {
                        GVar.gvLockBar = lvChkQLock;
                    }

                    GVar.gvPrintQLock = "Yes";
                }
                #endregion

            }
            else
            {
                lvPrint = true;
            }

            #region //คิวล็อก  ปี 64/65  ไม่ใช้
            ////เฉพาะคิว  สถานะเพิ่มคิว เท่านั้น   สถานะคิวต้องเปิด
            //string lvLockLoop = txtQLoop.Text; //รอบ 
            //string lvLockNo = txtQLockNo.Text; //ล็อก
            //string lvLockQNo = txtQLockQNo.Text; //คิวล็อก

            //string lvChk = GsysSQL.fncCheckQLockByQMain(lvQ);

            //if (GVar.gvTypeProgram == "Q" && lvChk == "" && GVar.gvQLockOnOff == "ON" && !pnShowQLock.Visible)
            //{
            //    //Update ตารางหลัก
            //    lvSQL = "Update Queue_Diary set Q_LockNo = '" + lvLockNo + "',Q_LockQNo = '" + lvLockQNo + "',Q_Lockloop = '" + lvLockLoop + "',Q_LockFail = '" + GVar.gvStatusFail + "',Q_LockBarCode = '" + GVar.gvLockBar + "'  where Q_No = '" + lvQ + "' and Q_Year = '' ";
            //    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

            //    //บันทึกลงตาราง Diary เก็บข้อมูลแต่ละรอบ
            //    lvSQL = "Update Queue_LockDiary Set L_LockNextCarNum = '" + lvCarNum + "', L_LockNextDate = '" + lvDate + "', L_LockNextTime = '" + lvTime + "', L_QSub = '" + lvQ + "' Where L_PK = '" + GVar.gvLockBar + "' ";
            //    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

            //    ////บันทึกลงตาราง Active ไว้ด้วย
            //    //lvSQL = "Update Queue_LockActive set L_CarNum = '" + lvCarNum + "',L_DateRegister = '" + lvDate + "' where L_LockQNo = '" + lvLockQNo + "' ";
            //    //lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

            //    ////บันทึกลงตาราง Diary เก็บข้อมูลแต่ละรอบ
            //    //lvSQL = "Insert into Queue_LockDiary (L_Loop, L_LockNo, L_LockQNo, L_LockCarNum, L_LockDate, L_LockTime, L_QMain) ";
            //    //lvSQL += "Values ('" + lvLockLoop + "', '" + lvLockNo + "', '" + lvLockQNo + "', '" + lvCarNum + "', '" + lvDate + "', '" + lvTime + "', '" + lvQ + "')";
            //    //lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
            //}
            #endregion

            //ถ้าบันทุึกทุกอย่างสำเร็จ
            if (lvResault == "Success" )
            {
                double lvWeightIN = Gstr.fncToDouble(txtWeightIN.Text);
                string lvWeightINDate = Gstr.fncChangeTDate(txtWeightINDate.Text);
                string lvWeightINTime = txtWeightINTime.Text;
                double lvWeightOUT = Gstr.fncToDouble(txtWeightOut.Text);
                string lvWeightOUTDate = Gstr.fncChangeTDate(txtWeightOutDate.Text);
                string lvWeightOUTTime = txtWeightOutTime.Text;
                //string lvSubQuotaName = GsysSQL.fncFindQuotaName(GVar.gvSubQuota, GVar.gvOnline);

                string lvTXT = "เพิ่ม";
                if (pvmode != "") lvTXT = "แก้ไข";

                lvCaneType = GsysSQL.fncFindCaneTypeName(lvCaneType, GVar.gvOnline);

                //เก็บ Log การบันทึกข้อมูล
                lvTXT += " ข้อมูลคิวที่= " + lvQ + " ลานที่= " + lvStation + " โควต้า= " + lvQuota + " ทะเบียน= " + lvCarNum; // + " รอบที่= " + lvLockLoop + " ล็อก= " + lvLockNo + " ล็อกคิวที่= " + lvLockQNo
                lvTXT += " ชั่งเข้า= " + lvWeightIN + " (" + lvWeightINDate + " " + lvWeightINTime + ")";
                lvTXT += " ชั่งออก= " + lvWeightOUT + " (" + lvWeightOUTDate + " " + lvWeightOUTTime + ")";
                lvTXT += " เลขตัวอย่าง= " + lvSampleNo + lvSampleNo2 + " เลขที่บิล= " + lvBilling + " โควต้าที่ไม่ได้ลงทะเบียน= " + GVar.gvSubQuota;
                lvTXT += " ประเภทอ้อย= " + lvCaneType;
                GsysSQL.fncKeepLogData(GVar.gvUser, "โปรแกรมคิว", lvTXT);
                
                string lvState = "";
                if (ChkA.Checked) lvState = "A";
                else if (ChkB.Checked) lvState = "B";
                else
                    lvState = "-";

                string lvTypeProgram = GVar.gvTypeProgram;
                if (GVar.gvTypeProgram == "W" && lbIN.Visible) lvTypeProgram = "Q";

                //ถ้าชั่งรวมให้แสดงเลขทะเบียนพ่วงด้วย
                if (ChkWeightAll.Checked)
                {
                    string lvQNew = "";

                    if (lvQ.Contains('.'))
                        lvQNew = lvQ;
                    else
                        lvQNew = lvQ + ".1";

                    if (GVar.gvINOUT == "OUT")
                    {
                        if (lvCarNum == "")
                            lvCarNum = GsysSQL.fncFindCarNum2(lvQNew, GVar.gvOnline);
                        else
                        {
                            string lvChkCarNum2 = GsysSQL.fncFindCarNum2(lvQNew, GVar.gvOnline);

                            if (lvChkCarNum2 != "")
                                lvCarNum += " - " + lvChkCarNum2;
                            else
                                lvCarNum += " - " + lvCarNum2;
                        }
                            
                    }

                    lvSampleNo = txtSampleNo.Text;
                }

                #region //ประกาศตัวแปรสั่งปริ้นท์ใบนำตัดอีกครั้ง
                //ข้อมูลผู้รับเหมาตัด
                if (CutOwner != "")
                {
                    if (txtCutOwner.Text.Contains(':'))
                    {
                        lvCarcutRmsplit = CutOwner.Split(':');
                        lvCarcutRmid = lvCarcutRmsplit[0]; //รหัส 20
                        lvCarcutRmname = lvCarcutRmsplit[1]; //ชื่อ 21
                    }
                    else
                    {
                        lvCarcutRmid = txtCutOwner.Text; //รหัส 20
                    }

                    lvCarcutRmPrice = txtCutPrice.Text; //ราคา 22
                }


                //ข้อมูลผู้รับเหมาบรรทุก
                if (TruckOwner != "")
                {
                    if (txtCarOwner.Text.Contains(':'))
                    {
                        lvTruckRmsplit = TruckOwner.Split(':');
                        lvTruckRmid = lvTruckRmsplit[0]; //รหัส 23
                        lvTruckRmname = lvTruckRmsplit[1]; //ชื่อ 24

                    }
                    else
                    {
                        lvTruckRmid = txtCarOwner.Text;
                    }
                    lvTruckRmprice = txtCarryPrice.Text; //ราคา 25
                }

                //ข้อมูลผู้รับเหมาคีบ
                if (KeebOwner != "")
                {
                    if (txtKeepOwner.Text.Contains(':'))
                    {
                        lvKeebRmsplit = KeebOwner.Split(':');
                        lvKeebRmid = lvKeebRmsplit[0]; //รหัส 26
                        lvKeebRmname = lvKeebRmsplit[1]; //ชื่อ 27

                    }
                    else
                    {
                        lvKeebRmid = txtKeepOwner.Text;
                    }
                    lvKeebRmprice = txtKeepPrice.Text; //ราคา 28
                }

                //ข้อมูลผู้รับเหมารวม
                if (AllOwner != "")
                {
                    if (txtContractAll.Text.Contains(':'))
                    {
                        lvAllRmsplit = AllOwner.Split(':');
                        lvAllRmid = lvAllRmsplit[0]; //รหัส 29
                        lvAllRmname = lvAllRmsplit[1]; //ชื่อ 30
                    }
                    else
                    {
                        lvAllRmid = txtContractAll.Text;
                    }

                    lvAllRmprice = txtContractAllPrice.Text; //ราคา 31
                }
                #endregion
                
                //พิมพ์ครั้งแรก
                GVar.gvPrintAgain = "";
                FncPrintQueue(txtQ.Text,txtQuota.Text, txtQuotaName.Text, lvCarNum, lvCaneType, chkPreview.Checked, lvState, lvTypeProgram, lvDate, lvCaneDoc, lvWeightIN.ToString("#,##0"), lvWeightINTime, lvWeightOUT.ToString("#,##0"), lvWeightOUTTime, lvPrint, lvTypeProgram, lvSampleNo, lvBilling, lvStation, lvTKNo, lvRail, lvCutCar, lvCarType, lvTime, lvWeightOUTDate, lvCutCar, lvSampleNo, lvCarcutRmid, lvCarcutRmname, lvCarcutRmPrice, lvTruckRmid, 
                    lvTruckRmname, lvTruckRmprice, lvKeebRmid, lvKeebRmname, lvKeebRmprice, lvAllRmid, lvAllRmname, lvAllRmprice, lvCutDoc);

                //Update ข้อมูล สถานะ + คิว ให้ตารางใบนำตัด
                string lvStatus = "";

                if (lvWeightIN > 0 && lvWeightOUT > 0)
                    lvStatus = "ชั่งออกแล้ว";
                else if (lvWeightIN > 0 && lvWeightOUT == 0)
                    lvStatus = "ชั่งเข้าแล้ว";
                else if (lvQ != "" && lvWeightIN == 0 && lvWeightOUT == 0)
                    lvStatus = "แจ้งคิวแล้ว";


                if(lvWeightINDate == "  /  /" && lvWeightINTime == "  :  :")
                {
                    lvWeightINDate = "";
                    lvWeightINTime = "";
                }

                if(lvWeightOUTDate == "  /  /" && lvWeightOUTTime == "  :  :")
                {
                    lvWeightOUTDate = "";
                    lvWeightOUTTime = "";
                }

                //อัพเดทสถานะเข้าไปที่เลขใบนำตัด ถ้าเป็นคิวลูกไม่ต้องบันทึก เพราะบันทึกที่คิวแม่แล้ว *วันที่ 30
                if (!lvQ.Contains('.'))
                {
                    lvSQL = "Update Cane_QueueData set C_Queue = '" + lvQ + "',C_QueueStatus = '" + lvStatus + "', " +
                        "Q_WeightINTimeDate = '" + Gstr.fncChangeSDate(lvWeightINDate) + " " + lvWeightINTime + "', " +
                        "Q_WeightTimeOUTDate = '" + Gstr.fncChangeSDate(lvWeightOUTDate) + " " + lvWeightOUTTime + "', " +
                        "Q_QueueTimeDate = '" + Gstr.fncChangeSDate(lvDate) + " " + lvTime + "', " +
                        "Q_BillingNos = '" + lvBilling + "', Q_UserUpd = '" + GVar.gvUser + " " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "' where C_ID = '" + lvCutDoc + "' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, true);

                    //*วันที่ 30
                    GsysSQL.fncKeepLogData(GVar.gvUser, "ป้องกันใบนำตัด อัพเดทสถานะคิว", "เลขที่คิว : " + lvQ + "เลขที่โควต้า : " + lvQuota + "เลขใบนำตัด : " + lvCutDoc + "ผู้รับเหมาตัด : " + lvCarcutRmid + "ชื่อผู้รับเหมาตัด" + lvCarcutRmname + "ราคาผู้รับเหมาตัด : " + lvCarcutRmPrice + "ผู้รับเหมาบรรทุก : " + lvTruckRmid + "ชื่อผู้รับเหมาบรรทุก : " + lvTruckRmname +
                "ราคาผู้รับเหมาบรรทุก : " + lvTruckRmprice + "ผู้รับเหมาคีบ : " + lvKeebRmid + "ชื่อรับเหมาคีบ : " + lvKeebRmname + "ราคาผู้รับเหมาคีบ : " + lvKeebRmprice + "ผู้รับเหมารวม : " + lvAllRmid + "ชื่อผู้รับเหมารวม : " + lvAllRmname + "ราคาผู้รับเหมารวม : " + lvAllRmprice +
                 "วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "User ที่อัพเดท : " + GVar.gvUser + "Mode : " + pvmode + "Type ที่กด" + GVar.gvTypeProgram + "สถานะเรียกเก็บ : " + lvPaystatus);
                }
                
                //ถ้าเป็นพ่วงให้พิมพ์ใบลูกด้วย
                if (ChkWeightAll.Checked && GVar.gvINOUT == "IN")
                {
                    string lvQNew = "";
                    if (lvQ.Contains("."))
                    {
                        lvQNew = lvQ;
                    }
                    else
                    {
                        lvQNew = lvQ + ".1";
                    }

                     
                    //Get Data
                    DataTable DT = new DataTable();

                    lvSQL = "Select * from Queue_Diary where Q_No = '" + lvQNew + "' And Q_Station = '" + lvStation + "' And Q_CloseStatus = '' and Q_Year = '' ";
                    DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        lvQuota = DT.Rows[i]["Q_Quota"].ToString();
                        string lvQuotaName = GsysSQL.fncFindQuotaName(DT.Rows[i]["Q_Quota"].ToString(), GVar.gvOnline);
                        lvCarNum = DT.Rows[i]["Q_CarNum"].ToString();
                        lvCaneType = DT.Rows[i]["Q_CaneType"].ToString();
                        lvState = DT.Rows[i]["Q_Rail"].ToString();
                        lvCaneType = GsysSQL.fncFindCaneTypeName(lvCaneType, GVar.gvOnline);
                        lvDate = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightINDate"].ToString());
                        lvCaneDoc = DT.Rows[i]["Q_CaneDoc"].ToString();
                        lvWeightIN = Gstr.fncToDouble(DT.Rows[i]["Q_WeightIN"].ToString());
                        string lvWeoghtINTime = DT.Rows[i]["Q_WeightINTime"].ToString();
                        lvWeightOUT = Gstr.fncToDouble(DT.Rows[i]["Q_WeightOUT"].ToString());
                        lvWeightOUTDate = DT.Rows[i]["Q_WeightOUTDate"].ToString();
                        string lvWeoghtOUTTime = DT.Rows[i]["Q_WeightOUTTime"].ToString();
                        lvSampleNo = DT.Rows[i]["Q_SampleNo"].ToString();
                        lvBilling = DT.Rows[i]["Q_BillingNo"].ToString();
                        lvTKNo = DT.Rows[i]["Q_TKNo"].ToString();
                        lvRail = DT.Rows[i]["Q_Rail"].ToString();
                        lvCutCar = DT.Rows[i]["Q_CutCar"].ToString();
                        lvCarType = DT.Rows[i]["Q_CarType"].ToString();
                        lvCarType = DT.Rows[i]["Q_CarType"].ToString();
                        //string lvSubQuota = DT.Rows[i]["Q_SubQuota"].ToString();
                        //lvSubQuotaName = GsysSQL.fncFindQuotaName(lvSubQuota, GVar.gvOnline);

                        //if (GVar.gvPrintAgain != "No")
                        FncPrintQueue(lvQNew, lvQuota, lvQuotaName, lvCarNum, lvCaneType, chkPreview.Checked, lvState, "Q", lvDate, lvCaneDoc, lvWeightIN.ToString("#,##0"), lvWeoghtINTime, lvWeightOUT.ToString("#,##0"), lvWeoghtOUTTime, true, "Q", lvSampleNo, lvBilling, lvStation, lvTKNo, lvRail, lvCutCar, lvCarType, lvWeoghtINTime, lvWeightOUTDate, lvCutCar, lvSampleNo
                            , lvCarcutRmid, lvCarcutRmname, lvCarcutRmPrice, lvTruckRmid,
                    lvTruckRmname, lvTruckRmprice, lvKeebRmid, lvKeebRmname, lvKeebRmprice, lvAllRmid, lvAllRmname, lvAllRmprice, lvCutDoc);
                    }
                }
                
                //*วันที่ 30 เก็บ Log เช็คใบนำตัดใช้ซ้ำครั้งสุดท้าย
                GsysSQL.fncKeepLogData(GVar.gvUser, "ป้องกันใบนำตัด หลังปริ้นท์", "เลขที่คิว : " + lvQ + "เลขที่โควต้า : " + lvQuota + "เลขใบนำตัด : " + lvCutDoc + "ผู้รับเหมาตัด : " + lvCarcutRmid + "ชื่อผู้รับเหมาตัด" + lvCarcutRmname + "ราคาผู้รับเหมาตัด : " + lvCarcutRmPrice + "ผู้รับเหมาบรรทุก : " + lvTruckRmid + "ชื่อผู้รับเหมาบรรทุก : " + lvTruckRmname +
                "ราคาผู้รับเหมาบรรทุก : " + lvTruckRmprice + "ผู้รับเหมาคีบ : " + lvKeebRmid + "ชื่อรับเหมาคีบ : " + lvKeebRmname + "ราคาผู้รับเหมาคีบ : " + lvKeebRmprice + "ผู้รับเหมารวม : " + lvAllRmid + "ชื่อผู้รับเหมารวม : " + lvAllRmname + "ราคาผู้รับเหมารวม : " + lvAllRmprice +
                 "วันที่ : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "User ที่อัพเดท : " + GVar.gvUser + "Mode : " + pvmode + "Type ที่กด" + GVar.gvTypeProgram + "สถานะเรียกเก็บ : " + lvPaystatus);

                MessageBox.Show("บันทึกข้อมูลเรียบร้อย", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnClear_Click(sender, e);

                //โหลดข้อมูลใน GridView
                LoadData();
                
            }
            else
            {
                MessageBox.Show("บันทึกข้อมูลเรียบร้อย  " + lvResault, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnClear_Click(sender, e);
                 
                //โหลดข้อมูลใน GridView
                LoadData();
            }

            this.Cursor = Cursors.Default;
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            pvmode = "";
        }

        private string FncChkTKNoForReset()
        {
            string lvTKNo = "";
            string lvSQL = "";
            string lvResault = "";

            //Gen คิวตะกาว
            if (ChkA.Checked)
                lvTKNo = GsysSQL.fncGetTakaoNo("Weight_RailA", GVar.gvOnline);
            else if (ChkB.Checked)
                lvTKNo = GsysSQL.fncGetTakaoNo("Weight_RailB", GVar.gvOnline);

            //ถ้าตะกาวเกิน 100 ให้รีเซ็ต
            if (Gstr.fncToInt(lvTKNo) > 100)
            {
                if (ChkA.Checked)
                {
                    //อัพเดทราง A
                    lvSQL = "Update SysDocNo Set S_RunNo = 0 Where S_MCode = 'Weight_RailA' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                }
                else if (ChkB.Checked)
                {
                    //อัพเดทราง B
                    lvSQL = "Update SysDocNo Set S_RunNo = 0 Where S_MCode = 'Weight_RailB' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                }

                //Gen คิวตะกาว
                if (ChkA.Checked)
                    lvTKNo = GsysSQL.fncGetTakaoNo("Weight_RailA", GVar.gvOnline);
                else if (ChkB.Checked)
                    lvTKNo = GsysSQL.fncGetTakaoNo("Weight_RailB", GVar.gvOnline);
            }

            return lvTKNo;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            label48.Visible = false;

            GVar.gvStationSelect = "";
            GVar.gvTKNoOld = "";
            GVar.gvBillingOld = "";
            GVar.gvLockBar = "";
            GVar.gvCovidsound = "";
            GVar.gvBonsucroStatus = "";

            txtCarNum.Text = "";
            txtCarNum2.Text = "";
            txtCutDoc.Text = "";
            txtCutDoc2.Text = "";
            txtQuota.Text = "";
            txtCaneDoc.Text = "";
            txtMainCar.Text = "";
            txtQuotaName.Text = "";
            txtCutCar.Text = "";
            txtCutCar2.Text = "";
            txtCutPrice.Text = "";
            txtCarryPrice.Text = "";
            txtSampleNo.Text = "";
            txtTakaoNo.Text = "";
            txtDump.Text = "";
            txt_Sai.Text = "";
            txt_Tel.Text = "";
            txt_MName.Text = "";
            label48.Text = "โควต้าโรงงาน";

            //ข้อมูลน้ำหนัก
            txtBillNo.Text = "";
            txtWeightMain.Text = "";
            txtWeightIN.Text = "";
            txtWeightINDate.Text = "";
            txtWeightINTime.Text = "";
            txtWeightOut.Text = "";
            txtWeightOutDate.Text = "";
            txtWeightOutTime.Text = "";
            richWeightBox.Text = "";


            #region//คิวล็อก
            //string lvOnOff = GsysSQL.fncFindQLockOnOff();
            //string lvOnOffAlert = GsysSQL.fncFindQLockOnOffAlert();

            //if (lvOnOff == "1")// && lvOnOffAlert == "1"
            //{
            //    btnLockRegister.Text = "คิวล๊อก";
            //    btnLockRegister.Enabled = true;
            //    GVar.gvQLockOnOff = "ON";

            //    string lvChkBtnLock = GsysSQL.fncFindQBtnClearLock();
            //    if (lvChkBtnLock == "1")
            //        btnClearQLock.Enabled = true;
            //    else
            //        btnClearQLock.Enabled = false;
            //}
            //else
            //{
            //    btnLockRegister.Text = "ปิดการใช้งานคิวล็อก";
            //    btnLockRegister.Enabled = false;
            //    GVar.gvQLockOnOff = "OFF";
            //}
            #endregion

            sp2.ActiveSheet.RowCount = 0;
            btnShowRegisCar.Visible = false;

            //ChkA.Checked = false;
            //ChkB.Checked = false;
            //เปิดค้นหาโควต้า
            if (GVar.gvTypeProgram == "W")
            {
                //ล๊อคไม่ได้แก้ไขข้อมูลบริเวณขาออก
                if (GVar.gvINOUT == "OUT")
                {
                    btnSearchQ.Enabled = false;
                    txtQuota.Enabled = false;

                    //ChkA.Enabled = false;
                    //ChkB.Enabled = false;
                    ChkNone.Enabled = false;

                    //GDetailQ.Enabled = false; //เปิดรายละเอียดขึ้นมา
                    btnSearchQuota.Enabled = false;
                    btnSearhCar.Enabled = false;

                    //ซ่อนแสดงช่องเลขตัวอย่าง
                    txtSampleNo.Enabled = true;
                }
                else
                {
                    btnSearchQ.Enabled = true;
                    txtQuota.Enabled = true;

                    ChkA.Enabled = true;
                    ChkB.Enabled = true;
                    ChkNone.Enabled = true;

                    GDetailQ.Enabled = true;

                    //ซ่อนแสดงช่องเลขตัวอย่าง
                    txtSampleNo.Enabled = false;
                }
                
                //แสดงข้อมูลลาน
                ChkA.Visible = true;
                ChkB.Visible = true;
                ChkNone.Visible = true;
                label4.Visible = true;
                ChkShowA.Checked = true;
                ChkShowB.Checked = true;

                btnSearchQ2.Visible = false;
                btnSearchQ.Visible = true;

                tabControl2.SelectedIndex = 0;
                พมพสลปToolStripMenuItem.Enabled = true;
                btnCloseStatus.Enabled = true; 
                btnReport.Enabled = true;
                pnBlock.Visible = false;

                //ปิด Check แก้ตะกาว และบิล
                chkChangeTK.Checked = false;
                chkChangeBill.Checked = false;
                chkQLate.Checked = false;

                gWeightAll.Visible = true;
                btnPrv.Visible = true;

                txtQ.ReadOnly = false; 
                txtQ.Text = "";

                //แสดงข้อมูลน้ำหนัก แก้ไขได้
                ChkWeightAll.Enabled = true;
                chkChangeTK.Enabled = true;
                chkChangeBill.Enabled = true;
                btnConnect.Enabled = true;

                txtCarNum.Focus();
            }
            else
            {
                btnPrv.Visible = false;
                
                //แสดงข้อมูลลาน
                ChkA.Visible = true;
                ChkB.Visible = true;
                ChkNone.Visible = true;
                label4.Visible = true;

                btnSearchQ2.Visible = true;
                
                ChkA.Enabled = true;
                ChkB.Enabled = true;
                ChkNone.Enabled = true;
                
                tabControl2.SelectedIndex = 0;
                พมพสลปToolStripMenuItem.Enabled = false;
                btnCloseStatus.Enabled = false;
                btnReport.Enabled = true;
                pnBlock.Visible = true;
                btnSearchQ.Enabled = false;
                txtQ.ReadOnly = true;
                txtQ.Text = GsysSQL.fncGenQueueNo(GVar.gvOnline);

                //แสดงข้อมูลน้ำหนัก ห้ามแก้ไข
                ChkWeightAll.Enabled = false;
                chkChangeTK.Enabled = false;
                chkChangeBill.Enabled = false;
                btnConnect.Enabled = false;
                txtSampleNo.Enabled = false;

                txtQuota.Focus();
            }
            
            //คิวล๊อก
            pnShowQLock.Visible = true;

            if (GVar.gvUserType == "admin" || GVar.gvUserType == "head")
            {
                rdIN.Visible = false;
                rdOut.Visible = false;

                //rdIN.Visible = true;
                //rdOut.Visible = true;
                //pnIN.Enabled = true;
                //pnOut.Enabled = true;

                //if (GVar.gvTypeProgram == "W")
                //    rdIN.Checked = true;
                //else
                //    rdOut.Checked = true;

                //แสดงปุ่ม Admin
                btnAdmin.Visible = true;
                deleteToolStripMenuItem.Enabled = true;
                btnDelQueue.Visible = true;
            }
            else
            {
                rdIN.Visible = false;
                rdOut.Visible = false;

                //แสดงปุ่ม Admin
                btnAdmin.Visible = false;
                deleteToolStripMenuItem.Enabled = false;
                btnDelQueue.Visible = false;
            }

            lbDate.Enabled = false;
            lbTime.Enabled = false;

            btnSave.Enabled = true;
            btnClear.Enabled = true;

            //ChkBox
            ChkWeightAll.Checked = false;
            txtCarNum2.Visible = false;
            
            //ล้างข้อมูลใบนำตัดทั้งหมด
            txtCaneDoc.Text = "";
            txtCutDoc.Text = "";
            txtCutOwner.Text = "";
            txtCutCar.Text = "";
            txtCutPrice.Text = "";
            txtKeepOwner.Text = "";
            txtKeepPrice.Text = "";
            txtCarOwner.Text = "";
            txtCarNumPlate.Text = "";
            txtCarNumPlate2.Text = "";
            txtCarryPrice.Text = "";
            txtContractAll.Text = "";
            txtContractAllPrice.Text = "";
            chkCarryPrice.Checked = false;
            txtCaneDoc.Enabled = false;
            txtCutDoc.Enabled = false;
            txtCutOwner.Enabled = false;
            txtCutCar.Enabled = false;
            txtCutPrice.Enabled = false;
            txtKeepOwner.Enabled = false;
            txtKeepPrice.Enabled = false;
            txtCarOwner.Enabled = false;
            txtCarNumPlate.Enabled = false;
            txtCarNumPlate2.Enabled = false;
            txtCarryPrice.Enabled = false;
            txtContractAll.Enabled = false;
            txtContractAllPrice.Enabled = false;
            ChkBonsugo.Checked = false;
            chkCarryPrice.Checked = false;
            chk_CutdocA.Checked = false;
            btnSearchcarcut.Enabled = false;
            btnRmTruckSearch.Enabled = false;
            btnRmKeebSearch.Enabled = false;
            btnRmAllSearch.Enabled = false;
            btnRmTruckSearch.Enabled = false;
            btmSearchCarRm.Enabled = false;
            groupBox18.Enabled = false;
            groupBox24.Enabled = false;
            groupBox25.Enabled = false;
            //txtCutOwner.Properties.Items.Clear();
            txtCutOwner.Properties.Items.Clear();
            txtKeepOwner.Items.Clear();
            txtCarOwner.Items.Clear();
            txtCarryPrice.Items.Clear();
            txtContractAll.Items.Clear();
            GVar.gvAddCarnum2 = false;

            if (GVar.gvINOUT == "OUT")
            {
                btnSearhCar.Enabled = false;
            }
            else
            {
                btnSearhCar.Enabled = true;
            }
            

            //Clear ข้อมูลหน้า Monitor
            fncClearDataMonitor();

            LoadData();
            timer1.Enabled = true;
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            pvmode = "";
        }

        private void ChkA_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkA.Checked)
            {
                if (GVar.gvINOUT != "OUT")
                {
                    FncGenTKNo_New("A", txtQ.Text);
                }

                ChkA.ForeColor = Color.Red;
                lbState.Text = "A";
                ChkB.Checked = false;
                ChkNone.Checked = false;
                txtQuota.Focus();
            }
            else
            {
                ChkA.ForeColor = Color.Black;
            }
        }

        private void ChkB_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkB.Checked)
            {
                if (GVar.gvINOUT != "OUT")
                {
                    FncGenTKNo_New("B", txtQ.Text);
                }

                ChkB.ForeColor = Color.Red;
                lbState.Text = "B";
                ChkA.Checked = false;
                ChkNone.Checked = false;
                txtQuota.Focus();
            }
            else
            {
                ChkB.ForeColor = Color.Black;
            }
        }

        private void FncGenTKNo(string lvRail, string lvQ)
        {
            reGen:

            //หน่วงเวลาสุ่ม 1-5 วิ
            Random rnd = new Random();
            int lvSleepTime = rnd.Next(1000, 2000);
            System.Threading.Thread.Sleep(lvSleepTime);

            if (ChkA.Checked)
                //txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo("Weight_RailA", lvRail, GVar.gvOnline);
                txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo_New("A", GVar.gvTypeDocTakao, GVar.gvOnline);
            else if (ChkB.Checked)
                //txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo("Weight_RailB", lvRail, GVar.gvOnline);
                txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo_New("B", GVar.gvTypeDocTakao, GVar.gvOnline);

            //รีเซ็ตถ้ามากกว่า 200
            if (Gstr.fncToInt(txtTakaoNo.Text) > 501)
            {
                //อัพเดทราง A
                string lvSQL = "Update Queue_Diary Set Q_TKNoCheck = '1' Where Q_Rail = '" + lvRail + "' And Q_TKNo <> '' and Q_Year = '' ";
                string lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                goto reGen;
            }

            //เช็คว่าตะกาวที่ Gen ได้ซ้ำไหม
            string lvChkTK = GsysSQL.fncCheckTKNo_New(txtTakaoNo.Text, lvRail, GVar.gvOnline);
            if (lvChkTK != "")
            {
                if (ChkA.Checked)
                    //txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo("Weight_RailA", lvRail, GVar.gvOnline);
                    txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo_New("A", GVar.gvTypeDocTakao, GVar.gvOnline);
                else if (ChkB.Checked)
                    //txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo("Weight_RailB", lvRail, GVar.gvOnline);
                    txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo_New("B", GVar.gvTypeDocTakao, GVar.gvOnline);

                goto reGen;
            }
        }

        private void FncGenTKNo_New(string lvRail, string lvQ)
        {
            reGen:

            //หน่วงเวลาสุ่ม 1-2 วิ
            Random rnd = new Random();
            int lvSleepTime = rnd.Next(0, 1000);
            System.Threading.Thread.Sleep(lvSleepTime);

            string lvLastDocType = GsysSQL.fncGetLastTakaoTypeDoc(lvRail);
            txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo_New(lvRail, lvLastDocType, GVar.gvOnline);
            
            //เช็คว่าตะกาวที่ Gen ได้ซ้ำไหม
            string lvChkTK = GsysSQL.fncCheckTKNo_New(txtTakaoNo.Text, lvRail, GVar.gvOnline);
            if (lvChkTK != "")
            {
                txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo_New(lvRail, GVar.gvTypeDocTakao, GVar.gvOnline);
                //goto reGen; ใส่แล้ว Not Responding
            }

            ////เช็คว่ากระโดดไหม โดยการเช็คเล่มก่อนหน้า ถ้ายังไม่ถึง 200 ให้ใช้เล่มเดิมแล้ว Gen ต่อไป
            //string lvOldType = (Gstr.fncToInt(GVar.gvTypeDocTakao) - 1).ToString();
            //string lvLastNum = GsysSQL.fncGetLastTakaoNo_New(lvRail, lvOldType, GVar.gvOnline);
        }

        private void ChkNone_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkNone.Checked)
            {
                ChkNone.ForeColor = Color.Red;
                lbState.Text = "-";
                ChkA.Checked = false;
                ChkB.Checked = false;
                txtTakaoNo.Text = "";
                txtQuota.Focus();
            }
            else
            {
                ChkNone.ForeColor = Color.Black;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbTime.Text = DateTime.Now.ToString("HH:mm:ss"); 
        }

        private void frmMain2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private string fncGetDataCheck(string lvType, string lvID)
        {
            string lvReturn = "";

            if (lvType == "Car" && lvID == "")
            {
                if (chkCar1.Checked) lvReturn = chkCar1.Text;
                else if (chkCar2.Checked) lvReturn = chkCar2.Text;
                else if (chkCar3.Checked) lvReturn = chkCar3.Text;
                else if (chkCar4.Checked) lvReturn = chkCar4.Text;
                else if (chkCar5.Checked) lvReturn = chkCar5.Text;
                else if (chkCar6.Checked) lvReturn = chkCar6.Text;
                else if (chkCar7.Checked) lvReturn = chkCar7.Text;
            }
            else if(lvType == "Cane" && lvID == "")
            {
                if (chkCane1.Checked) lvReturn = "1";
                else if (chkCane2.Checked) lvReturn = lvReturn = "17";
                else if (chkCane3.Checked) lvReturn = lvReturn = "3";
                else if (chkCane4.Checked) lvReturn = lvReturn = "7";
                else if (chkCane5.Checked) lvReturn = lvReturn = "5";
                else if (chkCane6.Checked) lvReturn = lvReturn = "15";
                else if (chkCane7.Checked) lvReturn = lvReturn = "11";
                else if (chkCane8.Checked) lvReturn = lvReturn = "13";
                else if (chkCane9.Checked) lvReturn = lvReturn = "2";
                else if (chkCane10.Checked) lvReturn = lvReturn = "4";
                else if (chkCane11.Checked) lvReturn = lvReturn = "6";
                else if (chkCane12.Checked) lvReturn = lvReturn = "8";
                else if (chkCane13.Checked) lvReturn = lvReturn = "12";
                else if (chkCane14.Checked) lvReturn = lvReturn = "14";
                else if (chkCane15.Checked) lvReturn = lvReturn = "18";
                else if (chkCane16.Checked) lvReturn = lvReturn = "16";
            }
            else if (lvType == "Car" && lvID != "")
            {
                if (lvID == "สาวแต๋น") chkCar1.Checked = true;
                else if (lvID == "เทนเลอร์") chkCar2.Checked = true;
                else if (lvID == "บรรทุก") chkCar3.Checked = true;
                else if (lvID == "พ่วง") chkCar4.Checked = true;
                else if (lvID == "สิบล้อ") chkCar5.Checked = true;
                else if (lvID == "6 ล้อ") chkCar6.Checked = true;
                else if (lvID == "6 ล้อ (เล็ก)") chkCar7.Checked = true;
            }
            else if (lvType == "Cane" && lvID != "")
            {
                if (lvID == "1") chkCane1.Checked = true;//-- ใช้บ่อย
                else if (lvID == "5") chkCane5.Checked = true;//-- ใช้บ่อย
                else if (lvID == "17") chkCane2.Checked = true;//-- ใช้บ่อย
                else if (lvID == "11") chkCane7.Checked = true;//-- ใช้บ่อย
                else if (lvID == "3") chkCane3.Checked = true;
                else if (lvID == "7") chkCane4.Checked = true;
                else if (lvID == "15") chkCane6.Checked = true;
                else if (lvID == "13") chkCane8.Checked = true;

                else if (lvID == "2") chkCane9.Checked = true;//-- ใช้บ่อย
                else if (lvID == "6") chkCane11.Checked = true;//-- ใช้บ่อย
                else if (lvID == "12") chkCane13.Checked = true;//-- ใช้บ่อย
                else if (lvID == "18") chkCane15.Checked = true;//-- ใช้บ่อย
                else if (lvID == "4") chkCane10.Checked = true;
                else if (lvID == "8") chkCane12.Checked = true;
                else if (lvID == "14") chkCane14.Checked = true;
                else if (lvID == "16") chkCane16.Checked = true;

                if (lvID == "1" || lvID == "5" || lvID == "17" || lvID == "11" || lvID == "3" || lvID == "7" || lvID == "15" || lvID == "13")
                    tabControl1.SelectedIndex = 0;
                else
                    tabControl1.SelectedIndex = 1;
            }
            else if (lvType == "Rail" && lvID != "")
            {
                if (lvID == "A") ChkA.Checked = true;
                else if (lvID == "B") ChkB.Checked = true;
                else if (lvID == "-") ChkNone.Checked = true;
            }

            return lvReturn;
        }

        private void txtCutCar_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCutCar.Text != "")
            {
               string lvCutCar = txtCutCar.Text;
                if (e.KeyCode == Keys.Enter)
                {
                    string lvCheckCar = txtCutCar.Text;
                    
                    string[] lvCutcarCheck = { "NO.01", "NO.02", "NO.03", "NO.04", "NO.05", "NO.06", "NO.07", "NO.08", "NO.09", "NO.10", "NO.11", "NO.12", "NO.13",
                    "NO.14", "NO.15", "NO.16", "NO.17", "NO.18", "NO.19", "NO.20", "NO.21", "NO.22", "NO.23", "NO.24", "NO.25", "NO.26", "NO.27", "NO.28", "NO.29",
                    "NO.30", "NO.31", "NO.32", "NO.33", "NO.34", "NO.35", "NO.36", "NO.37", "NO.38"};

                    foreach(string x in lvCutcarCheck)
                    {
                        if (!lvCheckCar.Contains(x))
                        {
                            txtCutCar.Text = "";
                        }
                        else
                        {
                            txtCutCar.Text = lvCheckCar;
                            break;
                        }
                    }

                    if(txtCutCar.Text == "")
                    {
                        MessageBox.Show("เลขรถตัดไม่ตรงกับที่ลงทะเบียนไว้  กรุณาเลือกรถตัดที่ลงทะเบียนไว้", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtCutCar.Focus();

                        GVar.gvCutcarNo = "";

                        frmCutCarSearch frm = new frmCutCarSearch();
                        frm.ShowDialog();

                        if (GVar.gvCutcarNo != "")
                        {
                            txtCutCar.Text = GVar.gvCutcarNo;
                        }
                        else
                        {
                            txtCutCar.Focus();
                        }
                    }
                    else
                    {
                        txtCutCar2.Focus();
                    }
                }
            }
        }

        private void chkCar1_CheckedChanged(object sender, EventArgs e)
        {
            //chkCar1.Checked = true;
            if (chkCar1.Checked)
            {
                chkCar1.ForeColor = Color.Red;
                chkCar2.Checked = false;
                chkCar3.Checked = false;
                chkCar4.Checked = false;
                chkCar5.Checked = false;
                chkCar6.Checked = false;
                chkCar7.Checked = false;
                lbCarName.Text = chkCar1.Text;
                txtCarryPrice.Focus();
            }
            else
            {
                chkCar1.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCar2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCar2.Checked)
            {
                chkCar2.ForeColor = Color.Red;
                chkCar1.Checked = false;
                chkCar3.Checked = false;
                chkCar4.Checked = false;
                chkCar5.Checked = false;
                chkCar6.Checked = false;
                chkCar7.Checked = false;
                lbCarName.Text = chkCar2.Text;
                txtCarryPrice.Focus();
            }
            else
            {
                chkCar2.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCar3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCar3.Checked)
            {
                chkCar3.ForeColor = Color.Red;
                chkCar1.Checked = false;
                chkCar2.Checked = false;
                chkCar4.Checked = false;
                chkCar5.Checked = false;
                chkCar6.Checked = false;
                chkCar7.Checked = false;
                lbCarName.Text = chkCar3.Text;
                txtCarryPrice.Focus();
            }
            else
            {
                chkCar3.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCar4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCar4.Checked)
            {
                chkCar4.ForeColor = Color.Red;
                chkCar1.Checked = false;
                chkCar2.Checked = false;
                chkCar3.Checked = false;
                chkCar5.Checked = false;
                chkCar6.Checked = false;
                chkCar7.Checked = false;
                lbCarName.Text = chkCar4.Text;
                txtCarryPrice.Focus();
            }
            else
            {
                chkCar4.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCar5_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCar5.Checked)
            {
                chkCar5.ForeColor = Color.Red;
                chkCar1.Checked = false;
                chkCar2.Checked = false;
                chkCar3.Checked = false;
                chkCar4.Checked = false;
                lbCarName.Text = chkCar5.Text;
                txtCarryPrice.Focus();
            }
            else
            {
                chkCar5.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void fncClearCutwhenleave()
        {
            txtCutOwner.Text = "";
            txtCutCar.Text = "";
            txtCutPrice.Text = "";
        }

        private void chkCane1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane1.Checked)
            {
                GVar.gvCaneType = "1"; //อ้อยสด
                chkCane1.ForeColor = Color.Red;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane1.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane1.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane2.Checked)
            {
                GVar.gvCaneType = "17"; //อ้อยสด
                chkCane2.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane2.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane2.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane3.Checked)
            {
                GVar.gvCaneType = "3"; //อ้อยสด
                chkCane3.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane3.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane3.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane4.Checked)
            {
                GVar.gvCaneType = "7"; //อ้อยสด
                chkCane4.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane4.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane4.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane5_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane5.Checked)
            {
                GVar.gvCaneType = "5"; //อ้อยสด
                chkCane5.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane5.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane5.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane6_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane6.Checked)
            {
                GVar.gvCaneType = "15"; //อ้อยสด
                chkCane6.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane6.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane6.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane7_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane7.Checked)
            {
                GVar.gvCaneType = "11"; //อ้อยสด
                chkCane7.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane7.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane7.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane8_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane8.Checked)
            {
                GVar.gvCaneType = "13"; //อ้อยสด
                chkCane8.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane8.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane8.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane9_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane9.Checked)
            {
                GVar.gvCaneType = "2"; //อ้อยไฟไหม้
                chkCane9.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane9.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane9.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane10_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane10.Checked)
            {
                GVar.gvCaneType = "4"; //อ้อยไฟไหม้
                chkCane10.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane10.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane10.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane11_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane11.Checked)
            {
                GVar.gvCaneType = "6"; //อ้อยไฟไหม้
                chkCane11.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane11.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane11.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane12_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane12.Checked)
            {
                GVar.gvCaneType = "8"; //อ้อยไฟไหม้
                chkCane12.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane12.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane12.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane13_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane13.Checked)
            {
                GVar.gvCaneType = "12"; //อ้อยไฟไหม้
                chkCane13.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane13.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane13.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane14_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane14.Checked)
            {
                GVar.gvCaneType = "14"; //อ้อยไฟไหม้
                chkCane14.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane15.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane14.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane14.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane15_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane15.Checked)
            {
                GVar.gvCaneType = "18"; //อ้อยไฟไหม้
                chkCane15.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane16.Checked = false;
                lbCaneName.Text = chkCane15.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane15.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCane16_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCane16.Checked)
            {
                GVar.gvCaneType = "16"; //อ้อยไฟไหม้
                chkCane16.ForeColor = Color.Red;
                chkCane1.Checked = false;
                chkCane2.Checked = false;
                chkCane3.Checked = false;
                chkCane4.Checked = false;
                chkCane5.Checked = false;
                chkCane6.Checked = false;
                chkCane7.Checked = false;
                chkCane8.Checked = false;
                chkCane9.Checked = false;
                chkCane10.Checked = false;
                chkCane11.Checked = false;
                chkCane12.Checked = false;
                chkCane13.Checked = false;
                chkCane14.Checked = false;
                chkCane15.Checked = false;
                lbCaneName.Text = chkCane16.Text;
                txtCarryPrice.Focus();
                fncClearCutwhenleave();
            }
            else
            {
                chkCane16.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void ChkShow0_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ChkShowA_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ChkShowB_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }
        
        private void txtRecord_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (pnRecord.Visible)
                pnRecord.Visible = false;
            else
                pnRecord.Visible = true;
        }

        private void btnRemote_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + "\\AnyDesk.exe");
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GVar.gvPermitDel != "1")
            {
                MessageBox.Show("คุณไม่มีสิทธิ์ในการใช้งาน ฟังก์ชันนี้ กรุณาติดต่อผู้ดูแล !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (sp1.ActiveSheet.Cells[GVar.gvRowIndex,9].Text != "รอชั่งเข้า")
            {
                MessageBox.Show("ไม่สามารถลบข้อมูลได้ เนื่องจาก คิวนี้มีการชั่งเข้าแล้ว", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string lvQ = sp1.ActiveSheet.Cells[GVar.gvRowIndex, 1].Text;
            string lvQuota = sp1.ActiveSheet.Cells[GVar.gvRowIndex, 3].Text;
            string lvDate = Gstr.fncChangeTDate(sp1.ActiveSheet.Cells[GVar.gvRowIndex, 5].Text);
            string lvCarNum = sp1.ActiveSheet.Cells[GVar.gvRowIndex, 4].Text;

            //ยืนยัน
            string lvTxtAlert = "ยืนยันการลบข้อมูล คิวที่ " + lvQ + " โควต้า " + lvQuota ;
            DialogResult dialogResult = MessageBox.Show(lvTxtAlert, "Confirm?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }

            //ลบ
            //string lvSQL = "Delete from Queue_Diary where Q_No = '"+ lvQ +"' ";
            //string lvSQL = "Update Queue_Diary set Q_Status = 'Cancel' where Q_No = '" + lvQ + "' And Q_CloseStatus = '' And Q_Date = '"+ lvDate + "' ";
            string lvSQL = "Delete From Queue_Diary where Q_No = '" + lvQ + "' And Q_CloseStatus = '' And Q_Date = '" + lvDate + "' And Q_CarNum = '" + lvCarNum + "' and Q_Year = '' ";
            string lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

            sp1.ActiveSheet.Rows[GVar.gvRowIndex].Remove();
            //sp1.ActiveSheet.RowCount += 1;

            if (lvResault == "Success")
            {
                GsysSQL.fncKeepLogData(GVar.gvUser, "ลบข้อมูลคิว", "ลบข้อมูลคิวที่ " + lvQ + " โควต้า " + lvQuota + " วันที่ " + lvDate + " ทะเบียนรถ " + lvCarNum);
                MessageBox.Show("ลบข้อมูลเรียบร้อย", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("ไม่สามารถลบข้อมูลได้ เนื่องจาก " + lvResault, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void sp1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            sp1.ActiveSheet.SetActiveCell(e.Row, e.Column);
            GVar.gvRowIndex = e.Row;

            if (e.Button == MouseButtons.Right)
            {
                double lvQ = Gstr.fncToDouble(sp1.ActiveSheet.Cells[e.Row, 1].Text);
                if ((lvQ % Math.Floor(lvQ)) > 0)
                    เพมขอมลพวงToolStripMenuItem.Enabled = false;
                else
                    เพมขอมลพวงToolStripMenuItem.Enabled = true;
                               
                CRightMenu.Show(Cursor.Position);
            }
        }
        
        private void FncPrintQLock(string lvDate, string lvCarNum, string lvQuota)
        {
            this.Cursor = Cursors.WaitCursor;
            //ลบข้อมูล เก่า
            string lvSQL = "Delete From SysTemp "; //HD
            string lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);

            //แยก PK To Loop lock Q
            if (GVar.gvLockBar.Length == 7) GVar.gvLockBar = "0" + GVar.gvLockBar;

            string lvBarCode = GVar.gvLockBar;
            string lvLoop = Gstr.Left(lvBarCode, 2);
            string lvLock = Gstr.Mid(lvBarCode, 3, 2);
            string lvLockQ = Gstr.Right(lvBarCode, 4);

            lvSQL = "Insert into SysTemp (Field1, Field2, Field3, Field4, Field5, Field6, Field7) ";//
            lvSQL += "Values ('" + lvLoop + "', '" + lvLock + "', '" + lvLockQ + "', '" + lvDate + "', '" + lvCarNum + "', '" + lvQuota + "', '" + lvBarCode + "') ";
            lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);

            //พิมพ์เลย

            string lvPrinterSelect = "";
            bool lvChkPrinterStatus = false;
            //เช็ค Printer
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (printer.IndexOf("XP-80") != -1)
                {
                    lvChkPrinterStatus = FncCheckPrinterStatus(printer);
                    if (lvChkPrinterStatus)
                    {
                        lvPrinterSelect = printer;
                        break;
                    }
                }
            }

            if (lvPrinterSelect == "") lvPrinterSelect = "XP-80";

            rptSlip1 Report = new rptSlip1();

            Report.PrinterName = lvPrinterSelect;
            Report.PrintingSystem.ShowMarginsWarning = false;
            Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
            Report.CreateDocument();

            using (ReportPrintTool printTool = new ReportPrintTool(Report))
            {
                if (lvChkPrinterStatus)
                {
                    printTool.PrinterSettings.PrinterName = lvPrinterSelect;
                    printTool.Print();

                    printTool.Dispose();
                }
                else
                {
                    GVar.gvPrintAgain = "No";
                    MessageBox.Show("ไม่พบเครื่องพิมพ์ หรือ เครื่องพิมพ์อยู่ในสถานะ OFFLINE กรุณาตรวจสอบ !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Cursor = Cursors.Default;
                    return;
                }
            }

            Report.Dispose();
        }

        private void FncPrintQueue(string lvQ, string lvQuota, string lvName, string lvCarNum, string lvTypeCane, bool lvShowPrint, string lvState, string lvPrintReport, string lvDates, string lvCaneDoc, string lvWeightIN, string lvWeightTimeIN, string lvWeightOut, string lvWeightTimeOut,bool lvPrint, string lvPrintMode, string lvSimpleNo, string lvBillingNo, string lvStation, string lvTKNo, string lvRail, string lvCutNo, string lvCarType, string lvTime, 
            string lvWeightOutDate, string lvCutCar, string lvSampleNo, string lvcutrmid, string lvcutrmname, string lvcutrmprice, string lvtruckrm, string lvtruckrmname, string lvtruckrmprice, string lvkeebrmid, string lvkeebrmname, string lvkeebrmprice, string lvallrmid, string lvallrmname, string lvallrmprice, string lvCutdoc)
        {
            this.Cursor = Cursors.WaitCursor;
            //ลบข้อมูลเก่า
            string lvSQL = "Delete From SysTemp "; //HD
            string lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
            string lvDateE = lbDate.Text;
            string lvDate = lvDateE;
            string lvQOld = lvQ; //lvStation + "_" + 
            //if (lvStation != "") lvQ = lvStation + "_" + lvQ;
            if (lvWeightIN == "0") lvWeightIN = "";

            if(GVar.gvSubQuota != "")
            {
                lvQuota += "(" + GVar.gvSubQuota + ")";
            }

            //Insert 
            if (lvPrintReport == "Q")
            {
                //ปรับทศนิยม
                lvWeightIN = (Gstr.fncToDouble(lvWeightIN)).ToString("#,##0");/// 1000

                lvSQL = "Insert into SysTemp (Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field18, Field19, Field20, Field21, Field22) ";//
                lvSQL += "Values ('" + lvQ + "', '" + lvQuota + "', '" + lvName + "', '" + lvCarNum + "', '" + lvTypeCane + "', '" + lvQOld + "', '" + lvStation + "', '" + lvWeightIN + "', '" + lvTKNo + "', '" + lvRail + "', '" + lvCutNo + "', '" + lvCarType + "', '" + lvDate + "', '" + lvTime + "', '" + GVar.gvUser + "', '" + lvCaneDoc + "', '" + lvCutCar + "', '" + lvSampleNo + "', '" + lvCutdoc + "') ";
            }
            else
            {
                //แยกวันที่
                try
                {
                    System.Globalization.CultureInfo _cultureTHInfo = new System.Globalization.CultureInfo("th-TH");
                    DateTime dateThai = Convert.ToDateTime(Gstr.fncChangeSDate(lvWeightOutDate), _cultureTHInfo);

                    string lvDay = dateThai.ToString("dd");
                    string lvMonth = dateThai.ToString("MMMM");
                    string lvYear = dateThai.ToString("yyyy");

                    double lvSubWeight = Gstr.fncToDouble(lvWeightIN) - Gstr.fncToDouble(lvWeightOut);
                    if (lvWeightTimeIN == "  :  :") lvWeightTimeIN = "";
                    if (lvWeightTimeOut == "  :  :") lvWeightTimeOut = "";

                    //ปรับทศนิยม
                    lvWeightIN = (Gstr.fncToDouble(lvWeightIN)).ToString("#,##0"); /// 1000
                    lvWeightOut = (Gstr.fncToDouble(lvWeightOut)).ToString("#,##0"); /// 1000
                    string lvSubWeightS = (lvSubWeight).ToString("#,##0"); /// 1000

                    if (lvTypeCane == "อ้อยสด" || lvTypeCane == "อ้อยไฟไหม้")
                        lvTypeCane = "";

                    lvSQL = "Insert into SysTemp (Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18, " +
                        "Field19 , Field20, Field21, Field22, Field23, Field24, Field25, Field26, Field27, Field28, Field29, Field30, Field31) ";//
                    lvSQL += "Values ('" + lvQ + "','" + lvDay + "', '" + lvMonth + "', '" + lvYear + "', '" + lvName + "', '" + lvQuota + "', '" + lvBillingNo + "', '" + lvCarNum + "', '" + lvWeightTimeIN + "', '" + lvWeightTimeOut + "', '" + lvWeightIN + "', '" + lvWeightOut + "', '" + lvSubWeightS + "', '', '" + lvTypeCane + "','" + lvSimpleNo + "', '" + lvCaneDoc + "', '" + GVar.gvSubQuota + "'" +
                        ", '" + lvcutrmid + "', '" + lvcutrmname + "', '" + lvcutrmprice + "', '" + lvtruckrm + "', '" + lvtruckrmname + "', '" + lvtruckrmprice + "', '" + lvkeebrmid + "', '" + lvkeebrmname + "', '" + lvkeebrmprice + "', '" + lvallrmid + "'" +
                        ", '" + lvallrmname + "', '" + lvallrmprice + "') ";
                }
                catch (Exception ex)
                {
                    string lvDay = Gstr.Right(lvWeightOutDate,2);      
                    string lvMonth = Gstr.Mid(lvWeightOutDate, 5, 2);
                    if (lvMonth == "02") lvMonth = "กุมภาพันธ์";
                    else if (lvMonth == "03") lvMonth = "มีนาคม";
                    string lvYear = Gstr.Left(lvWeightOutDate, 4);

                    double lvSubWeight = Gstr.fncToDouble(lvWeightIN) - Gstr.fncToDouble(lvWeightOut);
                    if (lvWeightTimeIN == "  :  :") lvWeightTimeIN = "";
                    if (lvWeightTimeOut == "  :  :") lvWeightTimeOut = "";

                    //ปรับทศนิยม
                    lvWeightIN = (Gstr.fncToDouble(lvWeightIN)).ToString("#,##0"); /// 1000
                    lvWeightOut = (Gstr.fncToDouble(lvWeightOut)).ToString("#,##0"); /// 1000

                    string lvSubWeightS = (lvSubWeight).ToString("#,##0"); /// 1000

                    if (lvTypeCane == "อ้อยสด" || lvTypeCane == "อ้อยไฟไหม้")
                        lvTypeCane = "";

                    lvSQL = "Insert into SysTemp (Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, Field13, Field14, Field15, Field16, Field17, Field18, " +
                       "Field19 , Field20, Field21, Field22, Field23, Field24, Field25, Field26, Field27, Field28, Field29, Field30, Field31) ";//
                    lvSQL += "Values ('" + lvQ + "','" + lvDay + "', '" + lvMonth + "', '" + lvYear + "', '" + lvName + "', '" + lvQuota + "', '" + lvBillingNo + "', '" + lvCarNum + "', '" + lvWeightTimeIN + "', '" + lvWeightTimeOut + "', '" + lvWeightIN + "', '" + lvWeightOut + "', '" + lvSubWeightS + "', '', '" + lvTypeCane + "','" + lvSimpleNo + "', '" + lvCaneDoc + "', '" + GVar.gvSubQuota + "'" +
                        ", '" + lvcutrmid + "', '" + lvcutrmname + "', '" + lvcutrmprice + "', '" + lvtruckrm + "', '" + lvtruckrmname + "', '" + lvtruckrmprice + "', '" + lvkeebrmid + "', '" + lvkeebrmname + "', '" + lvkeebrmprice + "', '" + lvallrmid + "'" +
                        ", '" + lvallrmname + "', '" + lvallrmprice + "') ";
                }
            }

            lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
            this.Cursor = Cursors.Default;

            if (lvResault.StartsWith("The 'Microsoft.ACE.OLEDB"))
            {
                MessageBox.Show("ไม่สามารถพิมพ์รายงานได้ กรุณาติดตั้งโปรแกรม Access Database Engine 2007 ก่อน","แจ้งเตือนลงโปรแกรมไม่สมบูรณ์",MessageBoxButtons.OK,MessageBoxIcon.Error);
                Process.Start(Application.StartupPath + "\\Setup\\AccessDatabaseEngine.exe");
                return;
            }
            
            if (lvShowPrint)
            {
                //แสดงก่อนพิมพ์
                frmPrint frm = new frmPrint();

                if (lvPrintReport == "Q")
                {
                    if (GVar.gvTypeProgram == "W")
                        frm.documentViewer1.DocumentSource = typeof(PSQueue.rptQueue3);
                    else
                        frm.documentViewer1.DocumentSource = typeof(PSQueue.rptQueue2);
                }
                else
                {
                    frm.documentViewer1.DocumentSource = typeof(PSQueue.rptSlip);
                }

                if (lvPrint)
                frm.ShowDialog();

                btnClear.PerformClick();
            }
            else
            {
                try
                {
                    //พิมพ์เลย
                    if (lvPrintReport == "Q")
                    {
                        //PrinterSettings settings = new PrinterSettings();

                        string lvPrinterSelect = "";
                        bool lvChkPrinterStatus = false;
                        //เช็ค Printer
                        foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                        {
                            if (printer.IndexOf("XP-80") != -1)
                            {
                                lvChkPrinterStatus = FncCheckPrinterStatus(printer);
                                if (lvChkPrinterStatus)
                                {
                                    lvPrinterSelect = printer;
                                    break;
                                }
                            }
                        }

                        if (lvPrinterSelect == "") lvPrinterSelect = "XP-80";
                        cmbPrinter.Text = lvPrinterSelect;

                        if (GVar.gvTypeProgram == "W")
                        {
                            rptQueue3 Report = new rptQueue3();

                            Report.PrinterName = cmbPrinter.Text;
                            Report.PrintingSystem.ShowMarginsWarning = false;
                            Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                            Report.CreateDocument();

                            using (ReportPrintTool printTool = new ReportPrintTool(Report))
                            {
                                if (lvChkPrinterStatus)
                                {
                                    printTool.PrinterSettings.PrinterName = cmbPrinter.Text;
                                    printTool.Print();

                                    printTool.Dispose();
                                }
                                else
                                {
                                    GVar.gvPrintAgain = "No";
                                    MessageBox.Show("ไม่พบเครื่องพิมพ์ หรือ เครื่องพิมพ์อยู่ในสถานะ OFFLINE กรุณาตรวจสอบ !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }

                            //var jobPrint = 

                            Report.Dispose();
                        }
                        else
                        {
                            rptQueue2 Report = new rptQueue2();

                            Report.PrinterName = cmbPrinter.Text;
                            Report.PrintingSystem.ShowMarginsWarning = false;
                            Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                            Report.CreateDocument();

                            using (ReportPrintTool printTool = new ReportPrintTool(Report))
                            {
                                if (lvChkPrinterStatus)
                                {
                                    printTool.PrinterSettings.PrinterName = cmbPrinter.Text;
                                    printTool.Print();

                                    printTool.Dispose();
                                }
                                else
                                {
                                    GVar.gvPrintAgain = "No";
                                    MessageBox.Show("ไม่พบเครื่องพิมพ์ หรือ เครื่องพิมพ์อยู่ในสถานะ OFFLINE กรุณาตรวจสอบ !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }

                            Report.Dispose();
                        }
                    }
                    else
                    {
                        string lvPrinterSelect = "";
                        bool lvChkPrinterStatus = false;

                        string lvPrinter = "LQ-310";
                        if (lvPrintMode == "Q") lvPrinter = "XP-80";

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

                        if (lvPrinterSelect == "") lvPrinterSelect = "LQ-310";
                        cmbPrinter.Text = lvPrinterSelect;
                        rptSlip Report = new rptSlip();
                        Report.PrinterName = cmbPrinter.Text;
                        Report.PrintingSystem.ShowMarginsWarning = false;
                        Report.ExportOptions.Pdf.ShowPrintDialogOnOpen = true;
                        Report.CreateDocument();

                        using (ReportPrintTool printTool = new ReportPrintTool(Report))
                        {
                            if (lvPrint)
                            {
                                if (lvChkPrinterStatus)
                                {
                                    printTool.PrinterSettings.PrinterName = cmbPrinter.Text;
                                    printTool.Print();

                                    printTool.Dispose();
                                }
                                else
                                {
                                    GVar.gvPrintAgain = "No";
                                    MessageBox.Show("ไม่พบเครื่องพิมพ์ หรือ เครื่องพิมพ์อยู่ในสถานะ OFFLINE กรุณาตรวจสอบ !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                        }

                        Report.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Unhandled exception has occurred") != -1)
                    {
                        MessageBox.Show("เครื่องพิมพ์ไม่พร้อมใช้งาน กรุณาตรวจสอบไฟสถานะที่เครื่องพิมพ์","แจ้งเตือน",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    MessageBox.Show(ex.Message);
                }
            }
        }
        
        private void LoadQueueData(string lvQ, string lvStation,bool lvChkWeight)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                GVar.gvStationSelect = lvStation;
                GVar.gvSampleNoOld = "";

                //เช็คการเปลี่ยนภาษา Q
                if (Gstr.fncToInt(lvQ) == 0)
                {
                    MessageBox.Show("เลขที่คิวไม่ถูกต้อง กรุณาตรวจสอบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtQ.Text = "";
                    return;
                }
                
                //Get Data
                DataTable DT = new DataTable();

                string lvSQL = "Select * from Queue_Diary where Q_No = '" + lvQ + "' and Q_Year = '' ";//And Q_CloseStatus = '' And Q_Station = '" + lvStation + "'
                //*14-01-2565 ห้ามเรียกตัวเก่าขึ้นมาซ้ำ
                //if (GVar.gvUser == "weight")
                //{
                //    lvSQL += "And (Q_WeightOUT = 0) ";//Q_WeightIN = 0 OR 
                //    lvSQL += "And Q_CloseStatus = '' ";
                //}
                if (lvChkWeight)
                    lvSQL += "And (Q_WeightIN = '' OR Q_WeightOUT = '') ";

                if (GVar.gvTypeProgram == "Q")
                {
                    //ถ้าติ๊กถูกจะดูบิลเก่าๆได้
                    if (chkShowFinish.Checked)
                    {
                        lvSQL += "And Q_CloseStatus = '1' ";
                    }
                    else
                    {
                        lvSQL += "And Q_CloseStatus = '' ";
                    }
                }

                DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

                string lvQChild = lvQ + ".1";
                string lvCutCar2 = GsysSQL.fncGetCurCar2(lvQChild);
                string lvCutDoc2 = GsysSQL.fncGetCutDoc2(lvQChild);

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    txtQ.Text = lvQ;
                    GVar.gvQuota = DT.Rows[i]["Q_Quota"].ToString();
                    GVar.gvSubQuota = DT.Rows[i]["Q_SubQuota"].ToString();

                    if(GVar.gvSubQuota != "" && GVar.gvSubQuota != null)
                    {
                        txtQuota.Text = DT.Rows[i]["Q_Quota"].ToString() + "(" + GVar.gvSubQuota + ")";
                    }
                    else
                    {
                        txtQuota.Text = GVar.gvQuota;
                    }

                    txtQuotaName.Text = GsysSQL.fncFindQuotaName(DT.Rows[i]["Q_Quota"].ToString(), GVar.gvOnline);
                    txtCarNum.Text = DT.Rows[i]["Q_CarNum"].ToString();
                    //txtCutCar.Text = DT.Rows[i]["Q_CutCar"].ToString();  //ตัดออกไปดึงมาจากตาราง ใบนำตัดเเทน
                    //txtCutCar2.Text = lvCutCar2;  //ตัดออกไปดึงมาจากตาราง ใบนำตัดเเทน
                    txtCutDoc.Text = DT.Rows[i]["Q_CutDoc"].ToString();
                    //txtCutDoc2.Text = lvCutDoc2;  //ตัดออกไปดึงมาจากตาราง ใบนำตัดเเทน
                    //txtCaneDoc.Text = DT.Rows[i]["Q_CaneDoc"].ToString();  //ตัดออกไปดึงมาจากตาราง ใบนำตัดเเทน
                    txtMainCar.Text = DT.Rows[i]["Q_MainCar"].ToString();
                    string lvCaneType = DT.Rows[i]["Q_CaneType"].ToString();
                    string lvCarType = DT.Rows[i]["Q_CarType"].ToString();
                    string lvRail = DT.Rows[i]["Q_Rail"].ToString();
                    //txtCutPrice.Text = DT.Rows[i]["Q_CutPrice"].ToString();  //ตัดออกไปดึงมาจากตาราง ใบนำตัดเเทน

                    //if (DT.Rows[i]["Q_CarryPriceStatus"].ToString() == "1")  //ตัดออกไปดึงมาจากตาราง ใบนำตัดเเทน
                    //{
                    //    GVar.gvCarryPriceStatus = true;
                    //    chkCarryPrice.Checked = true;
                    //}
                    //else
                    //{
                    //    GVar.gvCarryPriceStatus = false;
                    //    chkCarryPrice.Checked = false;
                    //}

                    //txtCarryPrice.Text = DT.Rows[i]["Q_CarryPrice"].ToString();  //ตัดออกไปดึงมาจากตาราง ใบนำตัดเเทน

                    txtSampleNo.Text = DT.Rows[i]["Q_SampleNo"].ToString();                    
                    txtDump.Text = DT.Rows[i]["Q_DumNo"].ToString();

                    if (DT.Rows[i]["Q_Bonsugo"].ToString() == "1")
                        ChkBonsugo.Checked = true;
                    else
                        ChkBonsugo.Checked = false;

                    if (DT.Rows[i]["Q_WeightALLStatus"].ToString() == "1")
                        ChkWeightAll.Checked = true;
                    else
                        ChkWeightAll.Checked = false;
                    
                    //ถ้าเป็นชั่งรวมให้แสดงเลขตัวอย่าง 2 ชุด
                    if (ChkWeightAll.Checked)
                    {
                        string lvQNew = lvQ + ".1";
                        string lvSample2 = GsysSQL.fncFindSample2(lvQNew, false, GVar.gvOnline);
                        
                        if (!lvQ.Contains("."))
                        {
                            txtCarNum2.Text = GsysSQL.fncFindCarNum2(lvQNew, GVar.gvOnline);

                            if (txtCarNum2.Text == "") txtCarNum2.Text = DT.Rows[i]["Q_CarNum2"].ToString();

                            txtSampleNo.Text += lvSample2;
                            label33.Visible = true;
                            txtCarNum2.Visible = true;
                        }
                        else
                        {
                            txtCarNum2.Text = "";
                            label33.Visible = false;
                            txtCarNum2.Visible = false;
                        }
                    }

                    lbDate.Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_Date"].ToString());
                    lbTime.Text = DT.Rows[i]["Q_Time"].ToString();

                    //ข้อมูลนำหนัก
                    txtBillNo.Text = DT.Rows[i]["Q_BillingNo"].ToString();
                    GVar.gvBillingOld = DT.Rows[i]["Q_BillingNo"].ToString();

                    //Gen เลขที่บิล ตอนขาออกเท่านั้น
                    if (txtBillNo.Text == "" && GVar.gvINOUT == "OUT")
                    {
                        //หน่วงเวลาสุ่ม 1-5 วิ
                        Random rnd = new Random();
                        int lvSleepTime = rnd.Next(1000, 2000);
                        System.Threading.Thread.Sleep(lvSleepTime);

                        //เช็คบิลก่อนหน้า
                        string lvChkJumpBill = FncCheckJumpBill();
                        string lvBilling = lvChkJumpBill;

                        //เช็คเลขที่บิลอีกรอบ
                        if (lvChkJumpBill == "")
                        {
                            lvBilling = GsysSQL.fncGetLastBillNo(GVar.gvOnline);
                            GVar.gvChangeBill = false;
                        }
                        else
                            GVar.gvChangeBill = true;

                        Rechk:
                        string lvChkBill2 = GsysSQL.FindBillNo(lvBilling, GVar.gvOnline);
                        if (lvChkBill2 != "")
                        {
                            //อัพเดทบิล
                            lvSQL = "Update SysDocNo Set S_RunNo += 1 Where S_MCode = 'Weight_Bill' ";
                            string lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                            lvBilling = GsysSQL.fncGetLastBillNo(GVar.gvOnline);
                            goto Rechk;
                        }

                        if (lvChkJumpBill == "")
                        {
                            txtBillNo.Text = GsysSQL.fncGetLastBillNo(GVar.gvOnline);
                            chkChangeBill.Checked = false;
                            GVar.gvChangeBill = false;
                        }
                        else
                        {
                            txtBillNo.Text = lvChkJumpBill;
                            chkChangeBill.Checked = true;
                            GVar.gvChangeBill = true;
                        }

                        GVar.gvNewBill = true;
                        //}
                    }
                    else
                    {
                        GVar.gvNewBill = false;
                    }

                    txtWeightIN.Text = Gstr.FncSetFormatDecimal(DT.Rows[i]["Q_WeightIN"].ToString(), 0);
                    txtWeightINDate.Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightINDate"].ToString());
                    txtWeightINTime.Text = DT.Rows[i]["Q_WeightINTime"].ToString();
                    txtWeightOut.Text = Gstr.FncSetFormatDecimal(DT.Rows[i]["Q_WeightOUT"].ToString(), 0);
                    txtWeightOutDate.Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightOUTDate"].ToString());
                    txtWeightOutTime.Text = DT.Rows[i]["Q_WeightOUTTime"].ToString();

                    if(GVar.gvUser == "jum")
                    {
                        label46.Visible = true;
                        txtWeightAll.Visible = true;
                        txtWeightAll.Text = (Gstr.fncToInt(txtWeightIN.Text) - Gstr.fncToInt(txtWeightOut.Text)).ToString("#,##0.00");
                    }
                    else
                    {

                    }
                   
                    fncGetDataCheck("Rail", lvRail);
                    fncGetDataCheck("Cane", lvCaneType);
                    fncGetDataCheck("Car", lvCarType);

                    //Gen คิวตะกาว
                    if (DT.Rows[i]["Q_TKNo"].ToString() == "")
                    {
                        FncGenTKNo_New(lvRail, lvQ);

                        GVar.gvTKNew = true;
                    }
                    else
                    {
                        txtTakaoNo.Text = DT.Rows[i]["Q_TKNo"].ToString();

                        GVar.gvTKNew = false;
                    }

                    GVar.gvTKNoOld = DT.Rows[i]["Q_TKNo"].ToString();
                    GVar.gvSampleNoOld = txtSampleNo.Text;

                    if (lvQ.Contains("."))
                        gWeightAll.Visible = false;
                    else
                        gWeightAll.Visible = true;

                    pvmode = "Edit";
                    timer1.Enabled = false;

                    if (GVar.gvTypeProgram == "W")
                        tabControl2.SelectedIndex = 1;
                    else
                        tabControl2.SelectedIndex = 0;

                    #region ระบบคิวล๊อก
                    //เช็คสถานะคิวล๊อก
                    string lvChk = DT.Rows[i]["Q_LockQNo"].ToString();
                    if (lvChk != "")
                    {
                        txtQLoop.Text = DT.Rows[i]["Q_Lockloop"].ToString();
                        txtQLockNo.Text = DT.Rows[i]["Q_LockNo"].ToString();
                        txtQLockQNo.Text = DT.Rows[i]["Q_LockQNo"].ToString();
                        txtQLockCarNum.Text = txtCarNum.Text;

                        pnShowQLock.Visible = false;
                        txtQLockNo.Enabled = false;
                        txtQLockQNo.Enabled = false;
                        txtQLockSearch.Enabled = true;
                    }
                    else
                    {
                        pnShowQLock.Visible = true;
                        txtQLockNo.Enabled = true;
                        txtQLockQNo.Enabled = true;
                        txtQLockSearch.Enabled = true;
                    }

                    LoadDataQLock();
                    #endregion
                }

                lbDate.Enabled = true;
                lbTime.Enabled = true;
                DT.Dispose();

                if (DT.Rows.Count == 0)
                {
                    MessageBox.Show("ไม่พบข้อมูล คิวที่ " + lvQ + " กรุณาตรวจสอบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtQ.Text = "";
                    txtQ.Focus();
                    //GVar.gvStatusCancel = true;
                    //this.Cursor = Cursors.Default;
                    //return;
                }
                else
                {
                    txtSampleNo.Focus();
                }
                
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                btnClear.PerformClick();
            }
        }

        private void โหลดขอมลToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string lvQ = sp1.ActiveSheet.Cells[GVar.gvRowIndex, 1].Text;
            string lvStation = sp1.ActiveSheet.Cells[GVar.gvRowIndex, 0].Text;

            txtCutDoc.Text = GsysSQL.fncGetCutdoc(lvQ);
           
            if (!txtCutDoc.Text.Contains('/'))
            {
                LoadQueueData(lvQ, lvStation, false);
                LoadCutDocData();
            }
            else
            {
                DataTable DT = new DataTable();
                string SQL = "Select * From  Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_Year = '' ";
                DT = GsysSQL.fncGetQueryData(SQL, DT, true);

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    txtCutDoc.Text = DT.Rows[i]["Q_CutDoc"].ToString();

                    if (chkCane7.Checked == true || chkCane13.Checked == true) txtCutCar.Text = DT.Rows[i]["Q_CutCar"].ToString();
                    string CutPrice = DT.Rows[i]["Q_CutPrice"].ToString();
                    string CarryPrice = DT.Rows[i]["Q_CarryPrice"].ToString();
                    if (CarryPrice != "0" && pvmode != "Edit" || CarryPrice != "" && pvmode != "Edit")
                    {
                        txtCarryPrice.Text = CarryPrice;
                        chkCarryPrice.Checked = true;
                    }
                    if (CutPrice != "0" || CutPrice != "")
                    {
                        txtCutPrice.Text = CutPrice;
                    }
                }

                LoadCutDocData();
            }
            LoadQueueData(lvQ, lvStation, false);

            fncEnableBtnWhenClick(lvQ);
        }

        private void พมพใบควToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GVar.gvPermitPrint != "1")
            {
                MessageBox.Show("คุณไม่มีสิทธิ์ในการใช้งาน ฟังก์ชันนี้ กรุณาติดต่อผู้ดูแล !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            โหลดขอมลToolStripMenuItem_Click(sender, e);
            
            string lvQ = sp1.ActiveSheet.Cells[GVar.gvRowIndex, 1].Text;
            string lvStation = sp1.ActiveSheet.Cells[GVar.gvRowIndex, 0].Text;

            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "Select * from Queue_Diary where Q_No = '" + lvQ + "' And Q_Station = '" + lvStation + "' And Q_CloseStatus = '' and Q_Year = '' ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                GVar.gvQuota = DT.Rows[i]["Q_Quota"].ToString();
                GVar.gvSubQuota = DT.Rows[i]["Q_SubQuota"].ToString();
                string lvQuota = GVar.gvQuota;
                string lvSubQuota = GVar.gvSubQuota;
                string lvQuotaName = GsysSQL.fncFindQuotaName(DT.Rows[i]["Q_Quota"].ToString(), GVar.gvOnline);
                string lvCarNum = DT.Rows[i]["Q_CarNum"].ToString();
                string lvCaneType = DT.Rows[i]["Q_CaneType"].ToString();
                string lvState = DT.Rows[i]["Q_Rail"].ToString();
                lvCaneType = GsysSQL.fncFindCaneTypeName(lvCaneType, GVar.gvOnline);
                string lvCaneDoc = DT.Rows[i]["Q_CaneDoc"].ToString();
                string lvDate = Gstr.fncChangeSDate(DT.Rows[i]["Q_Date"].ToString());
                string lvWeightIN = DT.Rows[i]["Q_WeightIN"].ToString();
                string lvWeoghtINTime = DT.Rows[i]["Q_WeightINTime"].ToString();
                string lvWeightOUT = DT.Rows[i]["Q_WeightOUT"].ToString();
                string lvWeightOUTDate = DT.Rows[i]["Q_WeightOUTDate"].ToString();
                string lvWeoghtOUTTime = DT.Rows[i]["Q_WeightOUTTime"].ToString();
                string lvSampleNo = DT.Rows[i]["Q_SampleNo"].ToString();
                string lvBilling = DT.Rows[i]["Q_BillingNo"].ToString();
                string lvTKNo = DT.Rows[i]["Q_TKNo"].ToString();
                string lvRail = DT.Rows[i]["Q_Rail"].ToString();
                string lvCutCar = DT.Rows[i]["Q_CutCar"].ToString();
                string lvCarType = DT.Rows[i]["Q_CarType"].ToString();
                string lvTime = DT.Rows[i]["Q_Time"].ToString();
                //string lvSubQuota = DT.Rows[i]["Q_Time"].ToString();
                //string lvSubQuotaName = GsysSQL.fncFindQuotaName(lvSubQuota, GVar.gvOnline);

                FncPrintQueue(lvQ, lvQuota, lvQuotaName, lvCarNum, lvCaneType, chkPreview.Checked, lvState, "Q", lvDate, lvCaneDoc, lvWeightIN, lvWeoghtINTime, lvWeightOUT, lvWeoghtOUTTime, true,"Q", lvSampleNo, lvBilling, lvStation, lvTKNo, lvRail, lvCutCar, lvCarType, lvTime, lvWeightOUTDate, lvCutCar, lvSampleNo
                    , "", "", "", "", "", "", "", "", "", "", "", "", "");
            }

            DT.Dispose();

            //Clear ข้อมูล
            btnClear_Click(sender, e);
        }
         
        private void txtCutPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCutPrice.Text != "")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtCaneDoc.Focus();
                }
            }
        }

        private void txtCarryPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCarryPrice.Text != "")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnSave_Click(sender, e);
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmSend frm = new frmSend();
            frm.ShowDialog();
        }

        private string ReadDataToMachine()
        {
            string lvReturn = "";

            try
            {
                //SerialPort mySerialPort = new SerialPort("COM" + txtE.Text);

                //mySerialPort.PortName = "COM3";
                //mySerialPort.BaudRate = 2400;
                //mySerialPort.Parity = Parity.None;
                //mySerialPort.StopBits = StopBits.One;
                //mySerialPort.DataBits = 8;
                //mySerialPort.Handshake = Handshake.None;

                mySerialPort.Open();
                //lvReturn = Gstr.Mid(mySerialPort.ReadLine(), 4, 5).Trim();

                //if (Gstr.fncToDouble(lvReturn) == 0) lvReturn = "";

                mySerialPort.Close();
                return lvReturn;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return lvReturn;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            mySerialPort.Open();
        }

        private void เพมขอมลพวงToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GVar.gvPermitNew != "1")
            {
                MessageBox.Show("คุณไม่มีสิทธิ์ในการใช้งาน ฟังก์ชันนี้ กรุณาติดต่อผู้ดูแล !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string lvQ = sp1.ActiveSheet.Cells[GVar.gvRowIndex, 1].Text;
            string lvCaneType = "";

            //เช็คสถานะ
            fncCheckONLINE();

            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "Select * from Queue_Diary where Q_No = '" + lvQ + "' And Q_CloseStatus = '' and Q_Year = '' ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                txtQ.Text = lvQ + ".1";
                txtQuota.Text = "";
                txtQuotaName.Text = "";
                txtCarNum.Text = "";
                txtCutCar.Text = "";
                txtCutDoc.Text = DT.Rows[i]["Q_CutDoc"].ToString();
                txtCaneDoc.Text = DT.Rows[i]["Q_CaneDoc"].ToString();
                txtMainCar.Text = DT.Rows[i]["Q_MainCar"].ToString();
                lvCaneType = DT.Rows[i]["Q_CaneType"].ToString();
                string lvCarType = DT.Rows[i]["Q_CarType"].ToString();
                txtCutPrice.Text = DT.Rows[i]["Q_CutPrice"].ToString();
                txtCarryPrice.Text = DT.Rows[i]["Q_CarryPrice"].ToString();
                string lvBonsucroStatus = DT.Rows[i]["Q_Bonsugo"].ToString();

                if(lvBonsucroStatus == "1")
                {
                    ChkBonsugo.Checked = true;
                }
                else
                {
                    ChkBonsugo.Checked = false;
                }

                fncGetDataCheck("Cane", lvCaneType);
                //fncGetDataCheck("Car", lvCarType);
                

                string lvChk = GsysSQL.fncCheckQueue(txtQ.Text, GVar.gvOnline);
                if (lvChk == "")
                    pvmode = "Add Truck";
                else
                    pvmode = "Edit";

                //สถานะ Bonsugo คิวแม่
                string lvBonsugo = GsysSQL.fncFindBonsugo(lvQ,GVar.gvOnline);
                if (lvBonsugo == "1")
                    ChkBonsugo.Checked = true;
                else
                    ChkBonsugo.Checked = false;

                //ราง
                string lvRail = DT.Rows[i]["Q_Rail"].ToString();
                if (lvRail == "A") ChkA.Checked = true;
                else if (lvRail == "B") ChkB.Checked = true;
                else ChkNone.Checked = true;
            }

            if (!txtCutDoc.Text.Contains('/'))
            {
                LoadCutDocData();
            }

            chkCar4.Checked = true;
            
            chk_CutdocA.Checked = true;
            txtCutDoc.Enabled = false;
            txtCaneDoc.Enabled = false;
            txtCutOwner.Enabled = false;
            txtCutCar.Enabled = false;
            txtCutPrice.Enabled = false;
            txtKeepOwner.Enabled = false;
            txtKeepPrice.Enabled = false;
            txtCarOwner.Enabled = false;
            txtCarNumPlate.Enabled = false;
            txtCarNumPlate2.Enabled = false;
            chkCarryPrice.Enabled = false;
            txtCarryPrice.Enabled = false;
            txtContractAll.Enabled = false;
            txtContractAllPrice.Enabled = false;
            
            //เปิดกล่องข้อมูล

            GVar.gvAddCarnum2 = true;
            DT.Dispose();
            ChkA.Enabled = false;
            ChkB.Enabled = false;
            ChkNone.Enabled = false;

            txtQuota.Focus();
        }

        private void btnSearchQ_Click(object sender, EventArgs e)
        {
            if (GVar.gvPermitNew != "1")
            {
                MessageBox.Show("คุณไม่มีสิทธิ์ในการใช้งาน ฟังก์ชันนี้ กรุณาติดต่อผู้ดูแล !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtCarNum.Text = "";
            txtCarNum2.Text = "";
            txtCutDoc.Text = "";
            txtQuota.Text = "";
            txtCaneDoc.Text = "";
            txtMainCar.Text = "";
            txtQuotaName.Text = "";
            txtCutCar.Text = "";
            txtCutPrice.Text = "";
            txtCarryPrice.Text = "";
            txtSampleNo.Text = "";

            //ซ่อน
            label33.Visible = false;
            txtCarNum2.Visible = false;

            btnSearchQ.Visible = false;
            btnSearchQ2.Visible = true;

            btnSearchQ.Enabled = false;
            txtQ.ReadOnly = true;
            txtQ.Text = GsysSQL.fncGenQueueNo(GVar.gvOnline);   
            
            //เลขบิลให้ Gen ใหม่
            //ย้ายไป Gen ขาออก
            //if (txtBillNo.Text == "") txtBillNo.Text = GsysSQL.fncGetLastBillNo(GVar.gvOnline);

            //Gen คิวตะกาว
            if (ChkA.Checked)
                txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo_New("A", GVar.gvTypeDocTakao, GVar.gvOnline);
            else if (ChkB.Checked)
                txtTakaoNo.Text = GsysSQL.fncGetLastTakaoNo_New("B", GVar.gvTypeDocTakao, GVar.gvOnline);

            txtQuota.Focus();

            pvmode = "";
            
        }

        private void txtQ_KeyDown(object sender, KeyEventArgs e)
        {
           if (e.KeyCode == Keys.Enter)
            {
                string lvQ = txtQ.Text;
                string lvStation = Gstr.Left(lvQ,1);
                //string[] lvArr = lvQ.Split('_');

                //if (lvArr.Count() > 1)
                //{
                //    lvStation = lvArr[0];
                GVar.gvStationSelect = lvStation;
                //    lvQ = lvArr[1];
                //}
                //else
                //{
                //    lvQ = lvArr[0];
                //}

                txtCutDoc.Text = GsysSQL.fncGetCutdoc(txtQ.Text);
              

                if (!txtCutDoc.Text.Contains('/'))
                {
                    LoadQueueData(lvQ, lvStation, false);
                    ////ถ้ามี Status Cancel
                    //if(GVar.gvStatusCancel == true)
                    //{
                    //    GVar.gvStatusCancel = false;
                    //    txtCutDoc.Text = "";
                    //    return;
                    //}
                    LoadCutDocData();
                }
                else
                {
                    DataTable DT = new DataTable();
                    string SQL = "Select * From  Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_Year = '' ";
                    if(GVar.gvUser == "weight")
                    {
                        SQL += "And (Q_WeightOUT = 0) ";//Q_WeightIN = 0 OR 
                        SQL += "And Q_CloseStatus = '' ";
                    }
                    DT = GsysSQL.fncGetQueryData(SQL, DT, true);

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        txtCutDoc.Text = DT.Rows[i]["Q_CutDoc"].ToString();
                        
                        if(chkCane7.Checked == true || chkCane13.Checked == true) txtCutCar.Text = DT.Rows[i]["Q_CutCar"].ToString();
                        string CutPrice = DT.Rows[i]["Q_CutPrice"].ToString();
                        string CarryPrice = DT.Rows[i]["Q_CarryPrice"].ToString();
                        if(CarryPrice != "0" && pvmode != "Edit" || CarryPrice != "" && pvmode != "Edit")
                        {
                            txtCarryPrice.Text = CarryPrice;
                            chkCarryPrice.Checked = true;
                        }
                        if(CutPrice != "0" || CutPrice != "")
                        {
                            txtCutPrice.Text = CutPrice;
                        }
                    }
                }
                
                LoadQueueData(lvQ, lvStation, false);
                fncEnableBtnWhenClick(lvQ);
            }
        }

        private void btnLoadDT_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void อพเดทขอมลวนทToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GVar.gvPermitEdit != "1")
            {
                MessageBox.Show("คุณไม่มีสิทธิ์ในการใช้งาน ฟังก์ชันนี้ กรุณาติดต่อผู้ดูแล !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string lvQ = sp1.ActiveSheet.Cells[GVar.gvRowIndex, 1].Text;
            string lvQuota = sp1.ActiveSheet.Cells[GVar.gvRowIndex, 3].Text;

            //ยืนยัน
            string lvTxtAlert = "ยืนยันการแก้ไขวันที่ ของ คิวที่ " + lvQ + " โควต้า " + lvQuota;
            DialogResult dialogResult = MessageBox.Show(lvTxtAlert, "Confirm?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }

            string lvDate = Gstr.fncChangeTDate(sp1.ActiveSheet.Cells[GVar.gvRowIndex, 5].Text);
            string lvTime = sp1.ActiveSheet.Cells[GVar.gvRowIndex, 6].Text;
            //อัพเดท
            string lvSQL = "Update Queue_Diary set Q_Date = '" + lvDate + "' , Q_Time = '"+ lvTime +"' where Q_No = '" + lvQ + "' And Q_CloseStatus = '' and Q_Year = '' ";
            string lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

            sp1.ActiveSheet.Rows[GVar.gvRowIndex].Remove();
            //sp1.ActiveSheet.RowCount += 1;

            if (lvResault == "Success")
            {
                GsysSQL.fncKeepLogData(GVar.gvUser, "แก้ไขวันที่ข้อมูลคิว", "แก้ไขวันที่คิวที่ " + lvQ + " โควต้า " + lvQuota);
                MessageBox.Show("แก้ไขข้อมูลเรียบร้อย", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("ไม่สามารถแก้ไขข้อมูลได้ เนื่องจาก " + lvResault, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadData();
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GVar.gvTypeProgram == "Q" && tabControl2.SelectedIndex == 1)
            {
                tabControl2.SelectedIndex = 0;
            }

        }

        private void btnLogOff_Click(object sender, EventArgs e)
        {
            //if (lbUser.Text == "admin")
            //{
            //Process.Start("notepad.exe", Application.StartupPath + "\\System_Data.dll");
            Application.Restart();
            //}

            //txtWeightMain.Text = "30.00";
        }

        private void btnStopConnect_Click(object sender, EventArgs e)
        {
            btnStopConnect.Enabled = false;
            btnConnect.Enabled = true;
            TGetWeight.Enabled = false;
            richWeightBox.Text = "";
            txtWeightMain.Text = "";

            if (mySerialPort.IsOpen) mySerialPort.Close();
        }

        private void btnSendWeight_Click(object sender, EventArgs e)
        {
            if (txtQ.Text == "")
            {
                MessageBox.Show("กรุณาเลือกคิว กรุณาตรวจสอบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (txtWeightMain.Text == "" || txtWeightMain.Text == "0.00")
            {
                MessageBox.Show("ไม่พบข้อมูลน้ำหนัก กรุณาตรวจสอบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (GVar.gvINOUT == "IN" && Gstr.fncToDouble(txtWeightIN.Text) > 0 && Gstr.fncToDouble(txtWeightOut.Text) > 0)
            {
                MessageBox.Show("คิวนี้มีการชั่งออกเรียบร้อยแล้ว ไม่สามารถดึงน้ำหนักได้อีก" + Environment.NewLine + Environment.NewLine + "กรุณาตรวจสอบเลขที่คิว ว่าถูกต้องหรือไม่?", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //string lvCloseStatus = GsysSQL.fncFindCloseStatusForEdit(txtQ.Text, txtCarNum.Text, GVar.gvOnline);
            //if (lvCloseStatus == "")
            //{
            if (lbOUT.Visible)
                {
                    txtWeightOut.Text = txtWeightMain.Text;
                    txtWeightOutDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtWeightOutTime.Text = DateTime.Now.ToString("HH:mm:ss");
                if(GVar.gvUser == "jum")
                {
                    label46.Visible = true;
                    txtWeightAll.Visible = true;
                    txtWeightAll.Text = (Gstr.fncToInt(txtWeightIN.Text) - Gstr.fncToInt(txtWeightOut.Text)).ToString("#,##0.00");
                }

            }
                else
                {
                    txtWeightIN.Text = txtWeightMain.Text;
                    txtWeightINDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtWeightINTime.Text = DateTime.Now.ToString("HH:mm:ss");
                }
            //}

            txtSampleNo.Focus();
        }
        
        private void mySerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (mySerialPort.IsOpen == true)
            {
                string pvStr = mySerialPort.ReadExisting();
                UpdateSrData(pvStr);
            }            
        }
        
        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnStopConnect.Enabled = true;
            btnConnect.Enabled = false;
            TGetWeight.Enabled = true;

            reCheck:
            try
            {
                mySerialPort.PortName = GVar.gvComport;
                mySerialPort.BaudRate = 2400;
                mySerialPort.Parity = Parity.Even;
                mySerialPort.StopBits = StopBits.One;
                mySerialPort.DataBits = 7;
                mySerialPort.Handshake = Handshake.None;

                if (mySerialPort.IsOpen) mySerialPort.Close();
                mySerialPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ไม่สามารถติดต่อเครื่องชั่งได้ กรุณาตรวจสอบ Port เชื่อมต่อ" + Environment.NewLine + Environment.NewLine + "Error : " + ex.Message, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnStopConnect.Enabled = false;
                btnConnect.Enabled = true;
                TGetWeight.Enabled = false;
                return;

                //GVar.gvComport = (Gstr.fncToInt(GVar.gvComport) + 1).ToString();

                //if (Gstr.fncToInt(GVar.gvComport) > 10)
                //{
                //    MessageBox.Show("ไม่สามารถติดต่อเครื่องชั่งได้ กรุณาตรวจสอบ Port เชื่อมต่อ" + Environment.NewLine + Environment.NewLine + "Error : " + ex.Message, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    btnStopConnect.Enabled = false;
                //    btnConnect.Enabled = true;
                //    TGetWeight.Enabled = false;
                //    return;
                //}
                //else
                //{
                //    goto reCheck;
                //}
            }
        }

        private void fncDecryptData(string lvWeight)
        {
            //lvWeight = ")0     2154.56    00";
            //4\u0010 0\u0001\u0002\u0001\u0003

            double lvReturn = 0;
            
            //lvWeight = lvWeight.Replace(" ", "|");
            string[] lvArr = lvWeight.Split(' ');

            for (int i = 0; i < lvArr.Count(); i++)
            {
                string lvData = lvArr[i];

                if (lvData != "")
                {
                    try
                    {
                        double lvChk = Convert.ToDouble(lvData);

                        if (lvChk > 0)
                        {
                            lvReturn = lvChk;
                            break;
                        }
                    }
                    catch
                    {
                        lvReturn = 0;
                    }
                }
            }

            //lvWeight = (lvReturn / 1000).ToString("#,##0"); //หน่วยตัน
            lvWeight = (lvReturn).ToString("#,##0"); //หน่วย กก.

            txtWeightMain.Text = lvWeight;
        }

        delegate void pvSerialDataDelegate(string srData);

        private void UpdateSrData(string srData)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new pvSerialDataDelegate(UpdateSrData), new object[] { srData });
                return;
            }

            richWeightBox.AppendText(srData);
        }

        private void TGetWeight_Tick(object sender, EventArgs e)
        {
            try 
            {
                int totalLines = richWeightBox.Lines.Length;
                string lastLine = richWeightBox.Lines[totalLines - 2]; // Here is the last Line out.
                fncDecryptData(lastLine);

                if (lastLine == pvLastWeight) pvCountWeight += 1; else pvCountWeight = 0;

                if (pvCountWeight >= 3 && txtWeightMain.Text != "0")
                {
                    btnSendWeight.Enabled = true;
                }
                else
                {
                    btnSendWeight.Enabled = false;
                }

                pvLastWeight = lastLine;

                if (totalLines > 100) richWeightBox.Text = "";
            }
            catch
            {
                txtWeightMain.Text = "";
                btnSendWeight.Enabled = false;
            }
        }

        private void btnCloseStatus_Click(object sender, EventArgs e)
        {
            frmCloseStatus frm = new frmCloseStatus();
            frm.ShowDialog();

            LoadData();
        }

        private void rdIN_CheckedChanged(object sender, EventArgs e)
        {
            if (GVar.gvUserType == "admin" || GVar.gvUserType == "head")
            {
                lbIN.Visible = true;
                lbOUT.Visible = false;

                GVar.gvINOUT = "IN";
            }
        }

        private void rdOut_CheckedChanged(object sender, EventArgs e)
        {
            if (GVar.gvUserType == "admin" || GVar.gvUserType == "head")
            {
                lbIN.Visible = false;
                lbOUT.Visible = true;

                GVar.gvINOUT = "OUT";
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if(GVar.gvTypeProgram == "W")
            {
                frmShowReport frm = new frmShowReport();
                frm.ShowDialog();
            }
            else
            {
                frmCloseStatus frm = new frmCloseStatus();
                frm.ShowDialog();

                if(GVar.gvQNo != "")
                {
                    txtQ.Text = GVar.gvQNo;
                    string lvQ = txtQ.Text;
                    string lvStation = Gstr.Left(lvQ, 1);
                    GVar.gvStationSelect = lvStation;
                    LoadQueueData(lvQ, lvStation, false);
                    LoadCutDocData();
                }
            }
           
        }

        private void พมพสลปToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GVar.gvPermitPrint != "1")
            {
                MessageBox.Show("คุณไม่มีสิทธิ์ในการใช้งาน ฟังก์ชันนี้ กรุณาติดต่อผู้ดูแล !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            โหลดขอมลToolStripMenuItem_Click(sender, e);

            string lvQ = sp1.ActiveSheet.Cells[GVar.gvRowIndex, 1].Text;

            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "Select * from Queue_Diary where Q_No = '" + lvQ + "' And Q_CloseStatus = '' and Q_Year = '' ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                GVar.gvQuota = DT.Rows[i]["Q_Quota"].ToString();
                string lvQuota = GVar.gvQuota;
                GVar.gvSubQuota = DT.Rows[i]["Q_SubQuota"].ToString();
                string lvSubQuota = GVar.gvSubQuota;
                string lvQuotaName = GsysSQL.fncFindQuotaName(DT.Rows[i]["Q_Quota"].ToString(), GVar.gvOnline);
                string lvCarNum = DT.Rows[i]["Q_CarNum"].ToString();

                //ถ้าชั่งรวมให้แสดงเลขทะเบียนพ่วงด้วย
                if (ChkWeightAll.Checked)
                {
                    string lvQNew = lvQ + ".1";

                    if (GVar.gvINOUT == "OUT")
                    {
                        if (lvCarNum == "")
                            lvCarNum = GsysSQL.fncFindCarNum2(lvQNew, GVar.gvOnline);
                        else
                            lvCarNum += " - " + GsysSQL.fncFindCarNum2(lvQNew, GVar.gvOnline);
                    }
                }

                string lvCutdoc = DT.Rows[i]["Q_CutDoc"].ToString();
                string lvCaneType = DT.Rows[i]["Q_CaneType"].ToString();
                string lvState = DT.Rows[i]["Q_Rail"].ToString();
                lvCaneType = GsysSQL.fncFindCaneTypeName(lvCaneType, GVar.gvOnline);
                string lvDate = Gstr.fncChangeSDate(DT.Rows[i]["Q_Date"].ToString());
                string lvCaneDoc = DT.Rows[i]["Q_CaneDoc"].ToString();
                string lvWeightIN = DT.Rows[i]["Q_WeightIN"].ToString();
                string lvWeoghtINTime = DT.Rows[i]["Q_WeightINTime"].ToString();
                string lvWeightOUT = DT.Rows[i]["Q_WeightOUT"].ToString();
                string lvWeightOUTDate = DT.Rows[i]["Q_WeightOUTDate"].ToString();
                string lvWeoghtOUTTime = DT.Rows[i]["Q_WeightOUTTime"].ToString();
                string lvSampleNo = DT.Rows[i]["Q_SampleNo"].ToString();
                //string lvSampleNo2 = GsysSQL.fncFindSample2(lvQ+".1", true,GVar.gvOnline);

                //if (lvSampleNo2 != "" && lvSampleNo.Length == 8) lvSampleNo += lvSampleNo2;

                string lvBilling = DT.Rows[i]["Q_BillingNo"].ToString();
                string lvStation = DT.Rows[i]["Q_Station"].ToString();
                string lvTKNo = DT.Rows[i]["Q_TKNo"].ToString();
                string lvRail = DT.Rows[i]["Q_Rail"].ToString();
                string lvCutCar = DT.Rows[i]["Q_CutCar"].ToString();
                string lvCarType = DT.Rows[i]["Q_CarType"].ToString();
                string lvTime = DT.Rows[i]["Q_Time"].ToString();
                //string lvSubQuota = DT.Rows[i]["Q_SubQuota"].ToString();
                //string lvSubQuotaName = GsysSQL.fncFindQuotaName(lvSubQuota, GVar.gvOnline);
                
                lvSampleNo = txtSampleNo.Text;

                //ข้อมูลผู้รับเหมาตัด
                string[] lvCarcutRmsplit = { };
                string lvCarcutRmid = ""; //รหัส
                string lvCarcutRmname = ""; //ชื่อ
                string lvCarcutRmPrice = ""; //ราคา

                if(txtCutOwner.Text != "")
                {
                    string CutOwner = txtCutOwner.Text;
                    lvCarcutRmsplit = CutOwner.Split(':');
                    lvCarcutRmid = lvCarcutRmsplit[0]; //รหัส
                    lvCarcutRmname = lvCarcutRmsplit[1]; //ชื่อ
                    lvCarcutRmPrice = txtCutPrice.Text; //ราคา
                }
                

                string[] lvTruckRmsplit = { };
                string lvTruckRmid = ""; //รหัส
                string lvTruckRmname = ""; //ชื่อ
                string lvTruckRmprice = ""; //ราคา
                
                if(txtCarOwner.Text != "")
                {
                    //ข้อมูลผู้รับเหมาบรรทุก
                    string TruckOwner = txtCarOwner.Text;
                    lvTruckRmsplit = TruckOwner.Split(':');
                    lvTruckRmid = lvTruckRmsplit[0]; //รหัส
                    lvTruckRmname = lvTruckRmsplit[1]; //ชื่อ
                    lvTruckRmprice = txtCarryPrice.Text; //ราคา
                }

                string[] lvKeebRmsplit = { };
                string lvKeebRmid = ""; //รหัส
                string lvKeebRmname = ""; //ชื่อ
                string lvKeebRmprice = ""; //ราคา

                if(txtKeepOwner.Text != "")
                {
                    //ข้อมูลผู้รับเหมาคีบ
                    string KeebOwner = txtKeepOwner.Text;
                    lvKeebRmsplit = KeebOwner.Split(':');
                    lvKeebRmid = lvKeebRmsplit[0]; //รหัส
                    lvKeebRmname = lvKeebRmsplit[1]; //ชื่อ
                    lvKeebRmprice = txtKeepPrice.Text; //ราคา
                }

                string[] lvAllRmsplit = { };
                string lvAllRmid = ""; //รหัส
                string lvAllRmname = ""; //ชื่อ
                string lvAllRmprice = ""; //ราคา

                if(txtContractAll.Text != "")
                {
                    //ข้อมูลผู้รับเหมารวม
                    string AllOwner = txtContractAll.Text;
                    lvAllRmsplit = AllOwner.Split(':');
                    lvAllRmid = lvAllRmsplit[0]; //รหัส
                    lvAllRmname = lvAllRmsplit[1]; //ชื่อ
                    lvAllRmprice = txtContractAllPrice.Text; //ราคา
                }
                

                FncPrintQueue(lvQ, lvQuota, lvQuotaName, lvCarNum, lvCaneType, chkPreview.Checked, lvState, "W", lvDate, lvCaneDoc, lvWeightIN, lvWeoghtINTime, lvWeightOUT, lvWeoghtOUTTime, true,"", lvSampleNo, lvBilling, lvStation, lvTKNo, lvRail, lvCutCar, lvCarType, lvTime, lvWeightOUTDate, lvCutCar, lvSampleNo, lvCarcutRmid, lvCarcutRmname, lvCarcutRmPrice, lvTruckRmid,
                    lvTruckRmname, lvTruckRmprice, lvKeebRmid, lvKeebRmname, lvKeebRmprice, lvAllRmid, lvAllRmname, lvAllRmprice, lvCutdoc);
            }

            DT.Dispose();

            //Clear ข้อมูล
            btnClear_Click(sender, e);

        }

        private void ChkShowOut_CheckedChanged(object sender, EventArgs e)
        {
            //ChkShowIN.Checked = false;
            LoadData();
        }

        private void cmbPrinter_DropDown(object sender, EventArgs e)
        {
            PrinterSettings settings = new PrinterSettings();

            string lvPrinterSelect = settings.PrinterName;
            //เช็ค Printer
            cmbPrinter.Items.Clear();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                cmbPrinter.Items.Add(printer);
            }
        }

        private void chkShowFinish_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fncDecryptData("4 0");
        }

        private void ChkShowIN_CheckedChanged(object sender, EventArgs e)
        {
            //ChkShowOut.Checked = false;
            LoadData();
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

        private void frmMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                btnSave_Click(sender, e);
            else if (e.KeyCode == Keys.F6)
                btnClear_Click(sender, e);
            else if (e.KeyCode == Keys.F7)
            {
                if (btnSendWeight.Enabled)
                    btnSendWeight_Click(sender, e);
            }
            else if (e.KeyCode == Keys.F12)
            {
                if (!richWeightBox.Visible)
                    richWeightBox.Visible = true;
                else
                    richWeightBox.Visible = false;
            }
        }

        private void txtSampleNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave_Click(sender, e);
            }
        }

        private void ChkShowA_1_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();

            tabControl2.SelectedIndex = 0;
        }

        private void ChkShowB_1_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();

            tabControl2.SelectedIndex = 0;
        }

        private void ChkShowIN_1_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();

            tabControl2.SelectedIndex = 0;
        }

        private void ChkShowOut_1_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();

            tabControl2.SelectedIndex = 0;
        }

        private void chkShowFinish_1_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();

            tabControl2.SelectedIndex = 0;
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            frmSetDoc frm = new frmSetDoc();
            frm.ShowDialog();
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            txtWeightIN.Text = "1000";
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            txtWeightOut.Text = "500";
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

        }

        private void txtWeightMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.I)
            {
                Random rnd = new Random();
                int lvWeight = rnd.Next(30000,40000);
                txtWeightMain.Text = lvWeight.ToString("#,##0");
                txtWeightIN.Text = lvWeight.ToString("#,##0");
                txtWeightINDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtWeightINTime.Text = DateTime.Now.ToString("HH:mm:ss");
            }
            else if (e.Control && e.KeyCode == Keys.O)
            {
                Random rnd = new Random();
                int lvWeight = rnd.Next(3000, 10000);
                txtWeightMain.Text = lvWeight.ToString("#,##0");
                txtWeightOut.Text = lvWeight.ToString("#,##0");
                txtWeightOutDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtWeightOutTime.Text = DateTime.Now.ToString("HH:mm:ss");
            }
        }

        private void sp1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //ปิดการใช้งานเนื่องจาก ใช้ปุ๋มค้นหาของระบบ ใบนำตัดออนไลน์

           /* if (txtCarNum.Text != "" && pvmode == "")
            {
                //ค้นหาทะเบียนรถ
                DataTable DT = new DataTable();

                string lvSQL = "Select * from Queue_Diary Where Q_CarNum like '%" + txtCarNum.Text + "%' and Q_Year = '' ";
                lvSQL += "And (Q_WeightOUT = 0) ";//Q_WeightIN = 0 OR 
                lvSQL += "And Q_CloseStatus = '' ";
                DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

                int lvNumRow = DT.Rows.Count;

                string lvQ = "";
                if (lvNumRow > 0)
                {
                    if (lvNumRow == 1)
                    {
                        lvQ = DT.Rows[0]["Q_No"].ToString();
                        txtCutDoc.Text = GsysSQL.fncGetCutdoc(lvQ);

                        if (!txtCutDoc.Text.Contains('/'))
                        {
                            LoadQueueData(lvQ, GVar.gvStation, false);
                            LoadCutDocData();
                        }
                        else
                        {
                            DataTable DTs = new DataTable();
                            string SQL = "Select * From  Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_Year = '' ";
                            DTs = GsysSQL.fncGetQueryData(SQL, DTs, true);

                            for (int i = 0; i < DT.Rows.Count; i++)
                            {
                                txtCutDoc.Text = DTs.Rows[i]["Q_CutDoc"].ToString();
                                txtCutCar.Text = DTs.Rows[i]["Q_CutCar"].ToString();
                                string CutPrice = DTs.Rows[i]["Q_CutPrice"].ToString();
                                string CarryPrice = DTs.Rows[i]["Q_CarryPrice"].ToString();
                                if (CarryPrice != "0" || CarryPrice != "")
                                {
                                    txtCarryPrice.Text = CarryPrice;
                                    chkCarryPrice.Checked = true;
                                }
                                if (CutPrice != "0" || CutPrice != "")
                                {
                                    txtCutPrice.Text = CutPrice;
                                }
                            }
                        }
                        LoadQueueData(lvQ, GVar.gvStation, true);
                    }
                    else if (lvNumRow > 1)
                    {
                        GVar.gvSCode = "";
                        frmCarBrowse frm = new frmCarBrowse();
                        frm.LoadData(lvSQL);
                        frm.ShowDialog();

                        if (GVar.gvSCode != "")
                        {
                            txtCutDoc.Text = GsysSQL.fncGetCutdoc(lvQ);
                            if (!txtCutDoc.Text.Contains('/'))
                            {
                                LoadQueueData(lvQ, GVar.gvStation, false);
                                LoadCutDocData();
                            }
                            else
                            {
                                DataTable DTs = new DataTable();
                                string SQL = "Select * From  Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_Year = '' ";
                                DTs = GsysSQL.fncGetQueryData(SQL, DTs, true);

                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    txtCutDoc.Text = DTs.Rows[i]["Q_CutDoc"].ToString();
                                    txtCutCar.Text = DTs.Rows[i]["Q_CutCar"].ToString();
                                    string CutPrice = DTs.Rows[i]["Q_CutPrice"].ToString();
                                    string CarryPrice = DTs.Rows[i]["Q_CarryPrice"].ToString();
                                    if (CarryPrice != "0" || CarryPrice != "")
                                    {
                                        txtCarryPrice.Text = CarryPrice;
                                        chkCarryPrice.Checked = true;
                                    }
                                    if (CutPrice != "0" || CutPrice != "")
                                    {
                                        txtCutPrice.Text = CutPrice;
                                    }
                                }
                            }
                            LoadQueueData(lvQ, GVar.gvStation, true);

                        }
                    }
                    fncEnableBtnWhenClick(lvQ);*/

            string lvQ = sp1.ActiveSheet.Cells[e.Row, 1].Text;
            GVar.gvKeepTakao = sp1.ActiveSheet.Cells[e.Row, 11].Text;

            txtCutDoc.Text = GsysSQL.fncGetCutdoc(lvQ);

            //โหลดข้อมูลใบนำตัด
            if (!txtCutDoc.Text.Contains('/'))
            {
                LoadQueueData(lvQ, GVar.gvStation, false);
                LoadCutDocData();
            }
            else
            {
                DataTable DT = new DataTable();
                string SQL = "Select * From  Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_Year = '' ";
                DT = GsysSQL.fncGetQueryData(SQL, DT, true);

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    txtCutDoc.Text = DT.Rows[i]["Q_CutDoc"].ToString();
                    txtCutCar.Text = DT.Rows[i]["Q_CutCar"].ToString();
                    string CutPrice = DT.Rows[i]["Q_CutPrice"].ToString();
                    string CarryPrice = DT.Rows[i]["Q_CarryPrice"].ToString();
                    if (CarryPrice != "0" || CarryPrice != "")
                    {
                        txtCarryPrice.Text = CarryPrice;
                        chkCarryPrice.Checked = true;
                    }
                    if (CutPrice != "0" || CutPrice != "")
                    {
                        txtCutPrice.Text = CutPrice;
                    }
                }
            }

            //โหลดข้อมูลคิว
            LoadQueueData(lvQ, GVar.gvStation, false);
            fncEnableBtnWhenClick(lvQ);

            /*string lvCaneType = GsysSQL.fncGetCaneTypeDetail(lvQ);
            fncGetDataCheck("Cane", lvCaneType);*/

            /*txtQuota.Text = GVar.gvQuota;
            txtQuotaName.Text = GsysSQL.fncFindQuotaName(GVar.gvQuota, GVar.gvOnline);*/

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void fncEnableBtnWhenClick(string lvQ)
        {
            if (txtCutDoc.Text != "" && !lvQ.Contains('.'))
            {
                //Feature ปิดเปิดปุ่มตรงกล่องใบนำตัด
                chk_CutdocA.Checked = true;
                txtCutDoc.Enabled = false;
                txtCaneDoc.Enabled = true;
                txtCutOwner.Enabled = true;
                txtCutCar.Enabled = true;
                //*05-01-2565 ปิดการใส่ราคาค่าตัดต่างๆ
                if (GVar.gvTypeProgram == "Q") txtCutPrice.Enabled = true;
                else txtCutPrice.Enabled = true;
                txtKeepOwner.Enabled = true;
                //*05-01-2565 ปิดการใส่ราคาค่าตัดต่างๆ
                if (GVar.gvTypeProgram == "Q") txtKeepPrice.Enabled = true;
                else txtKeepPrice.Enabled = true;
                txtCarOwner.Enabled = true;
                txtCarNumPlate.Enabled = true;
                txtCarNumPlate2.Enabled = true;
                //*05-01-2565 ปิดการใส่ราคาค่าตัดต่างๆ
                if (GVar.gvTypeProgram == "Q") chkCarryPrice.Enabled = true;
                else chkCarryPrice.Enabled = true;
                //*05-01-2565 ปิดการใส่ราคาค่าตัดต่างๆ
                if (GVar.gvTypeProgram == "Q") txtCarryPrice.Enabled = true;
                else txtCarryPrice.Enabled = true;
                btnSearchcarcut.Enabled = true;
                btnRmTruckSearch.Enabled = true;
                btnRmKeebSearch.Enabled = true;
                btnRmTruckSearch.Enabled = true;
                btmSearchCarRm.Enabled = true;

                if (txtCutOwner.Text == "")
                {
                    btnRmAllSearch.Enabled = true;
                    txtContractAll.Enabled = true;
                    //*05-01-2565 ปิดการใส่ราคาค่าตัดต่างๆ
                    if (GVar.gvTypeProgram == "Q") txtContractAllPrice.Enabled = true;
                    else txtContractAllPrice.Enabled = true;
                    groupBox25.Enabled = true;
                    groupBox18.Enabled = true;
                    groupBox24.Enabled = true;
                }
                else if (txtKeepOwner.Text == "")
                {
                    btnRmAllSearch.Enabled = true;
                    txtContractAll.Enabled = true;
                    //*05-01-2565 ปิดการใส่ราคาค่าตัดต่างๆ
                    if (GVar.gvTypeProgram == "Q") txtContractAllPrice.Enabled = true;
                    else txtContractAllPrice.Enabled = true;
                    groupBox25.Enabled = true;
                    groupBox18.Enabled = true;
                    groupBox24.Enabled = true;
                }
                else
                {
                    btnRmAllSearch.Enabled = true;
                    txtContractAll.Enabled = true;
                    //*05-01-2565 ปิดการใส่ราคาค่าตัดต่างๆ
                    if (GVar.gvTypeProgram == "Q") txtContractAllPrice.Enabled = true;
                    else txtContractAllPrice.Enabled = true;
                    groupBox25.Enabled = true;
                    groupBox18.Enabled = true;
                    groupBox24.Enabled = true;
                }

            }
            else if (txtCutDoc.Text != "" && lvQ.Contains('.'))
            {
                chk_CutdocA.Checked = true;
                txtCutDoc.Enabled = false;
                txtCaneDoc.Enabled = false;
                txtCutOwner.Enabled = false;
                txtCutCar.Enabled = false;
                txtCutPrice.Enabled = false;
                txtKeepOwner.Enabled = false;
                txtKeepPrice.Enabled = false;
                txtCarOwner.Enabled = false;
                txtCarNumPlate.Enabled = false;
                txtCarNumPlate2.Enabled = false;
                chkCarryPrice.Enabled = false;
                txtCarryPrice.Enabled = false;
                txtContractAll.Enabled = false;
                txtContractAllPrice.Enabled = false;
                btnSearchcarcut.Enabled = false;
                btnRmTruckSearch.Enabled = false;
                btnRmKeebSearch.Enabled = false;
                btnRmAllSearch.Enabled = false;
                btnRmTruckSearch.Enabled = false;
                btmSearchCarRm.Enabled = false;
                groupBox25.Enabled = false;
                groupBox18.Enabled = false;
                groupBox24.Enabled = false;
            }
            else
            {
                chk_CutdocA.Checked = false;
                txtCutDoc.Enabled = false;
                txtCaneDoc.Enabled = false;
                txtCutOwner.Enabled = false;
                txtCutCar.Enabled = false;
                txtCutPrice.Enabled = false;
                txtKeepOwner.Enabled = false;
                txtKeepPrice.Enabled = false;
                txtCarOwner.Enabled = false;
                txtCarNumPlate.Enabled = false;
                txtCarNumPlate2.Enabled = false;
                chkCarryPrice.Enabled = false;
                txtCarryPrice.Enabled = false;
                txtContractAll.Enabled = false;
                txtContractAllPrice.Enabled = false;
                btnSearchcarcut.Enabled = false;
                btnRmTruckSearch.Enabled = false;
                btnRmKeebSearch.Enabled = false;
                btnRmAllSearch.Enabled = false;
                btnRmTruckSearch.Enabled = false;
                btmSearchCarRm.Enabled = false;
                groupBox25.Enabled = false;
                groupBox18.Enabled = false;
                groupBox24.Enabled = false;
            }
        }

        private void txtQ_Click(object sender, EventArgs e)
        {
            txtQ.SelectAll();
        }

        private void chkCar5_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkCar5.Checked)
            {
                chkCar5.ForeColor = Color.Red;
                chkCar1.Checked = false;
                chkCar2.Checked = false;
                chkCar3.Checked = false;
                chkCar4.Checked = false;
                chkCar6.Checked = false;
                chkCar7.Checked = false;
                lbCarName.Text = chkCar5.Text;
                txtCarryPrice.Focus();
            }
            else
            {
                chkCar5.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCarryPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCarryPrice.Checked)
            {
                if (txtCarNum.Text != "")
                {
                    //txtCarNum.Text = txtCarNum.Text.Replace('.', 'a');
                }

                //*05-01-2565 ปิดเปิด ราคาค่ารับเหมาบรรทุก
                chkCarryPrice.ForeColor = Color.Red;
                if (GVar.gvTypeProgram == "Q") txtCarryPrice.Enabled = true;
                else txtCarryPrice.Enabled = true;
                txtCarryPrice.Focus();
            }
            else
            {
                if (txtCarNum.Text != "")
                {
                    //txtCarNum.Text = txtCarNum.Text.Replace('a', '.');
                }
                txtCarryPrice.Text = "";

                chkCarryPrice.ForeColor = Color.Black;
                txtCarryPrice.Enabled = false;
            }
        }

        private void ChkBonsugo_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkBonsugo.Checked)
            {
                ChkBonsugo.ForeColor = Color.Red;
                txtSampleNo.Focus();
            }
            else
            {
                ChkBonsugo.ForeColor = Color.Black;
            }
        }

        private void ChkWeightAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkWeightAll.Checked)
            {
                ChkWeightAll.ForeColor = Color.Red;

                string lvQNew = txtQ.Text + ".1";

                txtCarNum2.Text = GsysSQL.fncFindCarNum2(lvQNew, GVar.gvOnline);
                label33.Visible = true;
                txtCarNum2.Visible = true;
            }
            else
            {
                ChkWeightAll.ForeColor = Color.Black;
                txtCarNum2.Text = "";
                label33.Visible = false;
                txtCarNum2.Visible = false;
            }
        }

        private void tChkOnline_Tick(object sender, EventArgs e)
        {
            fncCheckONLINE();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            frmAdmin frm = new frmAdmin();
            frm.ShowDialog();
        }

        private string FncChangeEngToThaiCar(string lvTXT)
        {
            string lvReturn = "";

            //เปลี่ยนภาษาไทยเป็น Eng
            //แยกส่วน
            string[] lvArr = lvTXT.Split('.');
            string lvPCar = GsysSQL.fncFindShotEngCar(lvArr[0]);

            //if (lvPCar != "")
            //{
            //    //เช็คช่วงหลังว่ามีภาษาไทยอีกไหม
            //    int lvCount1 = lvArr[1].Length;
            //    string lvTXTChange = "";
            //    string lvTXTOLD = "";
            //    for (int i = 0; i < lvCount1; i++)
            //    {
            //        string lvData = lvArr[1].Substring(i, 1).ToString();
            //        string lvChkData = GsysSQL.fncFindShotEngCar(lvData);

            //        if (lvChkData != "")
            //        {
            //            if (lvTXTChange != "") lvTXTChange += ",";
            //            lvTXTChange += lvChkData;
            //        }
            //        else
            //        {
            //            if (lvTXTChange != "")
            //            {
            //                if (i > 1 && lvArr[1].Substring(i - 1, 1).ToString().All(char.IsLetter))
            //                    lvTXTChange += ",";
            //            }

            //            lvTXTChange += lvData;
            //        }
            //    }

            //    //ตัด Comma ออก
            //    lvTXTChange = lvTXTChange.Replace(",", "");
            //    lvTXTChange = lvTXTChange.Trim();

            //    //ถ้ามีข้อมูล
            //    lvReturn = lvPCar + "." + lvTXTChange;
            //}
            //else
            //{
                //ถ้าไม่มี
                if (lvArr.Count() > 1)
                {
                    int lvCount0 = lvArr[0].Length;
                    string lvTXTChange = "";
                    //แยกส่วนแล้วแปลงทีละตัว
                    for (int i = 0; i < lvArr.Count(); i++)
                    {
                        string lvData = lvArr[i].ToString();
                        string[] lvArrChk = lvArr[i].Split(',');

                        for (int l = 0; l < lvArrChk.Count(); l++)
                        {
                            string lvChkData = GsysSQL.fncFindShotEngCar(lvArrChk[l]);

                            if (lvChkData != "" && lvArr[0] != "NO")
                            {

                                if (i == 1 && !lvTXTChange.Contains(".")) lvTXTChange += ".";
                                lvTXTChange += lvChkData;
                            }
                            else
                            {
                                if (lvArrChk[l] != ",")
                                {
                                    if (i == 1 && !lvTXTChange.Contains(".")) lvTXTChange += ".";
                                    lvTXTChange += lvArrChk[l];
                                }
                            }
                        }
                    }

                    //if (lvTXTChange == "") lvTXTChange = lvArr[0];
                    lvReturn = lvTXTChange;
                }
                else
                {
                    lvReturn = lvArr[0];
                }
            //}

            return lvReturn;
        }

        private void btnSearchQ2_Click(object sender, EventArgs e)
        {
            btnSearchQ.Visible = true;
            btnSearchQ2.Visible = false;
            btnSearchQ.Enabled = true;

            txtQ.Text = "";
            txtQ.ReadOnly = false;
            txtQ.Focus();
        }

        private void chkShowPOM_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtQuota_Leave(object sender, EventArgs e)
        {
            //txtQuotaName.Text = GsysSQL.fncFindQuotaName(txtQuota.Text,GVar.gvOnline);
        }

        private void btnXls_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Convert.ToString(Environment.SpecialFolder.MyDocuments);
            saveFileDialog1.Filter = "Excel File (*.xls)|*.xls";
            saveFileDialog1.FilterIndex = 1;

            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            sp1.SaveExcel(saveFileDialog1.FileName, FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
            MessageBox.Show("บันทึกข้อมูลเรียบร้อย", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void btnNext_Click(object sender, EventArgs e)
        {
            gQueue.Visible = true;
            gWeight.Visible = false;
            ChkShow0.Visible = false;
            ChkShowA.Visible = false;
            ChkShowB.Visible = false;
            chkShowPOM.Visible = false;
            ChkShowIN.Visible = false;
            ChkShowOut.Visible = false;
            chkShowFinish.Visible = false;

            LoadData();
        }

        private void btnPrv_Click(object sender, EventArgs e)
        {
            gQueue.Visible = false;
            gWeight.Visible = true;

            LoadData();
        }

        private string FncCheckSampleNo(string lvSample, string lvRail)
        {
            string lvReturn = "";
            int lvSam = Gstr.fncToInt(lvSample);

            //1000-2999   ราง A
            //3000-4999   ราง B
            //5000-5499   ราง A
            //5500 ราง B

            if (lvSam > 1 && lvSam <= 2999 && !ChkA.Checked)
            {
                lvReturn = "เลขที่ตัวอย่าง " + lvSam + " ไม่ตรงกับ ราง A กรุณาแก้ไขรางใหม่";
            }
            else if (lvSam > 3000 && lvSam <= 4999 && !ChkB.Checked)
            {
                lvReturn = "เลขที่ตัวอย่าง " + lvSam + " ไม่ตรงกับ ราง B กรุณาแก้ไขรางใหม่";
            }
            else if (lvSam > 5000 && lvSam <= 5499 && !ChkA.Checked)
            {
                lvReturn = "เลขที่ตัวอย่าง " + lvSam + " ไม่ตรงกับ ราง A กรุณาแก้ไขรางใหม่";
            }
            else if (lvSam > 5500 && !ChkB.Checked)
            {
                lvReturn = "เลขที่ตัวอย่าง " + lvSam + " ไม่ตรงกับ ราง B กรุณาแก้ไขรางใหม่";
            }

            return lvReturn;
        }

        private void chkChangeTK_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChangeTK.Checked)
            {
                txtTakaoNo.ReadOnly = false;
                txtTakaoNo.Focus();
            }
            else
            {
                txtTakaoNo.ReadOnly = true;
            }
        }

        private void chkChangeBill_CheckedChanged(object sender, EventArgs e)
        {
            if (chkChangeBill.Checked)
            {
                txtBillNo.ReadOnly = false;
                txtBillNo.Focus();
            }
            else
            {
                txtBillNo.ReadOnly = true;
            }
        }

        private void btnDelQueue_Click(object sender, EventArgs e)
        {
            if (lbUser.Text != "jum" && lbUser.Text != "admin")
            {
                MessageBox.Show("คุณไม่มีสิทธิ์ในการใช้งาน ฟังก์ชันนี้ กรุณาติดต่อผู้ดูแล !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (GVar.gvPermitDel != "1")
            {
                MessageBox.Show("คุณไม่มีสิทธิ์ในการใช้งาน ฟังก์ชันนี้ กรุณาติดต่อผู้ดูแล !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (sp1.ActiveSheet.Cells[GVar.gvRowIndex, 9].Text != "รอชั่งเข้า")
            {
                MessageBox.Show("ไม่สามารถลบข้อมูลได้ เนื่องจาก คิวนี้มีการชั่งเข้าแล้ว", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (txtQ.Text == "")
            {
                MessageBox.Show("กรุณาเลือกคิวที่ต้องการลบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string lvQ = txtQ.Text;
            string lvQuota = txtQuota.Text;
            string lvDate = Gstr.fncChangeTDate(lbDate.Text);
            string lvCarNum = txtCarNum.Text;

            //ยืนยัน
            string lvTxtAlert = "ยืนยันการลบข้อมูล คิวที่ " + lvQ + " โควต้า " + lvQuota;
            DialogResult dialogResult = MessageBox.Show(lvTxtAlert, "Confirm?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }

            //ลบ
            string lvSQL = "Delete From Queue_Diary where Q_No = '" + lvQ + "' And Q_CloseStatus = '' And Q_Date = '" + lvDate + "' And Q_CarNum = '" + lvCarNum + "' and Q_Year = '' ";
            string lvResault = GsysSQL.fncExecuteQueryData(lvSQL, true);

            string lvCutdoc = txtCutDoc.Text;
            if (lvResault == "Success")
            {
                //คืนค่าใบนำตัดกรณีหยิบใบนำตัดมาใช้
                if (lvCutdoc != "")
                {
                    lvSQL = "Update Cane_QueueData set C_Queue = '',C_QueueStatus = ''  where C_ID = '" + lvCutdoc + "' ";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, true);
                }

                GsysSQL.fncKeepLogData(GVar.gvUser, "ลบข้อมูลคิว", "ลบข้อมูลคิวที่ " + lvQ + " โควต้า " + lvQuota + " วันที่ " + lvDate + " ทะเบียนรถ " + lvCarNum);
                MessageBox.Show("ลบข้อมูลเรียบร้อย", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("ไม่สามารถลบข้อมูลได้ เนื่องจาก " + lvResault, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string FncCheckJumpBill()
        {
            string lvReturn = "";

            //Get Data
            DataTable DT = new DataTable();
            DataTable DT2 = new DataTable();

            string lvSQL = "SELECT Max(Q_BillingNo) as LastDoc, Min(Q_BillingNo) as StartDoc, (Cast(Max(Q_BillingNo) as int) - Cast(Min(Q_BillingNo) as int) + 1) as Total, Count(Q_BillingNo) as CountBill ";
            lvSQL += "FROM Queue_Diary ";
            lvSQL += "WHERE Q_CloseStatus = '' And Q_BillingNo <> '' And Q_WeightOUT > 0 and Q_Year = '' ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumRow = DT.Rows.Count;

            if (lvNumRow > 0)
            {
                int lvLastDoc = Gstr.fncToInt(DT.Rows[0]["LastDoc"].ToString());
                int lvStartDoc = Gstr.fncToInt(DT.Rows[0]["StartDoc"].ToString());
                int lvTotal = Gstr.fncToInt(DT.Rows[0]["Total"].ToString());
                int lvCountBill = Gstr.fncToInt(DT.Rows[0]["CountBill"].ToString());

                if (lvTotal != lvCountBill)
                {
                    int lvDocBillCheck = lvLastDoc - 100;
                    lvSQL = "SELECT Top 1 B_PK as MissingNumber FROM Queue_BillCheck WHERE B_PK not in "; //ไม่ต้องเอาเลขที่กระโดดก่อนหน้ามา ถ้ามันกระโดดเเล้วให้ผ่านไป
                    lvSQL += "(SELECT top 500 Cast(Q_BillingNo as int) as Bill FROM Queue_Diary t2 WHERE Q_Year = '' order by Cast(Q_BillingNo as int) Desc) ";
                    lvSQL += "and Cast(B_PK as int) > "+ lvDocBillCheck + " order by B_PK";

                    DT2 = GsysSQL.fncGetQueryData(lvSQL, DT2, GVar.gvOnline);

                    int lvNumRow2 = DT2.Rows.Count;

                    if (lvNumRow2 > 0)
                    {
                        lvReturn = Gstr.fncToInt(DT2.Rows[0]["MissingNumber"].ToString()).ToString("000000");
                    }
                    else
                    {
                        lvReturn = "";
                    }
                }
            }

            return lvReturn;
        }

        private void txtQuota_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void btnLockRegister_Click(object sender, EventArgs e)
        {
            if (txtQ.Text == "")
            {
                MessageBox.Show("กรุณาระบุข้อมูลคิวก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (txtCarNum.Text == "")
            {
                MessageBox.Show("กรุณาระบุข้อมูลทะเบียนรถก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (txtQ.Text.Contains("."))
            {
                MessageBox.Show("คิวพ่วง ไม่สามารถลงทะเบียนคิวล็อกได้ ต้องเป็นคิวแม่เท่านั้น", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            pnShowQLock.Visible = false;
            //ClearData
            txtQLoop.Text = "";
            txtQLockNo.Text = "";
            txtQLockQNo.Text = "";
            txtQLockCarNum.Text = "";

            txtQLockNo.Enabled = true;
            txtQLockQNo.Enabled = true;
            txtQLockSearch.Enabled = true;

            //หารอบปัจจุบัน
            txtQLoop.Text = GsysSQL.fncFindLastLoop();
            txtQLockCarNum.Text = txtCarNum.Text;

            //หาประวัติการใช้คิวล็อก


            txtQLockSearch.Text = "";
            txtQLockSearch.Focus();
        }

        private void LoadDataQLock()
        {
            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "";
            //lvQLockCar = lvQLockCar.Replace('a','.');

            //if (lvQLockNo != "")
                lvSQL = "Select TOP 50 * from Queue_Diary inner join Queue_LockDiary on L_QMain = Q_No Where Q_Year = '' order by L_PK DESC"; //And Q_LockQNo <> ''
            //else if (lvQLockCar != "")
            //    lvSQL = "Select * from Queue_Diary inner join Queue_LockDiary on L_QMain = Q_No ";//And Q_LockQNo <> ''

            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumRow = DT.Rows.Count;
            sp2.ActiveSheet.RowCount = lvNumRow;

            for (int i = 0; i < lvNumRow; i++)
            {
                sp2.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["Q_Lockloop"].ToString();
                sp2.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["Q_LockNo"].ToString();
                sp2.ActiveSheet.Cells[i, 2].Text = DT.Rows[i]["Q_LockQNo"].ToString();
                sp2.ActiveSheet.Cells[i, 3].Text = DT.Rows[i]["Q_No"].ToString();
                sp2.ActiveSheet.Cells[i, 4].Text = DT.Rows[i]["Q_Quota"].ToString();
                sp2.ActiveSheet.Cells[i, 5].Text = DT.Rows[i]["Q_CarNum"].ToString().Replace('a','.');
                sp2.ActiveSheet.Cells[i, 6].Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_Date"].ToString());
                sp2.ActiveSheet.Cells[i, 7].Text = DT.Rows[i]["Q_Time"].ToString();
                sp2.ActiveSheet.Cells[i, 8].Text = DT.Rows[i]["Q_CarType"].ToString();
                sp2.ActiveSheet.Cells[i, 9].Text = GsysSQL.fncFindCaneTypeName(DT.Rows[i]["Q_CaneType"].ToString(), GVar.gvOnline);
                sp2.ActiveSheet.Cells[i, 10].Text = DT.Rows[i]["Q_BillingNo"].ToString();
                sp2.ActiveSheet.Cells[i, 11].Text = ""; //"รอบ " + DT.Rows[i]["L_Loop"].ToString() + " ล๊อก " + DT.Rows[i]["L_LockNo"].ToString() + " คิวที่ " + DT.Rows[i]["L_LockQNo"].ToString();
                sp2.ActiveSheet.Cells[i, 12].Text = "0" + DT.Rows[i]["L_PK"].ToString();

                string lvUseQ = GsysSQL.fncFindLockQSub(DT.Rows[i]["L_PK"].ToString());
                if (lvUseQ != "")
                {
                    sp2.ActiveSheet.Cells[i, 13].Text = "ใช้ไปงานแล้ว";
                    sp2.ActiveSheet.Rows[i].BackColor = Color.LightPink;
                }
                else
                {
                    sp2.ActiveSheet.Cells[i, 13].Text = "สามารถใช้คิวนี้ได้";
                    sp2.ActiveSheet.Rows[i].BackColor = Color.LightGreen;
                }

                if (DT.Rows[i]["Q_Lockloop"].ToString() == "0")
                {
                    sp2.ActiveSheet.AddSpanCell(i, 0, 1, 3);
                    sp2.ActiveSheet.Cells[i, 0].Text = "รอบ " + DT.Rows[i]["L_Loop"].ToString() + " ล๊อก " + DT.Rows[i]["L_LockNo"].ToString() + " คิวที่ " + DT.Rows[i]["L_LockQNo"].ToString();
                }
                else
                {
                    sp2.ActiveSheet.AddSpanCell(i, 0, 1, 1);
                }
            }
        }

        private void btnQLockSetting_Click(object sender, EventArgs e)
        {
            frmQLock frm = new frmQLock();
            frm.ShowDialog();

            btnClear.PerformClick();
        }

        private void txtCarNum_Leave(object sender, EventArgs e)
        {
            try
            {
                txtCarNum.Text = txtCarNum.Text.Replace('a','.');

                //ค้นหาข้อมูลคิวล็อก
                string lvChk = GsysSQL.fncCheckQLockByCarNum(txtCarNum.Text, "", "");

                if (lvChk != ",,")
                {
                    btnShowRegisCar.Visible = true;

                    LoadDataQLock();
                }
                else
                {
                    sp2.ActiveSheet.RowCount = 0;
                    btnShowRegisCar.Visible = false;
                }
            }
            catch
            {
                btnShowRegisCar.Visible = false;
            }
        }

        private void FncCheckQLockStatus()
        {
            string lvOnOff = GVar.gvQLockOnOff;
            if (lvOnOff == "OFF") return;

            string lvChk = GsysSQL.fncCheckQLockByCarNum(txtCarNum.Text, "", "");

            if (lvChk != "")
            {
                pnShowQLock.Visible = false;
                btnShowRegisCar.Visible = true;

                //ClearData
                txtQLoop.Text = "";
                txtQLockNo.Text = "";
                txtQLockQNo.Text = "";
                txtQLockCarNum.Text = "";

                //หารอบปัจจุบัน
                txtQLoop.Text = GsysSQL.fncFindLastLoop();
                txtQLockCarNum.Text = txtCarNum.Text;

                txtQLockSearch.Text = "";
                txtQLockSearch.Enabled = true;
                txtQLockNo.Enabled = false;
                txtQLockQNo.Enabled = false;

                string[] lvArr = lvChk.Split(',');
                txtQLockNo.Text = lvArr[0];
                txtQLockQNo.Text = lvArr[1];

                LoadDataQLock();
            }
            else
            {
                btnShowRegisCar.Visible = false;
                pnShowQLock.Visible = true;
                txtQLockSearch.Enabled = true;
                txtQLockNo.Enabled = true;
                txtQLockQNo.Enabled = true;
            }
        }

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }

        private void txtQLockSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string lvSearch = txtQLockSearch.Text; //ยิงคิวล็อค
                string lvChkBar = GsysSQL.fncCheckQLockByCarNum("", txtQLockSearch.Text, ""); //เช็คทะเบียนรถ
                string lvLoop = txtQLoop.Text; //รอบปัจจุบัน

                if (lvSearch.Length == 7) lvSearch = "0" + lvSearch;
                if (lvSearch.Length == 9) lvSearch = Gstr.Right(lvSearch,8);

                //เช็ครูปแบบ Barcode ให้เท่ากับ 1 และต้องเป็นตัวเลข
                if (lvSearch.Length > 4 || IsNumeric(lvSearch) == false)
                {
                    MessageBox.Show("รูปแบบ BarCode ไม่ถูกต้อง กรุณาตรวจสอบ","แจ้งเตือน",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    txtQLockSearch.Text = "";
                    return;
                }

                //else if (lvChkBar == "" || lvChkBar == ",,")
                //{
                //    MessageBox.Show("ไม่พบข้อมูลคิวล็อกนี้ในระบบ กรุณาตรวจสอบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    txtQLockSearch.Text = "";
                //    txtQLoop.Text = "";
                //    txtQLockNo.Text = "";
                //    txtQLockQNo.Text = "";
                //    return;
                //}

                //เช็คคิวล็อคว่ามีหรือไม่
                string lvQLockLoop = GsysSQL.fncCheckQLockLoop(lvSearch); //เช็คข้อมูลจากตารางQLock (L_Loop)
                string L_NowLoop = GsysSQL.fncCheckQLockLoopNow(); //รอบ Active ปัจจุบัน
                string L_NowLock = GsysSQL.fncCheckQLockNoNow(); //ล็อค Active ปัจจุบัน

                int L_Qstart = Gstr.fncToInt(GsysSQL.fncCheckQStart(L_NowLock)); //ล็อคเริ่มปัจจุบัน
                int L_Qend = Gstr.fncToInt(GsysSQL.fncCheckQEnd(L_NowLock)); //ล็อคสุดท้ายปัจจุบัน

                //ถ้าคิวล็อคนี้ยังไม่มีข้อมูล
                if (lvQLockLoop == "")
                {
                    //ถ้ามาไม่ตรงกับรอบที่ 1 
                    if(L_NowLoop != "1" && chkQLate.Checked != true)
                    {
                        MessageBox.Show("คิวล็อคนี้มาไม่ตรงรอบที่ประกาศ กรุณาไปรอลาน 5", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnClear_Click(sender, e);
                        return;
                    }

                    //ถ้าไม่ตรงล็อคที่ประกาศ
                    if(Gstr.fncToInt(lvSearch) < L_Qstart && chkQLate.Checked != true || Gstr.fncToInt(lvSearch) > L_Qend && chkQLate.Checked != true)
                    {
                        MessageBox.Show("คิวล็อคนี้มาไม่ตรงคิวที่ประกาศ กรุณาไปรอลาน 5", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnClear_Click(sender, e);
                        return;
                    }

                    DataTable DT = new DataTable();
                    string lvSQL = "Select * From Queue_LockActive Where L_Lock = '" + txtQLockSearch.Text + "'";
                    DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

                    int L_Loop = 0; //รอบที่
                    int L_LockNo = 0; //ล็อคที่
                    int L_LockQNo = 0; //คิวที่

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        L_Loop = Gstr.fncToInt(txtQLoop.Text); //รอบที่
                        L_LockNo = Gstr.fncToInt(DT.Rows[i]["L_Lock"].ToString()); //ล็อคที่
                        L_LockQNo = Gstr.fncToInt(DT.Rows[i]["L_LockQNo"].ToString()); //คิวที่
                    }
                    
                    string L_LockCarNum = txtCarNum.Text; //เลขทะเบียนรถ
                    if (txtCarNum2.Text != "") L_LockCarNum += " " + txtCarNum2.Text; //เลขที่ทะเบียนรถพ่วง
                    string L_LockDate = Gstr.fncChangeTDate(DateTime.Now.ToString("dd/MM/yyyy")); //วันที่
                    string L_LockTime = DateTime.Now.ToString("HH:mm:ss"); //เวลา
                    string L_QMain = txtQ.Text; //คิวที่

                    lvSQL = "Insert Into Queue_LockDiary (L_Loop, L_LockQNo, L_LockNo, L_LockCarNum, L_LockDate, L_LockTime, L_QMain) " +
                        "Values ('" + L_Loop + "', '" + L_LockNo + "', '" + L_LockQNo + "', '" + L_LockCarNum + "', '" + L_LockDate + "', '" + L_LockTime + "', '" + L_QMain + "')";
                    string lvResult = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                }
                //ถ้ามีข้อมูลคิวล็อคแล้ว
                else
                {
                    int lvQlockLoopPlus = Gstr.fncToInt(lvQLockLoop) + 1; //เอารอบเก่ามา + 1

                    //ถ้ามาไม่ตรงกับรอบที่ประกาศ
                    if (L_NowLoop != lvQlockLoopPlus.ToString() && chkQLate.Checked != true)
                    {
                        MessageBox.Show("คิวล็อคนี้มาไม่ตรงรอบที่ประกาศ กรุณาไปรอลาน 5", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnClear_Click(sender, e);
                        return;
                    }

                    //ถ้ามาไม่ตรงกับล็อคที่ประกาศ
                    if (Gstr.fncToInt(lvSearch) < L_Qstart && chkQLate.Checked != true || Gstr.fncToInt(lvSearch) > L_Qend && chkQLate.Checked != true)
                    {
                        MessageBox.Show("คิวล็อคนี้มาไม่ตรงคิวที่ประกาศ กรุณาไปรอลาน 5", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnClear_Click(sender, e);
                        return;
                    }
                    
                    DataTable DT = new DataTable();
                    string lvSQL = "Select * From Queue_LockActive Where L_Lock = '" + txtQLockSearch.Text + "'";
                    DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

                    int L_Loop = 0; //รอบที่
                    int L_LockNo = 0; //ล็อคที่
                    int L_LockQNo = 0; //คิวที่

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        L_Loop = Gstr.fncToInt(txtQLoop.Text); //รอบที่
                        L_LockNo = Gstr.fncToInt(DT.Rows[i]["L_Lock"].ToString()); //ล็อคที่
                        L_LockQNo = Gstr.fncToInt(DT.Rows[i]["L_LockQNo"].ToString()); //คิวที่
                    }

                    string L_LockCarNum = txtCarNum.Text; //เลขทะเบียนรถ
                    if (txtCarNum2.Text != "") L_LockCarNum += " " + txtCarNum2.Text; //เลขที่ทะเบียนรถพ่วง
                    string L_LockDate = Gstr.fncChangeTDate(DateTime.Now.ToString("dd/MM/yyyy")); //วันที่
                    string L_LockTime = DateTime.Now.ToString("HH:mm:ss"); //เวลา
                    string L_QMain = txtQ.Text; //คิวที่

                    lvSQL = "Insert Into Queue_LockDiary (L_Loop, L_LockQNo, L_LockNo, L_LockCarNum, L_LockDate, L_LockTime, L_QMain) " +
                        "Values ('" + L_Loop + "', '" + L_LockNo + "', '" + L_LockQNo + "', '" + L_LockCarNum + "', '" + L_LockDate + "', '" + L_LockTime + "', '" + L_QMain + "')";
                    string lvResult = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
                }

                MessageBox.Show("ลงทะเบียนคิวล็อคสำเร็จ กรุณากดบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                #region //คิวล็อคเก่า
                //GVar.gvLockBar = lvSearch;
                //string lvQLock = Gstr.Right(lvSearch, 4);

                //string lvPK = Gstr.fncToInt(GVar.gvLockBar).ToString();
                //string lvChk = GsysSQL.fncCheckQLockByCarNum("", lvPK, "");
                //string lvChkCar = GsysSQL.fncCheckQLockByCarNum(txtCarNum.Text, "", "");
                //try
                //{
                //    if (lvChk != "")
                //    {
                //        string[] lvArr = lvChk.Split(',');
                //        string[] lvArrCar = lvChkCar.Split(',');

                //        string lvChkOptionCarRegis = GsysSQL.fncFindChkCarRegister();

                //        if (lvArr[2] != txtCarNum.Text && lvChkOptionCarRegis == "1")
                //        {
                //            MessageBox.Show("ทะเบียนรถ ต้องตรงกับที่ลงทะเบียน คิวล็อก ไว้เท่านั้น" + Environment.NewLine + "คิวล็อกที่ " + lvArr[1] + " ทะเบียนรถที่ลงทะเบียนไว้ " + lvArr[2], "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            txtQLockSearch.Text = "";
                //            txtQLockNo.Text = "";
                //            txtQLockQNo.Text = "";
                //            pnShowQLock.Visible = true;

                //            GVar.gvLockBar = "";
                //        }
                //        else if (lvArr[3] != "" && lvArr[4] != "" && lvArr[5] != "") //ถ้าคิวนี้มีการโดนใช้งานไปแล้ว แจ้งเตือนใช้เเล้ว วันที่ ทะเบียน เวลา
                //        {
                //            MessageBox.Show("ใบคิวนี้ถูกใช้ไปแล้ว" + Environment.NewLine + Environment.NewLine + "ทะเบียนที่ใชไป " + lvArr[3] + " วันที่ " + Gstr.fncChangeSDate(lvArr[4]) + " เวลา " + lvArr[5] + Environment.NewLine + Environment.NewLine + "ไม่สามารถนำมาใช้ซ้ำได้ " + Environment.NewLine + Environment.NewLine + "ให้รับคิวเสรีตามปกติ แล้วแจ้งให้รถไปรอที่ ลาน 5 ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            txtQLockSearch.Text = "";
                //            txtQLockNo.Text = "";
                //            txtQLockQNo.Text = "";
                //            pnShowQLock.Visible = true;

                //            GVar.gvLockBar = "";
                //        }
                //        else if (lvArr[2] != "" && lvArr[2] != txtCarNum.Text && lvArrCar[2] != "" && lvChkOptionCarRegis == "0")//ถ้าไม่เท่ากับช่องว่าง แสดงว่าเป็น คิวที่ผูกกับทะเบียนแล้ว สามารถใช้คิวคนอื่นได้ Pass  
                //        {
                //            txtQLoop.Text = Gstr.fncToInt(Gstr.Left(GVar.gvLockBar, 2)).ToString();
                //            txtQLockNo.Text = lvArr[0];
                //            txtQLockQNo.Text = lvArr[1];
                //            txtQLockCarNum.Text = lvArr[2];

                //            //LoadDataQLock(lvArr[1]);

                //            txtQLockSearch.Text = "";
                //            txtQLockSearch.Focus();
                //        }
                //        else if (lvArr[2] != "" && lvArr[2] != txtCarNum.Text && lvArrCar[2] == "" && lvChkOptionCarRegis == "0")//ถ้าตัวเองไม่ได้ลงทะเบียน แต่ยืมคนอื่นมาใช้
                //        {
                //            txtQLoop.Text = Gstr.fncToInt(Gstr.Left(GVar.gvLockBar, 2)).ToString();
                //            txtQLockNo.Text = lvArr[0];
                //            txtQLockQNo.Text = lvArr[1];
                //            txtQLockCarNum.Text = lvArr[2];

                //            //LoadDataQLock(lvArr[1]);

                //            txtQLockSearch.Text = "";
                //            txtQLockSearch.Focus();
                //        }
                //        else if (lvArr[2] == txtCarNum.Text)//ถ้าเป็นคิวตัวเอง Pass
                //        {
                //            txtQLoop.Text = Gstr.fncToInt(Gstr.Left(GVar.gvLockBar, 2)).ToString();
                //            txtQLockNo.Text = lvArr[0];
                //            txtQLockQNo.Text = lvArr[1];
                //            txtQLockCarNum.Text = txtCarNum.Text;

                //            txtQLockSearch.Text = "";
                //            txtQLockSearch.Focus();
                //        }
                //        else if (lvArr[2] == "" && lvArrCar[2] != "" && lvChkOptionCarRegis == "1")// ถ้าเจอคิวว่าง เช็คว่าตัวเองลงทะเบียนแล้วหรือยัง No
                //        {
                //            MessageBox.Show("ทะเบียนรถ " + txtCarNum.Text + " นี้ ได้ลงทะเบียนกับ คิวที่ " + lvArrCar[1] + " อยู่แล้ว", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            txtQLockSearch.Text = "";
                //            txtQLockNo.Text = "";
                //            txtQLockQNo.Text = "";
                //            pnShowQLock.Visible = true;

                //            GVar.gvLockBar = "";
                //        }
                //        else if (lvArrCar[2] == "") //ถ้าไม่มีข้อมูล Pass เพื่อลงทะเบียน && lvChkCar == ""
                //        {
                //            txtQLoop.Text = Gstr.fncToInt(Gstr.Left(GVar.gvLockBar, 2)).ToString();
                //            txtQLockNo.Text = lvArr[0];
                //            txtQLockQNo.Text = lvArr[1];
                //            txtQLockCarNum.Text = txtCarNum.Text;

                //            txtQLockSearch.Text = "";
                //            txtQLockSearch.Focus();
                //        }

                //        txtQLockNo.Enabled = false;
                //        txtQLockQNo.Enabled = false;

                //        txtQLockCarNum.Text = txtQLockCarNum.Text.Replace('a','.');
                //        //LoadDataQLock(txtQLockQNo.Text);

                //    }
                //    else
                //    {
                //        MessageBox.Show("ไม่พบข้อมูลคิวล็อกนี้ กรุณาตรวจสอบใหม่", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        txtQLockSearch.Text = "";
                //        txtQLockNo.Text = "";
                //        txtQLockQNo.Text = "";
                //        txtQLockSearch.Focus();
                //        return;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("รูปแบบ BarCode ไม่ถูกต้อง กรุณาตรวจสอบ", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    txtQLockSearch.Text = "";
                //    return;
                //}
                #endregion
            }
        }

        private void btnClearQLock_Click(object sender, EventArgs e)
        {
            string lvLockLoop = txtQLoop.Text; //รอบ 
            string lvLockNo = txtQLockNo.Text; //ล็อก
            string lvLockQNo = txtQLockQNo.Text; //คิวล็อก

            //ยืนยัน
            string lvTxtAlert = "ยืนยันการ Clear ข้อมูลคิวล็อก?";
            DialogResult dialogResult = MessageBox.Show(lvTxtAlert, "Confirm?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }

            if (lvLockQNo != "" && !pnShowQLock.Visible)
            {
                //Update ตารางหลัก
                string lvSQL = "Update Queue_Diary set Q_LockNo = '',Q_LockQNo = '',Q_Lockloop = '' where Q_No = '" + txtQ.Text + "' and Q_Year = '' ";
                string lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                //ตาราง Active 
                lvSQL = "Update Queue_LockActive set L_CarNum = '',L_DateRegister = '' where L_LockQNo = '" + lvLockQNo + "' ";
                lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                ////ตาราง Diary 
                //lvSQL = "Delete From Queue_LockDiary Where L_LockQNo = '" + lvLockQNo + "' ";
                //lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);
            }

            pnShowQLock.Visible = true;

            MessageBox.Show("Clear ข้อมูลคิวล็อกที่ " + lvLockQNo + " เรียบร้อย","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);

            string lvTXT = "Clear ข้อมูลคิวล็อกที่ " + lvLockQNo + " เลขที่คิว " + txtQ.Text + " ทะเบียนรถ " + txtCarNum.Text;
            GsysSQL.fncKeepLogData(GVar.gvUser, "โปรแกรมคิว", lvTXT);
        }

        private void btnSearchQuota_Click(object sender, EventArgs e)
        {
            GVar.gvSCode = "";
            GVar.gvSName = "";
            frmQuotaSearch frm = new frmQuotaSearch();
            frm.ShowDialog();

            if (GVar.gvSCode != "")
            {
                txtQuota.Text = GVar.gvSCode;
                txtQuotaName.Text = GVar.gvSName;
                txtCarNum.Focus();
            }
            else
            {
                txtQuota.Focus();
            }
        }

        private void sp2_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GVar.gvRow = e.Row;
            sp2.ActiveSheet.SetActiveCell(e.Row,e.Column);

            if (e.Button == MouseButtons.Right)
            {
                //if (GVar.gvUserType == "head")
                //    พมพใบควลอกToolStripMenuItem.Enabled = true;
                //else
                //    พมพใบควลอกToolStripMenuItem.Enabled = false;

                CRightMenuLock.Show(Cursor.Position);
            }
        }

        private void ใชควลอกนToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string lvPK = sp2.ActiveSheet.Cells[GVar.gvRow, 12].Text;

            //คลิ๊กปุ่ม คิวล็อก
            btnLockRegister_Click(sender, e);

            txtQLockSearch.Text = lvPK;
            tabControl2.SelectedIndex = 0;
            txtQLockSearch.Focus();
            SendKeys.Send("{Enter}");
        }

        private void พมพใบควลอกToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GVar.gvPermitPrint != "1")
            {
                MessageBox.Show("คุณไม่มีสิทธิ์ในการใช้งาน ฟังก์ชันนี้ กรุณาติดต่อผู้ดูแล !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            GVar.gvLockBar = sp2.ActiveSheet.Cells[GVar.gvRow,12].Text;
            string lvDate = sp2.ActiveSheet.Cells[GVar.gvRow, 6].Text + " " + sp2.ActiveSheet.Cells[GVar.gvRow, 7].Text;
            string lvCarNum = sp2.ActiveSheet.Cells[GVar.gvRow, 5].Text;

            FncPrintQLock(lvDate, lvCarNum, "");
        }

        private void lineNotify(string msg)
        {
            //string token = "Wuh3ocetjly3TUZgd9OV0YY3o9g6GouD3wPaIsthAna"; //ห้องคอม
            string token = "udbt8VL6gdsiycFhNA5tCZgBfI2u6JnlUFBJOoG28Bg"; //ส่วนตัว

            try
            {
                var request = (HttpWebRequest)WebRequest.Create("https://notify-api.line.me/api/notify");
                var postData = string.Format("message={0}", msg);
                var data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add("Authorization", "Bearer " + token);

                using (var stream = request.GetRequestStream()) stream.Write(data, 0, data.Length);
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void groupBox21_Enter(object sender, EventArgs e)
        {

        }

        public string fncCheckCutcar(string lvCutCar)
        {
            string str = lvCutCar;
            string lvReturn = "";

            if (str.Length == 0)
            {
                lvReturn = "กรุณากรอกเลขรถตัด";
            }

            else
            {
                string[] lvCutCarSplit = lvCutCar.Split('.');

                if (IsNumeric(lvCutCar))
                {
                    lvCutCar = "NO." + lvCutCar;
                    lvCutCarSplit = lvCutCar.Split('.');

                    if (lvCutCarSplit[1].Length == 1)
                    {
                        string lvNew = "0" + lvCutCarSplit[1];
                        lvCutCar = lvCutCarSplit[0] + "." + lvNew;
                    }
                }

                else if (lvCutCarSplit[1].Length == 1)
                {
                    string lvNew = "0" + lvCutCarSplit[1];
                    lvCutCar = lvCutCarSplit[0] + "." + lvNew;
                }

            lvReturn = lvCutCar.ToUpper();

            }
            return lvReturn;
        }

        private void label46_Click(object sender, EventArgs e)
        {

        }

        private void txtCutCar2_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCutCar2.Text != "")
            {
                string lvCutCar = txtCutCar2.Text;
                if (e.KeyCode == Keys.Enter)
                {
                    string lvCheckCar = txtCutCar2.Text;

                    string[] lvCutcarCheck = { "NO.01", "NO.02", "NO.03", "NO.04", "NO.05", "NO.06", "NO.07", "NO.08", "NO.09", "NO.10", "NO.11", "NO.12", "NO.13",
                    "NO.14", "NO.15", "NO.16", "NO.17", "NO.18", "NO.19", "NO.20", "NO.21", "NO.22", "NO.23", "NO.24", "NO.25", "NO.26", "NO.27", "NO.28", "NO.29",
                    "NO.30", "NO.31", "NO.32", "NO.33", "NO.34", "NO.35", "NO.36", "NO.37", "NO.38"};

                    foreach (string x in lvCutcarCheck)
                    {
                        if (!lvCheckCar.Contains(x))
                        {
                            txtCutCar2.Text = "";
                        }
                        else
                        {
                            txtCutCar2.Text = lvCheckCar;
                            break;
                        }
                    }

                    if (txtCutCar2.Text == "")
                    {
                        MessageBox.Show("เลขรถตัดไม่ตรงกับที่ลงทะเบียนไว้ กรุณาเลือกรถตัดที่ลงทะเบียนไว้", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtCutCar2.Focus();
                        GVar.gvCutcarNo = "";

                        frmCutCarSearch frm = new frmCutCarSearch();
                        frm.ShowDialog();

                        if (GVar.gvCutcarNo != "")
                        {
                            txtCutCar2.Text = GVar.gvCutcarNo;
                        }
                        else
                        {
                            txtCutCar2.Focus();
                        }
                    }
                    else
                    {
                        txtCutDoc.Focus();
                    }
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtCutCar_DoubleClick(object sender, EventArgs e)
        {
            //    GVar.gvSCode = "";
            //    GVar.gvSName = "";
            //    frmQuotaSearch frm = new frmQuotaSearch();
            //    frm.ShowDialog();

            //    if (GVar.gvSCode != "")
            //    {
            //        txtQuota.Text = GVar.gvSCode;
            //        txtQuotaName.Text = GVar.gvSName;
            //        txtCarNum.Focus();
            //    }
            //    else
            //    {
            //        txtQuota.Focus();
            //    }
            GVar.gvCutcarNo = "";

            frmCutCarSearch frm = new frmCutCarSearch();
            frm.ShowDialog();

            if(GVar.gvCutcarNo != "")
            {
                txtCutCar.Text = GVar.gvCutcarNo;
            }
            else
            {
                txtCutCar.Focus();
            }
        }

        private void txtCutCar2_DoubleClick(object sender, EventArgs e)
        {
            GVar.gvCutcarNo = "";

            frmCutCarSearch frm = new frmCutCarSearch();
            frm.ShowDialog();

            if (GVar.gvCutcarNo != "")
            {
                txtCutCar2.Text = GVar.gvCutcarNo;
            }
            else
            {
                txtCutCar2.Focus();
            }
        }

        private void chkCar6_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCar6.Checked)
            {
                chkCar6.ForeColor = Color.Red;
                chkCar1.Checked = false;
                chkCar2.Checked = false;
                chkCar3.Checked = false;
                chkCar5.Checked = false;
                chkCar4.Checked = false;
                chkCar7.Checked = false;
                lbCarName.Text = chkCar6.Text;
                txtCarryPrice.Focus();
            }
            else
            {
                chkCar6.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void chkCar7_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCar7.Checked)
            {
                chkCar7.ForeColor = Color.Red;
                chkCar1.Checked = false;
                chkCar2.Checked = false;
                chkCar3.Checked = false;
                chkCar5.Checked = false;
                chkCar4.Checked = false;
                chkCar6.Checked = false;
                lbCarName.Text = chkCar7.Text;
                txtCarryPrice.Focus();
            }
            else
            {
                chkCar7.ForeColor = Color.Black;
            }

            //ส่งข้อมูลไป Monitor
            fncRefreshDataMonitor();
        }

        private void label55_Click(object sender, EventArgs e)
        {

        }

        private void btnSearhCar_Click(object sender, EventArgs e)
        {
            //ถ้าเป็นโหมดว่างก็ให้กด เพิ่มคิวไปด้วย
            if (pvmode == "" && txtQ.Text == "")
            {
                btnSearchQ_Click(sender, e);
            }

            label48.Visible = false;

            if (GVar.gvSCode == ""  && GVar.gvTypeProgram == "Q")
            {
                frmSearchCar frm = new frmSearchCar();
                frm.ShowDialog();

                txtCutDoc.Text = GVar.gvSCode;

                //โหลดข้อมูลใบนำตัด
                LoadCutDocData();

                //ส่งข้อมูลไป Monitor
                fncRefreshDataMonitor();

                GVar.gvSCode = "";
            }

            //ถ้าเป็น Type Weight ให้ Focus Tab น้ำหนักเลยหลังกดหาคิว
            if (GVar.gvTypeProgram == "W")
            {
                frmCarBrowse frm = new frmCarBrowse();
                frm.ShowDialog();
                string lvQ = GVar.gvSCode;
                
                tabControl2.SelectedIndex = 1;

                if(GVar.gvSCode != "")
                {
                    txtCutDoc.Text = GsysSQL.fncGetCutdoc(txtQ.Text);
                    if (!txtCutDoc.Text.Contains('/'))
                    {
                        LoadCutDocData();
                    }
                    else
                    {
                        DataTable DT = new DataTable();
                        string SQL = "Select * From  Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_Year = '' ";
                        DT = GsysSQL.fncGetQueryData(SQL, DT, true);

                        for (int i = 0; i < DT.Rows.Count; i++)
                        {
                            txtCutDoc.Text = DT.Rows[i]["Q_CutDoc"].ToString();
                            txtCutCar.Text = DT.Rows[i]["Q_CutCar"].ToString();
                            string CutPrice = DT.Rows[i]["Q_CutPrice"].ToString();
                            string CarryPrice = DT.Rows[i]["Q_CarryPrice"].ToString();
                            if (CarryPrice != "0" || CarryPrice != "")
                            {
                                txtCarryPrice.Text = CarryPrice;
                                chkCarryPrice.Checked = true;
                            }
                            if (CutPrice != "0" || CutPrice != "")
                            {
                                txtCutPrice.Text = CutPrice;
                            }
                        }
                    }

                    LoadQueueData(lvQ, GVar.gvStation, true);

                    txtQuota.Text = GVar.gvQuota;
                    txtQuotaName.Text = GsysSQL.fncFindQuotaName(GVar.gvQuota, GVar.gvOnline);

                    GVar.gvSCode = "";
                }
                else
                {

                }
                
                //frm.LoadData("");
            }

            if(GVar.gvCovidsound == "1")
            {
                label48.Visible = true;
                label48.Text = "ผู้สุ่มเสี่ยงสูง";
            }


        }

        private void LoadCutDocData()
        {
            //ล้างข้อมูลใบนำตัดก่อน
            txtCaneDoc.Text = "";
            //txtCutDoc.Text = "";
            txtCutOwner.Text = "";
            txtCutCar.Text = "";
            txtCutPrice.Text = "";
            txtKeepOwner.Text = "";
            txtKeepPrice.Text = "";
            txtCarOwner.Text = "";
            txtCarNumPlate.Text = "";
            txtCarNumPlate2.Text = "";
            txtCarryPrice.Text = "";
            chkCarryPrice.Checked = false;
            txtContractAll.Text = "";
            txtContractAllPrice.Text = "";

            string lvQ = txtQ.Text;
            string lvName = "";

            DataTable DT = new DataTable();
            var lvSQL = "Select * From Cane_QueueData Where C_ID = '" + txtCutDoc.Text + "' ";

            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumrow = DT.Rows.Count;

            for (int i = 0; i < lvNumrow; i++)
            {
                if(DT.Rows[i]["C_PlanId"].ToString() != "")
                {
                    txtCaneDoc.Text = DT.Rows[i]["C_PlanId"].ToString() + " : " + DT.Rows[i]["C_PlanName"].ToString(); //เลขที่แปลง
                }
               
                if (DT.Rows[i]["C_CutContactorId"].ToString() != "")
                {
                    lvName = GsysSQL.fncGetOwnerName(DT.Rows[i]["C_CutContactorId"].ToString());
                    txtCutOwner.Text = DT.Rows[i]["C_CutContactorId"].ToString() + " : " + lvName; //รหัสผู้รับเหมาตัด

                    lvName = GsysSQL.fncGetCarCutName(DT.Rows[i]["C_CarcutNumber"].ToString());
                    if (DT.Rows[i]["C_CarcutNumber"].ToString() != "" && chkCane7.Checked == true || DT.Rows[i]["C_CarcutNumber"].ToString() != "" && chkCane13.Checked == true)
                    {
                        txtCutCar.Text = DT.Rows[i]["C_CarcutNumber"].ToString() + " : " + lvName; //เลขรถตัด
                    }
                    else
                    {

                    }
                    
                    txtCutPrice.Text = DT.Rows[i]["C_Price"].ToString(); //ค่าตัด
                }

                if (DT.Rows[i]["C_KeebContractorId"].ToString() != "")
                {
                    lvName = GsysSQL.fncGetOwnerName(DT.Rows[i]["C_KeebContractorId"].ToString());
                    txtKeepOwner.Text = DT.Rows[i]["C_KeebContractorId"].ToString() + " : " + lvName; //รหัสผู้รับเหมาคีบ
                    txtKeepPrice.Text = DT.Rows[i]["C_KeebContractorPrice"].ToString(); //ราคาคีบ
                }

                if (DT.Rows[i]["C_ContractorId"].ToString() != "")
                {
                    lvName = GsysSQL.fncGetOwnerName(DT.Rows[i]["C_ContractorId"].ToString());
                    txtCarOwner.Text = DT.Rows[i]["C_ContractorId"].ToString() + " : " + lvName; //รหัสผู้รับเหมารถบรรทุก
                    txtCarNumPlate.Text = DT.Rows[i]["C_BoxTruckId"].ToString(); //ทะเบียนรถแม่
                    txtCarNumPlate2.Text = DT.Rows[i]["C_TruckCarnum2"].ToString(); //ทะเบียนรถลูก
                    txtCarryPrice.Text = DT.Rows[i]["C_TruckPrice"].ToString(); //ค่าบรรทุก
                }

                if (txtCarryPrice.Text != "0" && pvmode != "Edit" || txtCarryPrice.Text != "" && pvmode != "Edit")
                {
                    chkCarryPrice.Checked = true;
                }
               


                if (DT.Rows[i]["C_PayStatus"].ToString() == "ชำระ") //สถานะเรียกเก็บรถบรรทุก
                {
                    chkCarryPrice.Checked = true;
                }
                else
                {
                    chkCarryPrice.Checked = false;
                }

                txtQuota.Text = DT.Rows[i]["C_Quota"].ToString(); //โควต้า
                GVar.gvQuota = txtQuota.Text; //ส่งค้าไปใช้หน้าอื่นๆ
                txtQuotaName.Text = DT.Rows[i]["C_FullName"].ToString(); //ชื่อ

                //ตั้งค่า
                /*if (DT.Rows[i]["C_TruckCarnum2"].ToString() != "")
                {
                    ChkWeightAll.Checked = true;
                }
                else
                {
                    ChkWeightAll.Checked = false;
                    txtCarNumPlate2.Text = "";
                }*/

                //ส่งค่าทะเบียนรถไปโซนหลัก
                if (!txtQ.Text.Contains("."))
                {
                    //ถ้าไม่ใช่คิวลูกให้แสดงทะเบียนตามใบนำตัด
                    txtCarNum.Text = DT.Rows[i]["C_TruckCarNum"].ToString(); //แม่
                  

                    //ถ้าไม่มีข้อมูลผู้รับเหมาบรรทุกไม่ต้องแสดงข้อมูลช่องนี้
                    if (txtCarOwner.Text == "")
                    {
                        txtCarNumPlate.Text = "";
                        txtCarNumPlate2.Text = "";
                    }
                }
                else
                {
                    //ถ้าเป็นคิวลูกให้แสดงทะเบียนลูกเป็นหลัก
                    txtCarNum2.Text = DT.Rows[i]["C_TruckCarNum2"].ToString(); //ลูก
                }

                //ค้นหาสถานะ อ้อย Bonsucro ในตารางใบนำตัด
                if(ChkBonsugo.Checked == false)
                {
                    if (DT.Rows[i]["C_bonsucro"].ToString() == "1") //Bonsucro
                    {
                        ChkBonsugo.Checked = true;
                    }
                    else
                    {
                        ChkBonsugo.Checked = false;
                    }
                }

                //เลือกรถ
                fncGetDataCheck("Cane", DT.Rows[i]["C_CaneType"].ToString());
                fncGetDataCheck("Car", DT.Rows[i]["C_TruckType"].ToString());

                //ผู้รับเหมารวม
                if (DT.Rows[i]["C_AllContractor"].ToString() != "")
                {
                    lvName = GsysSQL.fncGetOwnerName(DT.Rows[i]["C_AllContractor"].ToString());
                    txtContractAll.Text = DT.Rows[i]["C_AllContractor"].ToString() + " : " + lvName; //ชื่อผู้รับเหมารวม
                    txtContractAllPrice.Text = DT.Rows[i]["C_AllPrice"].ToString(); //ราคา
                }

                //ปิดปุ่มค้นหาทะเบียนรถใน Mode แก้ไข ถ้าจะเปลี่ยนให้ใช้การยกเลิกคิวแล้วเปิดใหม่เเทน
                if (pvmode == "Edit") btnSearhCar.Enabled = false;
            }

            if(txtQuota.Text != "")
            {
                fncLoadMCSSName();
            }
           
        }

        private void btnMonitor_Click(object sender, EventArgs e)
        {
            frmDisplay frm = new frmDisplay();
            frm.Show();
        }
        
        private void fncRefreshDataMonitor()
        {
            try
            {
                frmDisplay.instance.lb1.Text = lbCarName.Text;
                frmDisplay.instance.lb2.Text = lbCaneName.Text;
                frmDisplay.instance.lb3.Text = txtQuota.Text;
                frmDisplay.instance.lb4.Text = txtQuotaName.Text;
                if (txtQ.Text.Contains("."))
                {
                    frmDisplay.instance.lb5.Text = "";
                    frmDisplay.instance.lb6.Text = txtCarNum.Text;
                }
                else
                {
                    frmDisplay.instance.lb5.Text = txtCarNum.Text;
                    frmDisplay.instance.lb6.Text = "";
                }
               
            }
            catch
            {

            }
        }

        private void fncClearDataMonitor()
        {
            try
            {
                frmDisplay.instance.lb1.Text = "";
                frmDisplay.instance.lb2.Text = "";
                frmDisplay.instance.lb3.Text = "";
                frmDisplay.instance.lb4.Text = "";
                frmDisplay.instance.lb5.Text = "";
                frmDisplay.instance.lb6.Text = "";
            }
            catch
            {

            }
        }

        private void txtQ_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCarNum_TextChanged(object sender, EventArgs e)
        {

        }

        private void fncLoadMCSSName()
        {
            DataTable DT = new DataTable();
            string SQL = "Select AspNetUsers.AppSurveyUserCode, AspNetUsers.Tel, AspNetUsers.FullName From AspNetUsers INNER JOIN Quotas ON AspNetUsers.UserCode = Quotas.SurveyUserCode Where Quotas.Code = '" + txtQuota.Text + "' ";
            DT = GsysSQL.fncGetQueryDataMCSS(SQL, DT, GVar.gvOnline);

            string lvSai = "";
            string lvTel = "";
            string lvName = "";

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                lvSai = DT.Rows[i]["AppSurveyUserCode"].ToString();
                lvTel = DT.Rows[i]["Tel"].ToString();
                lvName = DT.Rows[i]["FullName"].ToString();

                txt_Sai.Text = lvSai;
                txt_Tel.Text = lvTel;
                txt_MName.Text = lvName;
            }
        }

        private void txtCutPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void txtCarNum_Click(object sender, EventArgs e)
        {
            fncRefreshDataMonitor();
        }
        

        private void chk_CutdocA_Click(object sender, EventArgs e)
        {
            string lvCaneType = fncGetDataCheck("Cane", "");
            if (lvCaneType == "")
            {
                MessageBox.Show("กรุณาเลือกข้อมูลประเภทอ้อยก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chk_CutdocA.Checked = false;
                return;
            }
            else if(txtCarNum.Text == "")
            {
                MessageBox.Show("กรุณากรอกทะเบียนรถบรรทุกก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                chk_CutdocA.Checked = false;
                return;
            }
            else
            {
                if (chk_CutdocA.Checked == true)
                {
                    txtCaneDoc.Enabled = true;
                    txtCutOwner.Enabled = true;
                    if (GVar.gvTypeProgram == "Q") txtCutPrice.Enabled = true;
                    else txtCutPrice.Enabled = true;
                    txtKeepOwner.Enabled = true;
                    if (GVar.gvTypeProgram == "Q") txtKeepPrice.Enabled = true;
                    else txtKeepPrice.Enabled = true;
                    txtCarOwner.Enabled = true;
                    txtCarNumPlate.Enabled = true;
                    txtContractAll.Enabled = true;
                    if (GVar.gvTypeProgram == "Q") txtContractAllPrice.Enabled = true;
                    else txtContractAllPrice.Enabled = true;
                    if (GVar.gvTypeProgram == "Q") chkCarryPrice.Enabled = true;
                    else chkCarryPrice.Enabled = true;
                    if (GVar.gvTypeProgram == "Q") txtCarryPrice.Enabled = true;
                    else txtCarryPrice.Enabled = true;
                    groupBox25.Enabled = true;
                    groupBox18.Enabled = true;
                    groupBox24.Enabled = true;
                    btnSearchcarcut.Enabled = true;
                    btnRmTruckSearch.Enabled = true;
                    btnRmKeebSearch.Enabled = true;
                    btnRmAllSearch.Enabled = true;
                    btmSearchCarRm.Enabled = true;

                    string lvCutdoc = GsysSQL.fncGetLastCutdocID();
                    txtCutDoc.Text = lvCutdoc;

                    fncLoadtxtCutOwner();
                    fncLoadtxtKeepOwner();
                    fncLoadtxtTruckOwner();
                    fnctxtLoadContractAll();
                }
                else
                {
                    btnClear_Click(sender, e);
                }
            }
        }

        private void fncLoadtxtCutOwner()
        {
            DataTable DT = new DataTable();
            string lvSQL = "";
            if (chkCane7.Checked == true || chkCane13.Checked == true)
            {
                lvSQL = "Select * From Cane_CarContractor WHERE P_CarContractorName = 'รับเหมา' AND P_CarContractorLastName != 'โรงงาน' ORDER BY P_CarContractorID ASC ";
            }
            else
            {
                lvSQL = "Select * From Cane_CarContractor Where P_CarContractorName != 'รับเหมา' AND P_CarContractorType NOT IN ( 'บรรทุก', 'คีบ' ) ORDER BY P_CarContractorID ASC ";
            }
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, true);

            txtCutOwner.Properties.Items.Clear();
            txtCutOwner.Properties.Items.Add("");
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                string lvContractorID = DT.Rows[i]["P_CarContractorID"].ToString();
                string lvContractorName = DT.Rows[i]["P_CarContractorName"].ToString();
                string lvContractorLastName = DT.Rows[i]["P_CarContractorLastName"].ToString();

                string CarcutCombobox = lvContractorID + " : " + lvContractorName + " " + lvContractorLastName;
                txtCutOwner.Properties.Items.Add(CarcutCombobox);
            }
        }

        private void fncLoadtxtKeepOwner()
        {
            DataTable DT = new DataTable();
            string lvSQL = "";
            lvSQL = "Select * From Cane_CarContractor WHERE P_CarContractorType = 'คีบ' ORDER BY P_CarContractorID ASC ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, true);

            txtKeepOwner.Items.Clear();
            txtKeepOwner.Items.Add("");
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                string lvContractorID = DT.Rows[i]["P_CarContractorID"].ToString();
                string lvContractorName = DT.Rows[i]["P_CarContractorName"].ToString();
                string lvContractorLastName = DT.Rows[i]["P_CarContractorLastName"].ToString();

                string CarcutCombobox = lvContractorID + " : " + lvContractorName + " " + lvContractorLastName;
                txtKeepOwner.Items.Add(CarcutCombobox);
            }
        }

        private void fncLoadtxtTruckOwner()
        {
            DataTable DT = new DataTable();
            string lvSQL = "";
            lvSQL = "Select * From Cane_CarContractor Where P_CarContractorName != 'รับเหมา' ORDER BY P_CarContractorID ASC ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, true);

            txtCarOwner.Items.Clear();
            txtCarOwner.Items.Add("");
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                string lvContractorID = DT.Rows[i]["P_CarContractorID"].ToString();
                string lvContractorName = DT.Rows[i]["P_CarContractorName"].ToString();
                string lvContractorLastName = DT.Rows[i]["P_CarContractorLastName"].ToString();

                string CarcutCombobox = lvContractorID + " : " + lvContractorName + " " + lvContractorLastName;
                txtCarOwner.Items.Add(CarcutCombobox);
            }
        }

        private void txtCutOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(chkCane7.Checked == true || chkCane13.Checked == true)
            {
                string[] txtCutownersplit = txtCutOwner.Text.Split(':');
                string lvCutcar = txtCutownersplit[0].Trim();
                txtCutCar.Text = lvCutcar;
                string TruckPrice = GsysSQL.fncGetCarContractorPrice(lvCutcar, "ตัด"); //ฟังก์ชั่นหาราคาตามประเภท
                txtCutPrice.Text = TruckPrice;
            }
            else
            {
                string[] txtCutownersplit = txtCutOwner.Text.Split(':');
                string lvCutcar = txtCutownersplit[0].Trim();
                string TruckPrice = GsysSQL.fncGetCarContractorPrice(lvCutcar, "ตัด"); //ฟังก์ชั่นหาราคาตามประเภท
                txtCutPrice.Text = TruckPrice;
            }

            if(txtCutOwner.Text != "")
            {
                groupBox25.Enabled = false;
            }
        }

        private void txtKeepOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] txtCutownersplit = txtKeepOwner.Text.Split(':');
            string lvCutcar = txtCutownersplit[0].Trim();
            string KeepPrice = GsysSQL.fncGetCarContractorPrice(lvCutcar, "คีบ"); //ฟังก์ชั่นหาราคาตามประเภท
            txtKeepPrice.Text = KeepPrice;

            if(txtCutOwner.Text != "")
            {
                //groupBox25.Enabled = false;
            }
        }

        private void txtCarOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkCarryPrice.Checked = true;
            txtCarNumPlate.Text = txtCarNum.Text;
            string lvCutcar = txtQuota.Text;

            DataTable DT = new DataTable();
            string lvSQL = "";
            lvSQL = "Select T_TruckPrice From Cane_TruckPrice Where T_Quota = '" + lvCutcar + "' GROUP BY T_TruckPrice "; //ฟังก์ชั่นหาราคาค่าบรรทุก
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, true);

            txtCarryPrice.Items.Clear();
            txtCarryPrice.Items.Add("");
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                string lvTruckPrice = DT.Rows[i]["T_TruckPrice"].ToString();

                txtCarryPrice.Items.Add(lvTruckPrice);
            }

            if(txtCarOwner.Text != "") if (DT.Rows.Count != 0) txtCarryPrice.SelectedIndex = 1;
        }

        private void fnctxtLoadContractAll()
        {
            DataTable DT = new DataTable();
            string lvSQL = "";
            lvSQL = "Select * From Cane_CarContractor Where P_CarContractorName != 'รับเหมา' ORDER BY P_CarContractorID ASC ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, true);

            txtContractAll.Items.Clear();
            txtContractAll.Items.Add("");

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                string lvContractorID = DT.Rows[i]["P_CarContractorID"].ToString();
                string lvContractorName = DT.Rows[i]["P_CarContractorName"].ToString();
                string lvContractorLastName = DT.Rows[i]["P_CarContractorLastName"].ToString();

                string CarcutCombobox = lvContractorID + " : " + lvContractorName + " " + lvContractorLastName;
                txtContractAll.Items.Add(CarcutCombobox);
            }
        }

        private void txtContractAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] txtCutownersplit = txtContractAll.Text.Split(':');
            string lvCutcar = txtCutownersplit[0].Trim();
            string KeepPrice = GsysSQL.fncGetCarContractorPrice(lvCutcar, "เหมา"); //ฟังก์ชั่นหาราคาตามประเภท
            txtContractAllPrice.Text = KeepPrice;

            if(txtContractAll.Text != "")
            {
                groupBox18.Enabled = false;
                groupBox24.Enabled = false;
            }
        }

        private void txtCutOwner_TextChanged(object sender, EventArgs e)
        {
            if (chkCane7.Checked == true || chkCane13.Checked == true)
            {
                string[] txtCutownersplit = txtCutOwner.Text.Split(':');
                string lvCutcar = txtCutownersplit[0].Trim();
                txtCutCar.Text = lvCutcar;
                string TruckPrice = GsysSQL.fncGetCarContractorPrice(lvCutcar, "ตัด"); //ฟังก์ชั่นหาราคาตามประเภท
                txtCutPrice.Text = TruckPrice;
            }
            else
            {
                string[] txtCutownersplit = txtCutOwner.Text.Split(':');
                string lvCutcar = txtCutownersplit[0].Trim();
                string TruckPrice = GsysSQL.fncGetCarContractorPrice(lvCutcar, "ตัด"); //ฟังก์ชั่นหาราคาตามประเภท
                txtCutCar.Text = "";
                txtCutPrice.Text = TruckPrice;
            }

            if(txtCutOwner.Text != "")
            {
                //groupBox25.Enabled = false;
            }
        }

        private void btnSearchcarcut_Click(object sender, EventArgs e)
        {
            if (chkCane7.Checked == true || chkCane13.Checked == true)
            {
                GVar.gvModeSearchRm = "CarcutRmIn";
            }
            else
            {
                GVar.gvModeSearchRm = "CarcutRmOut";
            }

            frmRmSearch frm = new frmRmSearch();
            frm.ShowDialog();

            if(GVar.gvCarcutRm != "")
            {
                txtCutOwner.Text = GVar.gvCarcutRm;
            }
            else
            {
                txtCutOwner.Focus();
            }

            GVar.gvCarcutRm = "";
            GVar.gvModeSearchRm = "";
        }

        private void btnRmTruckSearch_Click(object sender, EventArgs e)
        {
            GVar.gvModeSearchRm = "CarTruckRm";

            frmRmSearch frm = new frmRmSearch();
            frm.ShowDialog();

            if (GVar.gvCartruckRm != "")
            { 
                txtCarOwner.Text = GVar.gvCartruckRm;
                txtCarNumPlate.Text = txtCarNum.Text;
                chkCarryPrice.Checked = true;
                string lvQuota = txtQuota.Text;

                DataTable DT = new DataTable();
                string lvSQL = "";
                lvSQL = "Select T_TruckPrice From Cane_TruckPrice Where T_Quota = '" + lvQuota + "' GROUP BY T_TruckPrice "; //ฟังก์ชั่นหาราคาค่าบรรทุก
                DT = GsysSQL.fncGetQueryData(lvSQL, DT, true);

                txtCarryPrice.Items.Clear();
                txtCarryPrice.Items.Add("");
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    string lvTruckPrice = DT.Rows[i]["T_TruckPrice"].ToString();

                    txtCarryPrice.Items.Add(lvTruckPrice);
                }

                if (txtCarOwner.Text != "") if (DT.Rows.Count != 0) txtCarryPrice.SelectedIndex = 1;
            }
            else
            {
                txtCarOwner.Focus();
            }

            GVar.gvCartruckRm = "";
            GVar.gvModeSearchRm = "";
        }

        private void btnRmKeebSearch_Click(object sender, EventArgs e)
        {
            GVar.gvModeSearchRm = "CarKeebRm";

            frmRmSearch frm = new frmRmSearch();
            frm.ShowDialog();

            string lvCutcar = "";

            if (GVar.gvCarkeepRm != "")
            {
                txtKeepOwner.Text = GVar.gvCarkeepRm;
                string[] txtCutownersplit = txtKeepOwner.Text.Split(':');
                lvCutcar = txtCutownersplit[0].Trim();
                string KeepPrice = GsysSQL.fncGetCarContractorPrice(lvCutcar, "คีบ"); //ฟังก์ชั่นหาราคาตามประเภท
                txtKeepPrice.Text = KeepPrice; 
            }
            else
            {
                txtKeepOwner.Focus();
            }

            GVar.gvCarkeepRm = "";
            GVar.gvModeSearchRm = "";
        }

        private void btnRmAllSearch_Click(object sender, EventArgs e)
        {
            GVar.gvModeSearchRm = "CarAllRm";

            frmRmSearch frm = new frmRmSearch();
            frm.ShowDialog();

            string lvCutcar = "";

            if (GVar.gvAllRm != "")
            {
                txtContractAll.Text = GVar.gvAllRm;
                string[] txtCutownersplit = txtContractAll.Text.Split(':');
                lvCutcar = txtCutownersplit[0].Trim();
                string KeepPrice = GsysSQL.fncGetCarContractorPrice(lvCutcar, "เหมา"); //ฟังก์ชั่นหาราคาตามประเภท
                txtContractAllPrice.Text = KeepPrice;
            }
            else
            {
                txtContractAll.Focus();
            }

            GVar.gvAllRm = "";
            GVar.gvModeSearchRm = "";
        }

        private void txtContractAll_TextChanged(object sender, EventArgs e)
        {
            string[] txtCutownersplit = txtContractAll.Text.Split(':');
            string lvCutcar = txtCutownersplit[0].Trim();
            string KeepPrice = GsysSQL.fncGetCarContractorPrice(lvCutcar, "เหมา"); //ฟังก์ชั่นหาราคาตามประเภท
            txtContractAllPrice.Text = KeepPrice;

            if(txtContractAll.Text != "")
            {
                //groupBox18.Enabled = false;
                //groupBox24.Enabled = false;
            }
        }

        private void btmSearchCarRm_Click(object sender, EventArgs e)
        {
            GVar.gvModeSearchRm = "CarSearchRm";

            frmRmSearch frm = new frmRmSearch();
            frm.ShowDialog();

            if (GVar.gvCarSearchRm != "")
            {
                txtCarOwner.Text = GVar.gvCarSearchRm;
                txtCarNumPlate.Text = txtCarNum.Text;
                chkCarryPrice.Checked = true;
                string lvQuota = txtQuota.Text;

                DataTable DT = new DataTable();
                string lvSQL = "";
                lvSQL = "Select T_TruckPrice From Cane_TruckPrice Where T_Quota = '" + lvQuota + "' GROUP BY T_TruckPrice "; //ฟังก์ชั่นหาราคาค่าบรรทุก
                DT = GsysSQL.fncGetQueryData(lvSQL, DT, true);

                txtCarryPrice.Items.Clear();
                txtCarryPrice.Items.Add("");
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    string lvTruckPrice = DT.Rows[i]["T_TruckPrice"].ToString();

                    txtCarryPrice.Items.Add(lvTruckPrice);
                }

                if (txtCarOwner.Text != "") if (DT.Rows.Count != 0) txtCarryPrice.SelectedIndex = 1;
            }
            else
            {
                txtContractAll.Focus();
            }

            GVar.gvCarSearchRm = "";
            GVar.gvModeSearchRm = "";
        }

        private void txtKeepOwner_TextChanged(object sender, EventArgs e)
        {
            if (txtKeepOwner.Text == "")
            {
                txtKeepPrice.Text = "";
            }
        }

        private void txtCaneDoc_Leave(object sender, EventArgs e)
        {
            //*15-01-2565 ดึงอ้อย Bonsucro ป้อมคิว
            string lvCode = txtCaneDoc.Text;
            string lvYear = "64";
            if (!lvCode.Contains(":"))
            {
                if(ChkBonsugo.Checked == true)
                {
                    //ถ้าเช็ค Bonsucro แล้วไม่ต้องทำอะไร
                }
                else
                {
                    string lvChkBonsucro = GsysSQL.fncGetBonsucroMCSS(lvCode, lvYear);
                    if (lvChkBonsucro == "True")
                    {
                        ChkBonsugo.Checked = true;
                    }
                    else
                    {
                        ChkBonsugo.Checked = false;
                    }
                }
            }
            
        }
    }
}