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
    public partial class frmBillFake : Form
    {
        public frmBillFake()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string lvQ = txtQNo.Text;
            string lvYear = txtYear.Text;

            //ลบข้อมูลเก่า
            string lvSQL = "Delete From SysTemp "; //HD
            string lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);

            //Get Data
            DataTable DT = new DataTable();

            lvSQL = "Select * from Queue_Diary where Q_No = '" + lvQ + "' and Q_Year = '"+ lvYear +"' ";
            DT = GsysSQL.fncGetQueryData(lvSQL, DT, true);

            int lvNumRow = DT.Rows.Count;

            string lvQNo = ""; //เลขที่คิว
            string lvDate = "";
            string lvQuota = "";
            string lvName = "";
            string lvCarNum = "";
            string lvType = "";
            string lvTypeCar = "";
            string lvQName = "";

            for (int i = 0; i < lvNumRow; i++)
            {
                lvQNo = DT.Rows[i]["Q_No"].ToString();
                lvDate = Gstr.fncChangeSDate(DT.Rows[i]["Q_Date"].ToString()); 
                lvQuota = DT.Rows[i]["Q_Quota"].ToString();
                lvName = GsysSQL.fncFindQuotaName(lvQuota,true);
                lvCarNum = DT.Rows[i]["Q_CarNum"].ToString();
                lvType = GsysSQL.fncFindCaneTypeName(DT.Rows[i]["Q_CaneType"].ToString(),true);
                lvTypeCar = DT.Rows[i]["Q_CarType"].ToString();
                lvQName = "Q1";

                lvSQL = "Insert into SysTemp (Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8) ";
                lvSQL += "Values ('" + lvQNo + "','" + lvDate + "', '" + lvQuota + "', '" + lvName + "', '" + lvCarNum + "', '" + lvType + "', '" + lvTypeCar + "', '" + lvQName + "') ";
                lvResault = GsysSQL.fncExecuteQueryDataAccess(lvSQL);
            }

            DT.Dispose();

            //แสดงก่อนพิมพ์
            frmPrint frm = new frmPrint();
            frm.documentViewer1.DocumentSource = typeof(PSQueue.rptBillFake);
            frm.ShowDialog();
        }
    }
}
