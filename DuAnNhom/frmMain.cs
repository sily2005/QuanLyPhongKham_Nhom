using ReaLTaiizor.Forms;
using System;
using System.Windows.Forms;

namespace DuAnNhom
{
    public partial class frmMain : MaterialForm
    {
        string Conn = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyPhongKham;Integrated Security=True";

        private ucQuanLyNhanSu ucNhanSu;
        private ucQuanLyCaTruc ucCaTruc;

        public frmMain()
        {
            InitializeComponent();
            this.Load += frmMain_Load; // 👈 đảm bảo event luôn chạy
        }

        // =========================
        // LOAD FORM
        // =========================
        private void frmMain_Load(object sender, EventArgs e)
        {
            lblUser.Text = Session.TenNhanVien;

            PhanQuyen();

            btnQuanLyNhanSu_Click(sender, e);
        }

        // =========================
        // PHÂN QUYỀN
        // =========================
        private void PhanQuyen()
        {
            if (Session.MaVaiTro == (int)VaiTro.Admin)
            {
                btnQuanLyNhanSu.Visible = true;
                btnQuanLyCaTruc.Visible = true;
            }
            else if (Session.MaVaiTro == (int)VaiTro.BacSi)
            {
                btnQuanLyNhanSu.Visible = false;
                btnQuanLyCaTruc.Visible = true;
            }
            else if (Session.MaVaiTro == (int)VaiTro.LeTan)
            {
                btnQuanLyNhanSu.Visible = false;
                btnQuanLyCaTruc.Visible = false;
            }
        }

        // =========================
        // LOAD USERCONTROL
        // =========================
        private void AddUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(uc);
        }

        private void btnQuanLyNhanSu_Click(object sender, EventArgs e)
        {
            if (ucNhanSu == null)
                ucNhanSu = new ucQuanLyNhanSu(Conn);

            AddUserControl(ucNhanSu);
            ucNhanSu.LoadDauTien();
        }

        private void btnQuanLyCaTruc_Click(object sender, EventArgs e)
        {
            if (ucCaTruc == null)
                ucCaTruc = new ucQuanLyCaTruc();

            ucCaTruc.ConnectionString = Conn;

            AddUserControl(ucCaTruc);
            ucCaTruc.LoadDauTien();
        }

        // =========================
        // LOGOUT
        // =========================
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();

            //frmLogin f = new frmLogin();
            //f.Show();

            this.Close();
        }
    }
}