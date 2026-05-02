using ReaLTaiizor.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DuAnNhom
{
    public partial class frmMain : MaterialForm
    {
        string Conn = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyPhongKham;Integrated Security=True";

        private ucQuanLyNhanSu ucNhanSu;
        private ucQuanLyCaTruc ucCaTruc;
        private ucDatLich ucDatLich;
        private ucTiepNhanHoSo ucTiepNhanHoSo;

        public frmMain()
        {
            InitializeComponent();
            // --- THÊM 2 DÒNG NÀY ĐỂ GIẢ LẬP ĐĂNG NHẬP ---
            Session.TenNhanVien = "Admin (Test)";
            Session.MaVaiTro = 1; // Giả sử 1 là quyền Admin (Thấy hết menu)
            // -------------------------------------------
            this.Load += frmMain_Load;
        }

        // ==========================================
        // 2. SỰ KIỆN KHI MỞ FORM LÊN
        // ==========================================
        private void frmMain_Load(object sender, EventArgs e)
        {
            // Hiển thị tên người dùng lên góc phải (VD: Xin chào admin)
            lblUser.Text = "Xin chào " + Session.TenNhanVien;

            PhanQuyen();

            // Mặc định tự động mở màn hình đầu tiên dựa theo vai trò
            if (Session.MaVaiTro == (int)VaiTro.Admin)
            {
                btnQuanLyNhanSu_Click(sender, e);
            }
            else if (Session.MaVaiTro == (int)VaiTro.BacSi)
            {
                btnQuanLyCaTruc_Click(sender, e);
            }
            else if (Session.MaVaiTro == (int)VaiTro.LeTan)
            {
                btnTiepNhanHoSo_Click(sender, e);
            }
        }

        // ==========================================
        // 3. LOGIC PHÂN QUYỀN
        // ==========================================
        private void PhanQuyen()
        {
            if (Session.MaVaiTro == (int)VaiTro.Admin)
            {
                // Admin: Thấy tất cả
                btnQuanLyNhanSu.Visible = true;
                btnQuanLyCaTruc.Visible = true;
                btnDatLich.Visible = true;
                btnTiepNhanHoSo.Visible = true;
            }
            else if (Session.MaVaiTro == (int)VaiTro.BacSi)
            {
                // Bác sĩ: Thường chỉ xem lịch trực và khám (giấu nhân sự, đặt lịch, tiếp nhận)
                btnQuanLyNhanSu.Visible = false;
                btnQuanLyCaTruc.Visible = true;
                btnDatLich.Visible = false;
                btnTiepNhanHoSo.Visible = false;
            }
            else if (Session.MaVaiTro == (int)VaiTro.LeTan)
            {
                // Lễ tân: Quản lý bệnh nhân, đặt lịch, tiếp nhận (giấu Quản lý nhân sự)
                btnQuanLyNhanSu.Visible = false;
                btnQuanLyCaTruc.Visible = true; // Có thể xem lịch để biết bác sĩ nào trực
                btnDatLich.Visible = true;
                btnTiepNhanHoSo.Visible = true;
            }
        }

        // ==========================================
        // 4. HÀM XỬ LÝ ĐỔI MÀN HÌNH (KHÔNG SỬA)
        // ==========================================
        private void AddUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(uc);
            uc.BringToFront();
        }

        // ==========================================
        // 5. CÁC NÚT MENU CLICK
        // ==========================================
        private void btnQuanLyNhanSu_Click(object sender, EventArgs e)
        {
            if (ucNhanSu == null)
            {
                ucNhanSu = new ucQuanLyNhanSu(Conn); // Truyền DB
            }
            AddUserControl(ucNhanSu);
            ucNhanSu.LoadDauTien();
        }
        private void btnQuanLyCaTruc_Click(object sender, EventArgs e)
        {
            if (ucCaTruc == null)
            {
                ucCaTruc = new ucQuanLyCaTruc(Conn);
            }
            AddUserControl(ucCaTruc);
            ucCaTruc.LoadDauTien();
        }
        private void btnDatLich_Click(object sender, EventArgs e)
        {
            if (ucDatLich == null)
            {
                ucDatLich = new ucDatLich();
            }
            AddUserControl(ucDatLich);
        }
        private void btnTiepNhanHoSo_Click(object sender, EventArgs e)
        {
            if (ucTiepNhanHoSo == null)
            {
                ucTiepNhanHoSo = new ucTiepNhanHoSo();
            }
            AddUserControl(ucTiepNhanHoSo);
        }

        // ==========================================
        // 6. NÚT ĐĂNG XUẤT
        // ==========================================
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear(); // Xóa phiên đăng nhập

            // Mở lại màn hình Login (bạn nhớ bỏ comment 2 dòng dưới và đổi thành Form Login của bạn nhé)
            // frmLogin f = new frmLogin();
            // f.Show();

            this.Close(); // Đóng Form Main
        }
    }
}