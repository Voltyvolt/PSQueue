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
    public partial class frmQLock : Form
    {
        public frmQLock()
        {
            InitializeComponent();
        }

        private void toggleSwitch1_Toggled(object sender, EventArgs e)
        {
            if (tgOnOff.EditValue.ToString() == "False")
            {
                tgOnOff.ForeColor = Color.Red;
                lbAlert.Visible = true;
            }
            else
            {
                tgOnOff.ForeColor = Color.Green;
                lbAlert.Visible = false;
            }
        }

        private void txtLoop_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //ยืนยัน
            string lvTxtAlert = "ยืนยันการบันทึกข้อมูล ?";
            DialogResult dialogResult = MessageBox.Show(lvTxtAlert, "Confirm?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return;
            }

            string lvStatus = tgOnOff.EditValue.ToString();
            if (lvStatus == "True") lvStatus = "1"; else lvStatus = "0";
            string lvLoopNoS = txtLoopS.Text;
            string lvlockNoS = txtLockNoS.Text;
            string lvLoopNoE = txtLoopE.Text;
            string lvlockNoE = txtLockNoE.Text;
            string[] lvArr = txtDate.Text.Split(' ');
            string lvStartDate = Gstr.fncChangeTDate(lvArr[0]);
            string lvStartTime = txtTime.Text;
            string lvAlert = tgLockAlert.EditValue.ToString();
            if (lvAlert == "True") lvAlert = "1"; else lvAlert = "0";

            //ประเภทรถ
            string lvCar1 = "0";
            string lvCar2 = "0";
            string lvCar3 = "0";
            string lvCar4 = "0";
            string lvCar5 = "0";
            if (chkCar1.Checked) lvCar1 = "1";
            if (chkCar2.Checked) lvCar2 = "1";
            if (chkCar3.Checked) lvCar3 = "1";
            if (chkCar4.Checked) lvCar4 = "1";
            if (chkCar5.Checked) lvCar5 = "1";

            //ประเภทอ้อย
            string lvCane9 = "0";
            string lvCane10 = "0";
            string lvCane11 = "0";
            string lvCane12 = "0";
            string lvCane13 = "0";
            string lvCane14 = "0";
            string lvCane15 = "0";
            string lvCane16 = "0";
            if (chkCane9.Checked) lvCane9 = "1";
            if (chkCane10.Checked) lvCane10 = "1";
            if (chkCane11.Checked) lvCane11 = "1";
            if (chkCane12.Checked) lvCane12 = "1";
            if (chkCane13.Checked) lvCane13 = "1";
            if (chkCane14.Checked) lvCane14 = "1";
            if (chkCane15.Checked) lvCane15 = "1";
            if (chkCane16.Checked) lvCane16 = "1";

            string lvLockCarRegis = tgLockCarRegis.EditValue.ToString();
            if (lvLockCarRegis == "True") lvLockCarRegis = "1"; else lvLockCarRegis = "0";
            string lvLockClearBtn = tgButtonClear.EditValue.ToString();
            if (lvLockClearBtn == "True") lvLockClearBtn = "1"; else lvLockClearBtn = "0";

            if (lvLoopNoS == "")
            {
                MessageBox.Show("กรุณาเลือกรอบการหีบ","แจ้งเตือน",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            string lvSQL = "Update Queue_LockOption Set L_Status = '" + lvStatus + "', L_loopNo = '" + lvLoopNoS + "', L_lockNo = '" + lvlockNoS + "', L_StartDate = '" + lvStartDate + "', L_StartTime = '" + lvStartTime + "', L_AlertLock = '" + lvAlert + "', ";
            lvSQL += "L_LockCar1 = '"+ lvCar1 + "',L_LockCar2 = '" + lvCar2 + "',L_LockCar3 = '" + lvCar3 + "',L_LockCar4 = '" + lvCar4 + "',L_LockCar5 = '" + lvCar5 + "',";
            lvSQL += "L_LockCane9 = '" + lvCane9 + "',L_LockCane10 = '" + lvCane10 + "',L_LockCane11 = '" + lvCane11 + "',L_LockCane12 = '" + lvCane12 + "',L_LockCane13 = '" + lvCane13 + "',L_LockCane14 = '" + lvCane14 + "',L_LockCane15 = '" + lvCane15 + "',L_LockCane16 = '" + lvCane16 + "', ";
            lvSQL += "L_lockCarRegis = '" + lvLockCarRegis + "', L_loopNoE = '" + lvLoopNoE + "', L_lockNoE = '" + lvlockNoE + "', L_lockBtnClear = '" + lvLockClearBtn + "' ";
            string lvResault = GsysSQL.fncExecuteQueryData(lvSQL, true);

            string lvTXT = "แก้ไขข้อมูล สถานะ = " + lvStatus + " รอบ = " + lvLoopNoS + " / "+ lvlockNoS + " ถึง รอบ = " + lvLoopNoE + " / "+ lvlockNoE + " วันที่เริ่ม = " + lvStartDate + " เวลาเริ่ม = " + lvStartTime;
            lvTXT += " สถานะแจ้งเตือนคิวล็อค = " + lvAlert + "  สถานะล็อคทะเบียน = " + lvLockCarRegis + "  สถานะแสดงปุ่ม Clear = " + lvLockClearBtn;
            GsysSQL.fncKeepLogData(GVar.gvUser, "ระบบ คิวล๊อก", lvTXT);

            MessageBox.Show("บันทึกข้อมูลเรียบร้อย", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void FncLoadData()
        {
            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "Select * from Queue_LockOption ";            
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, true);

            int lvNumRow = DT.Rows.Count;
            if (lvNumRow > 0)
            {
                if (DT.Rows[0]["L_Status"].ToString() == "1")
                    tgOnOff.EditValue = true;
                else
                    tgOnOff.EditValue = false;

                txtLoopS.Text = DT.Rows[0]["L_loopNo"].ToString();
                txtLockNoS.Text = DT.Rows[0]["L_lockNo"].ToString();
                txtLoopE.Text = DT.Rows[0]["L_loopNoE"].ToString();
                txtLockNoE.Text = DT.Rows[0]["L_lockNoE"].ToString();
                txtDate.Text = Gstr.fncChangeSDate(DT.Rows[0]["L_StartDate"].ToString());
                txtTime.EditValue = DT.Rows[0]["L_StartTime"].ToString();

                if (DT.Rows[0]["L_AlertLock"].ToString() == "1")
                    tgLockAlert.EditValue = true;
                else
                    tgLockAlert.EditValue = false;

                //ประเภทรถ
                if (DT.Rows[0]["L_LockCar1"].ToString() == "1") chkCar1.Checked = true; else chkCar1.Checked = false;
                if (DT.Rows[0]["L_LockCar2"].ToString() == "1") chkCar2.Checked = true; else chkCar2.Checked = false;
                if (DT.Rows[0]["L_LockCar3"].ToString() == "1") chkCar3.Checked = true; else chkCar3.Checked = false;
                if (DT.Rows[0]["L_LockCar4"].ToString() == "1") chkCar4.Checked = true; else chkCar4.Checked = false;
                if (DT.Rows[0]["L_LockCar5"].ToString() == "1") chkCar5.Checked = true; else chkCar5.Checked = false;

                //ประเภทอ้อย
                if (DT.Rows[0]["L_LockCane9"].ToString() == "1") chkCane9.Checked = true; else chkCane9.Checked = false;
                if (DT.Rows[0]["L_LockCane10"].ToString() == "1") chkCane10.Checked = true; else chkCane10.Checked = false;
                if (DT.Rows[0]["L_LockCane11"].ToString() == "1") chkCane11.Checked = true; else chkCane11.Checked = false;
                if (DT.Rows[0]["L_LockCane12"].ToString() == "1") chkCane12.Checked = true; else chkCane12.Checked = false;
                if (DT.Rows[0]["L_LockCane13"].ToString() == "1") chkCane13.Checked = true; else chkCane13.Checked = false;
                if (DT.Rows[0]["L_LockCane14"].ToString() == "1") chkCane14.Checked = true; else chkCane14.Checked = false;
                if (DT.Rows[0]["L_LockCane15"].ToString() == "1") chkCane15.Checked = true; else chkCane15.Checked = false;
                if (DT.Rows[0]["L_LockCane16"].ToString() == "1") chkCane16.Checked = true; else chkCane16.Checked = false;

                if (DT.Rows[0]["L_lockCarRegis"].ToString() == "1")
                    tgLockCarRegis.EditValue = true;
                else
                    tgLockCarRegis.EditValue = false;

                if (DT.Rows[0]["L_lockBtnClear"].ToString() == "1")
                    tgButtonClear.EditValue = true;
                else
                    tgButtonClear.EditValue = false;
            }
        }

        private void frmQLock_Load(object sender, EventArgs e)
        {
            FncLoadData();
        }

        private void tgLockAlert_Toggled(object sender, EventArgs e)
        {
            if (tgLockAlert.EditValue.ToString() == "False")
            {
                tgLockAlert.ForeColor = Color.Red;
            }
            else
            {
                tgLockAlert.ForeColor = Color.Green;
            }
        }

        private void txtLockNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            FncLoadQRange();
        }

        private void FncLoadQRange()
        {
            string lvQS = "";
            string lvQE = "";
            string lvAll = "";
            string lvCaption = "";

            //Get Data 
            DataTable DT = new DataTable();
            string lvSQL = "Select L_QStart,L_QEnd from Queue_LockMaster where L_No = '" + txtLockNoS.Text + "' ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, true);

            if (DT.Rows.Count > 0)
                lvQS = DT.Rows[0]["L_QStart"].ToString() + " - " + DT.Rows[0]["L_QEnd"].ToString();

            DT.Dispose();

            DT = new DataTable();
            lvSQL = "Select L_QStart,L_QEnd from Queue_LockMaster where L_No = '" + txtLockNoE.Text + "' ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, true);

            if (DT.Rows.Count > 0)
                lvQE = DT.Rows[0]["L_QStart"].ToString() + " - " + DT.Rows[0]["L_QEnd"].ToString();
            DT.Dispose();

            try
            {
                //ถ้าเป็นรอบเดียวกัน เช้คที่คิวต่อ
                if (txtLoopS.Text == txtLoopE.Text)
                {
                    if (txtLockNoS.Text == txtLockNoE.Text)
                    {
                        lvAll = lvQS;
                    }
                    else
                    {
                        string[] lvArrS = lvQS.Replace(" ", "").Split('-');
                        string[] lvArrE = lvQE.Replace(" ", "").Split('-');

                        lvAll = lvArrS[0] + " - " + lvArrE[1];
                    }
                }
                else
                {
                    string[] lvArrS = lvQS.Replace(" ", "").Split('-');
                    string[] lvArrE = lvQE.Replace(" ", "").Split('-');

                    lvAll = txtLoopS.Text + "/" + lvArrS[0] + " - " + txtLoopE.Text + "/" + lvArrE[1];
                    //lvAll = "รอบที่ "+ txtLoopS.Text + " คิวที่ " + lvArrS[0] + " ถึง รอบที่ " + txtLoopE.Text + " คิวที่ " + lvArrE[1];
                }
            }
            catch (Exception ex)
            {

            }

            txtQ.Text = lvAll;
            txtQ.Tag = lvCaption;
        }

        private void tgLockCarRegis_Toggled(object sender, EventArgs e)
        {
            if (tgLockCarRegis.EditValue.ToString() == "False")
            {
                tgLockCarRegis.ForeColor = Color.Red;
            }
            else
            {
                tgLockCarRegis.ForeColor = Color.Green;
            }
        }

        private void txtLockNoE_SelectedIndexChanged(object sender, EventArgs e)
        {
            FncLoadQRange();
        }

        private void txtLoopS_EditValueChanged(object sender, EventArgs e)
        {
            FncLoadQRange();
        }

        private void txtLoopE_EditValueChanged(object sender, EventArgs e)
        {
            FncLoadQRange();
        }

        private void tgButtonClear_Toggled(object sender, EventArgs e)
        {
            if (tgButtonClear.EditValue.ToString() == "False")
            {
                tgButtonClear.ForeColor = Color.Red;
            }
            else
            {
                tgButtonClear.ForeColor = Color.Green;
            }
        }
    }
}
