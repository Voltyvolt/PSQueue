using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSQueue
{
    public partial class frmCloseStatus : Form
    {

        public frmCloseStatus()
        {
            InitializeComponent();

            if(GVar.gvTypeProgram == "Q")
            {
                sp1.ActiveSheet.Columns[0].Visible = false;
                btnSave.Visible = false;
                btnExportXls.Visible = false;
            }
        }

        private void frmCloseStatus_Load(object sender, EventArgs e)
        {
            //ตั้ง DatetimePicker
            DateTime DTNow = DateTime.Now;
            txtDateE.Text = DTNow.ToString("dd/MM/yyyy");
            txtDateS.Text = (DTNow.AddDays(-1)).ToString();

            //ตัวแปร
            GVar.gvESC = false;
            this.Focus();
        }

        private void LoadData()
        {
            TLoadData.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            btnSave.Enabled = false;
            btnClear.Enabled = false;
            btnSearch.Enabled = false;
            btnExportXls.Enabled = false;

            ////เช็คสถานะว่า Online อยู่หรือไม่
            //GVar.gvOnline = GsysSQL.fncChkOnline(GVar.gvServerIP);

            string lvDateS = Gstr.fncChangeTDate(txtDateS.Text);
            string lvDateE = Gstr.fncChangeTDate(txtDateE.Text);
            int lvBillS = Gstr.fncToInt(txtBillS.Text);
            int lvBillE = Gstr.fncToInt(txtBillE.Text);
            if (txtBillE.Text == "") lvBillE = lvBillS;

            string lvQuotaS = txtQuotaS.Text;
            string lvQuotaE = txtQuotaE.Text;
            if (lvQuotaE == "") lvQuotaE = lvQuotaS;

            string lvCarS = txtCarS.Text;
            string lvQS = txtQS.Text;
            string lvQE = txtQE.Text;
            if (lvQE == "") lvQE = lvQS;

            string lvSimpleS = txtSampleS.Text;
            string lvType = Gstr.fncGetDataCode(CmbTypeCane.Text, ":");

            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "select * ";
            lvSQL += "from Queue_Diary ";
            lvSQL += "LEFT JOIN Cane_QueueData ON Queue_Diary.Q_CutDoc = CAST(Cane_QueueData.C_ID AS NVARCHAR(20)) ";
            lvSQL += "Where Q_Status = 'Active' ";

            if (GVar.gvTypeProgram == "W")
            {
                lvSQL += "and Q_WeightOUTDate >= '" + lvDateS + "' and Q_WeightOUTDate <= '" + lvDateE + "' ";
            }
            else
            {
                lvSQL += "and Q_WeightOUTDate >= '' and Q_WeightOUTDate <= '' ";
            }

            //ตัดข้อมูลพ่วงออก
            //lvSQL += "and Q_No not like '%.%' ";

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

            // แสดงเฉพาะโควต้าลาน
            if (chkShowLan.Checked)
            {
                lvSQL += "and Q_Quota in ('7699') ";
            }

            //แสดงเฉพาะอ้อย Bonsucro
            if (chkBonsucro.Checked)
            {
                lvSQL += "and Q_Bonsugo = '1' ";
            }

            lvSQL += "and Q_Year = '' ";
            lvSQL += "Order by Q_BillingNo DESC ";

            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumRow = DT.Rows.Count;
            sp1.ActiveSheetIndex = 0;
            sp1.ActiveSheet.RowCount = 0;
            sp1.ActiveSheet.RowCount = lvNumRow;

            //Progressbar
            progressBar1.Maximum = lvNumRow;
            progressBar1.Value = 0;
            pnStatus.Visible = true;

            int lvCountCar = 0;
            double lvWeightIN = 0;
            double lvWeightOUT = 0;
            double lvSubWeight = 0;

            for (int i = 0; i < lvNumRow; i++)
            {
                //ถ้ากด ESC ให้ยกเลิกการโหลดข้อมูล
                if (GVar.gvESC)
                {
                    GVar.gvESC = false;
                    break;
                }

                sp1.ActiveSheet.Cells[i, 0].Text = "";
                sp1.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["Q_BillingNo"].ToString();
                sp1.ActiveSheet.Cells[i, 2].Text = DT.Rows[i]["Q_Quota"].ToString();

                string lvSampleNo = DT.Rows[i]["Q_SampleNo"].ToString();
                string lvSampleNo2 = GsysSQL.fncFindSample2(DT.Rows[i]["Q_No"].ToString() + ".1", chkShowClose.Checked,GVar.gvOnline);

                //ถ้าแม่ไม่ใช่ชั่งพ่วง ไม่ต้องเอาตัวอย่างลูกมาแสดง
                string[] lvArr = DT.Rows[i]["Q_No"].ToString().Split('.');
                string lvChkWeightAll = GsysSQL.fncCheckWeightALL(lvArr[0], GVar.gvOnline);
                if (lvChkWeightAll == "") lvSampleNo2 = "";

                sp1.ActiveSheet.Cells[i, 3].Text = lvSampleNo + lvSampleNo2;

                if(GVar.gvTypeProgram == "Q")
                {
                    sp1.ActiveSheet.Cells[i, 4].Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_Date"].ToString()) + "   " + DT.Rows[i]["Q_Time"].ToString();
                }
                else
                {
                    sp1.ActiveSheet.Cells[i, 4].Text = Gstr.fncChangeSDate(DT.Rows[i]["Q_WeightOUTDate"].ToString()) + "   " + DT.Rows[i]["Q_WeightOUTTime"].ToString();
                }
                
                sp1.ActiveSheet.Cells[i, 5].Text = DT.Rows[i]["Q_No"].ToString();
                sp1.ActiveSheet.Cells[i, 6].Text = DT.Rows[i]["Q_TKNo"].ToString();

                string lvCarNum = DT.Rows[i]["Q_CarNum"].ToString();
                string lvCarNum2 = DT.Rows[i]["Q_CarNum2"].ToString();
                if (lvCarNum2 == "") lvCarNum2 = GsysSQL.fncFindCarNum2(DT.Rows[i]["Q_No"].ToString() + ".1", GVar.gvOnline);
                if (lvCarNum2 != "" && lvChkWeightAll != "") lvCarNum += "/" + lvCarNum2;
                sp1.ActiveSheet.Cells[i, 7].Text = lvCarNum;

                sp1.ActiveSheet.Cells[i, 8].Text = GsysSQL.fncFindCaneTypeName(DT.Rows[i]["Q_CaneType"].ToString(), GVar.gvOnline);                
                sp1.ActiveSheet.Cells[i, 9].Text = FncChangeKiloToTon(DT.Rows[i]["Q_WeightIN"].ToString());
                sp1.ActiveSheet.Cells[i, 10].Text = FncChangeKiloToTon(DT.Rows[i]["Q_WeightOUT"].ToString());

                if (DT.Rows[i]["Q_CloseStatus"].ToString() == "1")
                {
                    sp1.ActiveSheet.Cells[i, 12].ForeColor = Color.Green;
                    sp1.ActiveSheet.Cells[i, 12].Text = "ปิดยอดแล้ว";
                }
                else
                {
                    sp1.ActiveSheet.Cells[i, 12].ForeColor = Color.Red;
                    sp1.ActiveSheet.Cells[i, 12].Text = "ยังไม่ปิดยอด";
                }

                sp1.ActiveSheet.Cells[i, 13].Text = DT.Rows[i]["Q_Quota"].ToString();
                sp1.ActiveSheet.Cells[i, 14].Text = DT.Rows[i]["Q_CaneType"].ToString();

                sp1.ActiveSheet.Cells[i, 15].Text = DT.Rows[i]["Q_WeightINDate"].ToString();
                sp1.ActiveSheet.Cells[i, 16].Text = DT.Rows[i]["Q_WeightINTime"].ToString();
                sp1.ActiveSheet.Cells[i, 17].Text = DT.Rows[i]["Q_WeightOUTDate"].ToString();
                sp1.ActiveSheet.Cells[i, 18].Text = DT.Rows[i]["Q_WeightOUTTime"].ToString();

                sp1.ActiveSheet.Cells[i, 19].Text = DT.Rows[i]["Q_PK"].ToString();
                //sp1.ActiveSheet.Cells[i, 20].Text = DT.Rows[i]["Q_Bonsugo"].ToString();
                //sp1.ActiveSheet.Cells[i, 21].Text = DT.Rows[i]["Q_CarryPriceStatus"].ToString();
                //sp1.ActiveSheet.Cells[i, 22].Text = DT.Rows[i]["C_ID"].ToString();
                //sp1.ActiveSheet.Cells[i, 23].Text = DT.Rows[i]["C_ContracType"].ToString();
                //sp1.ActiveSheet.Cells[i, 24].Text = DT.Rows[i]["C_ContracType"].ToString();
                sp1.ActiveSheet.Cells[i, 21].Text = DT.Rows[i]["Q_Bonsugo"].ToString();
                sp1.ActiveSheet.Cells[i, 22].Text = DT.Rows[i]["Q_CarryPriceStatus"].ToString();
                sp1.ActiveSheet.Cells[i, 23].Text = DT.Rows[i]["Q_CutDoc"].ToString();
                sp1.ActiveSheet.Cells[i, 25].Text = DT.Rows[i]["C_PlanId"].ToString();
                sp1.ActiveSheet.Cells[i, 28].Text = DT.Rows[i]["Q_CutCar"].ToString();
                sp1.ActiveSheet.Cells[i, 29].Text = DT.Rows[i]["C_CutContactorId"].ToString();
                sp1.ActiveSheet.Cells[i, 30].Text = DT.Rows[i]["Q_CutPrice"].ToString();
                sp1.ActiveSheet.Cells[i, 31].Text = DT.Rows[i]["C_ContractorId"].ToString();
                sp1.ActiveSheet.Cells[i, 32].Text = DT.Rows[i]["Q_CarryPrice"].ToString();
                sp1.ActiveSheet.Cells[i, 33].Text = DT.Rows[i]["C_KeebContractorId"].ToString();
                sp1.ActiveSheet.Cells[i, 34].Text = DT.Rows[i]["C_KeebContractorPrice"].ToString();
                sp1.ActiveSheet.Cells[i, 35].Text = DT.Rows[i]["C_AllContractor"].ToString();
                sp1.ActiveSheet.Cells[i, 36].Text = DT.Rows[i]["C_AllPrice"].ToString();
                sp1.ActiveSheet.Cells[i, 37].Text = DT.Rows[i]["Q_BillLan"].ToString();
                sp1.ActiveSheet.Cells[i, 38].Text = "";
                sp1.ActiveSheet.Cells[i, 39].Text = "";

                //ประเภทรับเหมา
                string lvcarcutid = DT.Rows[i]["C_CutContactorId"].ToString();
                string lvtruckid = DT.Rows[i]["C_ContractorId"].ToString();
                string lvkeebid = DT.Rows[i]["C_KeebContractorId"].ToString();
                string lvallid = DT.Rows[i]["C_AllContractor"].ToString();
                string lvcanetype = DT.Rows[i]["C_CaneType"].ToString();
                string lvContactrype = DT.Rows[i]["C_ContracType"].ToString();

                if (lvcarcutid != "")
                {
                    if(lvContactrype != "รถตัดใน")
                    {
                        lvcarcutid = "1";
                        sp1.ActiveSheet.Cells[i, 24].Text = lvcarcutid;
                    }
                    else
                    {
                        lvcarcutid = "";
                    }
                }
                
                if(lvtruckid != "")
                {
                    lvtruckid = "2";
                    if(lvcarcutid != "" && lvContactrype != "รถตัดใน")
                    {
                        sp1.ActiveSheet.Cells[i, 24].Text += "," + lvtruckid;
                    }
                    else
                    {
                        sp1.ActiveSheet.Cells[i, 24].Text += lvtruckid;
                    }
                   
                }
                
                if (lvkeebid != "")
                {
                    lvkeebid = "3";
                    if (lvcarcutid != "" && lvContactrype != "รถตัดใน" || lvtruckid != "")
                    {
                        sp1.ActiveSheet.Cells[i, 24].Text += "," + lvkeebid;
                    }
                    else
                    {
                        sp1.ActiveSheet.Cells[i, 24].Text += lvkeebid;
                    }
                }
                
                if (lvallid != "")
                {
                    lvallid = "4";
                    if ( lvcarcutid != "" || lvtruckid != "" || lvkeebid != "")
                    {
                        sp1.ActiveSheet.Cells[i, 24].Text += "," + lvallid;
                    }
                    else
                    {
                        sp1.ActiveSheet.Cells[i, 24].Text += lvallid;
                    }
                }

                if (lvContactrype == "รถตัดใน")
                {
                    string lvrodtudid = "5";
                   
                        if (lvtruckid != "" || lvkeebid != "" || lvallid != "")
                        {
                            sp1.ActiveSheet.Cells[i, 24].Text += "," + lvrodtudid;
                        }
                        else
                        {
                            sp1.ActiveSheet.Cells[i, 24].Text += lvrodtudid;
                        }
                }

                lvCountCar += 1;
                lvWeightIN += Gstr.fncToDouble(FncChangeKiloToTon(DT.Rows[i]["Q_WeightIN"].ToString()));
                lvWeightOUT += Gstr.fncToDouble(FncChangeKiloToTon(DT.Rows[i]["Q_WeightOUT"].ToString()));
                double lvNet = (Gstr.fncToDouble(FncChangeKiloToTon(DT.Rows[i]["Q_WeightIN"].ToString())) - Gstr.fncToDouble(FncChangeKiloToTon(DT.Rows[i]["Q_WeightOUT"].ToString())));
                if (Gstr.fncToDouble(DT.Rows[i]["Q_WeightOUT"].ToString()) == 0) lvNet = 0;
                lvSubWeight += lvNet;
                
                lbLoad.Text = "กำลังโหลดข้อมูล บิลที่ " + DT.Rows[i]["Q_BillingNo"].ToString() + " กรุณารอสักครู่";
                lbLoad.Refresh();

                progressBar1.Value += 1;
                Application.DoEvents();
            }

            //Progressbar
            pnStatus.Visible = false;

            lbCountCar.Text = lvCountCar.ToString();
            lbWeightIN.Text = lvWeightIN.ToString("#,##0.00");
            lbWeightOut.Text = lvWeightOUT.ToString("#,##0.00");
            lbSubWeight.Text = lvSubWeight.ToString("#,##0.00");

            ChkSelectAll.Checked = false;

            //เช็คบิลกระโดด
            FncCheckJumpBill();

            //เช็คบิลซ้ำ
            FncCheckDuptBill();

            //เช็คเลขที่ตัวอย่างซ้ำ
            FncCheckDuptSampleNo();

            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btnSearch.Enabled = true;
            btnExportXls.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        private string FncChangeTrueFalse(string lvData)
        {
            string lvReturn = "";

            if (lvData == "True")
                lvReturn = "1";
            else
                lvReturn = "";

            return lvReturn;
        }

        private void sp1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            GVar.gvRowIndex = e.Row;

            if(e.Button == MouseButtons.Right && GVar.gvTypeProgram == "Q")
            {
                cRightMenu.Show(Cursor.Position);
            }
        }

        private void ChkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSelectAll.Checked)
            {
                for (int i = 0; i < sp1.ActiveSheet.RowCount; i++)
                {
                    sp1.ActiveSheet.Cells[i, 0].Text = "True";
                }
            }
            else
            {
                for (int i = 0; i < sp1.ActiveSheet.RowCount; i++)
                {
                    sp1.ActiveSheet.Cells[i, 0].Text = "False";
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
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

            sp1.ActiveSheet.RowCount = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //เช็คเลือกข้อมูล
            int lvChkSelect = 0;
            string lvChkError = "";
            string lvErrorQ = "";
            string lvErrorCarNum = "";
            string lvErrorBill = "";
            for (int i = 0; i < sp1.ActiveSheet.RowCount; i++)
            {
                if (sp1.ActiveSheet.Cells[i,0].Text == "True")
                {
                    if (Gstr.fncToDouble(sp1.ActiveSheet.Cells[i, 10].Text) == 0)
                    {
                        lvChkError = "Weight";
                        lvErrorBill = sp1.ActiveSheet.Cells[i, 1].Text;
                        break;
                    }
                    else if (sp1.ActiveSheet.Cells[i, 1].Text == "")
                    {
                        lvChkError = "Bill";
                        lvErrorQ = sp1.ActiveSheet.Cells[i, 5].Text;
                        lvErrorCarNum = sp1.ActiveSheet.Cells[i, 6].Text;
                        lvErrorBill = sp1.ActiveSheet.Cells[i, 1].Text;
                        break;
                    }
                    else if (sp1.ActiveSheet.Cells[i, 3].Text == "")
                    {
                        lvChkError = "SampleNo";
                        lvErrorBill = sp1.ActiveSheet.Cells[i, 1].Text;
                        break;
                    }

                    lvChkSelect += 1;
                }
            }
            if (lvChkSelect <= 0 && lvChkError == "")
            {
                MessageBox.Show("กรุณาเลือกข้อมูลที่ต้องการบันทึก", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                return;
            }
            else if (lvChkError == "Weight")
            {
                MessageBox.Show("ไม่พบข้อมูลน้ำหนัก เลขที่บิล : " + lvErrorBill + " กรุณาตรวจสอบข้อมูล", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                return;
            }
            else if (lvChkError == "Bill")
            {
                MessageBox.Show("ไม่พบข้อมูลเลขที่บิล คิวที่ : " + lvErrorQ + " ทะเบียน : "+ lvErrorCarNum + " กรุณาตรวจสอบข้อมูล", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                return;
            }
            else if (lvChkError == "SampleNo")
            {
                MessageBox.Show("ไม่พบข้อมูลเลขตัวอย่าง เลขที่บิล : " + lvErrorBill + " กรุณาตรวจสอบข้อมูล", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                return;
            }
            //else if (chkShowClose.Checked)
            //{
            //    MessageBox.Show("การแสดงข้อมูลที่ปิดแล้วยังถูกเลือกอยู่ กรุณานำติ๊กถูกออกก่อน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    this.Cursor = Cursors.Default;
            //    return;
            //}
            //ยืนยัน
            string lvTxtAlert = "ยืนยันการบันทึกข้อมูล ?";
            //if (GVar.gvDateBill != Gstr.fncChangeTDate(txtDate.Text)) lvTxtAlert = "  **วันที่ไม่ตรงกับใบเสร็จ**"+ Environment.NewLine + Environment.NewLine + "ยืนยันการทำรายการต่อหรือไม่?";
            DialogResult dialogResult = MessageBox.Show(lvTxtAlert, "Confirm?", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.No)
            {
                this.Cursor = Cursors.Default;
                return;
            }
            
            int lvCount = 0;

            //Progressbar
            progressBar1.Maximum = FncCountChk();
            progressBar1.Value = 0;
            pnStatus.Visible = true;

            //บันทึก
            if (sp1.ActiveSheet.RowCount > 0)
            {
                for (int i = 0; i < sp1.ActiveSheet.RowCount; i++)
                {
                    string lvChk = sp1.ActiveSheet.Cells[i, 0].Text;
                    string lvBillingNo = sp1.ActiveSheet.Cells[i, 1].Text;

                    if (lvChk == "True")
                    {
                        lvCount += 1;

                        //อัพ Status ตัวแม่
                        string lvSQL = "update Queue_Diary set Q_CloseStatus = '1' where Q_BillingNo = '"+ lvBillingNo +"' ";
                        string lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                        //เช็คมีพ่วงหรือไม่?
                        string lvQ = sp1.ActiveSheet.Cells[i, 5].Text + ".1";
                        lvSQL = "update Queue_Diary set Q_CloseStatus = '1' where Q_No = '" + lvQ + "' and Q_CloseStatus = '' ";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                        //อัพ Status บิลลาน
                        lvSQL = "update MiniCane_BillHD set M_CloseStatus = '1' where Cast(M_BillNo as int) = '" + Gstr.fncToInt(lvBillingNo) + "' ";
                        lvResault = GsysSQL.fncExecuteQueryData(lvSQL, GVar.gvOnline);

                        lbLoad.Text = "กำลังบันทึกข้อมูล บิลที่ " + lvBillingNo + " กรุณารอสักครู่";
                        lbLoad.Refresh();

                        progressBar1.Value += 1;
                        Application.DoEvents();
                    }
                }
            }

            //Export ข้อมูลก่อน
            btnExportXls_Click(sender, e);

            //Progressbar
            pnStatus.Visible = false;

            //System.Diagnostics.Process.Start("D:\\Test\\");
            System.Diagnostics.Process.Start("L:\\");
            MessageBox.Show("บันทึกข้อมูลเรียบร้อย จำนวน " + lvCount + " รายการ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
            this.Cursor = Cursors.Default;
        }

        private void chkShowClose_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void txtDateS_ValueChanged(object sender, EventArgs e)
        {
            //txtDateE.Text = txtDateS.Text;
        }

        private void frmCloseStatus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btnSave_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                GVar.gvESC = true;
            }
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

        private void FncGetMaxMin(string lvMinMax)
        {

        }

        private void btnExportXls_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            btnExportXls.Enabled = false;
            
            //หา Min Max Bill
            int[] lvArrBill = new int[sp1.Sheets[0].RowCount];

            for (int i = 0; i < sp1.ActiveSheet.RowCount; i++)
            {
                if (sp1.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    int lvBill = Gstr.fncToInt(sp1.Sheets[0].Cells[i, 1].Text);

                    lvArrBill[i] = lvBill;
                }
            }

            int lvMin = 0;
            int lvMax = 0;

            try
            {
                lvMin = lvArrBill.Where(x => x > 0).Min();
                lvMax = lvArrBill.Max();
            }
            catch
            {
                MessageBox.Show("กรุณาเลือกอย่างน้อย 1 บิล","แจ้งเตือน",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
                btnExportXls.Enabled = true;
                return;
            }

            int lvBillStart = lvMin;
            int lvBillEnd = lvMax;

            string lvPath = "L:\\";
            //string lvPath = "D:\\TESTD";
            string lvFileName = "NEWTRUCK";
            if (File.Exists(lvPath + "NEWTRUCK.dbf"))
            {
                File.Delete(lvPath + "NEWTRUCK.dbf");
            }

            #region Add DataTable
            DataTable DTExport = new DataTable();
            DTExport.Columns.Add("CAR_NO", typeof(String)); //3
            DTExport.Columns.Add("BILLNO", typeof(Double)); //1
            DTExport.Columns.Add("IN_NO", typeof(Double)); //5
            DTExport.Columns.Add("QUO_NO", typeof(Double)); //2
            DTExport.Columns.Add("TYPE", typeof(String)); //15,8
            DTExport.Columns.Add("PLATE", typeof(String)); //7
            DTExport.Columns.Add("DATE_IN", typeof(String)); //4
            DTExport.Columns.Add("TIME_IN", typeof(String)); //4
            DTExport.Columns.Add("TOTAL_WT", typeof(Double)); //9
            DTExport.Columns.Add("DATE_OUT", typeof(String)); //17
            DTExport.Columns.Add("TIME_OUT", typeof(String)); //18
            DTExport.Columns.Add("CAR_WT", typeof(Double)); //10
            DTExport.Columns.Add("NET_WT", typeof(Double)); //11

            DTExport.Columns.Add("PLOT", typeof(String)); //13
            DTExport.Columns.Add("W1WEIGHT", typeof(Double)); //21
            DTExport.Columns.Add("W2WEIGHT", typeof(Double)); //-
            DTExport.Columns.Add("REMARK1", typeof(String)); //22
            DTExport.Columns.Add("REMARK2", typeof(String)); //23
            DTExport.Columns.Add("APPROVE1", typeof(String)); //-
            DTExport.Columns.Add("APPROVE2", typeof(String)); //-
            DTExport.Columns.Add("TYPE1", typeof(Double)); //-
            DTExport.Columns.Add("TYPE2", typeof(Double)); //-
            DTExport.Columns.Add("AREA", typeof(String)); //25

            DTExport.Columns.Add("FINISH_TIM", typeof(String)); //26
            DTExport.Columns.Add("LOKHIB_NO", typeof(String)); //27
            DTExport.Columns.Add("HARVES_PLATE", typeof(String)); //28
            DTExport.Columns.Add("HARVESTOR_", typeof(String)); //29
            DTExport.Columns.Add("HARV_PRICE", typeof(Double)); //30
            DTExport.Columns.Add("TRUCK_NO", typeof(String)); //31
            DTExport.Columns.Add("TRUCK_PRICE", typeof(Double)); //32
            DTExport.Columns.Add("GRABBER_NO", typeof(String)); //33
            DTExport.Columns.Add("GRABBER_PRICE", typeof(Double)); //34
            DTExport.Columns.Add("MAORUAM_NO", typeof(String)); //35
            DTExport.Columns.Add("MAORUAM_PRICE", typeof(Double)); //36
            DTExport.Columns.Add("CONTROL_NO", typeof(String)); //37
            DTExport.Columns.Add("CRANE_QU_NO", typeof(String)); //--
            DTExport.Columns.Add("LAST_AREA", typeof(String)); //--
            DTExport.Columns.Add("T1_NET_WT", typeof(Double)); //38 
            DTExport.Columns.Add("T2_NET_WT", typeof(Double)); //39
            DTExport.Columns.Add("STATION", typeof(String)); //40
            #endregion

            sp1.Sheets[1].RowCount = 0;
            sp1.Sheets[1].RowCount = sp1.Sheets[0].RowCount;

            progressBar1.Maximum = sp1.Sheets[0].RowCount;
            progressBar1.Value = 0;
            pnStatus.Visible = true;

            for (int i = 0; i < sp1.Sheets[0].RowCount; i++)
            {
                string lvChk = sp1.ActiveSheet.Cells[i, 0].Text;
                string lvMainQuota = sp1.ActiveSheet.Cells[i, 2].Text; //รหัสโควต้าหลัก

                if (lvChk == "True")
                {
                    sp1.Sheets[1].Cells[i, 13].Text = sp1.Sheets[0].Cells[i, 19].Text; //PK
                    sp1.Sheets[1].Cells[i, 1].Text = sp1.Sheets[0].Cells[i, 1].Text; //เลขที่บิล
                    
                    //ถ้ามีลูกพ่วงให้เอาตัวลูกไปด้วย
                    string lvSampleNo = sp1.Sheets[0].Cells[i, 3].Text;
                    if (lvSampleNo.Length == 8) lvSampleNo = Gstr.Left(lvSampleNo, 4);

                    string lvSampleNo2 = "";
                    lvSampleNo2 = GsysSQL.fncFindSample2(sp1.Sheets[0].Cells[i, 5].Text + ".1", chkShowClose.Checked, GVar.gvOnline);

                    //ถ้าแม่ไม่ใช่ชั่งพ่วง ไม่ต้องเอาตัวอย่างลูกมาแสดง
                    string[] lvArr = sp1.Sheets[0].Cells[i, 5].Text.Split('.');
                    string lvChkWeightAll = GsysSQL.fncCheckWeightALL(lvArr[0], GVar.gvOnline);
                    if (lvChkWeightAll == "") lvSampleNo2 = "";

                    //ถ้ามีลูกพ่วงให้เอาตัวลูกไปด้วย
                    string lvCarNum = sp1.Sheets[0].Cells[i, 7].Text;
                    string lvCarNum2 = GsysSQL.fncFindCarNum2(sp1.Sheets[0].Cells[i, 5].Text + ".1", GVar.gvOnline);
                    if (lvCarNum2 != "" && !lvCarNum.Contains("/")) lvCarNum = lvCarNum + "/" + lvCarNum2;

                    sp1.Sheets[1].Cells[i, 0].Text = lvSampleNo; //เลขที่ตัวอย่าง
                    sp1.Sheets[1].Cells[i, 2].Text = sp1.Sheets[0].Cells[i, 5].Text; //เลขที่คิวเข้าชั่ง 
                    sp1.Sheets[1].Cells[i, 3].Text = sp1.Sheets[0].Cells[i, 13].Text; //รหัสโควตา
                    sp1.Sheets[1].Cells[i, 4].Text = sp1.Sheets[0].Cells[i, 14].Text; //รหัสอ้อย
                    sp1.Sheets[1].Cells[i, 5].Text = lvCarNum; //ทะเบียนรถ
                    sp1.Sheets[1].Cells[i, 6].Text = sp1.Sheets[0].Cells[i, 15].Text; //วันที่ชั่งเข้า (YYYYMMDD)
                    sp1.Sheets[1].Cells[i, 7].Text = sp1.Sheets[0].Cells[i, 16].Text; //เวลาชั่งเข้า (HH:MM:SS)
                    sp1.Sheets[1].Cells[i, 8].Text = sp1.Sheets[0].Cells[i, 9].Text; //น้ำหนักรถหนัก (อ้อย + รถ)
                    sp1.Sheets[1].Cells[i, 9].Text = sp1.Sheets[0].Cells[i, 17].Text; //วันที่ชั่งออก (YYYYMMDD)
                    sp1.Sheets[1].Cells[i, 10].Text = sp1.Sheets[0].Cells[i, 18].Text; //เวลาชั่งออก (HH:MM:SS) 
                    sp1.Sheets[1].Cells[i, 11].Text = sp1.Sheets[0].Cells[i, 10].Text; //น้ำหนักรถเปล่า
                    sp1.Sheets[1].Cells[i, 12].Text = sp1.Sheets[0].Cells[i, 11].Text; //น้ำหนักอ้อยสุทธิ

                    if (sp1.Sheets[0].Cells[i, 1].Text == "74481")
                    {

                    }

                    //เช็คว่าเป็นรถมาจากลานหรือไม่
                    string lvChkCarLan = GsysSQL.fncCheckBillCarLan(sp1.Sheets[0].Cells[i, 1].Text, GVar.gvOnline);

                    //ถ้าเป็นทะเบียนรถลาน แต่ไม่ใช่โควต้าลานให้ลบออก
                    if (lvMainQuota != "7699")
                        lvChkCarLan = "";
                    
                    //ถ้าเป็นลานแล้วชั่งแยก ให้เช็คที่ทะเบียนพ่วงด้วย
                    if ((lvChkCarLan == "" && sp1.Sheets[0].Cells[i, 13].Text == "7699"))
                    {
                        if (sp1.Sheets[0].Cells[i, 5].Text.Contains(".1"))
                            lvChkCarLan = GsysSQL.fncCheckBillCarLanByCarNum(lvCarNum, GVar.gvOnline);
                    }

                    if (lvChkCarLan == "")
                    {
                        DataRow drExport = DTExport.NewRow();

                        //ถ้าไม่ใช่รถมาจากลาน
                        drExport["CAR_NO"] = lvSampleNo; //เลขที่ตัวอย่าง บิลแม่
                        drExport["BILLNO"] = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 1].Text); //เลขที่บิล
                        drExport["IN_NO"] = sp1.Sheets[0].Cells[i, 5].Text.Replace(".", "0"); //เลขที่คิวเข้าชั่ง 
                        drExport["QUO_NO"] = sp1.Sheets[0].Cells[i, 13].Text; //รหัสโควตา
                        drExport["TYPE"] = sp1.Sheets[0].Cells[i, 14].Text; //รหัสอ้อย

                        drExport["PLATE"] = lvCarNum; //ทะเบียนรถ
                        drExport["DATE_IN"] = sp1.Sheets[0].Cells[i, 15].Text; //วันที่ชั่งเข้า (YYYYMMDD)
                        drExport["TIME_IN"] = Gstr.FncGetTimeOnly(sp1.Sheets[0].Cells[i, 16].Text, "HH:mm"); //เวลาชั่งเข้า (HH:MM:SS)
                        drExport["TOTAL_WT"] = FncChangeTonToKilo(sp1.Sheets[0].Cells[i, 9].Text); //น้ำหนักรถหนัก (อ้อย + รถ)
                        drExport["DATE_OUT"] = sp1.Sheets[0].Cells[i, 17].Text; //วันที่ชั่งออก (YYYYMMDD)
                        drExport["TIME_OUT"] = Gstr.FncGetTimeOnly(sp1.Sheets[0].Cells[i, 18].Text, "HH:mm"); //เวลาชั่งออก (HH:MM:SS)
                        drExport["CAR_WT"] = FncChangeTonToKilo(sp1.Sheets[0].Cells[i, 10].Text); //น้ำหนักรถเปล่า
                        drExport["NET_WT"] = FncChangeTonToKilo(sp1.Sheets[0].Cells[i, 11].Text); //น้ำหนักอ้อยสุทธิ

                        drExport["PLOT"] = sp1.Sheets[0].Cells[i, 13].Text; //ใช้บอกเลขที่บิลแม่ในกรณีที่เป็นบิลลานหรือขนถ่ายรวม
                        drExport["W1WEIGHT"] = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 22].Text); //เรียกเก็บค่าบรรทุก
                        drExport["W2WEIGHT"] = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 21].Text); //Bonsucro
                        drExport["REMARK1"] = sp1.Sheets[0].Cells[i, 23].Text; //เลขใบนำตัด
                        drExport["REMARK2"] = sp1.Sheets[0].Cells[i, 24].Text; //ใช้บ่งบอก Type เหมาต่างๆ
                        drExport["APPROVE1"] = "";
                        drExport["APPROVE2"] = "";
                        drExport["TYPE1"] = 0;
                        drExport["TYPE2"] = 0;
                        drExport["AREA"] = sp1.Sheets[0].Cells[i, 25].Text; //รหัสแปลงอ้อย

                        drExport["FINISH_TIM"] = lvSampleNo2;
                        drExport["LOKHIB_NO"] = sp1.Sheets[0].Cells[i, 27].Text; //โรงหรือรางที่ผลิต
                        drExport["HARVES_PLATE"] = sp1.Sheets[0].Cells[i, 28].Text; //เลขรถตัด
                        drExport["HARVESTOR_"] = sp1.Sheets[0].Cells[i, 29].Text; //โควต้าผู้รับเหมาตัด

                        ////ฟังก์ชั่นเปลี่ยนราคาให้เป็น 0
                        //string Contractor = sp1.Sheets[0].Cells[i, 29].Text;
                        //string QuotaNo = sp1.Sheets[0].Cells[i, 13].Text;
                        //string ContractorCheck = GsysSQL.fncGetQuotaChangeZero(QuotaNo, Contractor);
                        //if(Contractor == ContractorCheck)
                        //{
                        //    drExport["HARV_PRICE"] = "0";
                        //}
                        //else
                        //{
                        //    drExport["HARV_PRICE"] = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 30].Text);
                        //}

                        drExport["HARV_PRICE"] = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 30].Text); //ค่าเหมาตัด
                        drExport["TRUCK_NO"] = sp1.Sheets[0].Cells[i, 31].Text; //โควต้าผู้รับเหมาบรรทุก
                        drExport["TRUCK_PRICE"] = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 32].Text); //ค่ารับเหมาบรรทุก
                        drExport["GRABBER_NO"] = sp1.Sheets[0].Cells[i, 33].Text; //โควต้าผู้รับเหมาคีบ
                        drExport["GRABBER_PRICE"] = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 34].Text); //ค่ารับเหมาคีบ
                        drExport["MAORUAM_NO"] = sp1.Sheets[0].Cells[i, 35].Text; //โควต้าผู้รับเหมารวม
                        drExport["MAORUAM_PRICE"] = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 36].Text); //ค่ารับเหมารวม
                        drExport["CONTROL_NO"] = sp1.Sheets[0].Cells[i, 37].Text; //ใช้ในการเก็บข้อมูลกรณีบิลลาน
                        drExport["CRANE_QU_NO"] = "";
                        drExport["LAST_AREA"] = "";

                        drExport["T1_NET_WT"] = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 38].Text); //น้ำหนักอ้อยรถเล็กหน้าลานชั่งเข้า
                        drExport["T2_NET_WT"] = Gstr.fncToDouble(sp1.Sheets[0].Cells[i, 39].Text); //น้ำหนักอ้อย ผลรวม นน รวมของการชั่งหน้าโรงงาน

                        drExport["STATION"] = ""; //ว่าง 40

                        DTExport.Rows.Add(drExport);

                        //เช็คถ้าเป็นโควต้าลาน แล้วไม่แตกรายละเอียดให้แจ้ง เลขบิลลาน
                        //if (sp1.Sheets[0].Cells[i, 13].Text == "7699")
                        //{
                        //    string lvMsgA = "เลขที่บิล = " + sp1.Sheets[0].Cells[i, 1].Text + " โควต้า = " + sp1.Sheets[0].Cells[i, 13].Text + " ยังไม่ได้แตกข้อมูล" + Environment.NewLine + " กรุณาตรวจสอบความถูกต้อง";
                        //    lineNotify(lvMsgA);
                        //    lineNotify2(lvMsgA);
                        //}
                    }
                    else if (lvChkCarLan != "")
                    {
                        //ใช้ทะเบียนรถแม่เป้นหลัก
                        //lvCarNum = sp1.Sheets[0].Cells[i, 7].Text;

                        //ถ้าเป็นรถลานให้แจงข้อมูลโควต้าย่อยๆ ออกมา
                        string lvSQL = "Select * from MiniCane_BillDT DT Inner Join MiniCane_BillHD HD ON DT.M_DocNo = HD.M_DocNo Where HD.M_DocNo = '" + lvChkCarLan + "' And HD.M_Year = '63/64' and DT.M_Date > 20210101 ";
                        DataTable DT = new DataTable();
                        DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

                        int lvNumRow = DT.Rows.Count;
                        string lvBillOld = "";
                        int lvCountDoc = 1;
                        double lvTotalLanNew = 0;
                        for (int l = 0; l < lvNumRow; l++)
                        {
                            DataRow drExport = DTExport.NewRow();

                            drExport["CAR_NO"] = lvSampleNo; //เลขที่ตัวอย่าง

                            string lvDocAll = sp1.Sheets[0].Cells[i, 1].Text + lvCountDoc.ToString("0000"); //บิลใหญ่ + รัน Number
                            drExport["BILLNO"] = Gstr.fncToDouble(lvDocAll); //เลขที่บิล
                            if (lvBillOld != "" &&lvBillOld != sp1.Sheets[0].Cells[i, 1].Text)
                                lvCountDoc = 1;
                            else
                                lvCountDoc += 1;

                            drExport["IN_NO"] = sp1.Sheets[0].Cells[i, 5].Text.Replace(".", "0"); //เลขที่คิวเข้าชั่ง
                            drExport["QUO_NO"] = DT.Rows[l]["M_Quota"].ToString(); //รหัสโควตา
                            drExport["TYPE"] = sp1.Sheets[0].Cells[i, 14].Text; //รหัสอ้อย

                            drExport["PLATE"] = lvCarNum; //ทะเบียนรถ
                            drExport["DATE_IN"] = sp1.Sheets[0].Cells[i, 15].Text; //วันที่ชั่งเข้า (YYYYMMDD)
                            drExport["TIME_IN"] = Gstr.FncGetTimeOnly(sp1.Sheets[0].Cells[i, 16].Text, "HH:mm"); //เวลาชั่งเข้า (HH:MM:SS)
                            drExport["TOTAL_WT"] = FncChangeTonToKilo(sp1.Sheets[0].Cells[i, 9].Text); //น้ำหนักรถหนัก (อ้อย + รถ)
                            drExport["DATE_OUT"] = sp1.Sheets[0].Cells[i, 17].Text; //วันที่ชั่งออก (YYYYMMDD)
                            drExport["TIME_OUT"] = Gstr.FncGetTimeOnly(sp1.Sheets[0].Cells[i, 18].Text, "HH:mm"); //เวลาชั่งออก (HH:MM:SS)
                            drExport["CAR_WT"] = FncChangeTonToKilo(sp1.Sheets[0].Cells[i, 10].Text); //น้ำหนักรถเปล่า

                            double lvTotalWeightMain = Gstr.fncToDouble(FncChangeTonToKilo(sp1.Sheets[0].Cells[i, 11].Text)); //นน.รวม โรงงาน
                            double lvTotalWeightLan = Gstr.fncToDouble(DT.Rows[l]["M_TotalW"].ToString()); //นน.รวม ลาน
                            double lvWeightLan = Gstr.fncToDouble(DT.Rows[l]["M_TotalWeight"].ToString()); //นน.โควต้าย่อย

                            //หา % การส่งคำนวนจาก นน.ลาน ก่อน
                            double lvPercentLan = (lvWeightLan / lvTotalWeightLan) * 100;

                            //เอา % ที่ได้ คุณกับ นน.รวม โรงงาน จะได้ยอดน้ำหนักที่ส่ง อ้างอิงตาม นน.โรงงาน
                            double lvWeightCal = (lvPercentLan * lvTotalWeightMain) / 100;
                            lvWeightCal = Gstr.fncToDouble(lvWeightCal.ToString("#,###"));
                            //หาตัวสุดท้าย
                            int lvLastDigit = Gstr.fncToInt(Gstr.Right(lvWeightCal.ToString("#,###"), 1));
                            if (lvLastDigit < 5)
                                lvWeightCal = lvWeightCal - lvLastDigit;
                            else
                                lvWeightCal = (lvWeightCal - lvLastDigit) + 10;

                            double lvTotalNet = Gstr.fncToDouble(lvWeightCal.ToString("#,###"));

                            //เก็บข้อมูล นน.ลานที่ ปัดแล้วทั้งหมด
                            string lvBillNow = DT.Rows[l]["M_BillNo"].ToString();
                            string lvNextBill = "";
                            try
                            {
                                //ถ้า + 1 แล้ว Error แสดงว่าเป็นบรรทัดสุดท้ายแล้ว
                                lvNextBill = DT.Rows[l + 1]["M_BillNo"].ToString();
                            }
                            catch
                            {
                                lvNextBill = "999999";
                            }

                            if (lvBillNow == lvNextBill)
                            {
                                //ยังไม่ใช่บิลสุดท้าย + สะสมไปเรื่อยๆ
                                lvTotalLanNew = lvTotalLanNew + lvTotalNet;
                            }
                            else
                            {
                                lvTotalLanNew = lvTotalLanNew + lvTotalNet;

                                //ยังเป็นบิลสุดท้ายให้เอา (ผลรวมทั้งหมดที่ปัดเศษได้ - ผลรวม นน.ลาน) - นน.บิลสุดท้าย
                                double lvDiffTotal = (lvTotalLanNew - lvTotalWeightMain);
                                if (lvDiffTotal > 0)
                                {
                                    //ถ้าลบแล้ว เป็น +
                                    lvWeightCal = lvWeightCal - lvDiffTotal;
                                }
                                else
                                {
                                    //ถ้าลบแล้ว เป็น -
                                    lvWeightCal = lvWeightCal + Math.Abs(lvDiffTotal);
                                }
                            }

                            //นน.โควต้าย่อยที่คำนวนได้
                            lvTotalNet = Gstr.fncToDouble(lvWeightCal.ToString("#,###"));

                            //เก็บข้อมูลบิลเก่า
                            lvBillOld = sp1.Sheets[0].Cells[i, 1].Text;

                            drExport["NET_WT"] = lvTotalNet; //น้ำหนักอ้อยสุทธิ

                            drExport["PLOT"] = Gstr.fncToInt(sp1.Sheets[0].Cells[i, 1].Text); //เลขที่บิลโรงงาน
                            drExport["W1WEIGHT"] = 0;
                            drExport["W2WEIGHT"] = 0;
                            drExport["REMARK1"] = "";
                            drExport["REMARK2"] = "";
                            drExport["APPROVE1"] = "";
                            drExport["APPROVE2"] = "";
                            drExport["TYPE1"] = 0;
                            drExport["TYPE2"] = 0;
                            drExport["AREA"] = "";

                            drExport["FINISH_TIM"] = lvSampleNo2;
                            drExport["LOKHIB_NO"] = "";
                            drExport["HARVESTOR_"] = "";
                            drExport["HARV_PRICE"] = 0;
                            drExport["TRUCK_NO"] = lvMainQuota;
                            drExport["TRUCK_PRICE"] = 0;
                            drExport["CONTROL_NO"] = DT.Rows[l]["M_DocNo"].ToString(); //เลขที่บิลลาน
                            drExport["CRANE_QU_NO"] = "";
                            drExport["LAST_AREA"] = "";

                            drExport["T1_NET_WT"] = Gstr.fncToDouble(DT.Rows[l]["M_TotalWeight"].ToString()); //น้ำหนักสุทธิของชาวไร่ Kg เท่านั้น
                            drExport["T2_NET_WT"] = Gstr.fncToDouble(DT.Rows[l]["M_TotalW"].ToString()); //น้ำหนักสุทธิรวมจากลาน

                            drExport["STATION"] = "";

                            DTExport.Rows.Add(drExport);
                        }
                    }
                    
                    lbLoad.Text = "กำลัง Export ข้อมูลบิลที่ " + sp1.Sheets[0].Cells[i, 1].Text + " กรุณารอสักครู่";
                    lbLoad.Refresh();

                    progressBar1.Value += 1;
                    Application.DoEvents();
                }
            }

            pnStatus.Visible = false;
            this.Cursor = Cursors.Default;

            DataSet ds = new DataSet();
            ds.Tables.Add(DTExport);
            
            var bgw = new BackgroundWorker();
            bgw.DoWork += (_, __) =>
            {
                //Function นี้จะใช้เวลานาน
                DataSetIntoDBF(lvFileName, ds);
            };
            bgw.RunWorkerCompleted += (_, __) =>
            {
                //Rename File Newtruck
                string lvPathSorce = "L:\\";
                string lvPathDes = "L:\\";
                //string lvPathSorce = "D:\\TESTD\\";
                //string lvPathDes = "D:\\TESTD\\";
                lvFileName = "NEWTRUCK";
                if (File.Exists(lvPath + "NEWTRUCK.dbf"))
                {
                    string lvSorce = lvPathSorce + "NEWTRUCK.dbf";

                    string lvDay = DateTime.Now.ToString("dd");
                    string lvMonth = DateTime.Now.ToString("MM");
                    string lvYear = DateTime.Now.ToString("yyyy");

                    if (Gstr.fncToInt(lvYear) < 2500)
                        lvYear = (Gstr.fncToInt(lvYear) + 543).ToString();

                    //ปีเอาแค่ 61 พอ
                    lvYear = Gstr.Right(lvYear, 2);
                    string lvDes = lvPathDes + "Newtruck" + lvDay + lvMonth + lvYear + ".dbf";

                    //ถ้ามีก็ลบ
                    if (File.Exists(lvSorce))
                    {
                        File.Delete(lvDes);
                    }

                ReCopy:
                    //เช็คว่าไฟล์มีหรือไม่ ถ้าไม่มี
                    File.Copy(lvSorce, lvDes);

                    //เช็คว่า Copy เรียบร้อยหรือยัง ถ้าปลายทางยังไม่มีให้ Copy ใหม่อีกที ถ้ามีแล้วลบตัวต้นฉบับออก
                    if (File.Exists(lvDes))
                    {
                        //ลบตัวต้นฉบับ
                        File.Delete(lvSorce);
                    }
                    else
                    {
                        goto ReCopy;
                    }

                    try
                    {
                        GVar.gvSourcePath = lvSorce;
                        GVar.gvDesPath = lvDes;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ส่งข้อมูลไม่สำเร็จ เนื่องจากไม่พบ Drive ปลายทาง กรุณาติดต่อ IT " + Environment.NewLine + Environment.NewLine + ex.Message,"แจ้งเตือน",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }

                sp1.Sheets[1].Visible = true;
                btnExportXls.Enabled = true;

                string lvMsg = "ปิดยอดประจำวันที่ " + DateTime.Now.ToString() + " เลขบิลที่ " + lvBillStart + " ถึง " + lvBillEnd;
                lineNotify(lvMsg);
                lineNotify2(lvMsg);

            };
            bgw.RunWorkerAsync();

        }
        
        private void FncRenameFile(string lvPath)
        {
            
        }

        public static void DataSetIntoDBF(string fileName, DataSet dataSet)
        {
            string lvPath = "L:\\";
            //string lvPath = "D:\\TESTD\\";
            ArrayList list = new ArrayList();
            
            string createSql = "create table " + fileName + " (";

            foreach (DataColumn dc in dataSet.Tables[0].Columns)
            {
                string fieldName = dc.ColumnName;

                string type = dc.DataType.ToString();

                type = fncGetSizeField(fieldName, type);

                createSql = createSql + "[" + fieldName + "]" + " " + type + ",";

                list.Add(fieldName);
            }

            createSql = createSql.Substring(0, createSql.Length - 1) + ")";

            OleDbConnection con = new OleDbConnection(GetConnection(lvPath));

            OleDbCommand cmd = new OleDbCommand();

            cmd.Connection = con;

            con.Open();

            cmd.CommandText = createSql;

            cmd.ExecuteNonQuery();

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                string insertSql = "insert into " + fileName + " values(";

                for (int i = 0; i < list.Count; i++)
                {
                    insertSql = insertSql + "'" + ReplaceEscape(row[list[i].ToString()].ToString()) + "',";
                }

                insertSql = insertSql.Substring(0, insertSql.Length - 1) + ")";

                cmd.CommandText = insertSql;

                cmd.ExecuteNonQuery();
            }

            con.Close();
        }

        private static string GetConnection(string path)
        {
            //return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=dBASE IV;";//
            return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=dBASE IV;";
        }

        public static string ReplaceEscape(string str)
        {
            str = str.Replace("'", "''");
            return str;
        }

        private static string fncGetSizeField(string lvField, string lvType)
        {
            string lvReturn = "";

            string lvSize = "";
            if (lvField == "CAR_NO") lvSize = "50";
            else if (lvField == "BILLNO") lvSize = "20,2";
            else if (lvField == "IN_NO") lvSize = "20,2";
            else if (lvField == "QUO_NO") lvSize = "20,2";
            else if (lvField == "TYPE") lvSize = "3";
            else if (lvField == "PLATE") lvSize = "50";
            else if (lvField == "DATE_IN") lvSize = "8";
            else if (lvField == "TIME_IN") lvSize = "5";
            else if (lvField == "TOTAL_WT") lvSize = "20,2";
            else if (lvField == "DATE_OUT") lvSize = "8";
            else if (lvField == "TIME_OUT") lvSize = "5";
            else if (lvField == "CAR_WT") lvSize = "20,2";
            else if (lvField == "NET_WT") lvSize = "20,2";
            else if (lvField == "PLOT") lvSize = "50";
            else if (lvField == "W1WEIGHT") lvSize = "20,2";
            else if (lvField == "W2WEIGHT") lvSize = "20,2";
            else if (lvField == "REMARK1") lvSize = "250";
            else if (lvField == "REMARK2") lvSize = "250";
            else if (lvField == "APPROVE1") lvSize = "50";
            else if (lvField == "APPROVE2") lvSize = "50";
            else if (lvField == "TYPE1") lvSize = "20,2";
            else if (lvField == "TYPE2") lvSize = "20,2";
            else if (lvField == "AREA") lvSize = "50";
            else if (lvField == "FINISH_TIM") lvSize = "50";
            else if (lvField == "LOKHIB_NO") lvSize = "50";
            else if (lvField == "HARVES_PLATE") lvSize = "50";
            else if (lvField == "HARVESTOR_") lvSize = "50";
            else if (lvField == "HARV_PRICE") lvSize = "20,2";
            else if (lvField == "TRUCK_NO") lvSize = "50";
            else if (lvField == "TRUCK_PRICE") lvSize = "20,2";
            else if (lvField == "GRABBER_NO") lvSize = "50";
            else if (lvField == "GRABBER_PRICE") lvSize = "20,2";
            else if (lvField == "MAORUAM_NO") lvSize = "50";
            else if (lvField == "MAORUAM_PRICE") lvSize = "20,2";
            else if (lvField == "CONTROL_NO") lvSize = "50";
            else if (lvField == "CRANE_QU_NO") lvSize = "50";
            else if (lvField == "LAST_AREA") lvSize = "1";
            else if (lvField == "T1_NET_WT") lvSize = "20,2";
            else if (lvField == "T2_NET_WT") lvSize = "20,2";
            else if (lvField == "STATION") lvSize = "50"; 

            switch (lvType)
            {
                case "System.String":
                    lvReturn = "varchar("+ lvSize + ")";
                    break;

                case "System.Boolean":
                    lvReturn = "varchar(" + lvSize + ")";
                    break;

                case "System.Int32":
                    lvReturn = "int";
                    break;

                case "System.Double":
                    lvReturn = "numeric(" + lvSize + ")";//numeric
                    break;

                case "System.DateTime":
                    lvReturn = "TimeStamp";
                    break;
            }

            return lvReturn;
        }

        private void txtBillS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBillE.Focus();
            }
        }

        private void txtBillE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void txtSampleS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void txtQS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQE.Focus();
            }
        }

        private void txtQE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void txtCarS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void txtQuotaS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtQuotaE.Focus();
            }
        }

        private void txtQuotaE_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void CmbTypeCane_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        private void ChkA_CheckedChanged(object sender, EventArgs e)
        {
            //ChkB.Checked = false;
            if (ChkA.Checked) LoadData();
        }

        private void ChkB_CheckedChanged(object sender, EventArgs e)
        {
            //ChkA.Checked = false;
            if (ChkB.Checked) LoadData();
        }

        private void ChkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkAll.Checked) LoadData();
        }

        private string FncChangeTonToKilo(string lvWeight)
        {
            string lvReturn = "";

            lvReturn = (Gstr.fncToDouble(lvWeight) * 1000).ToString("#,##0.00");

            return lvReturn;
        }

        private string FncChangeKiloToTon(string lvWeight)
        {
            string lvReturn = "";

            lvReturn = (Gstr.fncToDouble(lvWeight) / 1000).ToString("#,##0.000");

            return lvReturn;
        }

        private int FncCountChk()
        {
            int lvReturn = 0;
            
            for (int i = 0; i < sp1.ActiveSheet.RowCount; i++)
            {
                if (sp1.ActiveSheet.Cells[i,0].Text == "True")
                {
                    lvReturn += 1;
                }
            }

            return lvReturn;
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
                    lvSQL += "and Cast(B_PK as int) > " + lvDocBillCheck + " order by B_PK";

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

        private void FncCheckDuptBill()
        {
            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "SELECT Q_BillingNo, COUNT(Q_BillingNo)  ";
            lvSQL += "FROM Queue_Diary ";
            lvSQL += "WHERE Q_CloseStatus = '' And Q_BillingNo <> '' And Q_Year = '' ";
            lvSQL += "GROUP BY Q_BillingNo ";
            lvSQL += "HAVING COUNT(Q_BillingNo) > 1 ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumRow = DT.Rows.Count;

            if (lvNumRow > 0)
            {
                string lvDoc = Gstr.fncToInt(DT.Rows[0]["Q_BillingNo"].ToString()).ToString();

                if (lvDoc != "")
                    lbAlert2.Text = "** เลขที่บิล " + lvDoc + " ซ้ำ กรุณาตรวจสอบ **";
                else
                    lbAlert2.Text = "";
            }
        }

        private void FncCheckDuptSampleNo()
        {
            //Get Data
            DataTable DT = new DataTable();

            string lvSQL = "SELECT Q_SampleNo, COUNT(Q_SampleNo)  ";
            lvSQL += "FROM Queue_Diary ";
            lvSQL += "WHERE Q_CloseStatus = '' And Q_SampleNo <> '' And Q_Year = '' ";
            lvSQL += "GROUP BY Q_SampleNo ";
            lvSQL += "HAVING COUNT(Q_SampleNo) > 1 ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumRow = DT.Rows.Count;

            if (lvNumRow > 0)
            {
                string lvDoc = Gstr.fncToInt(DT.Rows[0]["Q_SampleNo"].ToString()).ToString();

                if (lvDoc != "")
                    lbAlert2.Text = "** เลขที่ตัวอย่าง " + lvDoc + " ซ้ำ กรุณาตรวจสอบ **";
                else
                    lbAlert2.Text = "";
            }
            else
            {
                lbAlert2.Text = "";
            }
        }

        private void TLoadData_Tick(object sender, EventArgs e)
        {
            LoadData();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("L:\\");
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

        private void lineNotify2(string msg)
        {
            //string token = "Wuh3ocetjly3TUZgd9OV0YY3o9g6GouD3wPaIsthAna"; //ห้องคอม
            string token = "vRtm1XpGxkReljIAAsVjSK0rfWFBzpnXgaZdMXZWgkP"; //ส่วนตัว

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

        private void sp1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void sp1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                GVar.gvESC = true;
            }
        }

        private void chkShowLan_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void แกไขToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int lvIndex = sp1.ActiveSheet.ActiveRowIndex;
            GVar.gvQNo = sp1.ActiveSheet.Cells[lvIndex, 5].Text;

            this.Close();
        }

        private void chkBonsucro_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
