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
    public partial class frmDisplay : Form
    {
        //ตัวแปรเอาไปส่งค่าไปฟอร์มอื่น
        public static frmDisplay instance;
        public Label lb1;
        public Label lb2;
        public Label lb3;
        public Label lb4;
        public Label lb5;
        public Label lb6;

        public frmDisplay()
        {
            InitializeComponent();
            instance = this;
            lb1 = lbCarName;
            lb2 = lbCaneType;
            lb3 = lbQuota;
            lb4 = lbName;
            lb5 = lbCar1;
            lb6 = lbCar2;
        }

        private void frmDisplay_Load(object sender, EventArgs e)
        {

        }

    }
}
