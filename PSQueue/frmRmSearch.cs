using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSQueue
{
    public partial class frmRmSearch : Form
    {
        public frmRmSearch()
        {
            InitializeComponent();
        }

        private void frmCutCarSearch_Load(object sender, EventArgs e)
        {
            fncLoadDataBase();
        }

        private void fncLoadDataBase()
        {
            DataTable DT = new DataTable();
            var lvSQL = "";
            if (GVar.gvModeSearchRm == "CarcutRmIn")
            {
                lvSQL = "SELECT * FROM Cane_CarContractor WHERE P_CarContractorType = 'เหมา ตัดใน' ORDER BY P_CarContractorID ASC ";
            }
            else if (GVar.gvModeSearchRm == "CarcutRmOut")
            {
                lvSQL = "Select * From Cane_CarContractor Where P_CarContractorType != 'เหมา ตัดใน' AND P_CarContractorType NOT IN ( 'บรรทุก', 'คีบ' ) ORDER BY P_CarContractorID ASC ";
            }
            else if(GVar.gvModeSearchRm == "CarTruckRm")
            {
                lvSQL = "Select * From Cane_CarContractor Where P_CarContractorType != 'เหมา ตัดใน' AND P_CarContractorType NOT IN ( 'คีบ', 'ตัด' ) ORDER BY P_CarContractorID ASC ";
            }
            else if(GVar.gvModeSearchRm == "CarKeebRm")
            {
                lvSQL = "Select * From Cane_CarContractor Where P_CarContractorType like '%คีบ%'  ";
            }
            else if (GVar.gvModeSearchRm == "CarSearchRm")
            {
                lvSQL = "Select * From Cane_Truckbase ";
            }
            else
            {
                lvSQL = "Select * From Cane_CarContractor WHERE P_CarContractorName != 'รับเหมา' AND P_CarContractorLastName != 'โรงงาน' ORDER BY P_CarContractorID ASC ";
            }

            DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

            int lvNumrow = DT.Rows.Count;

            sp1.ActiveSheet.Rows.Count = lvNumrow;

            for (int i = 0; i < lvNumrow; i++)
            {
                if(GVar.gvModeSearchRm != "CarSearchRm")
                {
                    sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["P_CarContractorID"].ToString();
                    sp1.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["P_CarContractorName"].ToString() + " " + DT.Rows[i]["P_CarContractorLastName"].ToString();
                }
                else
                {
                    sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["C_TruckCarNum"].ToString();

                    string lvCode = DT.Rows[i]["C_OwnedCode"].ToString();
                    string rmsearchname = fncGetRmName(lvCode);
                    sp1.ActiveSheet.Cells[i, 1].Text = rmsearchname;
                }
               
            }
        }

        private void sp1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
           //ถ้าเป็นรถตัด
           if(GVar.gvModeSearchRm == "CarcutRmIn" || GVar.gvModeSearchRm == "CarcutRmOut")
            {
                GVar.gvCarcutRm = sp1.ActiveSheet.Cells[e.Row, 0].Text + ":" + sp1.ActiveSheet.Cells[e.Row, 1].Text;
                this.Close();
            }
           else if(GVar.gvModeSearchRm == "CarTruckRm")
            {
                GVar.gvCartruckRm = sp1.ActiveSheet.Cells[e.Row, 0].Text + ":" + sp1.ActiveSheet.Cells[e.Row, 1].Text;
                this.Close();
            }
           else if(GVar.gvModeSearchRm == "CarKeebRm")
            {
                GVar.gvCarkeepRm = sp1.ActiveSheet.Cells[e.Row, 0].Text + ":" + sp1.ActiveSheet.Cells[e.Row, 1].Text;
                this.Close();
            }
           else if(GVar.gvModeSearchRm == "CarSearchRm")
            {
                GVar.gvCarSearchRm = sp1.ActiveSheet.Cells[e.Row, 1].Text;
                this.Close();
            }
            else
            {
                GVar.gvAllRm = sp1.ActiveSheet.Cells[e.Row, 0].Text + ":" + sp1.ActiveSheet.Cells[e.Row, 1].Text;
                this.Close();
            }

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                string lvSearch = txtSearch.Text;

                if (e.KeyCode == Keys.Enter)
                {
                    DataTable DT = new DataTable();
                    var lvSQL = "";
                    if (GVar.gvModeSearchRm == "CarcutRmIn")
                    {
                        lvSQL = "Select * From Cane_CarContractor WHERE P_CarContractorName = 'รับเหมา' AND P_CarContractorLastName != 'โรงงาน' And (P_CarContractorName LIKE '%" + lvSearch + "%' OR P_CarContractorID LIKE '%" + lvSearch + "%') ORDER BY P_CarContractorID ASC ";
                    }
                    else if (GVar.gvModeSearchRm == "CarcutRmOut")
                    {
                        lvSQL = "Select * From Cane_CarContractor Where P_CarContractorName != 'รับเหมา' AND P_CarContractorType NOT IN ( 'บรรทุก', 'คีบ' ) And (P_CarContractorName LIKE '%" + lvSearch + "%' OR P_CarContractorID LIKE '%" + lvSearch + "%') ORDER BY P_CarContractorID ASC ";
                    }
                    else if (GVar.gvModeSearchRm == "CarTruckRm")
                    {
                        lvSQL = "Select * From Cane_CarContractor Where P_CarContractorName != 'รับเหมา' AND P_CarContractorType != 'คีบ' And (P_CarContractorName LIKE '%" + lvSearch + "%' OR P_CarContractorID LIKE '%" + lvSearch + "%') ORDER BY P_CarContractorID ASC ";
                    }
                    else if(GVar.gvModeSearchRm == "CarKeebRm")
                    {
                        lvSQL = "Select * From Cane_CarContractor Where P_CarContractorType like '%คีบ%' And (P_CarContractorName LIKE '%" + lvSearch + "%' OR P_CarContractorID LIKE '%" + lvSearch + "%') ORDER BY P_CarContractorID ASC ";
                    }
                    else if (GVar.gvModeSearchRm == "CarSearchRm")
                    {
                        lvSQL = "Select * From Cane_Truckbase WHERE C_TruckCarNum LIKE '%" + lvSearch + "%' ";
                    }
                    else
                    {
                        lvSQL = "Select * From Cane_CarContractor WHERE P_CarContractorName != 'รับเหมา' AND P_CarContractorLastName != 'โรงงาน' And (P_CarContractorName LIKE '%" + lvSearch + "%' OR P_CarContractorID LIKE '%" + lvSearch + "%') ORDER BY P_CarContractorID ASC ";
                    }

                    DT = GsysSQL.fncGetQueryData(lvSQL, DT, GVar.gvOnline);

                    int lvNumrow = DT.Rows.Count;

                    sp1.ActiveSheet.Rows.Count = lvNumrow;

                    for (int i = 0; i < lvNumrow; i++)
                    {
                        if (GVar.gvModeSearchRm != "CarSearchRm")
                        {
                            sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["P_CarContractorID"].ToString();
                            sp1.ActiveSheet.Cells[i, 1].Text = DT.Rows[i]["P_CarContractorName"].ToString() + " " + DT.Rows[i]["P_CarContractorLastName"].ToString();
                        }
                        else
                        {
                            sp1.ActiveSheet.Cells[i, 0].Text = DT.Rows[i]["C_TruckCarNum"].ToString();
                            
                            string lvCode = DT.Rows[i]["C_OwnedCode"].ToString();
                            string rmsearchname = fncGetRmName(lvCode);
                            sp1.ActiveSheet.Cells[i, 1].Text = rmsearchname;
                        }
                    }
                }
                if(e.KeyCode == Keys.Enter && txtSearch.Text == "")
                {
                    fncLoadDataBase();
                }
            }
            catch (Exception ex)
            {
                sp1.ActiveSheet.Rows.Count = 1;
                sp1.ActiveSheet.Cells[0, 0].Text = "";
                sp1.ActiveSheet.Cells[0, 1].Text = "" + " " + "";
            }
            
        }

        public static string fncGetRmName(string lvID)
        {
            string lvReturn = "";
            try
            {
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT P_CarContractorID, P_CarContractorName, P_CarContractorLastName FROM Cane_CarContractor WHERE P_Qouta = '" + lvID + "' ORDER BY P_CarContractorID DESC ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string id = dr["P_CarContractorID"].ToString();
                        string name = dr["P_CarContractorName"].ToString() + " " + dr["P_CarContractorLastName"].ToString();
                        lvReturn = id + " : " + name;
                    }
                }
                dr.Close();
                con.Close();

            }
            catch
            {

            }
            return lvReturn;
        }

    }
}
