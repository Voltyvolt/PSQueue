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
    public partial class frmLoginOption : Form
    {
        public frmLoginOption()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmLoginOption_Load(object sender, EventArgs e)
        {
            FncLoadData();
        }

        private void FncLoadData()
        {
            //อ่าน TXT
            string[] lvArr = System.IO.File.ReadAllText("C:\\DataFile\\System_Data.dll").Split('/');

            if (lvArr[1] == "IN")
                chkIn.Checked = true;
            else
                chkOut.Checked = true;

            if (lvArr[2] == "Q")
                chkQ.Checked = true;
            else
                chkWeight.Checked = true;

            cmbPort.Text = lvArr[3];
            cmbQ.Text = lvArr[4];
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string lvINOUT = "";
            string lvQWeight = "";
            string lvPorts = "";
            string lvQNo = "";

            if (chkIn.Checked)
                lvINOUT = "IN";
            else
                lvINOUT = "OUT";

            if (chkQ.Checked)
                lvQWeight = "Q";
            else
                lvQWeight = "W";

            lvPorts = cmbPort.Text;
            lvQNo = cmbQ.Text;

            //เขียนลงไปใน txtFile
            string text = "10.104.1.9" + "/" + lvINOUT + "/" + lvQWeight + "/" + lvPorts + "/" + lvQNo + "/" + "ONLINE" + "/DBPS";
            System.IO.File.WriteAllText("C:\\DataFile\\System_Data.dll", text);

            MessageBox.Show("บันทึกเรียบร้อย");
            this.Close();
        }
    }
}
