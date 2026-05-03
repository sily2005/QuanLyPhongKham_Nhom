using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DuAnNhom
{
    public partial class ucDatLich : Form
    {
        private readonly string _conStr = @"Server=.;Database=QuanLyPhongKham;Trusted_Connection=True;TrustServerCertificate=True;";
        private decimal GiaDichVu;

        public ucDatLich()
        {
            InitializeComponent();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            this.cyberButton1.Click -= btnTimKiem_Click;
            this.cyberButton1.Click += btnTimKiem_Click;

            this.btnXacnhan.Click -= btnXacnhan_Click;
            this.btnXacnhan.Click += btnXacnhan_Click;

            this.btnSua.Click -= btnSua_Click;
            this.btnSua.Click += btnSua_Click;
            
            this.dgvLichHen.CellClick -= dgvLichHen_CellClick;
            this.dgvLichHen.CellClick += dgvLichHen_CellClick;

            this.Load -= UcDatLich_Load;
            this.Load += UcDatLich_Load;
        }

        private void dgvLichHen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLichHen.Rows[e.RowIndex];

                txtTenKH.Text = row.Cells["TenBenhNhan"].Value?.ToString();
                txtSdtKH.Text = row.Cells["SoDienThoai"].Value?.ToString();
                dtpNgayHen.Value = Convert.ToDateTime(row.Cells["NgayHen"].Value);
                cboTrangThai.Text = row.Cells["TrangThai"].Value?.ToString();
                txtGhiChu.Text = row.Cells["GhiChu"].Value?.ToString();

                // Sửa lại đoạn lấy giá tiền trong btnXacnhan_Click và btnSua_Click:
                string giaText = txtGiaDichVu.Text; // Trực tiếp lấy từ thuộc tính, không cần tìm qua Controls nữa
                decimal.TryParse(giaText.Replace(",", "").Replace(" ", ""), out GiaDichVu);
            }
        }

        private void UcDatLich_Load(object sender, EventArgs e)
        {
            LoadChuyenKhoa();
            LoadTrangThaiMacDinh();
            HienThiDanhSachLichHen();

            if (cboChuyenKhoaSearch.SelectedValue != null)
            {
                btnTimKiem_Click(null, null);
            }
        }

        private void LoadTrangThaiMacDinh()
        {
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
                    string sql = "SELECT MaChuyenKhoa, TenChuyenKhoa FROM ChuyenKhoa";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboChuyenKhoaSearch.SelectedIndexChanged -= CboChuyenKhoaSearch_SelectedIndexChanged;

                    cboChuyenKhoaSearch.DataSource = dt;
                    cboChuyenKhoaSearch.DisplayMember = "TenChuyenKhoa";
                    cboChuyenKhoaSearch.ValueMember = "MaChuyenKhoa";

                    cboChuyenKhoaSearch.SelectedIndexChanged += CboChuyenKhoaSearch_SelectedIndexChanged;
                    cboChuyenKhoaSearch.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tải chuyên khoa: " + ex.Message);
                }
            }
        }

        private void CboChuyenKhoaSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnTimKiem_Click(sender, e);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (cboChuyenKhoaSearch.SelectedValue == null) return;

            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                try
                {
                    string sql = @"SELECT MaNhanVien, TenNhanVien 
                           FROM NhanVien 
                           WHERE MaChuyenKhoa = @maCK 
                             AND MaVaiTro = (SELECT MaVaiTro FROM VaiTro WHERE TenVaiTro = N'Bác sĩ')";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@maCK", cboChuyenKhoaSearch.SelectedValue);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboBacSi.DataSource = dt;
                    cboBacSi.DisplayMember = "TenNhanVien";
                    cboBacSi.ValueMember = "MaNhanVien";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi tìm bác sĩ: " + ex.Message);
                }
            }
        }

        private void HienThiDanhSachLichHen()
        {
            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                try
                {
                    // Lấy chính xác cột GiaDichVu từ bảng LichHen trong CSDL
                    string sql = @"SELECT L.MaLichHen, B.TenBenhNhan, B.SoDienThoai, 
                                   N.TenNhanVien as TenBacSi, L.MaBacSi, 
                                   L.NgayHen, L.TrangThai, L.GhiChu, L.GiaDichVu
                            FROM LichHen L
                            JOIN BenhNhan B ON L.MaBenhNhan = B.MaBenhNhan
                            JOIN NhanVien N ON L.MaBacSi = N.MaNhanVien
                            ORDER BY L.MaLichHen DESC";

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvLichHen.DataSource = dt;

                    if (dgvLichHen.Columns["MaBacSi"] != null)
                        dgvLichHen.Columns["MaBacSi"].Visible = false;
                }
                catch (Exception ex) 
                { 
                    MessageBox.Show("Lỗi hiển thị: " + ex.Message); 
                }
            }
        }

        private void btnXacnhan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKH.Text) || cboBacSi.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập tên, chọn bác sĩ!");
                return;
            }

            decimal giaDichVu = 0;

            // Đọc trực tiếp từ txtGiaDichVu thay vì tìm kiếm qua Controls
            if (!string.IsNullOrWhiteSpace(txtGiaDichVu.Text))
            {
                decimal.TryParse(txtGiaDichVu.Text.Replace(",", "").Replace(" ", ""), out giaDichVu);
            }

            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string sqlBN = "INSERT INTO BenhNhan (TenBenhNhan, SoDienThoai) OUTPUT INSERTED.MaBenhNhan VALUES (@ten, @sdt)";
                        SqlCommand cmdBN = new SqlCommand(sqlBN, conn, trans);
                        cmdBN.Parameters.AddWithValue("@ten", txtTenKH.Text.Trim());
                        cmdBN.Parameters.AddWithValue("@sdt", txtSdtKH.Text.Trim());
                        int maBN = (int)cmdBN.ExecuteScalar();

                        // LƯU Ý: Không sử dụng MaDichVu ở bảng LichHen nữa
                        string sqlLH = @"INSERT INTO LichHen (MaBenhNhan, MaBacSi, NgayHen, TrangThai, GhiChu, GiaDichVu) 
                                 VALUES (@maBN, @maBS, @ngay, @tt, @gc, @gia)";
                        SqlCommand cmdLH = new SqlCommand(sqlLH, conn, trans);
                        cmdLH.Parameters.AddWithValue("@maBN", maBN);
                        cmdLH.Parameters.AddWithValue("@maBS", cboBacSi.SelectedValue);
                        cmdLH.Parameters.AddWithValue("@ngay", dtpNgayHen.Value.Date);
                        cmdLH.Parameters.AddWithValue("@tt", cboTrangThai.Text);
                        cmdLH.Parameters.AddWithValue("@gc", txtGhiChu.Text);
                        cmdLH.Parameters.AddWithValue("@gia", giaDichVu);

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

        public string GiaTien
        {
            get { return txtGiaDichVu.Text; }
            set { txtGiaDichVu.Text = value; }
        }
        public void CapNhatGiaDichVu(string giaDichVu)
        {
            txtGiaDichVu.Text = giaDichVu;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvLichHen.CurrentRow == null) return;

            if (cboBacSi.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn bác sĩ trước khi sửa!");
                return;
            }

            int maLH = Convert.ToInt32(dgvLichHen.CurrentRow.Cells["MaLichHen"].Value);

            using (SqlConnection conn = new SqlConnection(_conStr))
            {
                try
                {
                    conn.Open();
                    string sqlGetBN = "SELECT MaBenhNhan FROM LichHen WHERE MaLichHen = @id";
                    SqlCommand cmdGet = new SqlCommand(sqlGetBN, conn);
                    cmdGet.Parameters.AddWithValue("@id", maLH);
                    int maBN = (int)cmdGet.ExecuteScalar();

                    string sqlBN = "UPDATE BenhNhan SET TenBenhNhan=@ten, SoDienThoai=@sdt WHERE MaBenhNhan=@maBN";
                    SqlCommand cmdBN = new SqlCommand(sqlBN, conn);
                    cmdBN.Parameters.AddWithValue("@ten", txtTenKH.Text.Trim());
                    cmdBN.Parameters.AddWithValue("@sdt", txtSdtKH.Text.Trim());
                    cmdBN.Parameters.AddWithValue("@maBN", maBN);
                    cmdBN.ExecuteNonQuery();

                    string sqlLH = @"UPDATE LichHen SET MaBacSi=@maBS, NgayHen=@ngay, 
                                     TrangThai=@tt, GhiChu=@gc, GiaDichVu=@gia WHERE MaLichHen=@id";

                    SqlCommand cmdLH = new SqlCommand(sqlLH, conn);
                    cmdLH.Parameters.AddWithValue("@maBS", cboBacSi.SelectedValue);
                    cmdLH.Parameters.AddWithValue("@ngay", dtpNgayHen.Value.Date);
                    cmdLH.Parameters.AddWithValue("@tt", cboTrangThai.Text);
                    cmdLH.Parameters.AddWithValue("@gc", txtGhiChu.Text);

                    decimal gia = 0;
                    string giaText = "";

                    if (Controls.Find("txtGiaDichVu", true).Length > 0)
                    {
                        giaText = Controls["txtGiaDichVu"].Text;
                    }
                    else if (Controls.Find("txtGiaDichVu", true).Length > 0)
                    {
                        giaText = Controls["txtGiaDichVu"].Text;
                    }

                    decimal.TryParse(giaText.Replace(",", ""), out gia);

                    cmdLH.Parameters.AddWithValue("@gia", gia);
                    cmdLH.Parameters.AddWithValue("@id", maLH);

                    cmdLH.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thông tin thành công!");
                    HienThiDanhSachLichHen();
                }
                catch (Exception ex) 
                { 
                    MessageBox.Show("Lỗi sửa: " + ex.Message); 
                }
            }
        }
    }
}