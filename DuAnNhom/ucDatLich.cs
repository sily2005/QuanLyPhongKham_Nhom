using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DuAnNhom
{
    public partial class ucDatLich : Form
    {
        private readonly string _conStr = @"Server=.;Database=QuanLyPhongKham;Trusted_Connection=True;TrustServerCertificate=True;";

        public ucDatLich()
        {
            InitializeComponent();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            // Dùng toán tử -= trước += để đảm bảo dù có gọi lại cũng không bị lặp
            this.btnTimKiem.Click -= btnTimKiem_Click;
            this.btnTimKiem.Click += btnTimKiem_Click;

            this.btnXacnhan.Click -= btnXacnhan_Click;
            this.btnXacnhan.Click += btnXacnhan_Click;

            this.btnSua.Click -= btnSua_Click;
            this.btnSua.Click += btnSua_Click;
            this.dgvLichHen.CellClick += dgvLichHen_CellClick;

            // Nếu bạn sợ mất Form, hãy thêm dòng này nhưng phải có -= trước
            this.Load -= UcDatLich_Load;
            this.Load += UcDatLich_Load;
        }

        private void dgvLichHen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLichHen.Rows[e.RowIndex];

                txtTenKH.Text = row.Cells["TenBenhNhan"].Value?.ToString();

                // THÊM DÒNG NÀY:
                txtSdtKH.Text = row.Cells["SoDienThoai"].Value?.ToString();

                dtpNgayHen.Value = Convert.ToDateTime(row.Cells["NgayHen"].Value);
                cboTrangThai.Text = row.Cells["TrangThai"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();
                txtSoTien.Text = row.Cells["SoTien"].Value?.ToString();
                cboBacSi.Text = row.Cells["TenBacSi"].Value?.ToString();
            }
        }

        private void UcDatLich_Load(object sender, EventArgs e)
        {
            // PHẢI GỌI CÁC HÀM NÀY THÌ DỮ LIỆU MỚI LÊN COMBOBOX
            LoadChuyenKhoa();
            LoadTrangThaiMacDinh();
            HienThiDanhSachLichHen();
        }

        private void LoadTrangThaiMacDinh()
        {
            // THÊM DÒNG NÀY
            cboTrangThai.Items.Clear();

            cboTrangThai.Items.AddRange(new object[] { "Chờ xác nhận", "Đã xác nhận", "Đã hủy" });
            cboTrangThai.SelectedIndex = 0;
        }

        private void LoadChuyenKhoa()
        {
            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                try
                {
                    string sql = "SELECT MaChuyenKhoa, TenChuyenKhoa, GiaKham FROM ChuyenKhoa";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // THÊM DÒNG NÀY (Gỡ sự kiện cũ trước khi gán DataSource mới)
                    cboChuyenKhoaSearch.SelectedIndexChanged -= CboChuyenKhoaSearch_SelectedIndexChanged;

                    cboChuyenKhoaSearch.DataSource = dt;
                    cboChuyenKhoaSearch.DisplayMember = "TenChuyenKhoa";
                    cboChuyenKhoaSearch.ValueMember = "MaChuyenKhoa";

                    // THÊM DÒNG NÀY (Gán lại sự kiện sau khi nạp xong)
                    cboChuyenKhoaSearch.SelectedIndexChanged += CboChuyenKhoaSearch_SelectedIndexChanged;
                }
                catch (Exception ex) { MessageBox.Show("Lỗi tải chuyên khoa: " + ex.Message); }
            }
        }

        private void CboChuyenKhoaSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChuyenKhoaSearch.SelectedItem is DataRowView row)
            {
                decimal giaKham = row["GiaKham"] != DBNull.Value ? Convert.ToDecimal(row["GiaKham"]) : 0;
                txtSoTien.Text = giaKham.ToString("N0");
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cboChuyenKhoaSearch.SelectedValue == null) return;
            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                try
                {
                    string sql = "SELECT MaNhanVien, TenNhanVien FROM NhanVien WHERE MaChuyenKhoa = @maCK AND MaVaiTro = 1";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@maCK", cboChuyenKhoaSearch.SelectedValue);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cboBacSi.DataSource = dt;
                    cboBacSi.DisplayMember = "TenNhanVien";
                    cboBacSi.ValueMember = "MaNhanVien";
                }
                catch (Exception ex) { MessageBox.Show("Lỗi tìm bác sĩ: " + ex.Message); }
            }
        }

        private void HienThiDanhSachLichHen()
        {
            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                try
                {
                    // THÊM B.SoDienThoai vào sau B.TenBenhNhan
                    string sql = @"SELECT L.MaLichHen, B.TenBenhNhan, B.SoDienThoai, 
                           N.TenNhanVien as TenBacSi, L.NgayHen, L.TrangThai, L.GhiChu, L.SoTien
                           FROM LichHen L
                           JOIN BenhNhan B ON L.MaBenhNhan = B.MaBenhNhan
                           JOIN NhanVien N ON L.MaBacSi = N.MaNhanVien
                           ORDER BY L.MaLichHen DESC";

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvLichHen.DataSource = dt;
                }
                catch (Exception ex) { MessageBox.Show("Lỗi hiển thị: " + ex.Message); }
            }
        }

        private void btnXacnhan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKH.Text) || cboBacSi.SelectedValue == null )
            {
                MessageBox.Show("Vui lòng nhập tên, chọn bác sĩ!");
                return;
            }

            decimal soTien = 0;
            decimal.TryParse(txtSoTien.Text.Replace(",", ""), out soTien);

            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Thêm bệnh nhân
                        string sqlBN = "INSERT INTO BenhNhan (TenBenhNhan, SoDienThoai) OUTPUT INSERTED.MaBenhNhan VALUES (@ten, @sdt)";
                        SqlCommand cmdBN = new SqlCommand(sqlBN, conn, trans);
                        cmdBN.Parameters.AddWithValue("@ten", txtTenKH.Text.Trim());
                        cmdBN.Parameters.AddWithValue("@sdt", txtSdtKH.Text.Trim());
                        int maBN = (int)cmdBN.ExecuteScalar();

                        // 2. Thêm lịch hẹn (Có MaCaTruc và các thông tin bổ sung)
                        string sqlLH = @"INSERT INTO LichHen (MaBenhNhan, MaBacSi, NgayHen, TrangThai, GhiChu, SoTien) 
                                         VALUES (@maBN, @maBS, @ngay, @tt, @gc, @st)";
                        SqlCommand cmdLH = new SqlCommand(sqlLH, conn, trans);
                        cmdLH.Parameters.AddWithValue("@maBN", maBN);
                        cmdLH.Parameters.AddWithValue("@maBS", cboBacSi.SelectedValue);
                        cmdLH.Parameters.AddWithValue("@ngay", dtpNgayHen.Value.Date);
                        cmdLH.Parameters.AddWithValue("@tt", cboTrangThai.Text);
                        cmdLH.Parameters.AddWithValue("@gc", txtGhiChu.Text);
                        cmdLH.Parameters.AddWithValue("@st", soTien);

                        cmdLH.ExecuteNonQuery();
                        trans.Commit();
                        MessageBox.Show("Đặt lịch thành công!");
                        HienThiDanhSachLichHen();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvLichHen.CurrentRow == null) return;

            int maLH = Convert.ToInt32(dgvLichHen.CurrentRow.Cells["MaLichHen"].Value);

            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                try
                {
                    conn.Open();
                    // Lấy MaBenhNhan từ DB dựa trên MaLichHen để sửa thông tin bệnh nhân
                    string sqlGetBN = "SELECT MaBenhNhan FROM LichHen WHERE MaLichHen = @id";
                    SqlCommand cmdGet = new SqlCommand(sqlGetBN, conn);
                    cmdGet.Parameters.AddWithValue("@id", maLH);
                    int maBN = (int)cmdGet.ExecuteScalar();

                    // 1. Cập nhật thông tin Bệnh nhân (Tên, SĐT)
                    string sqlBN = "UPDATE BenhNhan SET TenBenhNhan=@ten, SoDienThoai=@sdt WHERE MaBenhNhan=@maBN";
                    SqlCommand cmdBN = new SqlCommand(sqlBN, conn);
                    cmdBN.Parameters.AddWithValue("@ten", txtTenKH.Text.Trim());
                    cmdBN.Parameters.AddWithValue("@sdt", txtSdtKH.Text.Trim());
                    cmdBN.Parameters.AddWithValue("@maBN", maBN);
                    cmdBN.ExecuteNonQuery();

                    // 2. Cập nhật thông tin Lịch hẹn
                    string sqlLH = @"UPDATE LichHen SET MaBacSi=@maBS, NgayHen=@ngay, 
                             TrangThai=@tt, GhiChu=@gc, SoTien=@st WHERE MaLichHen=@id";
                    SqlCommand cmdLH = new SqlCommand(sqlLH, conn);
                    cmdLH.Parameters.AddWithValue("@maBS", cboBacSi.SelectedValue);
                    cmdLH.Parameters.AddWithValue("@ngay", dtpNgayHen.Value.Date);
                    cmdLH.Parameters.AddWithValue("@tt", cboTrangThai.Text);
                    cmdLH.Parameters.AddWithValue("@gc", txtGhiChu.Text);

                    decimal st = 0;
                    decimal.TryParse(txtSoTien.Text.Replace(",", ""), out st);
                    cmdLH.Parameters.AddWithValue("@st", st);
                    cmdLH.Parameters.AddWithValue("@id", maLH);

                    cmdLH.ExecuteNonQuery();

                    MessageBox.Show("Cập nhật thông tin thành công!");
                    HienThiDanhSachLichHen();
                }
                catch (Exception ex) { MessageBox.Show("Lỗi sửa: " + ex.Message); }
            }
        }

        private void txtSdtKH_TextChanged(object sender, EventArgs e)
        {

        }
    }
}