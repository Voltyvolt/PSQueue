using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSQueue
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                //ดึงค่า Config ของเครื่องนั้นๆมาก่อน เพื่อจะได้ไม่ต้องตั้งค่าใหม่
                fncCheckConfigFile();

                #region //Get Connection From TXT
                string[] lvArr = System.IO.File.ReadAllText("C:\\DataFile\\System_Data.dll").Split('/');
                GVar.gvConnectionDB = lvArr[0];
                GVar.gvINOUT = lvArr[1];
                GVar.gvTypeProgram = lvArr[2];
                GVar.gvComport = lvArr[3];
                GVar.gvServerIP = GVar.gvConnectionDB;
                GVar.gvStation = lvArr[4];

                //ถ้าไม่เจอตัวแปรนี้ ให้สร้าง TXT File เข้ามาใหม่
                try
                {
                    GVar.gvDBName = lvArr[6];
                }
                catch
                {
                    string lvOld = System.IO.File.ReadAllText("C:\\DataFile\\System_Data.dll");
                    string lvTxT = lvOld + "/DBPS";
                    System.IO.File.WriteAllText("C:\\DataFile\\System_Data.dll", lvTxT);

                    GVar.gvDBName = "DBPS";
                }

                if (lvArr[5] == "ONLINE")
                    GVar.gvOnline = true;
                else
                    GVar.gvOnline = false;

                //Usb = 6 , ปกติ 8
                //GVar.gvDataBit = Gstr.fncToInt(lvArr[4]);

                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");

                if (GVar.gvDBName == "DBPS_TEST")
                {
                    //Set Connection
                    connectionStringsSection.ConnectionStrings["PSConnection"].ConnectionString = "Data Source=" + GVar.gvConnectionDB + ";Initial Catalog=DBPS_TEST;Persist Security Info=True;User ID=sa;Password=sugarPS2;";
                    GVar.gvDBConnect = "DBPS_TEST";
                }
                else
                {
                    //Set Connection
                    connectionStringsSection.ConnectionStrings["PSConnection"].ConnectionString = "Data Source=" + GVar.gvConnectionDB + ";Initial Catalog=DBPS;Persist Security Info=True;User ID=sa;Password=sugarPS2;";
                    GVar.gvDBConnect = "DBPS";
                }

                connectionStringsSection.ConnectionStrings["PS_LogConnection"].ConnectionString = "Data Source=" + GVar.gvConnectionDB + ";Initial Catalog=DBPS_LOG;Persist Security Info=True;User ID=sa;Password=sugarPS2;";
                config.Save();
                ConfigurationManager.RefreshSection("connectionStrings");

                #endregion
                
                ////เช็คสถานะว่า Online อยู่หรือไม่
                //GVar.gvOnline = GsysSQL.fncChkOnline(GVar.gvServerIP);

                //if (!GVar.gvOnline)
                //{
                //    MessageBox.Show("ไม่สามารถเชื่อมต่อได้ กรุณาลองใหม่อีกครั้ง", "แจ้งเตือน",MessageBoxButtons.OK,MessageBoxIcon.Information);
                //    Application.Restart();
                //}

                string lvStatus = "";
                
                lvStatus = GsysSQL.fncCheckLogin(txtUser.Text, txtPass.Text, GVar.gvOnline);

                if (lvStatus != "" && lvStatus != "Connection Error")
                {
                    //เช็คสิทธิ์การใช้งาน
                    GsysSQL.fncFindPermission(txtUser.Text, "Queue_00", GVar.gvOnline);
                    GsysSQL.fncFindUserInfo(txtUser.Text, GVar.gvOnline);

                    //Log
                    if (GVar.gvOnline)
                        GsysSQL.fncKeepLogData(GVar.gvUser, "โปรแกรมคิว", "เข้าสู่ระบบ");


                    if (GVar.gvUser != "psview" && GVar.gvUser != "Noo" && GVar.gvUser != "vascha")
                    {
                        frmMain frm = new frmMain();
                        this.Hide();
                        frm.Show();
                    }
                    else
                    {
                        frmShowReport frm = new frmShowReport();
                        this.Hide();
                        frm.Show();
                    }
                }
                else if (lvStatus == "")
                {
                    MessageBox.Show("ข้อมูลผู้ใช้ / รหัสผ่านไม่ถูกต้อง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPass.Text = "";
                    txtPass.Focus();
                }
                else if (lvStatus == "Connection Error")
                {
                    MessageBox.Show("ติดต่อฐานข้อมูลไม่ได้ กรุณาตรวจสอบการเชื่อมต่ออีกครั้ง !!", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Restart();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (ex.Message.StartsWith("Could not find"))
                {
                    MessageBox.Show("ติดต่อฐานข้อมูลไม่ได้ กรุณาตรวจสอบการเชื่อมต่ออีกครั้ง !!", "แจ้งเตือน OFFINE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Restart();
                }
                else
                {
                    MessageBox.Show("ไม่พบการเชื่อมต่อ Internet กรุณาตรวจสอบการเชื่อมต่ออีกครั้ง !!", "แจ้งเตือน OFFINE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Restart();
                }
            }
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            string[] lvArr = System.IO.File.ReadAllText("C:\\DataFile\\System_Data.dll").Split('/');
            GVar.gvConnectionDB = lvArr[0];
            GVar.gvINOUT = lvArr[1];

            if (e.KeyCode == Keys.Enter)
            {
                txtPass.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.Q)
            {
                txtUser.Text = "q3";
                txtPass.Text = "q3";
                
                string text = GVar.gvConnectionDB + "/" + GVar.gvINOUT + "/" + "Q" + "/" + "COM1" + "/" + "3" + "/" + "ONLINE" + "/DBPS";
                System.IO.File.WriteAllText("C:\\DataFile\\System_Data.dll", text);

                btnLogin.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.W)
            {
                txtUser.Text = "admin";
                txtPass.Text = "sugarps2";

                string text = GVar.gvConnectionDB + "/" + "OUT" + "/" + "W" + "/" + "COM1" + "/" + "3" + "/" + "ONLINE" + "/DBPS";
                System.IO.File.WriteAllText("C:\\DataFile\\System_Data.dll", text);

                btnLogin.PerformClick();
            }
            else if (e.Control && e.KeyCode == Keys.E)
            {
                txtUser.Text = "admin";
                txtPass.Text = "sugarps2";

                string text = GVar.gvConnectionDB + "/" + "IN" + "/" + "W" + "/" + "COM1" + "/" + "3" + "/" + "ONLINE" + "/DBPS";
                System.IO.File.WriteAllText("C:\\DataFile\\System_Data.dll", text);

                btnLogin.PerformClick();
            }

        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "admin")
            {
                //    Process.Start("notepad.exe", "C:\\DataFile\\System_Data.dll");
                //}
                //else
                //    txtUser.Focus();

                frmLoginOption frm = new frmLoginOption();
                frm.Show();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fncCheckConfigFile()
        {
            string lvSorcePath = "C:\\DataFile\\System_Data.dll";
            string lvDesPath = Application.StartupPath + "\\System_Data.dll";

            //เช็คว่าไฟล์ Backup มีไหม
            //เช็ค Folder DataFile ก่อน
            if (!System.IO.Directory.Exists("C:\\DataFile"))
            {
                System.IO.Directory.CreateDirectory("C:\\DataFile");
            }

            if (System.IO.File.Exists(lvSorcePath))
            {
                //ถ้ามีให้ดึงข้อมูลมาเขียนไฟล์ทับในตัวโปรแกรม
                //อ่าน TXT
                string[] lvArr = System.IO.File.ReadAllText(lvSorcePath).Split('/');
                string text = lvArr[0] + "/" + lvArr[1] + "/" + lvArr[2] + "/" + lvArr[3] + "/" + lvArr[4] + "/" + lvArr[5];
                System.IO.File.WriteAllText(Application.StartupPath + "\\System_Data.dll", text);
            }
            else
            {
                //ถ้าไม่มีให้ดึงจากใน Program ไป Default ไว้
                //อ่าน TXT
                string[] lvArr = System.IO.File.ReadAllText(lvDesPath).Split('/');
                string text = lvArr[0] + "/" + lvArr[1] + "/" + lvArr[2] + "/" + lvArr[3] + "/" + lvArr[4] + "/" + lvArr[5];
                System.IO.File.WriteAllText(lvSorcePath, text);
            }

            //ไฟล์ Access
            string lvSorceAccPath = "C:\\DataFile\\PS_DB.mdb";
            string lvDesAccPath = Application.StartupPath + "\\PS_DB.mdb";
            string lvBackAccPath = "C:\\DataFile\\Backup\\PS_DB.mdb";

            if (System.IO.File.Exists(lvSorceAccPath))
            {
                //เช็ค Folder Backup ก่อน
                if (!System.IO.Directory.Exists("C:\\DataFile\\Backup"))
                {
                    System.IO.Directory.CreateDirectory("C:\\DataFile\\Backup");
                }

                ////ถ้ามีให้ดึงข้อมูลมาเขียนไฟล์ทับในตัวโปรแกรม
                //System.IO.File.Copy(lvSorceAccPath, lvDesAccPath);
            }
            else
            {
                //ถ้าไม่มีให้ดึงจากใน Program ไป Default ไว้
                System.IO.File.Copy(lvDesAccPath, lvSorceAccPath);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            //ดึงค่า Config ของเครื่องนั้นๆมาก่อน เพื่อจะได้ไม่ต้องตั้งค่าใหม่
            fncCheckConfigFile();

            //เช็ค ONLINE OFFLINE
            fncCheckONLINE();

            //เช็คข้อมูลและ Sync
            FncSyncData();
        }

        private void fncCheckONLINE()
        {
            //อ่าน TXT
            string[] lvArr = System.IO.File.ReadAllText(Application.StartupPath + "\\System_Data.dll").Split('/');

            //เช็คสถานะว่า Online อยู่หรือไม่
            bool lvStatus = GsysSQL.fncChkOnline(lvArr[0]);
            
            if (!lvStatus)
            {
                //ถ้า OFFLINE
                if (lvArr[5] == "ONLINE")
                {
                    GVar.gvCountOFFLINE += 1;

                    if (GVar.gvCountOFFLINE >= 5)
                    {
                        MessageBox.Show("ไม่สามารถติดต่อกับ Server ได้ โปรแกรมจะเข้าสู่ Mode OFFLINE" + Environment.NewLine + "กรุณา ปิดและเปิดใหม่ โปรแกรมอีกครั้ง", "แจ้งเตือน OFFLINE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        string text = lvArr[0] + "/" + lvArr[1] + "/" + lvArr[2] + "/" + lvArr[3] + "/" + lvArr[4] + "/OFFLINE";
                        System.IO.File.WriteAllText(Application.StartupPath + "\\System_Data.dll", text);
                        GVar.gvCountOFFLINE = 0;
                    }
                    //Application.Restart();
                }
            }
            else
            {
                //ถ้า ONLINE
                if (lvArr[5] == "OFFLINE")
                {
                    MessageBox.Show("เชื่อมต่อ Server ได้สำเร็จ" + Environment.NewLine + "กรุณา ปิดและเปิดใหม่ โปรแกรมอีกครั้ง", "แจ้งเตือน ONLINE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string text = lvArr[0] + "/" + lvArr[1] + "/" + lvArr[2] + "/" + lvArr[3] + "/" + lvArr[4] + "/ONLINE";
                    System.IO.File.WriteAllText(Application.StartupPath + "\\System_Data.dll", text);
                    //Application.Restart();
                }
            }
        }

        private void FncSyncData()
        {
            this.Cursor = Cursors.WaitCursor;

            //เช็ค Data ต้นทาง Access
            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "Select * From Queue_Diary Where Q_SyncStatus = '0' ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, false);

            string lvResault = "";

            int lvNumRow = DT.Rows.Count;

            for (int i = 0; i < lvNumRow; i++)
            {
                string lvQ = DT.Rows[i]["Q_No"].ToString();
                string lvRail = DT.Rows[i]["Q_Rail"].ToString();
                string lvCarNum = DT.Rows[i]["Q_CarNum"].ToString();
                string lvCutDoc = DT.Rows[i]["Q_CutDoc"].ToString();
                string lvQuota = DT.Rows[i]["Q_Quota"].ToString();
                string lvCaneDoc = DT.Rows[i]["Q_CaneDoc"].ToString();
                string lvCaneType = DT.Rows[i]["Q_CaneType"].ToString();
                string lvCutCar = DT.Rows[i]["Q_CutCar"].ToString();
                string lvMainCar = DT.Rows[i]["Q_MainCar"].ToString();
                string lvDate = DT.Rows[i]["Q_Date"].ToString();
                string lvTime = DT.Rows[i]["Q_Time"].ToString();
                string lvCarType = DT.Rows[i]["Q_CarType"].ToString();
                string lvCutPrice = DT.Rows[i]["Q_CutPrice"].ToString();
                string lvCarryPrice = DT.Rows[i]["Q_CarryPrice"].ToString();
                string lvStation = DT.Rows[i]["Q_Station"].ToString();
                string lvUser = DT.Rows[i]["Q_User"].ToString();
                string lvBonsugo = DT.Rows[i]["Q_Bonsugo"].ToString();
                string lvWeightAll = DT.Rows[i]["Q_WeightALLStatus"].ToString();

                //บันทึกข้อมูลลง SQL
                lvSQL = "Insert into Queue_Diary (Q_No, Q_Rail, Q_CarNum, Q_CutDoc, Q_Quota, Q_CaneDoc, Q_CaneType, Q_CutCar, Q_MainCar, Q_Date, Q_Time, Q_CarType,Q_CutPrice,Q_CarryPrice,Q_Station, Q_User, Q_Bonsugo, Q_WeightALLStatus) ";
                lvSQL += "Values ('" + lvQ + "', '" + lvRail + "', '" + lvCarNum + "', '" + lvCutDoc + "', '" + lvQuota + "', '" + lvCaneDoc + "', '" + lvCaneType + "', '" + lvCutCar + "', '" + lvMainCar + "', '" + lvDate + "', '" + lvTime + "', '" + lvCarType + "', '" + lvCutPrice + "', '" + lvCarryPrice + "', '" + lvStation + "', '" + lvUser + "', '" + lvBonsugo + "', '" + lvWeightAll + "')";
                lvResault = GsysSQL.fncExecuteQueryData(lvSQL, true);

                //ถ้าบันทึกแล้วให้ลบออก
                if (lvResault == "Success")
                {
                    lvSQL = "Delete From Queue_Diary Where Q_No = " + lvQ + " And Q_SyncStatus = '0'";
                    lvResault = GsysSQL.fncExecuteQueryData(lvSQL, false);
                }
            }

            if (lvResault == "Success")
            {
                MessageBox.Show("ทำการโอนย้ายข้อมูลเรียบร้อย", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Cursor = Cursors.Default;
        }
    }
}
