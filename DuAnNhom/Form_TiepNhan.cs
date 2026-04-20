using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DuAnNhom
{
    public partial class Form_TiepNhan : Form
    {
        string connectionString = "Data Source=DESKTOP-PJNQ1CC;Initial Catalog=QuanLyPhongKham;Integrated Security=True;Trust Server Certificate=True";
        int maNhanVienDangNhap = 1;
        public Form_TiepNhan()
        {
            InitializeComponent();
            this.Load += Form_TiepNhan_Load;
            dgvDanhSachCho.CellClick += dgvDanhSachCho_CellClick;
        }

        private void Form_TiepNhan_Load(object sender, EventArgs e)
        {
            LoadComboBoxBacSi();
            LoadComboBoxCaTruc();
            LoadDanhSachCho();
        }
        private void LoadComboBoxBacSi()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT nv.MaNhanVien, nv.TenNhanVien 
                         FROM NhanVien nv
                         JOIN VaiTro vt ON nv.MaVaiTro = vt.MaVaiTro
                         WHERE vt.TenVaiTro = N'Bác sĩ' AND nv.TrangThai = 1";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboBacSi.DataSource = dt;
                cboBacSi.DisplayMember = "TenNhanVien";
                cboBacSi.ValueMember = "MaNhanVien";
                cboBacSi.SelectedIndex = -1; // không chọn gì
            }
        }

        private void LoadComboBoxCaTruc()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MaCaTruc, TenCaTruc FROM CaTruc";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cboCaTruc.DataSource = dt;
                cboCaTruc.DisplayMember = "TenCaTruc";
                cboCaTruc.ValueMember = "MaCaTruc";
                cboCaTruc.SelectedIndex = -1;
            }
        }

        private void LoadDanhSachCho()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT lh.MaLichHen, bn.TenBenhNhan, bn.SoDienThoai, 
                                 nv.TenNhanVien AS BacSi, lh.NgayHen, lh.TrangThai
                          FROM LichHen lh
                          JOIN BenhNhan bn ON lh.MaBenhNhan = bn.MaBenhNhan
                          JOIN NhanVien nv ON lh.MaBacSi = nv.MaNhanVien
                          WHERE lh.TrangThai = N'Chờ xác nhận'
                          ORDER BY lh.NgayHen ASC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvDanhSachCho.DataSource = dt;

                // Ẩn cột MaLichHen (cột đầu tiên)
                if (dgvDanhSachCho.Columns["MaLichHen"] != null)
                    dgvDanhSachCho.Columns["MaLichHen"].Visible = false;
            }
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string sdt = txtSDT.Text.Trim();
            if (string.IsNullOrEmpty(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại cần tìm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimBenhNhanTheoSDT(sdt);
        }

        private void TimBenhNhanTheoSDT(string sdt)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MaBenhNhan, TenBenhNhan, DiaChi, NgaySinh, GioiTinh, TieuSuBenh FROM BenhNhan WHERE SoDienThoai = @sdt";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sdt", sdt);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Đổ thông tin lên form
                    DataRow row = dt.Rows[0];
                    txtHoTen.Text = row["TenBenhNhan"].ToString();
                    txtDiaChi.Text = row["DiaChi"].ToString();
                    dtpNgaySinh.Value = row["NgaySinh"] != DBNull.Value ? Convert.ToDateTime(row["NgaySinh"]) : DateTime.Now.AddYears(-20);
                    cboGioiTinh.Text = row["GioiTinh"].ToString();
                    txtTieuSu.Text = row["TieuSuBenh"].ToString();
                    // Lưu MaBenhNhan vào Tag của form hoặc một label ẩn
                    this.Tag = row["MaBenhNhan"]; // tạm dùng Tag để lưu
                }
                else
                {
                    // Bệnh nhân mới: xóa trắng các trường
                    ClearBenhNhanInfo();
                    MessageBox.Show("Không tìm thấy bệnh nhân. Vui lòng nhập thông tin mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ClearBenhNhanInfo()
        {
            txtHoTen.Clear();
            txtDiaChi.Clear();
            dtpNgaySinh.Value = DateTime.Now.AddYears(-20);
            cboGioiTinh.SelectedIndex = -1;
            txtTieuSu.Clear();
            this.Tag = null; // xóa mã bệnh nhân
        }
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu nhập
            if (string.IsNullOrEmpty(txtHoTen.Text) || string.IsNullOrEmpty(txtSDT.Text))
            {
                MessageBox.Show("Họ tên và số điện thoại không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cboBacSi.SelectedValue == null || cboCaTruc.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn bác sĩ và ca trực.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 1. Thêm hoặc cập nhật bệnh nhân
            int maBenhNhan = ThemHoacCapNhatBenhNhan();
            if (maBenhNhan == -1) return;

            // 2. Tạo lịch hẹn
            bool taoLichThanhCong = TaoLichHen(maBenhNhan);
            if (taoLichThanhCong)
            {
                MessageBox.Show("Đã đăng ký khám thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Làm mới danh sách chờ và reset form
                LoadDanhSachCho();
                btnLamMoi_Click(null, null);
            }
            else
            {
                MessageBox.Show("Đăng ký thất bại. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int ThemHoacCapNhatBenhNhan()
        {
            int maBN = (this.Tag != null) ? (int)this.Tag : 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (maBN == 0) // Thêm mới
                {
                    string insertQuery = @"INSERT INTO BenhNhan(TenBenhNhan, SoDienThoai, DiaChi, NgaySinh, GioiTinh, TieuSuBenh)
                                   VALUES (@ten, @sdt, @diachi, @ngaysinh, @gt, @tieusu);
                                   SELECT SCOPE_IDENTITY();";
                    SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@ten", txtHoTen.Text);
                    cmd.Parameters.AddWithValue("@sdt", txtSDT.Text);
                    cmd.Parameters.AddWithValue("@diachi", txtDiaChi.Text);
                    cmd.Parameters.AddWithValue("@ngaysinh", dtpNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@gt", cboGioiTinh.Text);
                    cmd.Parameters.AddWithValue("@tieusu", txtTieuSu.Text);
                    conn.Open();
                    maBN = Convert.ToInt32(cmd.ExecuteScalar());
                    return maBN;
                }
                else // Cập nhật thông tin bệnh nhân cũ
                {
                    string updateQuery = @"UPDATE BenhNhan SET TenBenhNhan=@ten, DiaChi=@diachi, NgaySinh=@ngaysinh, 
                                    GioiTinh=@gt, TieuSuBenh=@tieusu WHERE MaBenhNhan=@mabn";
                    SqlCommand cmd = new SqlCommand(updateQuery, conn);
                    cmd.Parameters.AddWithValue("@ten", txtHoTen.Text);
                    cmd.Parameters.AddWithValue("@diachi", txtDiaChi.Text);
                    cmd.Parameters.AddWithValue("@ngaysinh", dtpNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@gt", cboGioiTinh.Text);
                    cmd.Parameters.AddWithValue("@tieusu", txtTieuSu.Text);
                    cmd.Parameters.AddWithValue("@mabn", maBN);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return maBN;
                }
            }
        }

        private bool TaoLichHen(int maBenhNhan)
        {
            int maBacSi = (int)cboBacSi.SelectedValue;
            int maCa = (int)cboCaTruc.SelectedValue;
            DateTime ngayHen = dtpNgayHen.Value;
            string phong = txtPhong.Text.Trim();
            if (string.IsNullOrEmpty(phong)) phong = "Chưa xác định";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO LichHen(MaBenhNhan, MaBacSi, NgayHen, MaCaTruc, TrangThai, GhiChu)
                         VALUES (@mabn, @mabs, @ngay, @maca, N'Chờ xác nhận', @phong)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@mabn", maBenhNhan);
                cmd.Parameters.AddWithValue("@mabs", maBacSi);
                cmd.Parameters.AddWithValue("@ngay", ngayHen);
                cmd.Parameters.AddWithValue("@maca", maCa);
                cmd.Parameters.AddWithValue("@phong", phong);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // Xóa trắng các textbox, reset combo, datepicker
            txtSDT.Clear();
            txtHoTen.Clear();
            txtDiaChi.Clear();
            dtpNgaySinh.Value = DateTime.Now.AddYears(-20);
            cboGioiTinh.SelectedIndex = -1;
            txtTieuSu.Clear();
            cboBacSi.SelectedIndex = -1;
            cboCaTruc.SelectedIndex = -1;
            dtpNgayHen.Value = DateTime.Now.AddDays(1);
            txtPhong.Clear();
            this.Tag = null;

            // Tùy chọn: focus vào ô tìm SĐT
            txtSDT.Focus();
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void dgvDanhSachCho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDanhSachCho.Rows[e.RowIndex];
                string tenBN = row.Cells["TenBenhNhan"].Value.ToString();
                string sdt = row.Cells["SoDienThoai"].Value.ToString();
                // Tự động tìm kiếm bệnh nhân theo SĐT
                txtSDT.Text = sdt;
                btnTimKiem_Click(null, null);
            }
        }
    }
}
