using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DuAnNhom
{
    // Đổi : Form thành UserControl nếu bạn đang nhúng vào Form chính, 
    // hoặc giữ nguyên : Form nếu bạn mở bằng Form Độc lập
    public partial class ucDichVu : UserControl
    {
        private readonly string _conStr = @"Server=.;Database=QuanLyPhongKham;Trusted_Connection=True;TrustServerCertificate=True;";
        private int maDichVuDangChon = -1; // Biến lưu mã dịch vụ thay cho txtMaDichVu

        public string MainConnectionString { get; set; }

        public ucDichVu()
        {
            InitializeComponent();
            this.Load += ucDichVu_Load;

            if (this.btnThem != null) btnThem.Click += btnThem_Click;
            if (this.btnSua != null) btnSua.Click += btnSua_Click;
            if (this.btnXoa != null) btnXoa.Click += btnXoa_Click;
            if (this.dgvDichVu != null) dgvDichVu.CellClick += dgvDichVu_CellClick;

            // Gắn sự kiện tìm kiếm
            if (this.btnTimKiem != null) btnTimKiem.Click += btnTimKiem_Click;
        }

        private void ucDichVu_Load(object sender, EventArgs e)
        {
            TaiDanhSachChuyenKhoa();
            HienThiDanhSachDichVu();
        }

        private void TaiDanhSachChuyenKhoa()
        {
            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT MaChuyenKhoa, TenChuyenKhoa FROM ChuyenKhoa";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (cboLoaiHinh != null)
                    {
                        cboLoaiHinh.DataSource = dt;
                        cboLoaiHinh.DisplayMember = "TenChuyenKhoa";
                        cboLoaiHinh.ValueMember = "MaChuyenKhoa";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải danh sách chuyên khoa: " + ex.Message);
                }
            }
        }

        private void HienThiDanhSachDichVu()
        {
            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT MaDichVu, TenDichVu, GiaDichVu, MaChuyenKhoa FROM DichVu";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDichVu.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi hiển thị danh sách dịch vụ: " + ex.Message);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenDichVu.Text) || string.IsNullOrWhiteSpace(txtGiaDichVu.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin dịch vụ!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                try
                {
                    conn.Open();
                    string sql = "INSERT INTO DichVu (TenDichVu, GiaDichVu, MaChuyenKhoa) VALUES (@ten, @gia, @maChuyenKhoa)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ten", txtTenDichVu.Text.Trim());
                    cmd.Parameters.AddWithValue("@gia", Convert.ToDecimal(txtGiaDichVu.Text.Replace(",", "")));
                    cmd.Parameters.AddWithValue("@maChuyenKhoa", cboLoaiHinh.SelectedValue ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Thêm dịch vụ thành công!");
                    HienThiDanhSachDichVu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn dịch vụ từ DataGridView chưa
            if (maDichVuDangChon == -1)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần cập nhật từ bảng!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                try
                {
                    conn.Open();
                    string sql = "UPDATE DichVu SET TenDichVu = @ten, GiaDichVu = @gia, MaChuyenKhoa = @maChuyenKhoa WHERE MaDichVu = @ma";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ma", maDichVuDangChon);
                    cmd.Parameters.AddWithValue("@ten", txtTenDichVu.Text.Trim());
                    cmd.Parameters.AddWithValue("@gia", Convert.ToDecimal(txtGiaDichVu.Text.Replace(",", "")));
                    cmd.Parameters.AddWithValue("@maChuyenKhoa", cboLoaiHinh.SelectedValue ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật dịch vụ thành công!");
                    HienThiDanhSachDichVu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn dịch vụ từ DataGridView chưa
            if (maDichVuDangChon == -1)
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần xóa!");
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dịch vụ này không?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(_conStr))
                {
                    try
                    {
                        conn.Open();
                        string sql = "DELETE FROM DichVu WHERE MaDichVu = @ma";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@ma", maDichVuDangChon);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Xóa dịch vụ thành công!");

                        // Reset lại trạng thái
                        maDichVuDangChon = -1;
                        HienThiDanhSachDichVu();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xóa dịch vụ: " + ex.Message);
                    }
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiemDichVu.Text.Trim();

            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT MaDichVu, TenDichVu, GiaDichVu, MaChuyenKhoa FROM DichVu WHERE TenDichVu LIKE @tuKhoa";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@tuKhoa", "%" + tuKhoa + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDichVu.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tìm kiếm dịch vụ: " + ex.Message);
                }
            }
        }

        private void dgvDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvDichVu.Rows[e.RowIndex];

                // Lưu lại mã dịch vụ đang chọn vào biến
                maDichVuDangChon = Convert.ToInt32(row.Cells["MaDichVu"].Value);

                if (txtTenDichVu != null)
                    txtTenDichVu.Text = row.Cells["TenDichVu"].Value?.ToString();
                if (txtGiaDichVu != null)
                    txtGiaDichVu.Text = row.Cells["GiaDichVu"].Value?.ToString();

                if (cboLoaiHinh != null && row.Cells["MaChuyenKhoa"].Value != DBNull.Value)
                {
                    cboLoaiHinh.SelectedValue = row.Cells["MaChuyenKhoa"].Value;
                }
            }
        }
    }
}