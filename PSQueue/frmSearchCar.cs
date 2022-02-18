using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSQueue
{
    public partial class frmSearchCar : Form
    {
        public frmSearchCar()
        {
            InitializeComponent();
        }

        private void frmSearchCar_Load(object sender, EventArgs e)
        {
            cmb_select.SelectedIndex = 0;
            txtSearch.Focus();

            fncLoadData(); //โหลดข้อมูล
            
        }

        private void fncLoadData()
        {
            DataTable DT = new DataTable();
            string lvSQL = "";
            if(cmb_select.SelectedIndex == 0)
            {
                lvSQL = "SELECT TOP 50 A.*, B.C_Name ";
            }
            else if(cmb_select.SelectedIndex == 1)
            {
                lvSQL = "SELECT TOP 100  A.*, B.C_Name ";
            }
            else
            {
                lvSQL = "SELECT A.*, B.C_Name ";
            }
            
            lvSQL += "FROM Cane_QueueData A ";
            lvSQL += "INNER JOIN Cane_CaneType B on A.C_CaneType = B.C_ID ";
            lvSQL += "Where 1=1 ";
            string lvSeartch = txtSearch.Text;

            if (lvSeartch != "")
                lvSQL += "And (C_TruckCarnum like '%"+ lvSeartch + "%' or C_TruckCarnum2 like '%"+ lvSeartch + "%') ";

            lvSQL += "And C_Queue = '' "; //ต้องเป็นใบที่ยังไม่ได้ใช้งาน
            lvSQL += "Order by C_ID ";

            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumrow = DT.Rows.Count;

            sp1.ActiveSheet.Rows.Count = lvNumrow;

            for (int i = 0; i < lvNumrow; i++)
            {
                sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["C_ID"].ToString(); //ใบนำตัด
                sp1.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["C_PlanId"].ToString(); //เลขที่แปลง
                sp1.ActiveSheet.Cells[i, 2].Text = DT.Rows[i]["C_PlanName"].ToString(); //ชื่อแปลง
                sp1.ActiveSheet.Cells[i, 3].Text = DT.Rows[i]["C_Quota"].ToString() + " : " + DT.Rows[i]["C_FullName"].ToString(); //โควต้า + ชื่อ
                sp1.ActiveSheet.Cells[i, 4].Text = DT.Rows[i]["C_Name"].ToString(); //ประเภทอ้อย
                sp1.ActiveSheet.Cells[i, 5].Text = DT.Rows[i]["C_TruckCarnum"].ToString(); //ทะเบียนรถแม่
                sp1.ActiveSheet.Cells[i, 6].Text = DT.Rows[i]["C_TruckCarnum2"].ToString(); //ทะเบียนรถลูก
                sp1.ActiveSheet.Cells[i, 7].Text = DT.Rows[i]["C_CutContactorId"].ToString(); //รหัสผู้รับเหมาตัด
                sp1.ActiveSheet.Cells[i, 8].Text = DT.Rows[i]["C_CarcutNumber"].ToString(); //เลขรถตัด
                sp1.ActiveSheet.Cells[i, 9].Text = DT.Rows[i]["C_Price"].ToString(); //ค่าตัด
                sp1.ActiveSheet.Cells[i, 10].Text = DT.Rows[i]["C_KeebContractorId"].ToString(); //รหัสผู้รับเหมาคีบ
                sp1.ActiveSheet.Cells[i, 11].Text = DT.Rows[i]["C_KeebContractorPrice"].ToString(); //ราคาคีบ
                sp1.ActiveSheet.Cells[i, 12].Text = DT.Rows[i]["C_ContractorId"].ToString(); //รหัสผู้รับเหมารถบรรทุก
                sp1.ActiveSheet.Cells[i, 13].Text = DT.Rows[i]["C_TruckPrice"].ToString(); //ค่าบรรทุก
                sp1.ActiveSheet.Cells[i, 14].Text = DT.Rows[i]["C_AllContractor"].ToString(); //ผู้รับเหมารวม
                sp1.ActiveSheet.Cells[i, 15].Text = DT.Rows[i]["C_AllPrice"].ToString(); //ค่าเหมารวม
                sp1.ActiveSheet.Cells[i, 16].Text = DT.Rows[i]["C_PayStatus"].ToString(); //สถานะเรียกเก็บรถบรรทุก
                sp1.ActiveSheet.Cells[i, 17].Text = DT.Rows[i]["C_ContracType"].ToString(); //สถานะการตัด
            }
            

            for (int l = 0; l < sp1.ActiveSheet.RowCount; l++)
            {
                string lvCar = sp1.ActiveSheet.Cells[l, 5].Text; //ทะเบียนรถ
                string StatusC = fncGetQueryDataCovidsound(lvCar, GVar.gvOnline); //ค้นหาสเตตัสรถบรรทุกที่เฝ้าระวัง Covid

                if (StatusC == "0")
                {
                    sp1.ActiveSheet.Rows[l].BackColor = Color.White; //ถ้าเป็นรถที่เฝ้าระวังให้เปลี่ยนเป็นสีแดง
                }
                else if (StatusC == "")
                {
                    sp1.ActiveSheet.Rows[l].BackColor = Color.Red; //ถ้าเป็นรถที่เฝ้าระวังให้เปลี่ยนเป็นสีแดง
                }
                else
                {
                    sp1.ActiveSheet.Rows[l].BackColor = Color.White; //ถ้าเป็นรถที่เฝ้าระวังให้เปลี่ยนเป็นสีแดง
                }
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                fncLoadData();
            }
        }

        private void sp1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int lvRow = sp1.ActiveSheet.ActiveRowIndex;

            GVar.gvSCode = sp1.ActiveSheet.Cells[lvRow,0].Text;
            string lvCar = sp1.ActiveSheet.Cells[lvRow, 5].Text; //ทะเบียนรถ
            string StatusC = fncGetQueryDataCovidsound(lvCar, GVar.gvOnline); //ค้นหาสเตตัสรถบรรทุกที่เฝ้าระวัง Covid

            if (StatusC == "")
            {
                GVar.gvCovidsound = "1";
            }
            else
            {
                GVar.gvCovidsound = "";
            }

            this.Close();
        }

        private string fncGetQueryDataCovidsound(string lvCar, bool lvOnline)
        {
            try
            {
                if (lvOnline)
                {
                    #region ONLINE
                    #region //Connect Database 
                    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader dr;
                    #endregion

                    string lvReturn = "0";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT * FROM PS_Covidsound WHERE C_car_registration = '" + lvCar + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Co_status"].ToString();
                        }
                    }
                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
                else
                {
                    #region OFFLINE
                    #region //Connect Database 
                    OleDbConnection con = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataReader dr;
                    #endregion

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT * FROM PS_Covidsound WHERE C_car_registration = '" + lvCar + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Co_status"].ToString();
                        }
                    }
                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
            }
            catch
            {
                return "";
            }
        }

        private void cmb_select_SelectedIndexChanged(object sender, EventArgs e)
        {
            fncLoadData();
        }

        private void fncLoadDataCutDoc()
        {
            DataTable DT = new DataTable();
            string lvSQL = "";
            if (cmb_select.SelectedIndex == 0)
            {
                lvSQL = "SELECT TOP 50 A.*, B.C_Name ";
            }
            else if (cmb_select.SelectedIndex == 1)
            {
                lvSQL = "SELECT TOP 100  A.*, B.C_Name ";
            }
            else
            {
                lvSQL = "SELECT A.*, B.C_Name ";
            }

            lvSQL += "FROM Cane_QueueData A ";
            lvSQL += "INNER JOIN Cane_CaneType B on A.C_CaneType = B.C_ID ";
            lvSQL += "Where 1=1 ";
            string lvSeartch = txtSearchDoc.Text;

            if (lvSeartch != "")
                lvSQL += "And A.C_ID = '" + lvSeartch + "'  ";

            lvSQL += "And C_Queue = '' "; //ต้องเป็นใบที่ยังไม่ได้ใช้งาน
            lvSQL += "Order by C_ID ";

            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumrow = DT.Rows.Count;

            sp1.ActiveSheet.Rows.Count = lvNumrow;

            for (int i = 0; i < lvNumrow; i++)
            {
                sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["C_ID"].ToString(); //ใบนำตัด
                sp1.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["C_PlanId"].ToString(); //เลขที่แปลง
                sp1.ActiveSheet.Cells[i, 2].Text = DT.Rows[i]["C_PlanName"].ToString(); //ชื่อแปลง
                sp1.ActiveSheet.Cells[i, 3].Text = DT.Rows[i]["C_Quota"].ToString() + " : " + DT.Rows[i]["C_FullName"].ToString(); //โควต้า + ชื่อ
                sp1.ActiveSheet.Cells[i, 4].Text = DT.Rows[i]["C_Name"].ToString(); //ประเภทอ้อย
                sp1.ActiveSheet.Cells[i, 5].Text = DT.Rows[i]["C_TruckCarnum"].ToString(); //ทะเบียนรถแม่
                sp1.ActiveSheet.Cells[i, 6].Text = DT.Rows[i]["C_TruckCarnum2"].ToString(); //ทะเบียนรถลูก
                sp1.ActiveSheet.Cells[i, 7].Text = DT.Rows[i]["C_CutContactorId"].ToString(); //รหัสผู้รับเหมาตัด
                sp1.ActiveSheet.Cells[i, 8].Text = DT.Rows[i]["C_CarcutNumber"].ToString(); //เลขรถตัด
                sp1.ActiveSheet.Cells[i, 9].Text = DT.Rows[i]["C_Price"].ToString(); //ค่าตัด
                sp1.ActiveSheet.Cells[i, 10].Text = DT.Rows[i]["C_KeebContractorId"].ToString(); //รหัสผู้รับเหมาคีบ
                sp1.ActiveSheet.Cells[i, 11].Text = DT.Rows[i]["C_KeebContractorPrice"].ToString(); //ราคาคีบ
                sp1.ActiveSheet.Cells[i, 12].Text = DT.Rows[i]["C_ContractorId"].ToString(); //รหัสผู้รับเหมารถบรรทุก
                sp1.ActiveSheet.Cells[i, 13].Text = DT.Rows[i]["C_TruckPrice"].ToString(); //ค่าบรรทุก
                sp1.ActiveSheet.Cells[i, 14].Text = DT.Rows[i]["C_AllContractor"].ToString(); //ผู้รับเหมารวม
                sp1.ActiveSheet.Cells[i, 15].Text = DT.Rows[i]["C_AllPrice"].ToString(); //ค่าเหมารวม
                sp1.ActiveSheet.Cells[i, 16].Text = DT.Rows[i]["C_PayStatus"].ToString(); //สถานะเรียกเก็บรถบรรทุก
                sp1.ActiveSheet.Cells[i, 17].Text = DT.Rows[i]["C_ContracType"].ToString(); //สถานะการตัด
            }


            for (int l = 0; l < sp1.ActiveSheet.RowCount; l++)
            {
                string lvCar = sp1.ActiveSheet.Cells[l, 5].Text; //ทะเบียนรถ
                string StatusC = fncGetQueryDataCovidsound(lvCar, GVar.gvOnline); //ค้นหาสเตตัสรถบรรทุกที่เฝ้าระวัง Covid

                if (StatusC == "0")
                {
                    sp1.ActiveSheet.Rows[l].BackColor = Color.White; //ถ้าเป็นรถที่เฝ้าระวังให้เปลี่ยนเป็นสีแดง
                }
                else if (StatusC == "")
                {
                    sp1.ActiveSheet.Rows[l].BackColor = Color.Red; //ถ้าเป็นรถที่เฝ้าระวังให้เปลี่ยนเป็นสีแดง
                }
                else
                {
                    sp1.ActiveSheet.Rows[l].BackColor = Color.White; //ถ้าเป็นรถที่เฝ้าระวังให้เปลี่ยนเป็นสีแดง
                }
            }
        }

        private void txtSearchDoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                fncLoadDataCutDoc();
            }
        }
    }
}
