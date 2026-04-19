using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuAnNhom
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            pnlContainer.Controls.Clear();
            ucQuanLyNhanSu uc = new ucQuanLyNhanSu();
            uc.Dock = DockStyle.Fill;
            pnlContainer.Controls.Add(uc);
        }
        private void addUC(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            pnlContainer.Controls.Clear();
            pnlContainer.Controls.Add(uc);
            uc.BringToFront();
        }
        private void btnNhanSu_Click(object sender, EventArgs e)
        {
            addUC(new ucQuanLyNhanSu());
        }
    }
}
