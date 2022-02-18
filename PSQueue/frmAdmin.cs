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
    public partial class frmAdmin : Form
    {
        public frmAdmin()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            this.Cursor = Cursors.WaitCursor;

            ////เช็คสถานะว่า Online อยู่หรือไม่
            //GVar.gvOnline = GsysSQL.fncChkOnline(GVar.gvServerIP);

            string lvDateS = Gstr.fncChangeTDate(txtDateS.Text);
            string lvDateE = Gstr.fncChangeTDate(txtDateE.Text);
            int lvBillS = Gstr.fncToInt(txtBillS.Text);
            int lvBillE = Gstr.fncToInt(txtBillE.Text);
            string lvQuotaS = txtQuotaS.Text;
            string lvQuotaE = txtQuotaE.Text;
            string lvCarS = txtCarS.Text;
            string lvQS = txtQS.Text;
            string lvQE = txtQE.Text;
            string lvSimpleS = txtSampleS.Text;
            string lvType = Gstr.fncGetDataCode(CmbTypeCane.Text, ":");

            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "select * ";
            lvSQL += "from Queue_Diary ";
            lvSQL += "where Q_Status = 'Active' ";
            //lvSQL += "and Q_WeightINDate <> '' ";

            if (ChkShowWeightFinish.Checked)
                lvSQL += "and Q_WeightOUTDate >= '" + lvDateS + "' and Q_WeightOUTDate <= '" + lvDateE + "' ";

            //ตัดข้อมูลพ่วงออก
            if (!chkTruck2.Checked)
            {
                lvSQL += "and Q_No not like '%.%' ";
            }

            if (ChkA.Checked)
            {
                lvSQL += "and Q_Rail = 'A' ";
            }
            else if (ChkB.Checked)
            {
                lvSQL += "and Q_Rail = 'B' ";
            }

            //กรองตามบิล
            if (txtBillS.Text != "")
            {
                if (txtBillE.Text == "")
                {
                    lvSQL += "and Q_BillingNo >= '" + lvBillS.ToString("000000") + "' and Q_BillingNo <= '" + lvBillS.ToString("000000") + "' ";
                }
                else
                {
                    lvSQL += "and Q_BillingNo >= '" + lvBillS.ToString("000000") + "' and Q_BillingNo <= '" + lvBillE.ToString("000000") + "' ";
                }
            }

            //กรองตามคิว
            if (lvQS != "")
            {
                if (lvQS == "")
                {
                    lvSQL += "and Q_No >= '" + lvQS + "' and Q_No <= '" + lvQS + "' ";
                }
                else
                {
                    lvSQL += "and Q_No >= '" + lvQS + "' and Q_No <= '" + lvQE + "' ";
                }
            }

            //กรองตามโควต้า
            if (lvQuotaS != "")
            {
                if (lvQuotaS == "")
                {
                    lvSQL += "and Q_Quota >= '" + lvQuotaS + "' and Q_Quota <= '" + lvQuotaS + "' ";
                }
                else
                {
                    lvSQL += "and Q_Quota >= '" + lvQuotaS + "' and Q_Quota <= '" + lvQuotaE + "' ";
                }
            }

            //กรองตามทะเบียนรถ
            if (lvCarS != "")
            {
                lvSQL += "and Q_CarNum like '%" + lvCarS + "%' ";
            }

            //กรองตามเลขตัวอย่าง
            if (lvSimpleS != "")
            {
                lvSQL += "and Q_SampleNo like '%" + lvSimpleS + "%' ";
            }

            //กรองตามประเภทอ้อย
            if (lvType != "")
            {
                lvSQL += "and Q_CaneType = '" + lvType + "' ";
            }

            //แสดงรายการที่ปิดยอดแล้ว
            if (chkShowClose.Checked)
            {
                lvSQL += "and Q_CloseStatus = '1' ";
            }
            else
            {
                lvSQL += "and Q_CloseStatus = '' ";
            }

            lvSQL += "and Q_Year = '' ";

            DT = GsysSQL.fncGetQueryData(lvSQL, DT, true);

            int lvNumRow = DT.Rows.Count;
            sp1.ActiveSheetIndex = 0;
            sp1.Sheets[0].RowCount = 0;
            sp1.Sheets[0].RowCount = lvNumRow;

            sp1.Sheets[1].RowCount = 0;
            sp1.Sheets[1].RowCount = lvNumRow;

            int lvCountCar = 0;
            double lvWeightIN = 0;
            double lvWeightOUT = 0;
            double lvSubWeight = 0;

            //ตารางหลัก
            for (int i = 0; i < lvNumRow; i++)
            {
                sp1.Sheets[0].Cells[i, 0].Text = "";
                sp1.Sheets[0].Cells[i, 1].Text = DT.Rows[i]["Q_BillingNo"].ToString();
                sp1.Sheets[0].Cells[i, 2].Text = DT.Rows[i]["Q_No"].ToString();
                sp1.Sheets[0].Cells[i, 3].Text = DT.Rows[i]["Q_Rail"].ToString();
                sp1.Sheets[0].Cells[i, 4].Text = DT.Rows[i]["Q_CarNum"].ToString();
                sp1.Sheets[0].Cells[i, 5].Text = DT.Rows[i]["Q_CarNum2"].ToString();
                sp1.Sheets[0].Cells[i, 6].Text = DT.Rows[i]["Q_CutDoc"].ToString();
                sp1.Sheets[0].Cells[i, 7].Text = DT.Rows[i]["Q_Quota"].ToString();
                sp1.Sheets[0].Cells[i, 8].Text = DT.Rows[i]["Q_CaneDoc"].ToString();
                sp1.Sheets[0].Cells[i, 9].Text = DT.Rows[i]["Q_CaneType"].ToString() + " : " + GsysSQL.fncFindCaneTypeName(DT.Rows[i]["Q_CaneType"].ToString(), GVar.gvOnline);
                sp1.Sheets[0].Cells[i, 10].Text = DT.Rows[i]["Q_CutCar"].ToString();
                sp1.Sheets[0].Cells[i, 11].Text = DT.Rows[i]["Q_CarType"].ToString();
                sp1.Sheets[0].Cells[i, 12].Text = DT.Rows[i]["Q_CutPrice"].ToString();
                sp1.Sheets[0].Cells[i, 13].Text = DT.Rows[i]["Q_CarryPrice"].ToString();

                sp1.Sheets[0].Cells[i, 14].Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightINDate"].ToString());
                sp1.Sheets[0].Cells[i, 15].Text = DT.Rows[i]["Q_WeightINTime"].ToString();
                sp1.Sheets[0].Cells[i, 16].Text = Gstr.fncToDouble(DT.Rows[i]["Q_WeightIN"].ToString()).ToString("#,##0.000");
                sp1.Sheets[0].Cells[i, 17].Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightOUTDate"].ToString());
                sp1.Sheets[0].Cells[i, 18].Text = DT.Rows[i]["Q_WeightOUTTime"].ToString();
                sp1.Sheets[0].Cells[i, 19].Text = Gstr.fncToDouble(DT.Rows[i]["Q_WeightOUT"].ToString()).ToString("#,##0.000");

                sp1.Sheets[0].Cells[i, 20].Text = DT.Rows[i]["Q_SampleNo"].ToString();
                sp1.Sheets[0].Cells[i, 21].Text = DT.Rows[i]["Q_TKNo"].ToString();
                sp1.Sheets[0].Cells[i, 22].Text = DT.Rows[i]["Q_DumNo"].ToString();

                if (DT.Rows[i]["Q_Bonsugo"].ToString() == "1")
                    sp1.Sheets[0].Cells[i, 23].Text = "อ้อย Bonsucro";
                else
                    sp1.Sheets[0].Cells[i, 23].Text = "";

                sp1.Sheets[0].Cells[i, 24].Text = DT.Rows[i]["Q_PK"].ToString();
            }

            //ตาราง Backup
            for (int i = 0; i < lvNumRow; i++)
            {
                sp1.Sheets[1].Cells[i, 0].Text = "";
                sp1.Sheets[1].Cells[i, 1].Text = DT.Rows[i]["Q_BillingNo"].ToString();
                sp1.Sheets[1].Cells[i, 2].Text = DT.Rows[i]["Q_No"].ToString();
                sp1.Sheets[1].Cells[i, 3].Text = DT.Rows[i]["Q_Rail"].ToString();
                sp1.Sheets[1].Cells[i, 4].Text = DT.Rows[i]["Q_CarNum"].ToString();
                sp1.Sheets[1].Cells[i, 5].Text = DT.Rows[i]["Q_CarNum2"].ToString();
                sp1.Sheets[1].Cells[i, 6].Text = DT.Rows[i]["Q_CutDoc"].ToString();
                sp1.Sheets[1].Cells[i, 7].Text = DT.Rows[i]["Q_Quota"].ToString();
                sp1.Sheets[1].Cells[i, 8].Text = DT.Rows[i]["Q_CaneDoc"].ToString();
                sp1.Sheets[1].Cells[i, 9].Text = DT.Rows[i]["Q_CaneType"].ToString() + " : " + GsysSQL.fncFindCaneTypeName(DT.Rows[i]["Q_CaneType"].ToString(), GVar.gvOnline);
                sp1.Sheets[1].Cells[i, 10].Text = DT.Rows[i]["Q_CutCar"].ToString();
                sp1.Sheets[1].Cells[i, 11].Text = DT.Rows[i]["Q_CarType"].ToString();
                sp1.Sheets[1].Cells[i, 12].Text = DT.Rows[i]["Q_CutPrice"].ToString();
                sp1.Sheets[1].Cells[i, 13].Text = DT.Rows[i]["Q_CarryPrice"].ToString();

                sp1.Sheets[1].Cells[i, 14].Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightINDate"].ToString());
                sp1.Sheets[1].Cells[i, 15].Text = DT.Rows[i]["Q_WeightINTime"].ToString();
                sp1.Sheets[1].Cells[i, 16].Text = Gstr.fncToDouble(DT.Rows[i]["Q_WeightIN"].ToString()).ToString("#,##0.000");
                sp1.Sheets[1].Cells[i, 17].Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightOUTDate"].ToString());
                sp1.Sheets[1].Cells[i, 18].Text = DT.Rows[i]["Q_WeightOUTTime"].ToString();
                sp1.Sheets[1].Cells[i, 19].Text = Gstr.fncToDouble(DT.Rows[i]["Q_WeightOUT"].ToString()).ToString("#,##0.000");

                sp1.Sheets[1].Cells[i, 20].Text = DT.Rows[i]["Q_SampleNo"].ToString();
                sp1.Sheets[1].Cells[i, 21].Text = DT.Rows[i]["Q_TKNo"].ToString();
                sp1.Sheets[1].Cells[i, 22].Text = DT.Rows[i]["Q_DumNo"].ToString();

                if (DT.Rows[i]["Q_Bonsugo"].ToString() == "1")
                    sp1.Sheets[1].Cells[i, 23].Text = "อ้อย Bonsucro";
                else
                    sp1.Sheets[1].Cells[i, 23].Text = "";
            }

            this.Cursor = Cursors.Default;
        }

        private void chkShowClose_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void chkTruck2_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ChkA_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ChkB_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ChkAll_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void CmbTypeCane_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtDateS_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtDateE_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void CmbTypeCane_DropDown(object sender, EventArgs e)
        {
            CmbTypeCane.Items.Clear();

            string lvSQL = "Select * from Cane_CaneType ";
            DataTable DT = new DataTable();
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                string lvData = DT.Rows[i]["C_ID"].ToString() + " : " + DT.Rows[i]["C_Name"].ToString();
                CmbTypeCane.Items.Add(lvData);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int lvCount = 0;

            //ยืนยัน
            string lvTxtAlert = "ยืนยันการบันทึกข้อมูล ?";
            //if (GVar.gvDateBill != Gstr.fncChangeTDate(txtDate.Text)) lvTxtAlert = "  **วันที่ไม่ตรงกับใบเสร็จ**"+ Environment.NewLine + Environment.NewLine + "ยืนยันการทำรายการต่อหรือไม่?";
            DialogResult dialogResult = MessageBox.Show(lvTxtAlert, "Confirm?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                this.Cursor = Cursors.Default;
                return;
            }

            //ตรวจสอบความถูกต้อง
            for (int i = 0; i < sp1.Sheets[0].RowCount; i++)
            {
                #region //ประกาศตัวแปร ตารางหลัก
                string lvChk = sp1.Sheets[0].Cells[i, 0].Text;
                string lvBillNo = sp1.Sheets[0].Cells[i, 1].Text;
                string lvQNo = sp1.Sheets[0].Cells[i, 2].Text;
                string lvRail = sp1.Sheets[0].Cells[i, 3].Text.ToUpper();
                string lvCarNum = sp1.Sheets[0].Cells[i, 4].Text;
                string lvCarNum2 = sp1.Sheets[0].Cells[i, 5].Text;
                string lvCutCar = sp1.Sheets[0].Cells[i, 6].Text;
                string lvQuota = sp1.Sheets[0].Cells[i, 7].Text;
                string lvCaneDoc = sp1.Sheets[0].Cells[i, 8].Text;
                string lvCaneType = Gstr.fncGetDataCode(sp1.Sheets[0].Cells[i, 9].Text, ":");
                string lvCutNo = sp1.Sheets[0].Cells[i, 10].Text;
                string lvCarType = sp1.Sheets[0].Cells[i, 11].Text;
                string lvCutPrice = sp1.Sheets[0].Cells[i, 12].Text;
                string lvCarryPrice = sp1.Sheets[0].Cells[i, 13].Text;

                string lvDateIN = Gstr.fncChangeTDate(sp1.Sheets[0].Cells[i, 14].Text);
                string lvTimeIN = sp1.Sheets[0].Cells[i, 15].Text;
                string lvWeightIN = sp1.Sheets[0].Cells[i, 16].Text;
                string lvDateOUT = Gstr.fncChangeTDate(sp1.Sheets[0].Cells[i, 17].Text);
                string lvTimeOUT = sp1.Sheets[0].Cells[i, 18].Text;
                string lvWeightOUT = sp1.Sheets[0].Cells[i, 19].Text;

                string lvSampleNo = sp1.Sheets[0].Cells[i, 20].Text;
                string lvTKNo = sp1.Sheets[0].Cells[i, 21].Text;
                string lvDump = sp1.Sheets[0].Cells[i, 22].Text;
                string lvBonsucro = sp1.Sheets[0].Cells[i, 23].Text;
                string lvPK = sp1.Sheets[0].Cells[i, 24].Text;
                #endregion

                #region//ประกาศตัวแปร ตาราง Backup
                string lvBillNo_1 = sp1.Sheets[1].Cells[i, 1].Text;
                string lvQNo_1 = sp1.Sheets[1].Cells[i, 2].Text;
                string lvRail_1 = sp1.Sheets[1].Cells[i, 3].Text.ToUpper();
                string lvCarNum_1 = sp1.Sheets[1].Cells[i, 4].Text;
                string lvCarNum2_1 = sp1.Sheets[1].Cells[i, 5].Text;
                string lvCutCar_1 = sp1.Sheets[1].Cells[i, 6].Text;
                string lvQuota_1 = sp1.Sheets[1].Cells[i, 7].Text;
                string lvCaneDoc_1 = sp1.Sheets[1].Cells[i, 8].Text;
                string lvCaneType_1 = Gstr.fncGetDataCode(sp1.Sheets[1].Cells[i, 9].Text, ":");
                string lvCutNo_1 = sp1.Sheets[1].Cells[i, 10].Text;
                string lvCarType_1 = sp1.Sheets[1].Cells[i, 11].Text;
                string lvCutPrice_1 = sp1.Sheets[1].Cells[i, 12].Text;
                string lvCarryPrice_1 = sp1.Sheets[1].Cells[i, 13].Text;

                string lvDateIN_1 = Gstr.fncChangeTDate(sp1.Sheets[1].Cells[i, 14].Text);
                string lvTimeIN_1 = sp1.Sheets[1].Cells[i, 15].Text;
                string lvWeightIN_1 = sp1.Sheets[1].Cells[i, 16].Text;
                string lvDateOUT_1 = Gstr.fncChangeTDate(sp1.Sheets[1].Cells[i, 17].Text);
                string lvTimeOUT_1 = sp1.Sheets[1].Cells[i, 18].Text;
                string lvWeightOUT_1 = sp1.Sheets[1].Cells[i, 19].Text;

                string lvSampleNo_1 = sp1.Sheets[1].Cells[i, 20].Text;
                string lvTKNo_1 = sp1.Sheets[1].Cells[i, 21].Text;
                string lvDump_1 = sp1.Sheets[1].Cells[i, 22].Text;
                string lvBonsucro_1 = sp1.Sheets[1].Cells[i, 23].Text;
                #endregion

                if (lvChk == "True")
                {
                    #region //--ตรวจสอบความถูกต้อง
                    ////เลขที่บิล
                    //if (lvBillNo.Length != 6 || lvBillNo == "")
                    //{
                    //    MessageBox.Show("รูปแบบเลขที่บิลไม่ถูกต้อง กรุณาตรวจสอบใหม่อีกครั้ง  รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}
                    //else if (lvBillNo.Any(char.IsLetter))
                    //{
                    //    MessageBox.Show("เลขที่บิลต้องเป็นตัวเลขเท่านั้น  รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}

                    //เลขที่คิว
                    if (lvQNo != lvQNo_1)
                    {
                        MessageBox.Show("ไม่สามารถแก้ไขเลขที่คิวได้ กรุณาตรวจสอบใหม่อีกครั้ง  รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    ////ราง
                    //if (lvRail.Length != 1 || lvRail == "")
                    //{
                    //    MessageBox.Show("รูปแบบ ราง ไม่ถูกต้อง กรุณาตรวจสอบใหม่อีกครั้ง  รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}
                    //else if (lvRail != "A" && lvRail != "B")
                    //{
                    //    MessageBox.Show("ราง จะต้องเป็น A หรือ B เท่านั้น รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}

                    ////โควต้า
                    //string lvChkQuota = GsysSQL.fncFindQuotaName(lvQuota , true);
                    //if (lvChkQuota == "")
                    //{
                    //    MessageBox.Show("ไม่พบข้อมูลโควต้า" + lvQuota + " รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}

                    ////ค่ารถตัด
                    //if (lvCutPrice.Any(char.IsLetter))
                    //{
                    //    MessageBox.Show("ค่ารถตัด จะต้องเป็น ตัวเลขเท่านั้น   รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}

                    ////ค่าบรรทุก
                    //if (lvCarryPrice.Any(char.IsLetter))
                    //{
                    //    MessageBox.Show("ค่าบรรทุก จะต้องเป็น ตัวเลขเท่านั้น   รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}

                    //น้ำหนัก ขาเข้า
                    //if (lvDateIN.Length != 8)
                    //{
                    //    MessageBox.Show("รูปแบบ วันที่ ขาเข้าไม่ถูกต้อง กรุณาตรวจสอบใหม่อีกครั้ง   รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}
                    //else if (lvTimeIN.Length != 8)
                    //{
                    //    MessageBox.Show("รูปแบบ เวลา ขาเข้าไม่ถูกต้อง กรุณาตรวจสอบใหม่อีกครั้ง   รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}

                    ////น้ำหนัก ขาออก
                    //if (lvDateOUT.Length != 8)
                    //{
                    //    MessageBox.Show("รูปแบบ วันที่ ขาเข้าไม่ถูกต้อง กรุณาตรวจสอบใหม่อีกครั้ง   รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}
                    //else if (lvTimeOUT.Length != 8)
                    //{
                    //    MessageBox.Show("รูปแบบ เวลา ขาเข้าไม่ถูกต้อง กรุณาตรวจสอบใหม่อีกครั้ง   รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}

                    //เลขตัวอย่าง
                    //if (lvSampleNo.Length != 4)
                    //{
                    //    MessageBox.Show("รูปแบบเลขที่คิวไม่ถูกต้อง กรุณาตรวจสอบใหม่อีกครั้ง  รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}
                    //else if (lvSampleNo.Any(char.IsLetter))
                    //{
                    //    MessageBox.Show("เลขตัวอย่าง จะต้องเป็น ตัวเลขเท่านั้น   รายการที่ " + (i + 1), "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    break;
                    //}
                    #endregion

                    lvCount += 1;
                    string lvMsg = "แก้ไขข้อมูล ";

                    //อัพ Status ตัวแม่
                    string lvSQL = "update Queue_Diary set ";

                    if (lvBillNo != lvBillNo_1)
                    {
                        lvMsg += "เลขที่บิล " + lvBillNo_1 + " --> " + lvBillNo + " ";
                        lvSQL += "Q_BillingNo = '" + lvBillNo + "' ";
                    }

                    if (lvQNo != lvQNo_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "คิว " + lvQNo_1 + " --> " + lvQNo + " ";
                        lvSQL += "Q_No = '" + lvQNo + "' ";
                    }

                    if (lvRail != lvRail_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "ราง " + lvRail_1 + " --> " + lvRail + " ";
                        lvSQL += "Q_Rail = '" + lvRail + "' ";
                    }

                    if (lvCarNum != lvCarNum_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "ทะเบียนรถ " + lvCarNum_1 + " --> " + lvCarNum + " ";
                        lvSQL += "Q_CarNum = '" + lvCarNum + "' ";
                    }

                    if (lvCarNum2 != lvCarNum2_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "พ่วง " + lvCarNum2 + " --> " + lvCarNum2_1 + " ";
                        lvSQL += "Q_CarNum2 = '" + lvCarNum2 + "' ";

                        //อัพเดทตัวลูก
                        string lvQNew = lvQNo + ".1";
                        string lvSQL2 = "update Queue_Diary set Q_CarNum = '" + lvCarNum2 + "' where Q_No = '" + lvQNew + "' ";
                        string lvResault2 = GsysSQL.fncExecuteQueryData(lvSQL2, true);
                    }

                    if (lvCutCar != lvCutCar_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "เลขรถตัด " + lvCutCar_1 + " --> " + lvCutCar + " ";
                        lvSQL += "Q_CutCar = '" + lvCutCar + "' ";
                    }

                    if (lvQuota != lvQuota_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "โควต้า " + lvQuota_1 + " --> " + lvQuota + " ";
                        lvSQL += "Q_Quota = '" + lvQuota + "' ";
                    }

                    if (lvCaneDoc != lvCaneDoc_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "เลขที่แปลง " + lvCaneDoc_1 + " --> " + lvCaneDoc + " ";
                        lvSQL += "Q_CaneDoc = '" + lvCaneDoc + "' ";
                    }

                    if (lvCaneType != lvCaneType_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "ประเภทอ้อย " + lvCaneType_1 + " --> " + lvCaneType + " ";
                        lvSQL += "Q_CaneType = '" + lvCaneType + "' ";
                    }

                    if (lvCutCar != lvCutCar_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "เลขรถตัด " + lvCutCar_1 + " --> " + lvCutCar + " ";
                        lvSQL += "Q_CutCar = '" + lvCutCar + "' ";
                    }

                    if (lvCarType != lvCarType_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "ประเภทรถ " + lvCarType_1 + " --> " + lvCarType + " ";
                        lvSQL += "Q_CarType = '" + lvCarType + "' ";
                    }

                    if (lvCutPrice != lvCutPrice_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "ค่าตัด " + lvCutPrice_1 + " --> " + lvCutPrice + " ";
                        lvSQL += "Q_CutPrice = '" + lvCutPrice + "' ";
                    }

                    if (lvCarryPrice != lvCarryPrice_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "ค่าบรรทุก " + lvCarryPrice_1 + " --> " + lvCarryPrice + " ";
                        lvSQL += "Q_CarryPrice = '" + lvCarryPrice + "' ";
                    }

                    if (lvDateIN != lvDateIN_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "วันที่ชั่งเข้า " + lvDateIN_1 + " --> " + lvDateIN + " ";
                        lvSQL += "Q_WeightINDate = '" + lvDateIN + "' ";
                    }

                    if (lvTimeIN != lvTimeIN_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "เวลาชั่งเข้า " + lvTimeIN_1 + " --> " + lvTimeIN + " ";
                        lvSQL += "Q_WeightINTime = '" + lvTimeIN + "' ";
                    }

                    if (lvWeightIN != lvWeightIN_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "น้ำหนักชั่งเข้า " + lvWeightIN_1 + " --> " + lvWeightIN + " ";
                        lvSQL += "Q_WeightIN = '" + lvWeightIN + "' ";
                    }

                    if (lvDateOUT != lvDateOUT_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "วันที่ชั่งออก " + lvDateOUT_1 + " --> " + lvDateOUT + " ";
                        lvSQL += "Q_WeightOUTDate = '" + lvDateOUT + "' ";
                    }

                    if (lvTimeOUT != lvTimeOUT_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "เวลาชั่งออก " + lvTimeOUT_1 + " --> " + lvTimeOUT + " ";
                        lvSQL += "Q_WeightOUTTime = '" + lvTimeOUT + "' ";
                    }

                    if (lvWeightOUT != lvWeightOUT_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "น้ำหนักชั่งออก " + lvWeightOUT_1 + " --> " + lvWeightOUT + " ";
                        lvSQL += "Q_WeightOUT = '" + lvWeightOUT + "' ";
                    }

                    if (lvSampleNo != lvSampleNo_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        string lvSampleChkNo2 = Gstr.Right(lvSampleNo, 4);
                        string lvSampleChkNo = Gstr.Left(lvSampleNo, 4);

                        lvMsg += "เลขตัวอย่าง แม่ลูก " + lvSampleNo_1 + " --> " + lvSampleChkNo + lvSampleChkNo2 + " ";
                        lvSQL += "Q_SampleNo = '" + lvSampleChkNo + "' ";

                        //อัพเดทตัวลูก
                        string lvQNew = lvQNo + ".1";
                        string lvSQL2 = "update Queue_Diary set Q_SampleNo = '" + lvSampleChkNo2 + "' where Q_No = '" + lvQNew + "' ";
                        string lvResault2 = GsysSQL.fncExecuteQueryData(lvSQL2, true);
                    }

                    if (lvTKNo != lvTKNo_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "คิวตะกาว " + lvTKNo_1 + " --> " + lvTKNo + " ";
                        lvSQL += "Q_TKNo = '" + lvTKNo + "' ";
                    }

                    if (lvDump != lvDump_1)
                    {
                        if (lvMsg != "แก้ไขข้อมูล ") lvMsg += ",";
                        if (lvSQL != "update Queue_Diary set ") lvSQL += ",";

                        lvMsg += "ดัมที่ " + lvDump_1 + " --> " + lvDump + " ";
                        lvSQL += "Q_DumNo = '" + lvDump + "' ";
                    }

                    lvMsg += " คิวเดิม " + lvQNo_1 + " เลขที่บิลเดิม " + lvBillNo_1;

                    lvSQL += " where Q_PK = '" + lvPK + "'";
                    string lvResault = GsysSQL.fncExecuteQueryData(lvSQL, true);

                    //Keep Log
                    string lvMsgCheck = "แก้ไขข้อมูล " + lvMsg;
                    if (lvMsgCheck != lvMsg)
                        GsysSQL.fncKeepLogData(GVar.gvUser, "ห้องชั่ง Admin", lvMsg);
                }
            }

            if (lvCount >= 1)
            {
                MessageBox.Show("บันทึกข้อมูลเรียบร้อย จำนวน " + lvCount + " รายการ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            else
            {
                MessageBox.Show("กรุณาเลือกรายการที่ต้องการบันทึกก่อน", "Success", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sp1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 0)
                timerHightlight.Enabled = true;
        }

        private void timerHightlight_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < sp1.Sheets[0].RowCount; i++)
            {
                if (sp1.Sheets[0].Cells[i, 0].Text == "True")
                {
                    sp1.Sheets[0].Rows[i].BackColor = Color.Wheat;
                }
                else
                {
                    sp1.Sheets[0].Rows[i].BackColor = Color.White;
                }
            }

            timerHightlight.Enabled = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBillS.Text = "";
            txtBillE.Text = "";
            txtDateS.Text = "";
            txtDateE.Text = "";
            txtQS.Text = "";
            txtQE.Text = "";
            txtSampleS.Text = "";
            txtCarS.Text = "";
            CmbTypeCane.Text = "";
            txtQuotaS.Text = "";
            txtQuotaE.Text = "";

            sp1.Sheets[0].RowCount = 0;
            sp1.Sheets[1].RowCount = 0;
        }

        private void ChkShowWeightFinish_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void chkShowCutcar_CheckedChanged(object sender, EventArgs e)
        {
            DataTable DT = new DataTable();
            string lvSQL = "select * ";
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
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumRow = DT.Rows.Count;
            sp1.ActiveSheetIndex = 0;
            sp1.Sheets[0].RowCount = 0;
            sp1.Sheets[0].RowCount = lvNumRow;

            if (lvNumRow > 0)
            {
                double lvCut1 = 0;
                double lvCut2 = 0;
                double lvCountCar = 0;

                for (int i = 0; i < lvNumRow; i++)
                {
                    //เอามาเฉพาะ คิวที่ชั่งเสร็จแล้ว
                    bool lvChkQFinish = GsysSQL.fncCheckQueueFinish(DT.Rows[i]["Q_No"].ToString());
                    if (lvChkQFinish)
                    {
                        sp1.Sheets[0].Cells[i, 0].Text = "";
                        sp1.Sheets[0].Cells[i, 1].Text = DT.Rows[i]["Q_BillingNo"].ToString();
                        sp1.Sheets[0].Cells[i, 2].Text = DT.Rows[i]["Q_No"].ToString();
                        sp1.Sheets[0].Cells[i, 3].Text = DT.Rows[i]["Q_Rail"].ToString();
                        sp1.Sheets[1].Cells[i, 4].Text = DT.Rows[i]["Q_CarNum"].ToString();
                        sp1.Sheets[1].Cells[i, 5].Text = DT.Rows[i]["Q_CarNum2"].ToString();
                        sp1.Sheets[1].Cells[i, 6].Text = DT.Rows[i]["Q_CutDoc"].ToString();
                        sp1.Sheets[1].Cells[i, 7].Text = DT.Rows[i]["Q_Quota"].ToString();
                        sp1.Sheets[1].Cells[i, 8].Text = DT.Rows[i]["Q_CaneDoc"].ToString();
                        sp1.Sheets[1].Cells[i, 9].Text = DT.Rows[i]["Q_CaneType"].ToString() + " : " + GsysSQL.fncFindCaneTypeName(DT.Rows[i]["Q_CaneType"].ToString(), GVar.gvOnline);
                        sp1.Sheets[1].Cells[i, 10].Text = DT.Rows[i]["Q_CutCar"].ToString();
                        sp1.Sheets[1].Cells[i, 11].Text = DT.Rows[i]["Q_CarType"].ToString();
                        sp1.Sheets[1].Cells[i, 12].Text = DT.Rows[i]["Q_CutPrice"].ToString();
                        sp1.Sheets[1].Cells[i, 13].Text = DT.Rows[i]["Q_CarryPrice"].ToString();

                        sp1.Sheets[1].Cells[i, 14].Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightINDate"].ToString());
                        sp1.Sheets[1].Cells[i, 15].Text = DT.Rows[i]["Q_WeightINTime"].ToString();
                        sp1.Sheets[1].Cells[i, 16].Text = Gstr.fncToDouble(DT.Rows[i]["Q_WeightIN"].ToString()).ToString("#,##0.000");
                        sp1.Sheets[1].Cells[i, 17].Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightOUTDate"].ToString());
                        sp1.Sheets[1].Cells[i, 18].Text = DT.Rows[i]["Q_WeightOUTTime"].ToString();
                        sp1.Sheets[1].Cells[i, 19].Text = Gstr.fncToDouble(DT.Rows[i]["Q_WeightOUT"].ToString()).ToString("#,##0.000");

                        sp1.Sheets[1].Cells[i, 20].Text = DT.Rows[i]["Q_SampleNo"].ToString();
                        sp1.Sheets[1].Cells[i, 21].Text = DT.Rows[i]["Q_TKNo"].ToString();
                        sp1.Sheets[1].Cells[i, 22].Text = DT.Rows[i]["Q_DumNo"].ToString();

                        if (DT.Rows[i]["Q_Bonsugo"].ToString() == "1")
                            sp1.Sheets[1].Cells[i, 23].Text = "อ้อย Bonsucro";
                        else
                            sp1.Sheets[1].Cells[i, 23].Text = "";
                    }

                }
            }
        }
    }
}
