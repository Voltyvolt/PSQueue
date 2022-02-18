using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Net.NetworkInformation;

namespace PSQueue
{
    public class GsysSQL
    {
        //Execute Data
        public static DataTable fncGetQueryData(string lvSQL,DataTable dt, bool lvOnline)
        {
            string query = lvSQL;

           if (lvOnline)
           {
               DataTable DTReturn = new DataTable();
               SqlDataAdapter DA = new SqlDataAdapter(query, System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
               DA.Fill(DTReturn);

               return DTReturn;
           }
           else
           {
               DataTable DTReturn = new DataTable();
               OleDbDataAdapter DA = new OleDbDataAdapter(query, System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
               DA.Fill(DTReturn);

               return DTReturn;
           }
        }

        public static DataTable fncGetQueryDataMCSS(string lvSQL, DataTable dt, bool lvOnline)
        {
            string query = lvSQL;

            if (lvOnline)
            {
                DataTable DTReturn = new DataTable();
                SqlDataAdapter DA = new SqlDataAdapter(query, System.Configuration.ConfigurationManager.ConnectionStrings["PSConnectionMCSS"].ToString());
                DA.Fill(DTReturn);

                return DTReturn;
            }
            else
            {
                DataTable DTReturn = new DataTable();
                OleDbDataAdapter DA = new OleDbDataAdapter(query, System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                DA.Fill(DTReturn);

                return DTReturn;
            }
        }

        //Execute Data
        public static DataTable fncGetQueryDataAccess(string lvSQL, DataTable dt)
        {
            string query = lvSQL;

            
                DataTable DTReturn = new DataTable();
                OleDbDataAdapter DA = new OleDbDataAdapter(query, System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                DA.Fill(DTReturn);

                return DTReturn;
        }

        public static string fncExecuteQueryData(string lvSQL, bool lvOnline)
        {
            string lvReturn = "";
            try
            {
                if (lvOnline)
                {
                    string query = lvSQL;
                    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = con;
                    con.Open();

                    // Start a local transaction.
                    SqlTransaction sqlTran = con.BeginTransaction();
                    cmd.Transaction = sqlTran;

                    cmd.CommandText = query;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        
                        // Commit the transaction.
                        sqlTran.Commit();

                        con.Close();
                        con.Dispose();

                        lvReturn = "Success";
                    }
                    catch (Exception ex)
                    {
                        sqlTran.Rollback();
                        lvReturn = ex.Message;
                    }

                }
                else
                {
                    //บันทึกลง Access
                    lvReturn = fncExecuteQueryDataAccess(lvSQL);
                }

                return lvReturn;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        public static string fncExecuteQueryDataAccess(string lvSQL)
        {
            string lvReturn = "";
            try
            {
                string query = lvSQL;
                OleDbConnection MyConn = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                MyConn.Open();
                OleDbCommand Cmd = new OleDbCommand(lvSQL, MyConn);
                Cmd.ExecuteNonQuery();

                lvReturn = "Success";
                MyConn.Close();
                Cmd.Dispose();

                return lvReturn;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //Function Check
        public static string fncCheckLogin(string lvUser,string lvPass, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT * FROM SysUser WHERE us_UserID = '" + lvUser + "' AND us_Password = '" + lvPass + "' And us_Active = '1' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["us_UserID"].ToString();
                            GVar.gvFirstUrl = dr["us_URL"].ToString();
                            GVar.gvKet = dr["us_Ket"].ToString();
                            GVar.gvUserType = dr["us_Type"].ToString();
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
                    cmd.CommandText = "SELECT * FROM SysUser WHERE us_UserID = '" + lvUser + "' AND us_Password = '" + lvPass + "' And us_Active = '1' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["us_UserID"].ToString();
                            GVar.gvFirstUrl = dr["us_URL"].ToString();
                            GVar.gvKet = dr["us_Ket"].ToString();
                            GVar.gvUserType = dr["us_Type"].ToString();
                        }
                    }
                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return "Connection Error";
            }
        }

        public static string fncCheckEmpType(string lvSearch, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT EmpType_ID FROM Emp_Type WHERE EmpType_ID = '" + lvSearch + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["EmpType_ID"].ToString();
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
                    cmd.CommandText = "SELECT EmpType_ID FROM Emp_Type WHERE EmpType_ID = '" + lvSearch + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["EmpType_ID"].ToString();
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

        public static string fncCheckEmpPrefix(string lvSearch, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT PrefixID FROM Emp_Prefix WHERE PrefixID = '" + lvSearch + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["PrefixID"].ToString();
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
                    cmd.CommandText = "SELECT PrefixID FROM Emp_Prefix WHERE PrefixID = '" + lvSearch + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["PrefixID"].ToString();
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

        public static string fncCheckEmpFaction(string lvSearch, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Faction_ID FROM Emp_Faction WHERE Faction_ID = '" + lvSearch + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Faction_ID"].ToString();
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
                    cmd.CommandText = "SELECT Faction_ID FROM Emp_Faction WHERE Faction_ID = '" + lvSearch + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Faction_ID"].ToString();
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

        public static string fncCheckEmpSection(string lvSearch, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Section_ID FROM Emp_Section WHERE Section_ID = '" + lvSearch + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Section_ID"].ToString();
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
                    cmd.CommandText = "SELECT Section_ID FROM Emp_Section WHERE Section_ID = '" + lvSearch + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Section_ID"].ToString();
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

        public static string fncCheckEmpPosition(string lvSearch, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Position_ID FROM Emp_Position WHERE Position_ID = '" + lvSearch + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Position_ID"].ToString();
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
                    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader dr;
                    #endregion

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Position_ID FROM Emp_Position WHERE Position_ID = '" + lvSearch + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Position_ID"].ToString();
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

        public static string fncCheckMainQuota(string lvQ,string lvQuota, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_No FROM Queue_Diary WHERE Q_No = '" + lvQ + "' and Q_Quota = '"+ lvQuota + "' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_No"].ToString();
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
                    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader dr;
                    #endregion

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_No FROM Queue_Diary WHERE Q_No = '" + lvQ + "' and Q_Quota = '" + lvQuota + "' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_No"].ToString();
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

        public static string fncCheckDocNo(string lvDocS,string lvDocNo, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT M_DocNo FROM MiniCane_BillHD WHERE M_DocNo = '" + lvDocNo + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["M_DocNo"].ToString();
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
                    cmd.CommandText = "SELECT M_DocNo FROM MiniCane_BillHD WHERE M_DocNo = '" + lvDocNo + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["M_DocNo"].ToString();
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

        public static string fncCheckQueue(string lvQ, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_No FROM Queue_Diary WHERE Q_No = '" + lvQ + "' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_No"].ToString();
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
                    cmd.CommandText = "SELECT Q_No FROM Queue_Diary WHERE Q_No = '" + lvQ + "' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_No"].ToString();
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

        public static string fncCheckOldCarnum2(string lvQ, bool lvOnline)
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
                    cmd.CommandText = "SELECT Q_CarNum FROM Queue_Diary WHERE Q_No = '" + lvQ + "' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_CarNum"].ToString();
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

        public static string fncCheckCloseDate(string lvDate, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_DateS FROM Queue_CloseStatus WHERE Q_DateS = '" + lvDate + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_DateS"].ToString();
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
                    cmd.CommandText = "SELECT Q_DateS FROM Queue_CloseStatus WHERE Q_DateS = '" + lvDate + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_DateS"].ToString();
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

        public static string fncCheckSampleNo(string lvSampleNo ,string lvQ, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_No,Q_SampleNo FROM Queue_Diary WHERE  Q_SampleNo like '%" + lvSampleNo + "%' and Q_CloseStatus = '' and Q_Year = '' ";//(Q_No = '"+ lvQ + "' OR Q_No = '"+ lvQ +".1') and
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string lvSamNo = dr["Q_SampleNo"].ToString();

                            string lvSam1 = Gstr.Left(lvSamNo, 4);
                            string lvSam2 = Gstr.Right(lvSamNo, 4);

                            if (lvSampleNo == lvSam1 || lvSampleNo == lvSam2)
                            {
                                lvReturn = dr["Q_No"].ToString();
                            }
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
                    cmd.CommandText = "SELECT Q_No,Q_SampleNo FROM Queue_Diary WHERE (Q_No = '" + lvQ + "' OR Q_No = '" + lvQ + ".1') and Q_SampleNo like '%" + lvSampleNo + "%' and Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string lvSamNo = dr["Q_SampleNo"].ToString();

                            string lvSam1 = Gstr.Left(lvSamNo, 4);
                            string lvSam2 = Gstr.Right(lvSamNo, 4);

                            if (lvSampleNo == lvSam1 || lvSampleNo == lvSam2)
                            {
                                lvReturn = dr["Q_No"].ToString();
                            }
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

        public static string fncCheckCarNum(string lvCarNum, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_No FROM Queue_Diary WHERE Q_CarNum = '" + lvCarNum + "' And Q_WeightOUT = '' And Q_CloseStatus = '' And Q_WeightFinish = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_No"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
                else
                {
                    #region OFFINE
                    #region //Connect Database 
                    OleDbConnection con = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataReader dr;
                    #endregion

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_No FROM Queue_Diary WHERE Q_CarNum = '" + lvCarNum + "' And Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_No"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string fncCheckCarNumStatus(string lvCarNum, bool lvOnline)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT Q_WeightINDate, Q_WeightOUTDate FROM Queue_Diary WHERE Q_CarNum = '" + lvCarNum + "' And Q_WeightOUT = '' And Q_CloseStatus = '' And Q_WeightFinish = '' and Q_Year = '' ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr["Q_WeightINDate"].ToString() != "" && dr["Q_WeightOUTDate"].ToString() == "")
                            lvReturn = "สถานะ อยู่ลานใน กำลังรอชั่งออก";
                        else if (dr["Q_WeightINDate"].ToString() == "" && dr["Q_WeightOUTDate"].ToString() == "")
                        {
                            lvReturn = "สถานะ อยู่ลานนอก กำลังรอชั่งเข้า";
                        }

                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string fncCheckCarLan(string lvCarNum,bool lvChkBill, bool lvOnline)
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

                    string lvReturn = "";

                    lvCarNum = lvCarNum.Replace("a", ".");

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT M_DocNo FROM MiniCane_BillHD WHERE M_CarNum = '" + lvCarNum + "' ";
                    if (lvChkBill) cmd.CommandText += "and M_BillNo = '' ";

                     dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["M_DocNo"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
                else
                {
                    #region OFFINE
                    #region //Connect Database 
                    OleDbConnection con = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataReader dr;
                    #endregion

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT M_DocNo FROM MiniCane_BillHD WHERE M_CarNum = '" + lvCarNum + "' and M_BillNo = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["M_DocNo"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string fncCheckBillCarLan(string lvBill, bool lvOnline)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT M_DocNo FROM MiniCane_BillHD WHERE Cast(M_BillNo as int) = '" + Gstr.fncToInt(lvBill) + "' And M_Year = '63/64' ";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["M_DocNo"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string fncCheckBillCarLanByCarNum(string lvCarNum, bool lvOnline)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT M_DocNo FROM MiniCane_BillHD WHERE M_CarTruck = '" + lvCarNum + "' And M_Year = '61/62' ";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["M_DocNo"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string fncCheckWeightALL(string lvQ, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_WeightALLStatus FROM Queue_Diary WHERE Q_No = '" + lvQ + "' and Q_Year = '' "; //And Q_CloseStatus = ''
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_WeightALLStatus"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
                else
                {
                    #region OFFINE
                    #region //Connect Database 
                    OleDbConnection con = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataReader dr;
                    #endregion

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_WeightALLStatus FROM Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_WeightALLStatus"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string fncCheckCarNumByCutDocID(string lvID)
        {
            try
            {
                    #region ONLINE
                    #region //Connect Database 
                    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader dr;
                    #endregion

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT C_TruckCarnum FROM Cane_QueueData WHERE C_ID = '" + lvID + "' "; //And Q_CloseStatus = ''
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["C_TruckCarnum"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string fncCheckCarNumByCutDocID2(string lvID)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT C_TruckCarnum2 FROM Cane_QueueData WHERE C_ID = '" + lvID + "' "; //And Q_CloseStatus = ''
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["C_TruckCarnum2"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        //Function Find
        public static string fncFindUserInfo(string lvID, bool lvOnline)
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

                    string lvReturn = "";
                    GVar.gvUser = "";
                    GVar.gvEmpID = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT * FROM SysUser WHERE us_UserID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GVar.gvUser = dr["us_UserID"].ToString();
                            GVar.gvEmpID = dr["us_EmpID"].ToString();

                            lvReturn = dr["us_UserID"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
                else
                {
                    #region OFFINE
                    #region //Connect Database 
                    OleDbConnection con = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataReader dr;
                    #endregion

                    string lvReturn = "";
                    GVar.gvUser = "";
                    GVar.gvEmpID = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT * FROM SysUser WHERE us_UserID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GVar.gvUser = dr["us_UserID"].ToString();
                            GVar.gvEmpID = dr["us_EmpID"].ToString();

                            lvReturn = dr["us_UserID"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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

        public static string fncFindFactionName(string lvID, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Faction_Name FROM Emp_Faction WHERE Faction_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Faction_Name"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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
                    cmd.CommandText = "SELECT Faction_Name FROM Emp_Faction WHERE Faction_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Faction_Name"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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

        public static string fncFindSectionName(string lvID, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Section_Name FROM Emp_Section WHERE Section_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Section_Name"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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
                    cmd.CommandText = "SELECT Section_Name FROM Emp_Section WHERE Section_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Section_Name"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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

        public static string fncFindSectionID(string lvName, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Section_ID FROM Emp_Section WHERE Section_Name = '" + lvName + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Section_ID"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvName;
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
                    cmd.CommandText = "SELECT Section_ID FROM Emp_Section WHERE Section_Name = '" + lvName + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Section_ID"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvName;
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

        public static string fncFindFactionID(string lvName, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Faction_ID FROM Emp_Faction WHERE Faction_Name = '" + lvName + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Faction_ID"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvName;
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
                    cmd.CommandText = "SELECT Faction_ID FROM Emp_Faction WHERE Faction_Name = '" + lvName + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Faction_ID"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvName;
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

        public static string fncFindPositionName(string lvID, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Position_Name FROM Emp_Position WHERE Position_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Position_Name"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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
                    cmd.CommandText = "SELECT Position_Name FROM Emp_Position WHERE Position_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Position_Name"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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

        public static string fncFindPrefixName(string lvID, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT PrefixName FROM Emp_Prefix WHERE PrefixID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["PrefixName"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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
                    cmd.CommandText = "SELECT PrefixName FROM Emp_Prefix WHERE PrefixID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["PrefixName"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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

        public static string fncFindEmpTypeName(string lvID, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT EmpType_Name FROM Emp_Type WHERE EmpType_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["EmpType_Name"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
                else
                {
                    #region ONLINE
                    #region //Connect Database 
                    OleDbConnection con = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataReader dr;
                    #endregion

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT EmpType_Name FROM Emp_Type WHERE EmpType_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["EmpType_Name"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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

        public static string fncFindEmpName(string lvID, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Employee_Name FROM Employee WHERE Employee_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Employee_Name"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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
                    cmd.CommandText = "SELECT Employee_Name FROM Employee WHERE Employee_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Employee_Name"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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

        public static string fncFindEmpIDByUser(string lvID, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT us_EmpID FROM SysUser WHERE us_UserID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["us_EmpID"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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
                    cmd.CommandText = "SELECT us_EmpID FROM SysUser WHERE us_UserID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["us_EmpID"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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

        public static string fncFindEmpLName(string lvID, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Employee_LName FROM Employee WHERE Employee_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Employee_LName"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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
                    cmd.CommandText = "SELECT Employee_LName FROM Employee WHERE Employee_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Employee_LName"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = lvID;
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

        public static void fncFindPermission(string lvUserID,string lvDisplayCode, bool lvOnline)
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

                    int lvReturn = 0;

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT * FROM SysPermission WHERE Permission_Code = '" + lvDisplayCode + "' And Permission_UserID = '" + lvUserID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GVar.gvPermitEntry = "Y";
                            GVar.gvPermitNew = dr["Permission_New"].ToString();
                            GVar.gvPermitEdit = dr["Permission_Edit"].ToString();
                            GVar.gvPermitDel = dr["Permission_Del"].ToString();
                            GVar.gvPermitApprove = dr["Permission_AppDoc"].ToString();
                            GVar.gvPermitCancel = dr["Permission_CancelDoc"].ToString();
                            GVar.gvPermitPrint = dr["Permission_Print"].ToString();
                            //GVar.gvUserType = dr["us_Type"].ToString();
                        }
                    }
                    else
                    {
                        GVar.gvPermitEntry = "N";
                        GVar.gvPermitNew = "";
                        GVar.gvPermitEdit = "";
                        GVar.gvPermitDel = "";
                        GVar.gvPermitApprove = "";
                        GVar.gvPermitCancel = "";
                        GVar.gvPermitPrint = "";
                        GVar.gvUserType = "";
                    }

                    dr.Close();
                    con.Close();
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

                    int lvReturn = 0;

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT * FROM SysPermission WHERE Permission_Code = '" + lvDisplayCode + "' And Permission_UserID = '" + lvUserID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GVar.gvPermitEntry = "Y";
                            GVar.gvPermitNew = dr["Permission_New"].ToString();
                            GVar.gvPermitEdit = dr["Permission_Edit"].ToString();
                            GVar.gvPermitDel = dr["Permission_Del"].ToString();
                            GVar.gvPermitApprove = dr["Permission_AppDoc"].ToString();
                            GVar.gvPermitCancel = dr["Permission_CancelDoc"].ToString();
                            GVar.gvPermitPrint = dr["Permission_Print"].ToString();
                            //GVar.gvUserType = dr["us_Type"].ToString();
                        }
                    }
                    else
                    {
                        GVar.gvPermitEntry = "N";
                        GVar.gvPermitNew = "";
                        GVar.gvPermitEdit = "";
                        GVar.gvPermitDel = "";
                        GVar.gvPermitApprove = "";
                        GVar.gvPermitCancel = "";
                        GVar.gvPermitPrint = "";
                        GVar.gvUserType = "";
                    }

                    dr.Close();
                    con.Close();
                    #endregion
                }
            }
            catch
            {
                return;
            }
        }

        public static string FindSectionByEmpID(string lvEmpID, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Section_Name FROM Employee Inner Join Emp_Section On Section_ID = Employee_Section WHERE Employee_ID = '" + lvEmpID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Section_Name"].ToString();
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
                    cmd.CommandText = "SELECT Section_Name FROM Employee Inner Join Emp_Section On Section_ID = Employee_Section WHERE Employee_ID = '" + lvEmpID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Section_Name"].ToString();
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
        
        public static string FindDocNo(string lvDocNo, string lvQuota, string lvCarNum, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_No FROM Queue_Diary WHERE Q_Quota = '"+ lvQuota + "' and Q_CarNum = '"+ lvCarNum + "' and Q_Status = 'Active' and Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_No"].ToString();
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
                    cmd.CommandText = "SELECT Q_No FROM Queue_Diary WHERE Q_Quota = '" + lvQuota + "' and Q_CarNum = '" + lvCarNum + "' and Q_Status = 'Active' and Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_No"].ToString();
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

        public static string FindBillNo(string lvDocNo, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_BillingNo FROM Queue_Diary WHERE Q_BillingNo = '" + lvDocNo + "' and Q_Status = 'Active' and Q_Year = '' ";//and Q_CloseStatus = ''
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_BillingNo"].ToString();
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
                    cmd.CommandText = "SELECT Q_BillingNo FROM Queue_Diary WHERE Q_BillingNo = '" + lvDocNo + "' and Q_Status = 'Active' and Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_BillingNo"].ToString();
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
        public static string FindBillNoByQueue(string lvQNo, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_BillingNo FROM Queue_Diary WHERE Q_No = '" + lvQNo + "' and Q_Status = 'Active' and Q_Year = '' ";//and Q_CloseStatus = ''
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_BillingNo"].ToString();
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
                    cmd.CommandText = "SELECT Q_BillingNo FROM Queue_Diary WHERE Q_No = '" + lvQNo + "' and Q_Status = 'Active' and Q_Year = '' ";//and Q_CloseStatus = ''
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_BillingNo"].ToString();
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

        public static string fncFindQuotaName(string lvID, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_Prefix, Q_FirstName, Q_LastName, Q_BonsucroStatus FROM Cane_Quota WHERE Q_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_Prefix"].ToString() + " " + dr["Q_FirstName"].ToString() + " " + dr["Q_LastName"].ToString();
                            GVar.gvBonsucroStatus = dr["Q_BonsucroStatus"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
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
                    cmd.CommandText = "SELECT Q_Prefix, Q_FirstName, Q_LastName FROM Cane_Quota WHERE Q_ID = " + lvID + " ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_Prefix"].ToString() + " " + dr["Q_FirstName"].ToString() + " " + dr["Q_LastName"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string fncFindQuotaNameByQ(string lvQ, bool lvOnline)
        {
            string lvReturn = "";
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

                   

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_Prefix, Q_FirstName, Q_LastName FROM Cane_Quota WHERE Q_ID = '" + lvQ + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_Prefix"].ToString() + " " + dr["Q_FirstName"].ToString() + " " + dr["Q_LastName"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion

                }
                else
                {
                    
                }
                return lvReturn;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string fncFindQuotaByQ(string lvQ, bool lvOnline)
        {
            string lvReturn = "";
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

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_Quota FROM Queue_Diary WHERE Q_No = '" + lvQ + "' AND Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_Quota"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
                else
                {
                    
                }
                return lvReturn;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string fncFindCaneTypeName(string lvID, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT C_Name FROM Cane_CaneType WHERE C_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["C_Name"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
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
                    cmd.CommandText = "SELECT C_Name FROM Cane_CaneType WHERE C_ID = '" + lvID + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["C_Name"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
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

        public static string fncCountQ(string lvStation,string lvCondition, bool lvOnline)
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

                    string lvReturn = "";
                    
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "select Count(Q_No) as C_Count from Queue_Diary where (1=1 " + lvCondition + " and Q_Rail = '" + lvStation + "') and Q_Year = '' ";
                    //cmd.CommandText = "select Count(Q_No) as C_Count from Queue_Diary where (Q_TKNo <> '') AND (Q_WeightINDate <> '') AND (Q_WeightOUTDate = '') AND (Q_CloseStatus = '') AND (Q_SampleNo = '') And Q_Rail = '"+ lvStation +"' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["C_Count"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
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
                    cmd.CommandText = "select Count(Q_No) as C_Count from Queue_Diary where 1=1 " + lvCondition + " and Q_Rail = '" + lvStation + "' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["C_Count"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
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

        public static string fncCountW(string lvStatus, bool lvSumData, bool lvOnline)
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

                    string lvReturn = "";
                    string lvSQL = "";
                    string lvCondition = "";

                    lvSQL = "Count(Q_No) as C_Count";
                    if (lvStatus == "IN")
                    {
                        lvCondition += "and Q_WeightIN != '' and Q_WeightOUT = '' ";
                    }
                    else if (lvStatus == "OUT")
                    {
                        lvCondition += "and Q_WeightIN != '' and Q_WeightOUT != '' ";
                    }
                    else if (lvSumData)
                    {
                        lvSQL = "sum(Q_WeightIN - Q_WeightOUT) as C_Count";
                        lvCondition += "and Q_WeightIN != '' and Q_WeightOUT != '' ";
                    }

                    lvCondition += "And Q_CloseStatus = '' and Q_Year = '' ";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "select "+ lvSQL + " from Queue_Diary where 1=1 " + lvCondition + " ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["C_Count"].ToString();

                            if (lvSumData) lvReturn = (Gstr.fncToDouble(lvReturn)).ToString("#,##0"); // / 1000
                        }
                    }
                    else
                    {
                        lvReturn = "";
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
                    string lvSQL = "";
                    string lvCondition = "";

                    lvSQL = "Count(Q_No) as C_Count";
                    if (lvStatus == "IN")
                    {
                        lvCondition += "and Q_WeightIN != '' and Q_WeightOUT = '' ";
                    }
                    else if (lvStatus == "OUT")
                    {
                        lvCondition += "and Q_WeightIN != '' and Q_WeightOUT != '' ";
                    }
                    else if (lvSumData)
                    {
                        lvSQL = "sum(Q_WeightIN - Q_WeightOUT) as C_Count";
                        lvCondition += "and Q_WeightIN != '' and Q_WeightOUT != '' ";
                    }

                    lvCondition += "And Q_CloseStatus = '' and Q_Year = '' ";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "select " + lvSQL + " from Queue_Diary where 1=1 " + lvCondition + " ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["C_Count"].ToString();

                            if (lvSumData) lvReturn = (Gstr.fncToDouble(lvReturn)).ToString("#,##0"); // / 1000
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static void fncGetDataCane(string lvRail,string lvCaneType,string lvDateS,string lvDateE,string lvTimeS,string lvTimeE,string lvCloseStatus, string lvBillS, string lvBillE, bool lvOnline)
        {
            GVar.gvDataCount = "0";
            GVar.gvDataSum = "0";

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

                    string lvSQL = "select count(Q_No) as CCount , sum(Q_WeightIN-Q_WeightOut) as CTotal ";
                    lvSQL += "from Queue_Diary ";
                    lvSQL += "where Q_CaneType in ('"+ lvCaneType +"') ";
                    if (lvRail != "") lvSQL += "And Q_Rail = '" + lvRail + "' ";
                    lvSQL += "And CONVERT(datetime, Q_WeightOUTDate + ' ' + Q_WeightOUTTime, 103) >= CONVERT(datetime, '" + lvDateS + "' + ' ' + '" + lvTimeS + "', 103) ";
                    lvSQL += "And CONVERT(datetime, Q_WeightOUTDate + ' ' + Q_WeightOUTTime, 103) <= CONVERT(datetime, '" + lvDateE +  "' + ' ' + '" + lvTimeE + "', 103) ";

                    lvSQL += "And Q_CloseStatus = '"+ lvCloseStatus + "' and Q_Year = '' ";
                    lvSQL += "And Q_WeightIN > 0 And Q_WeightOUT > 0 ";

                    if (lvBillS != "" && lvBillE != "")
                    {
                        lvSQL += "And Cast(Q_BillingNo as int) >= '" + lvBillS + "' And  Cast(Q_BillingNo as int) <= '" + lvBillE + "'";
                    }


                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = lvSQL;
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GVar.gvDataCount = dr["CCount"].ToString();
                            //GVar.gvDataSum = dr["CTotal"].ToString(); // กก
                            GVar.gvDataSum = (Gstr.fncToDouble(dr["CTotal"].ToString()) / 1000).ToString("#,##0.000"); //ตัน
                        }
                    }
                    else
                    {
                        GVar.gvDataCount = "0";
                        GVar.gvDataSum = "0";
                    }

                    dr.Close();
                    con.Close();                    
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

                    string lvSQL = "select count(Q_No) as CCount , sum(Q_WeightIN-Q_WeightOut) as CTotal ";
                    lvSQL += "from Queue_Diary ";
                    lvSQL += "where Q_CaneType in ('" + lvCaneType + "') ";
                    if (lvRail != "") lvSQL += "And Q_Rail = '" + lvRail + "' ";
                    lvSQL += "And CONVERT(datetime, Q_WeightOUTDate + ' ' + Q_WeightOUTTime, 103) >= CONVERT(datetime, '" + lvDateS + "' + ' ' + '" + lvTimeS + "', 103) ";
                    lvSQL += "And CONVERT(datetime, Q_WeightOUTDate + ' ' + Q_WeightOUTTime, 103) <= CONVERT(datetime, '" + lvDateE + "' + ' ' + '" + lvTimeE + "', 103) ";

                    lvSQL += "And Q_CloseStatus = '" + lvCloseStatus + "' and Q_Year = '' ";
                    lvSQL += "And Q_WeightIN > 0 And Q_WeightOUT > 0 ";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = lvSQL;
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            GVar.gvDataCount = dr["CCount"].ToString();
                            GVar.gvDataSum = dr["CTotal"].ToString();
                        }
                    }
                    else
                    {
                        GVar.gvDataCount = "0";
                        GVar.gvDataSum = "0";
                    }

                    dr.Close();
                    con.Close();
                    #endregion
                }
            }
            catch
            {
                GVar.gvDataCount = "0";
                GVar.gvDataSum = "0";
            }
        }

        public static string fncCheckTKNo(string lvTK, string lvRail, string lvQ, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_TKNo FROM Queue_Diary WHERE Q_TKNo = '" + lvTK + "' and Q_Rail = '" + lvRail + "' and Q_Status = 'Active' and Q_CloseStatus = '' And Q_WeightOUT = '' And Q_TKNoCheck = '' And Q_No != '"+ lvQ + "' And Q_No != '" + lvQ + ".1' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_TKNo"].ToString();
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
                    cmd.CommandText = "SELECT Q_TKNo FROM Queue_Diary WHERE Q_TKNo = '" + lvTK + "' and Q_Rail = '" + lvRail + "' and Q_Status = 'Active' and Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_TKNo"].ToString();
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

        public static string fncCheckTKNo_New(string lvTK, string lvRail, bool lvOnline)
        {
            try
            {
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT Q_Runno FROM Queue_TableNumber WHERE Q_Runno = '" + lvTK + "' and Q_Year = '' and Q_Rail = '" + lvRail + "' and Q_TypeDoc = '"+ GVar.gvTypeDocTakao +"' ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["Q_Runno"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
            }
            catch
            {
                return "";
            }
        }

        public static string fncCheckTKNo_ByTK(string lvTK, string lvLastTypeDoc, string lvRail)
        {
            try
            {
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT Q_Runno FROM Queue_TableNumber WHERE Q_Runno = '" + lvTK + "' And Q_Year = '' And Q_TypeDoc = '" + lvLastTypeDoc + "' And Q_Rail = '" + lvRail + "' ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["Q_Runno"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
            }
            catch
            {
                return "";
            }
        }

        public static string fncFindSample2(string lvQ, bool lvCloseStatus, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_SampleNo FROM Queue_Diary WHERE Q_No = '" + lvQ + "' and Q_Year = '' ";
                    //if (!lvCloseStatus) //ไม่ต้อง Where เพราะยังก็ where ด้วยคิวได้ยังไงก็ไม่ซ้ำ
                    //{
                    //    cmd.CommandText += "And Q_CloseStatus = '1' ";
                    //}
                    //else
                    //    cmd.CommandText += "And Q_CloseStatus = '1' ";

                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_SampleNo"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
                else
                {
                    #region OFFINE
                    #region //Connect Database 
                    OleDbConnection con = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataReader dr;
                    #endregion

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_SampleNo FROM Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_SampleNo"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
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

        public static string fncFineTKNo(string lvQ, string lvRail, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_TKNo FROM Queue_Diary WHERE Q_No = '" + lvQ + "' and Q_Rail = '" + lvRail + "' and Q_Status = 'Active' and Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_TKNo"].ToString();
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
                    cmd.CommandText = "SELECT Q_TKNo FROM Queue_Diary WHERE Q_No = '" + lvQ + "' and Q_Rail = '" + lvRail + "' and Q_Status = 'Active' and Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_TKNo"].ToString();
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

        public static string fncFindCarNum2(string lvQ, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_CarNum FROM Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_CarNum"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
                else
                {
                    #region OFFINE
                    #region //Connect Database 
                    OleDbConnection con = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataReader dr;
                    #endregion

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_CarNum FROM Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_CarNum"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
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

        public static string fncFindShotEngCar(string lvTXT)
        {
            #region //Connect Database 
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            #endregion  

            string lvReturn = "";

            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "SELECT P_ShotThai FROM SysProvince WHERE P_ShotEng = '" + lvTXT + "' ";
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lvReturn = dr["P_ShotThai"].ToString().ToUpper();
                }
            }
            dr.Close();
            con.Close();

            return lvReturn;
        }

        public static string fncFindBonsugo(string lvQ, bool lvOnline)
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

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_Bonsugo FROM Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_Bonsugo"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
                else
                {
                    #region OFFINE
                    #region //Connect Database 
                    OleDbConnection con = new OleDbConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSAccess"].ToString());
                    OleDbCommand cmd = new OleDbCommand();
                    OleDbDataReader dr;
                    #endregion

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_Bonsugo FROM Queue_Diary WHERE Q_No = '" + lvQ + "' And Q_CloseStatus = '' and Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_Bonsugo"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
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

        public static string fncFindCloseStatusForEdit(string lvQ, string lvCarNum, bool lvOnline)
        {
            #region //Connect Database 
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader dr;
            #endregion

            string lvReturn = "";

            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "SELECT Q_CloseStatus FROM Queue_Diary WHERE Q_No = '" + lvQ + "' and Q_Year = '' ";
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lvReturn = dr["Q_CloseStatus"].ToString();
                }
            }
            else
            {
                lvReturn = "";
            }

            dr.Close();
            con.Close();

            return lvReturn;
        }

        public static string fncFindLastLoop()
        {
            try
            {
                    #region ONLINE
                    #region //Connect Database 
                    SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader dr;
                    #endregion

                    string lvReturn = "";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT L_loopNo FROM Queue_LockOption ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["L_loopNo"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncFindQLockOnOff()
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT L_Status FROM Queue_LockOption ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["L_Status"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncFindQLockOnOffAlert()
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT L_AlertLock FROM Queue_LockOption ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["L_AlertLock"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncFindQBtnClearLock()
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT L_lockBtnClear FROM Queue_LockOption ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["L_lockBtnClear"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }
        public static string fncFindQLockAlert()
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT * FROM Queue_LockOption ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string lvCarChk = dr["L_LockCar1"].ToString() + "," + dr["L_LockCar2"].ToString() + "," + dr["L_LockCar3"].ToString() + "," + dr["L_LockCar4"].ToString() + "," + dr["L_LockCar5"].ToString();
                        string lvCaneChk = dr["L_LockCane9"].ToString() + "," + dr["L_LockCane10"].ToString() + "," + dr["L_LockCane11"].ToString() + "," + dr["L_LockCane12"].ToString() + "," + dr["L_LockCane13"].ToString() + "," + dr["L_LockCane14"].ToString() + "," + dr["L_LockCane15"].ToString() + "," + dr["L_LockCane16"].ToString();
                        string lvLockMaster = dr["L_loopNo"].ToString() + "," + dr["L_lockNo"].ToString() + "," + dr["L_loopNoE"].ToString() + "," + dr["L_lockNoE"].ToString();
                        lvReturn = dr["L_AlertLock"].ToString() + "," + lvCarChk + "," + lvCaneChk + "," + lvLockMaster;
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }
        public static string fncFindMaxLoopByLockQNo(string lvLockQNo)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT Max(Q_Lockloop) as MaxLoop FROM Queue_Diary Where Q_LockQNo = '"+ lvLockQNo + "' and Q_Year = '' ";//and Q_Date > '20190113' and Q_CloseStatus = '1'
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["MaxLoop"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncFindLastLockPK()
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT Max(L_PK) as MaxLoop FROM Queue_LockDiary ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string lvMaxLoop = Gstr.fncToInt(dr["MaxLoop"].ToString()).ToString("00000000");

                        if (lvMaxLoop == "00000000") lvMaxLoop = "04010000";

                        //แยก PK To Loop lock Q
                        string lvLoop = Gstr.Left(lvMaxLoop, 2);
                        string lvLock = Gstr.Mid(lvMaxLoop, 3, 2);
                        string lvLockQ = Gstr.Right(lvMaxLoop, 4);

                        int lvQNew = Gstr.fncToInt(lvLockQ) + 1;
                        int lvLockNew = Gstr.fncToInt(lvLock);
                        int lvLoopNew = Gstr.fncToInt(lvLoop);

                        if (lvLoopNew >= 7) //ถ้าเป็นรอบ 7 ขึ้นไปให้ Gen แบบใหม่
                        {
                            if (lvQNew <= 750) //ถ้าคิวที่กำหนด  ล็อก + 1  รอบเดิม
                            {
                                lvLockQ = lvQNew.ToString("0000");

                                if (lvQNew <= 50) lvLockNew = 1;
                                else if (lvQNew <= 100) lvLockNew = 2;
                                else if (lvQNew <= 150) lvLockNew = 3;
                                else if (lvQNew <= 200) lvLockNew = 4;
                                else if (lvQNew <= 250) lvLockNew = 5;
                                else if (lvQNew <= 300) lvLockNew = 6;
                                else if (lvQNew <= 350) lvLockNew = 7;
                                else if (lvQNew <= 400) lvLockNew = 8;
                                else if (lvQNew <= 450) lvLockNew = 9;
                                else if (lvQNew <= 500) lvLockNew = 10;
                                else if (lvQNew <= 550) lvLockNew = 11;
                                else if (lvQNew <= 600) lvLockNew = 12;
                                else if (lvQNew <= 650) lvLockNew = 13;
                                else if (lvQNew <= 700) lvLockNew = 14;
                                else if (lvQNew <= 750) lvLockNew = 15;

                                if (lvLockNew <= 15) //ถ้าล็อกยังไม่เกิน 15 รอบเดิม
                                {
                                    lvLock = lvLockNew.ToString("00");
                                    lvReturn = lvLoop + lvLock + lvLockQ;
                                }
                                else if (lvLockNew > 15) //ถ้าล็อกเกิน 15 ล็อก = 1 รอบ + 1
                                {
                                    lvLockNew = 1;
                                    lvLock = lvLockNew.ToString("00");

                                    lvLoopNew = lvLoopNew + 1;
                                    lvLoop = lvLoopNew.ToString("00");

                                    lvReturn = lvLoop + lvLock + lvLockQ;
                                }
                            }
                            else if (lvQNew >= 751)
                            {
                                lvQNew = 1;
                                lvLockNew = 1;
                                lvLoopNew = lvLoopNew + 1;

                                lvLockQ = lvQNew.ToString("0000");
                                lvLock = lvLockNew.ToString("00");
                                lvLoop = lvLoopNew.ToString("00");

                                lvReturn = lvLoop + lvLock + lvLockQ;
                            }
                        }
                        else
                        {
                            //ของเดิม ถ้ายังไม่ถึงรอบ 7 ใช้อันเก่า
                            if (lvQNew <= 50) //ถ้าคิวยังไม่เกิน 50 + คิวปกติ  ล็อกเดิม  รอบเดิม
                            {
                                lvLockQ = lvQNew.ToString("0000");
                                lvReturn = lvLoop + lvLock + lvLockQ;
                            }
                            else if (lvQNew > 50) //ถ้าคิวเกิน 50 คิว = 1  ล็อก + 1  รอบเดิม
                            {
                                lvQNew = 1;
                                lvLockQ = lvQNew.ToString("0000");

                                lvLockNew = lvLockNew + 1;
                                if (lvLockNew <= 15) //ถ้าล็อกยังไม่เกิน 15 รอบเดิม
                                {
                                    lvLock = lvLockNew.ToString("00");
                                    lvReturn = lvLoop + lvLock + lvLockQ;
                                }
                                else if (lvLockNew > 15) //ถ้าล็อกเกิน 15 ล็อก = 1 รอบ + 1
                                {
                                    lvLockNew = 1;
                                    lvLock = lvLockNew.ToString("00");

                                    lvLoopNew = lvLoopNew + 1;
                                    lvLoop = lvLoopNew.ToString("00");

                                    lvReturn = lvLoop + lvLock + lvLockQ;
                                }
                            }
                        }
                    }
                }
                else
                {
                    //ถ้าหาไม่เจอให้ใช้ตัวเริ่มต้น
                    lvReturn = "04010001";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncFindLockQSub(string lvPK)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT L_QSub FROM Queue_LockDiary WHERE L_PK = '" + lvPK + "' ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["L_QSub"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }
        public static string fncFindLockPK(string lvPK)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT L_PK FROM Queue_LockDiary WHERE L_PK = '" + lvPK + "' ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["L_PK"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }
        public static string fncFindLockPKByQ(string lvQ)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT L_PK FROM Queue_LockDiary WHERE L_QMain = '" + lvQ + "' ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["L_PK"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncFindChkCarRegister()
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT L_lockCarRegis FROM Queue_LockOption ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["L_lockCarRegis"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncCheckQLockByCarNum(string lvCarNum, string lvPK, string lvLockQ)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";
                string lvSearch = "";
                string lvSQL = "";
                lvCarNum = lvCarNum.Replace("a",".");

                if (lvCarNum != "")
                {
                    lvSearch = lvCarNum;
                    lvSQL = "SELECT L_Loop, L_LockNo, L_LockQNo, L_LockCarNum, L_LockNextCarNum, L_LockNextDate, L_LockNextTime FROM Queue_LockDiary Where REPLACE(L_LockCarNum, 'a', '.') = '" + lvSearch + "'  ";//And L_LockActive = '1'
                }
                else if (lvPK != "")
                {
                    lvSearch = Gstr.fncToInt(lvPK).ToString();
                    lvSQL = "SELECT L_Loop, L_LockNo, L_LockQNo, L_LockCarNum, L_LockNextCarNum, L_LockNextDate, L_LockNextTime FROM Queue_LockDiary Where L_PK = '" + lvSearch + "'  ";//And L_LockActive = '1'
                }
                else if (lvLockQ != "")
                {
                    lvSearch = Gstr.fncToInt(lvLockQ).ToString();
                    lvSQL = "SELECT L_Loop, L_LockNo, L_LockQNo, L_LockCarNum, L_LockNextCarNum, L_LockNextDate, L_LockNextTime FROM Queue_LockDiary Where L_LockQNo = '" + lvSearch + "'  ";//And L_LockActive = '1'
                }

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = lvSQL;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["L_LockNo"].ToString();
                        
                        if (lvReturn != "")
                            lvReturn += "," + dr["L_LockQNo"].ToString();

                        if (lvReturn != "")
                            lvReturn += "," + dr["L_LockCarNum"].ToString();

                        if (lvReturn != "")
                            lvReturn += "," + dr["L_LockNextCarNum"].ToString();

                        if (lvReturn != "")
                            lvReturn += "," + dr["L_LockNextDate"].ToString();

                        if (lvReturn != "")
                            lvReturn += "," + dr["L_LockNextTime"].ToString();
                    }
                }
                else
                {
                    lvReturn = ",,";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncCheckQLockLoop(string lvQLock)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";
                string lvSearch = "";
                string lvSQL = "Select * From Queue_LockDiary Where L_LockQNo = '" + lvQLock + "'";


                cmd.Connection = con;
                con.Open();
                cmd.CommandText = lvSQL;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["L_Loop"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                //if (lvReturn == "")
                //{
                //    lvReturn = "1";
                //}

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncCheckQLockLoopNow()
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";
                string lvSearch = "";
                string lvSQL = "Select L_loopNo From Queue_LockOption";


                cmd.Connection = con;
                con.Open();
                cmd.CommandText = lvSQL;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn += dr["L_loopNo"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncCheckQLockNoNow()
        {
            try {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";
                string lvSearch = "";
                string lvSQL = "Select L_lockNo From Queue_LockOption";


                cmd.Connection = con;
                con.Open();
                cmd.CommandText = lvSQL;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn += dr["L_lockNo"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncCheckQStart(string lvNo)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";
                string lvSearch = "";
                string lvSQL = "Select L_QStart From Queue_LockMaster Where L_No = '" + lvNo + "'";


                cmd.Connection = con;
                con.Open();
                cmd.CommandText = lvSQL;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn += dr["L_QStart"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncCheckQEnd(string lvNo)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";
                string lvSearch = "";
                string lvSQL = "Select L_QEnd From Queue_LockMaster Where L_No = '" + lvNo + "'";


                cmd.Connection = con;
                con.Open();
                cmd.CommandText = lvSQL;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn += dr["L_QEnd"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncCheckQLockByQMain(string lvQ)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";
                string lvSQL = "";

                lvSQL = "SELECT Q_LockBarCode FROM Queue_Diary Where Q_No = '" + lvQ + "' and Q_Year = '' ";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = lvSQL;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["Q_LockBarCode"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static bool fncCheckQueueFinish(string lvQ)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                //เอาโควต้าแม่มาหาว่าชั่งครบแล้วหรือยัง
                string[] lvArr = lvQ.Split('.');
                if (lvArr.Count() > 1)
                {
                    lvQ = lvArr[0];
                }

                bool lvReturn = false;
                string lvSQL = "";

                lvSQL = "SELECT Q_No FROM Queue_Diary Where Q_No = '" + lvQ + "' and Q_WeightIN <> '' and Q_WeightOUT <> '' and Q_Year = '' ";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = lvSQL;
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = true;
                    }
                }
                else
                {
                    lvReturn = false;
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return false;
            }
        }

        //Document
        public static string fncGetLastDocNo(bool lvOnline)
        {
            string lvReturn = "";
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

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Top 1 Q_No FROM Queue_Diary Where Q_CloseStatus = '' and Q_Station = '"+ GVar.gvStation + "' and Q_Year = '' Order by Q_No Desc ";// and Q_No not Like '%.%'
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //GenDoc
                            string lvQNo = Gstr.Right(dr["Q_No"].ToString(),4);
                            if (lvQNo == "")
                            {
                                lvReturn = GVar.gvStation + "0001";
                            }
                            else
                            {
                                lvReturn = GVar.gvStation + (Gstr.fncToInt(lvQNo) + 1).ToString("0000");
                            }
                        }
                    }
                    else
                    {
                        lvReturn = GVar.gvStation + "0001";
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

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Top 1 Q_No FROM Queue_Diary Where Q_CloseStatus = '' and Q_Station = '" + GVar.gvStation + "' and Q_Year = '' Order by Q_No Desc ";// and Q_No not Like '%.%'
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //GenDoc
                            string lvQNo = Gstr.Right(dr["Q_No"].ToString(), 4);
                            if (lvQNo == "")
                            {
                                lvReturn = GVar.gvStation + "0001";
                            }
                            else
                            {
                                lvReturn = GVar.gvStation + (Gstr.fncToInt(lvQNo) + 1).ToString("0000");
                            }
                        }
                    }
                    else
                    {
                        lvReturn = GVar.gvStation + "0001";
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

        public static string fncGetLastBillNo(bool lvOnline)
        {
            string lvReturn = "";
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

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT S_RunNo FROM SysDocNo Where S_MCode = 'Weight_Bill' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //GenDoc
                            string lvQNo = dr["S_RunNo"].ToString();
                            if (Gstr.fncToInt(lvQNo) == 0)
                            {
                                lvReturn = "000001";
                            }
                            else
                            {
                                lvReturn = (Gstr.fncToInt(lvQNo) + 1).ToString("000000");
                            }
                        }
                    }
                    else
                    {
                        lvReturn = "000001";
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

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT S_RunNo FROM SysDocNo Where S_MCode = 'Weight_Bill' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //GenDoc
                            string lvQNo = dr["S_RunNo"].ToString();
                            if (Gstr.fncToInt(lvQNo) == 0)
                            {
                                lvReturn = "000001";
                            }
                            else
                            {
                                lvReturn = (Gstr.fncToInt(lvQNo) + 1).ToString("000000");
                            }
                        }
                    }
                    else
                    {
                        lvReturn = "000001";
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

        public static string fncGetLastBillNo1(bool lvOnline)
        {
            string lvReturn = "";
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT S_RunNo FROM SysDocNo Where S_MCode = 'Weight_Bill' ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //GenDoc
                        string lvQNo = dr["S_RunNo"].ToString();

                        //ลบ 1 เพื่อเช็คว่ากระโดดข้ามไหม
                        lvQNo = (Gstr.fncToInt(lvQNo) - 2).ToString();

                        if (Gstr.fncToInt(lvQNo) == 0)
                        {
                            lvReturn = "000001";
                        }
                        else
                        {
                            lvReturn = (Gstr.fncToInt(lvQNo) + 1).ToString("000000");
                        }
                    }
                }
                else
                {
                    lvReturn = "000001";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion
            }
            catch
            {
                return "";
            }
        }

        public static string fncCheckTKNo_ByQueue(string lvQ)
        {
            try
            {
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT Q_QNo FROM Queue_TableNumber WHERE Q_QNo = '" + lvQ + "' And Q_Year = '' ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["Q_QNo"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
            }
            catch
            {
                return "";
            }
        }

        public static string fncGetTakaoNo(string lvMCode, bool lvOnline)
        {
            string lvReturn = "";
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

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT S_RunNo FROM SysDocNo Where S_MCode = '"+ lvMCode + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //GenDoc
                            string lvQNo = dr["S_RunNo"].ToString();
                            if (Gstr.fncToInt(lvQNo) == 0)
                            {
                                lvReturn = "1";
                            }
                            else
                            {
                                lvReturn = (Gstr.fncToInt(lvQNo) + 1).ToString();
                                //if (Gstr.fncToInt(lvReturn) > 100)
                                //{
                                //    lvReturn = "1";
                                //}
                            }
                        }
                    }
                    else
                    {
                        lvReturn = "1";
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

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT S_RunNo FROM SysDocNo Where S_MCode = '" + lvMCode + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //GenDoc
                            string lvQNo = dr["S_RunNo"].ToString();
                            if (Gstr.fncToInt(lvQNo) == 0)
                            {
                                lvReturn = "1";
                            }
                            else
                            {
                                lvReturn = (Gstr.fncToInt(lvQNo) + 1).ToString();
                                if (Gstr.fncToInt(lvReturn) > 100) lvReturn = "1";
                            }
                        }
                    }
                    else
                    {
                        lvReturn = "1";
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


        public static string fncGetTakaoNo2(string lvQNo, bool lvOnline)
        {
            string lvReturn = "0";
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

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_Runno FROM Queue_TableNumber Where Q_QNo = '" + lvQNo + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_Runno"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "0";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
                else
                {
                   
                }
                return lvReturn;
            }
            catch
            {
                return "0";
            }
        }

        public static string fncGetWeightIN(string lvQNo, bool lvOnline)
        {
            string lvReturn = "";
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

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT Q_TKNo FROM Queue_Diary Where Q_No = '" + lvQNo + "' And Q_Year = '' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            lvReturn = dr["Q_TKNo"].ToString();
                        }
                    }
                    else
                    {
                        lvReturn = "";
                    }

                    dr.Close();
                    con.Close();

                    return lvReturn;
                    #endregion
                }
                else
                {

                }
                return lvReturn;
            }
            catch
            {
                return "";
            }
        }


        ////public static string fncGetLastTakaoNo(string lvMCode, string lvRail, bool lvOnline)
        //{
        //    string lvReturn = "";
        //    try
        //    {
        //        #region ONLINE
        //        #region //Connect Database 
        //        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
        //        SqlCommand cmd = new SqlCommand();
        //        SqlDataReader dr;
        //        #endregion

        //        cmd.Connection = con;
        //        con.Open();
        //        cmd.CommandText = "SELECT Top 1 Q_TKNo FROM Queue_Diary Where Q_TKNoCheck = '' And Q_Rail = '" + lvRail + "' and Q_Year = '' Order by Cast(Q_TKNo as int) Desc ";

        //        //if (lvRail == "A")
        //        //    cmd.CommandText = "SELECT name, current_value FROM sys.sequences Where name = 'Doc_TKNoA' ";
        //        //else
        //        //    cmd.CommandText = "SELECT name, current_value FROM sys.sequences Where name = 'Doc_TKNoB' ";

        //        dr = cmd.ExecuteReader();
        //        if (dr.HasRows)
        //        {
        //            while (dr.Read())
        //            {
        //                //GenDoc
        //                string lvQNo = dr["Q_TKNo"].ToString();
        //                lvReturn = lvQNo;

        //                if (lvQNo == "")
        //                    lvReturn = "1";
        //                else
        //                {
        //                    lvReturn = (Gstr.fncToInt(lvQNo) + 1).ToString();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            lvReturn = "1";
        //        }

        //        dr.Close();
        //        con.Close();

        //        return lvReturn;
        //        #endregion  
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        public static string fncGetLastTakaoNo_New(string lvRail, string lvTypeDoc, bool lvOnline)
        {
            string lvReturn = "";
            try
            {
                if (GVar.gvTypeDocTakao == "" || GVar.gvTypeDocTakao == null)
                    GVar.gvTypeDocTakao = fncGetLastTakaoTypeDoc(lvRail);

                if (lvRail == "-") return "";
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT Top 1 Q_Runno,Q_TypeDoc FROM Queue_TableNumber Where Q_Rail = '" + lvRail + "' and Q_Year = '' and Q_TypeDoc = '"+ lvTypeDoc + "' Order by Q_TypeDoc Desc,Q_Runno Desc ";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //GenDoc
                        int lvQNo = Gstr.fncToInt(dr["Q_Runno"].ToString());
                        GVar.gvTypeDocTakao = dr["Q_TypeDoc"].ToString();

                        lvReturn = lvQNo.ToString();

                        if (lvQNo == 0)
                            lvReturn = "1";
                        else if (lvQNo >= 501)
                        {
                            lvReturn = "1";
                            GVar.gvTypeDocTakao = (Gstr.fncToInt(GVar.gvTypeDocTakao) + 1).ToString();
                        }
                        else
                        {
                            lvReturn = (lvQNo + 1).ToString();
                        }
                    }
                }
                else
                {
                    lvReturn = "1";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion  
            }
            catch
            {
                return "";
            }
        }

        public static string fncGetLastTakaoTypeDoc(string lvRail)
        {
            string lvReturn = "";
            try
            {
                if (lvRail == "-") return "0";
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT TOP 1 Q_TypeDoc FROM Queue_TableNumber Where Q_Rail = '" + lvRail + "' and Q_Year = '' Order by Cast(Q_TypeDoc as int) Desc,Q_Runno Desc ";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        GVar.gvTypeDocTakao = dr["Q_TypeDoc"].ToString();
                        lvReturn = GVar.gvTypeDocTakao;
                    }
                }
                else
                {
                    lvReturn = "0";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion  
            }
            catch
            {
                return "";
            }
        }

        public static string fncGenQueueNo(bool lvOnline)
        {
            string lvReturn = "";
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

                    string lvFeild = "Queue_01";
                    if (GVar.gvStation == "2") lvFeild = "Queue_02";
                    if (GVar.gvStation == "3") lvFeild = "Queue_03";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT S_RunNo, S_Format FROM SysDocNo Where S_MCode = '" + lvFeild + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //GenDoc
                            string lvQNo = dr["S_RunNo"].ToString();
                            string lvFormat = dr["S_Format"].ToString();
                            if (Gstr.fncToInt(lvQNo) == 0)
                            {
                                lvReturn = GVar.gvStation + Gstr.fncToInt("1").ToString(lvFormat);
                            }
                            else
                            {
                                int lvLast = Gstr.fncToInt(lvQNo);
                                if ((lvLast + 1) > 99999)
                                {
                                    lvLast = 0;

                                    string lvSQL = "Update SysDocNo Set S_RunNo = 0 Where S_MCode = '" + lvFeild + "' ";
                                    string lvResault = fncExecuteQueryData(lvSQL, GVar.gvOnline);
                                }

                                lvReturn = GVar.gvStation + (lvLast + 1).ToString(lvFormat);
                            }
                        }
                    }
                    //else
                    //{
                    //    lvReturn = GVar.gvStation + Gstr.fncToInt("1").ToString(lvFormat);
                    //}

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

                    string lvFeild = "Queue_01";
                    if (GVar.gvStation == "2") lvFeild = "Queue_02";

                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT S_RunNo FROM SysDocNo Where S_MCode = '" + lvFeild + "' ";
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            //GenDoc
                            string lvQNo = dr["S_RunNo"].ToString();
                            if (Gstr.fncToInt(lvQNo) == 0)
                            {
                                lvReturn = "000001";
                            }
                            else
                            {
                                int lvLast = Gstr.fncToInt(lvQNo);
                                if ((lvLast + 1) > 9999) lvLast = 1;

                                lvReturn = (lvLast + 1).ToString("000000");
                            }
                        }
                    }
                    else
                    {
                        lvReturn = "000001";
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

        public static string fncGetLastRealTakao()
        {
            string lvReturn = "";
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Select Q_Runno From Queue_TableNumber ";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //GenDoc
                        lvReturn = dr["Q_Runno"].ToString();
                    }
                }
                else
                {
                    lvReturn = "1";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion  
            }
            catch
            {
                return "";
            }
        }

        public static string fncGetQuotaActive(string lvQuota)
        {
            string lvReturn = "";
            try
            {
                #region ONLINE
                #region //Connect Database ~
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Select Cane_Quota From Cane_QuotaCheck Where Cane_Quota = '" + lvQuota + "' ";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //GenDoc
                        lvReturn = dr["Cane_Quota"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion  
            }
            catch
            {
                return "";
            }
        }

        public static string fncGetCurCar2(string lvQ)
        {
            string lvReturn = "";
            try
            {
                #region ONLINE
                #region //Connect Database ~
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Select Q_CutCar From Queue_Diary Where Q_No = '" + lvQ + "' And Q_Year = ''";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //GenDoc
                        lvReturn = dr["Q_CutCar"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion  
            }
            catch
            {
                return "";
            }
        }

        public static string fncGetCutDoc2(string lvQ)
        {
            string lvReturn = "";
            try
            {
                #region ONLINE
                #region //Connect Database ~
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Select Q_CutDoc From Queue_Diary Where Q_No = '" + lvQ + "' And Q_Year = ''";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //GenDoc
                        lvReturn = dr["Q_CutDoc"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion  
            }
            catch
            {
                return "";
            }
        }

        //Log
        public static string fncKeepLogData(string lvUser, string lvDisplay, string lvActivity)
        {
            string lvReturn = "";

            try
            {
                DateTime DTNow = DateTime.Now;
                if (Gstr.fncToInt(DTNow.ToString("yyyy")) > 2500)
                    DTNow = DTNow.AddYears(-543);

                string lvDate = DTNow.ToString("dd/MM/yyyy HH:mm:ss");

                string lvSQL = "insert into Log_History (Log_UserID, Log_Display, Log_Activity, Log_DateTime) ";
                lvSQL += "Values ('" + lvUser + "', '" + lvDisplay + "', '" + lvActivity + "', '" + lvDate + "')";

                string query = lvSQL;
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PS_LogConnection"].ToString());
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();

                lvReturn = "Success";
                return lvReturn;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool fncChkOnline(string nameOrAddress)
        {
            bool pingable = false;
            //Ping pinger = new Ping();
            //try
            //{
            //    PingReply reply = pinger.Send(nameOrAddress);
            //    pingable = reply.Status == IPStatus.Success;
            //}
            //catch (PingException)
            //{
            //    // Discard PingExceptions and return false;
            //}
            return pingable;
        }

        public static string fncGetOwnerName(string lvID)
        {
            string lvReturn = "";
            try
            {
                #region ONLINE
                #region //Connect Database ~
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Select P_CarContractorName,P_CarContractorLastName From Cane_CarContractor Where P_CarContractorID = '" + lvID + "' ";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //GenDoc
                        lvReturn = dr["P_CarContractorName"].ToString() + " " + dr["P_CarContractorLastName"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion  
            }
            catch
            {
                return "";
            }
        }

        public static string fncGetCarCutName(string lvID)
        {
            string lvReturn = "";
            try
            {
                #region ONLINE
                #region //Connect Database ~
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Select C_Name From Cane_CarCut Where C_Code = '" + lvID + "' ";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //GenDoc
                        lvReturn = dr["C_Name"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion  
            }
            catch
            {
                return "";
            }
        }

        public static string fncGetCaneTypeDetail(string lvQNo)
        {
            string lvReturn = "";
            try
            {
                #region ONLINE

                #region //Connect Database ~
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Select Q_CaneType From Queue_Diary WHERE Q_No = '" + lvQNo + "' AND Q_Year = '' ";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //GenDoc
                        lvReturn = dr["Q_CaneType"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion  
            }
            catch
            {
                return "";
            }
        }

        public static string fncGetCarnum1(string lvQNo)
        {
            string lvReturn = "";
            try
            {
                #region ONLINE

                #region //Connect Database ~
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Select Top 1 Q_No, Q_CarNum From Queue_Diary WHERE Q_No = '" + lvQNo + "' ";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //GenDoc
                        lvReturn = dr["Q_CarNum"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion  
            }
            catch
            {
                return "";
            }
        }

        public static string fncGetCutdoc(string lvQNo)
        {
            string lvReturn = "";
            try
            {
                #region ONLINE

                #region //Connect Database ~
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Select Q_CutDoc From Queue_Diary WHERE Q_No = '" + lvQNo + "' ";

                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //GenDoc
                        lvReturn = dr["Q_CutDoc"].ToString();
                    }
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion  
            }
            catch
            {
                return "";
            }
        }

        //ค้นหาเลข C_ID จาก Cane_QueueData
        public static string fncGetLastCutdocID()
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
                cmd.CommandText = "SELECT TOP 1 C_ID FROM Cane_QueueData ORDER BY C_ID DESC ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["C_ID"].ToString();
                        int lvGen = Gstr.fncToInt(lvReturn) + 1;
                        lvReturn = lvGen.ToString();
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

        //ค้นหาเลข C_ID จาก Cane_QueueData
        public static string fncGetLastCutdocIDByQ(string lvQNo)
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
                cmd.CommandText = "SELECT TOP 1 C_ID FROM Cane_QueueData WHERE C_Queue = '" + lvQNo + "'  ORDER BY C_ID DESC ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["C_ID"].ToString();

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

        public static string fncGetCarContractorPrice(string lvID, string lvType)
        {
            try
            {
                string lvReturn = "";
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion


                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT P_Price FROM Cane_CarContractor WHERE P_CarContractorID = '" + lvID + "' AND P_CarContractortype LIKE '%" + lvType + "%' "; //And Q_CloseStatus = ''
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["P_Price"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string fncGet(string lvID)
        {
            try
            {
                #region ONLINE
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnection"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                string lvReturn = "";

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT P_Price FROM Cane_CarContractor WHERE P_CarContractorID = '" + lvID + "' "; //And Q_CloseStatus = ''
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["P_Price"].ToString();
                    }
                }
                else
                {
                    lvReturn = "";
                }

                dr.Close();
                con.Close();

                return lvReturn;
                #endregion

            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string fncGetQByCutDocID(string lvSearch)
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
                cmd.CommandText = "SELECT TOP 1 C_Queue FROM Cane_QueueData WHERE C_ID = '" + lvSearch + "'  ORDER BY C_ID DESC ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["C_Queue"].ToString();

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

        public static string fncGetBonsucroMCSS(string lvSearch, string lvYear)
        {
            string lvReturn = "";
            try
            {
                #region //Connect Database 
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PSConnectionMCSS"].ToString());
                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                #endregion

                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT IsBonsucro From Plans WHERE Code = '" + lvSearch + "' AND SeasonYear = '" + lvYear + "'";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["IsBonsucro"].ToString();

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

        public static string fncGetQuotaChangeZero(string lvQuota, string lvContractor)
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
                cmd.CommandText = "SELECT Contractor From Con_ChangeZero WHERE Quota = '" + lvQuota + "' AND Contractor = '" + lvContractor + "' ";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        lvReturn = dr["Contractor"].ToString();

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