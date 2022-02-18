using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSQueue
{
    public partial class frmDBF : Form
    {
        public DataSet ds;

        public frmDBF()
        {
            InitializeComponent();
        }

        private void frmDBF_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            DataSetIntoDBF("NEWTRUCK", ds);

            //Rename File Newtruck
            string lvPath = "X:\\";
            string lvFileName = "NEWTRUCK";
            if (File.Exists(lvPath + "NEWTRUCK.dbf"))
            {
                string lvSorce = lvPath + "NEWTRUCK.dbf";

                string lvDay = DateTime.Now.ToString("dd");
                string lvMonth = DateTime.Now.ToString("MM");
                string lvYear = DateTime.Now.ToString("yyyy");

                if (Gstr.fncToInt(lvYear) < 2500)
                    lvYear = (Gstr.fncToInt(lvYear) + 543).ToString();

                //ปีเอาแค่ 61 พอ
                lvYear = Gstr.Right(lvYear, 2);
                string lvDes = lvPath + "Newtruck" + lvDay + lvMonth + lvYear + ".dbf";

                //ถ้ามีก็ลบ
                if (File.Exists(lvDes))
                {
                    File.Delete(lvDes);
                }

                File.Move(lvSorce, lvDes);
            }

            MessageBox.Show("Export เรียบร้อย");
            System.Diagnostics.Process.Start("X:\\");

            this.Close();
        }

        public void DataSetIntoDBF(string fileName, DataSet dataSet)
        {
            string lvPath = "X:\\";
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
            return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=dBASE IV;";
        }

        private static string fncGetSizeField(string lvField, string lvType)
        {
            string lvReturn = "";

            string lvSize = "";
            if (lvField == "CAR_NO") lvSize = "50";
            else if (lvField == "BILLNO") lvSize = "20,5";
            else if (lvField == "IN_NO") lvSize = "20,5";
            else if (lvField == "QUO_NO") lvSize = "20,5";
            else if (lvField == "TYPE") lvSize = "3";
            else if (lvField == "PLATE") lvSize = "50";
            else if (lvField == "DATE_IN") lvSize = "8";
            else if (lvField == "TIME_IN") lvSize = "5";
            else if (lvField == "TOTAL_WT") lvSize = "20,5";
            else if (lvField == "DATE_OUT") lvSize = "8";
            else if (lvField == "TIME_OUT") lvSize = "5";
            else if (lvField == "CAR_WT") lvSize = "20,5";
            else if (lvField == "NET_WT") lvSize = "20,5";
            else if (lvField == "PLOT") lvSize = "50";
            else if (lvField == "W1WEIGHT") lvSize = "20,5";
            else if (lvField == "W2WEIGHT") lvSize = "20,5";
            else if (lvField == "REMARK1") lvSize = "250";
            else if (lvField == "REMARK2") lvSize = "250";
            else if (lvField == "APPROVE1") lvSize = "50";
            else if (lvField == "APPROVE2") lvSize = "50";
            else if (lvField == "TYPE1") lvSize = "20,5";
            else if (lvField == "TYPE2") lvSize = "20,5";
            else if (lvField == "AREA") lvSize = "50";
            else if (lvField == "FINISH_TIM") lvSize = "50";
            else if (lvField == "LOKHIB_NO") lvSize = "50";
            else if (lvField == "HARVESTOR_") lvSize = "50";
            else if (lvField == "HARV_PRICE") lvSize = "20,2";
            else if (lvField == "TRUCK_NO") lvSize = "50";
            else if (lvField == "TRUCK_PRICE") lvSize = "20,2";
            else if (lvField == "CONTROL_NO") lvSize = "50";
            else if (lvField == "CRANE_QU_NO") lvSize = "50";
            else if (lvField == "LAST_AREA") lvSize = "1";
            else if (lvField == "T1_NET_WT") lvSize = "20,5";
            else if (lvField == "T2_NET_WT") lvSize = "20,5";
            else if (lvField == "STATION") lvSize = "50";

            switch (lvType)
            {
                case "System.String":
                    lvReturn = "varchar(" + lvSize + ")";
                    break;

                case "System.Boolean":
                    lvReturn = "varchar(" + lvSize + ")";
                    break;

                case "System.Int32":
                    lvReturn = "int";
                    break;

                case "System.Double":
                    lvReturn = "numeric(" + lvSize + ")";
                    break;

                case "System.DateTime":
                    lvReturn = "TimeStamp";
                    break;
            }

            return lvReturn;
        }

        public static string ReplaceEscape(string str)
        {
            str = str.Replace("'", "''");
            return str;
        }

        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSetIntoDBF("", ds);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Rename File Newtruck
            string lvPath = "X:\\";
            string lvFileName = "NEWTRUCK";
            if (File.Exists(lvPath + "NEWTRUCK.dbf"))
            {
                string lvSorce = lvPath + "NEWTRUCK.dbf";

                string lvDay = DateTime.Now.ToString("dd");
                string lvMonth = DateTime.Now.ToString("MM");
                string lvYear = DateTime.Now.ToString("yyyy");

                if (Gstr.fncToInt(lvYear) < 2500)
                    lvYear = (Gstr.fncToInt(lvYear) + 543).ToString();

                //ปีเอาแค่ 61 พอ
                lvYear = Gstr.Right(lvYear, 2);
                string lvDes = lvPath + "Newtruck" + lvDay + lvMonth + lvYear + ".dbf";

                //ถ้ามีก็ลบ
                if (File.Exists(lvDes))
                {
                    File.Delete(lvDes);
                }

                File.Move(lvSorce, lvDes);
            }

            System.Diagnostics.Process.Start("X:\\");

            this.Close();
        }
    }
}
