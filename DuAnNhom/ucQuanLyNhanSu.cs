using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;

namespace DuAnNhom
{
    public partial class ucQuanLyNhanSu : UserControl
    {
        public string ConnectionString { get; set; }
        private string duongDanAnhGoc = "";
        //C:\Users\Admin\source\repos\DuAnNhom\DuAnNhom\bin\Debug
        private readonly string thuMucAnh = Path.Combine(Application.StartupPath, "Images");

        public ucQuanLyNhanSu(string connStr)
        {
            InitializeComponent();
            this.ConnectionString = connStr;
            SetupGiaoDien();
            if (!Directory.Exists(thuMucAnh)) Directory.CreateDirectory(thuMucAnh);
        }
        private void SetupGiaoDien()
        {
            SetPlaceholder(txtHoTen, "Nhập họ và tên...");
            SetPlaceholder(txtTenDangNhap, "Nhập tên đăng nhập...");
            SetPlaceholder(txtMatKhau, "Nhập mật khẩu...");
            SetPlaceholder(txtSoDienThoai, "Nhập số điện thoại...");
            SetPlaceholder(txtTimKiem, "Nhập tên hoặc mã nhân viên...");
        }
        private void SetPlaceholder(Control txt, string noiDung)
        {
            txt.Text = noiDung;
            txt.ForeColor = Color.Gray;

            txt.Enter += (s, e) => {
                if (txt.Text == noiDung)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };

            txt.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = noiDung;
                    txt.ForeColor = Color.Gray;
                }
            };
        }
        // =========================
        // HÀM SQL DÙNG CHUNG
        // =========================
        private object ExecSql(string sql, SqlParameter[] pars = null, bool isScalar = false, bool isQuery = true)
        {
            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                if (pars != null) cmd.Parameters.AddRange(pars); //add nhiều par new[]{new SqlParameter("@k", "%An%"),new SqlParameter("@tt", 1)};
                con.Open();
                if (isScalar) return cmd.ExecuteScalar(); //COUNT (1 giá trị)
                if (!isQuery) return cmd.ExecuteNonQuery(); //INSERT/UPDATE/DELETE (số dòng ảnh hưởng)
                var dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
        }
        public void LoadDauTien()
        {
            LoadCombobox();
            LoadData("");
        }
        private void LoadCombobox()
        {
            cboVaiTro.DataSource = ExecSql("SELECT MaVaiTro, TenVaiTro FROM VaiTro WHERE TenVaiTro <> N'Admin'");
            cboVaiTro.DisplayMember = "TenVaiTro";
            cboVaiTro.ValueMember = "MaVaiTro";

            cboChuyenKhoa.DataSource = ExecSql("SELECT * FROM ChuyenKhoa");
            cboChuyenKhoa.DisplayMember = "TenChuyenKhoa";
            cboChuyenKhoa.ValueMember = "MaChuyenKhoa";

            cboTrangThai.Items.Clear();
            cboTrangThai.Items.AddRange(new[] { "Đã nghỉ", "Đang làm việc" });
            cboTrangThai.SelectedIndex = 1;
        }
        private void LoadData(string key = "")
        {
            string sql = @"SELECT nv.*, vt.TenVaiTro, ck.TenChuyenKhoa, 
                         CASE WHEN nv.TrangThai = 1 THEN N'Đang làm việc' ELSE N'Đã nghỉ' END AS TinhTrang 
                         FROM NhanVien nv INNER JOIN VaiTro vt ON nv.MaVaiTro = vt.MaVaiTro      
                         LEFT JOIN ChuyenKhoa ck ON nv.MaChuyenKhoa = ck.MaChuyenKhoa
                         WHERE vt.TenVaiTro <> N'Admin'";// LEFT JOIN: lấy tất cả dữ liệu cả khi chuyên khoa = null, INNER JOIN: chỉ lấy khi có chuyên khoa

            SqlParameter[] pars = string.IsNullOrEmpty(key) ? null : new[] { new SqlParameter("@k", $"%{key}%") };
            if (pars != null) sql += "  AND (nv.TenNhanVien LIKE @k OR CAST(nv.MaNhanVien AS NVARCHAR) LIKE @k)";

            var dt = (DataTable)ExecSql(sql, pars);
            dt.Columns.Add("AnhHienThi", typeof(Image));

            foreach (DataRow r in dt.Rows)
            {
                string path = Path.Combine(thuMucAnh, r["HinhAnh"]?.ToString() ?? "");
                r["AnhHienThi"] = File.Exists(path) ? Image.FromStream(new MemoryStream(File.ReadAllBytes(path))) : Properties.Resources.avatar_macdinh;
                // ĐỌC ẢNH TỪ FILE dữ liệu thô BYTE RỒI đưa dữ liệu đó vào RAM rồi chuyển dữ liệu → thành ảnh (Image)
            }
            dgvNhanVien.DataSource = dt;
            DinhDangLuoi();
        }
        private void DinhDangLuoi()
        {
            string[] hien = { "AnhHienThi", "MaNhanVien", "TenNhanVien", "TenVaiTro", "TenChuyenKhoa", "TenDangNhap", "SoDienThoai", "TinhTrang" };
            foreach (DataGridViewColumn col in dgvNhanVien.Columns)
                col.Visible = Array.IndexOf(hien, col.Name) >= 0; // -1
            dgvNhanVien.Columns["MaNhanVien"].HeaderText = "MaNV";
            dgvNhanVien.Columns["TenNhanVien"].HeaderText = "Họ tên";
            dgvNhanVien.Columns["TenVaiTro"].HeaderText = "Vai trò";
            dgvNhanVien.Columns["TenChuyenKhoa"].HeaderText = "Chuyên khoa";
            dgvNhanVien.Columns["TenDangNhap"].HeaderText = "Tài khoản";
            dgvNhanVien.Columns["SoDienThoai"].HeaderText = "SĐT";
            dgvNhanVien.Columns["TinhTrang"].HeaderText = "Trạng thái";
            dgvNhanVien.Columns["AnhHienThi"].HeaderText = "Ảnh";

            ((DataGridViewImageColumn)dgvNhanVien.Columns["AnhHienThi"])
                .ImageLayout = DataGridViewImageCellLayout.Zoom;
        }
        // ==========================================
        // HÀM KIỂM TRA ĐIỀU KIỆN NHẬP LIỆU (VALIDATION)
        // ==========================================
        private bool KiemTraNhapLieu()
        {
            // 1. Kiểm tra Họ tên
            if (txtHoTen.Text.Trim().Length < 5)
            {
                MessageBox.Show("Họ tên quá ngắn, vui lòng nhập đầy đủ!"); return false;
            }
            // 2. Kiểm tra Số điện thoại
            string sdt = txtSoDienThoai.Text.Trim();
            if (!Regex.IsMatch(sdt, @"^0\d{9}$")) // Kiểm tra xem chuỗi có đúng theo mẫu không
            {
                MessageBox.Show("Số điện thoại phải có đúng 10 chữ số và bắt đầu bằng số 0!"); return false;
            }
            if (KiemTraTrungLap("SoDienThoai", sdt, txtMaNhanVien.Text))
            {
                MessageBox.Show("Số điện thoại này đã tồn tại!"); return false;
            }
            // 3. Kiểm tra Tên đăng nhập
            string user = txtTenDangNhap.Text.Trim();
            if (user.Length < 4 || Regex.IsMatch(user, @"[^a-zA-Z0-9]"))
            {
                MessageBox.Show("Tên đăng nhập ít nhất 4 ký tự và không chứa ký tự đặc biệt/có dấu!"); return false;
            }
            if (KiemTraTrungLap("TenDangNhap", user, txtMaNhanVien.Text))
            {
                MessageBox.Show("Tên đăng nhập này đã tồn tại!"); return false;
            }
            // 4. Kiểm tra Mật khẩu
            if (txtMatKhau.Text.Trim().Length < 6)
            {
                MessageBox.Show("Mật khẩu phải từ 6 ký tự trở lên để đảm bảo an toàn!"); return false;
            }
            // 5. Kiểm tra Chuyên khoa cho Bác sĩ
            if (cboVaiTro.Text == "Bác sĩ" && (cboChuyenKhoa.SelectedValue == null || string.IsNullOrWhiteSpace(cboChuyenKhoa.Text)))
            {
                MessageBox.Show("Bác sĩ bắt buộc phải có Chuyên khoa!"); return false;
            }

            return true;
        }
        private bool KiemTraTrungLap(string cot, string giaTri, string maNV)
        {
            string sql = $"SELECT COUNT(*) FROM NhanVien WHERE {cot} = @val"; //@val = biến truyền vào SQL, int.TryParse thử chuyển đổi chuỗi thành số, out _ (out x) là biến gán giá trị x = mnv nhưng _ nên không gán để lấy gtri
            // Nếu đang sửa thì phải loại trừ chính bản thân mình ra khỏi việc kiểm tra trùng lặp
            if (!string.IsNullOrEmpty(maNV) && int.TryParse(maNV, out _))
                sql += " AND MaNhanVien <> @ma";

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@val", giaTri);
                if (sql.Contains("@ma")) cmd.Parameters.AddWithValue("@ma", maNV);
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow r = dgvNhanVien.Rows[e.RowIndex];

            txtMaNhanVien.Text = r.Cells["MaNhanVien"].Value?.ToString();
            txtHoTen.Text = r.Cells["TenNhanVien"].Value?.ToString();
            txtHoTen.ForeColor = Color.Black;

            cboVaiTro.Text = r.Cells["TenVaiTro"].Value?.ToString();
            cboChuyenKhoa.Text = r.Cells["TenChuyenKhoa"].Value?.ToString();

            txtTenDangNhap.Text = r.Cells["TenDangNhap"].Value?.ToString();
            txtTenDangNhap.ForeColor = Color.Black;

            txtMatKhau.Text = r.Cells["MatKhau"].Value?.ToString();
            txtMatKhau.ForeColor = Color.Black;

            txtSoDienThoai.Text = r.Cells["SoDienThoai"].Value?.ToString();
            txtSoDienThoai.ForeColor = Color.Black;

            cboTrangThai.Text = r.Cells["TinhTrang"].Value?.ToString();

            string tenAnh = r.Cells["HinhAnh"].Value?.ToString();
            duongDanAnhGoc = ""; // Reset lại những gì bạn đã chọn trước đó

            if (!string.IsNullOrEmpty(tenAnh) && File.Exists(Path.Combine(thuMucAnh, tenAnh)))
            {
                using (var stream = new FileStream(Path.Combine(thuMucAnh, tenAnh), FileMode.Open, FileAccess.Read))
                {
                    picAvatar.Image = Image.FromStream(stream);
                }
            }
            else picAvatar.Image = Properties.Resources.avatar_macdinh;
        }
        private void ThucThiTruyVan(string hanhDong)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string sql = "";
                string tenAnhMoi = string.IsNullOrEmpty(duongDanAnhGoc) ? null : XuLyLuuAnh();

                switch (hanhDong)
                {
                    case "THEM":
                        sql = "INSERT INTO NhanVien (TenNhanVien, MaVaiTro, MaChuyenKhoa, TenDangNhap, MatKhau, SoDienThoai, TrangThai, HinhAnh) VALUES (@ten, @vt, @ck, @user, @pass, @sdt, @tt, @anh)";
                        break;
                    case "SUA":
                        sql = "UPDATE NhanVien SET TenNhanVien=@ten, MaVaiTro=@vt, MaChuyenKhoa=@ck, TenDangNhap=@user, MatKhau=@pass, SoDienThoai=@sdt, TrangThai=@tt "
                            + (tenAnhMoi != null ? ", HinhAnh=@anh " : "") + " WHERE MaNhanVien=@ma";
                        break;
                }

                SqlCommand cmd = new SqlCommand(sql, con);

                // Gán tham số dùng chung cho cả THÊM và SỬA
                cmd.Parameters.AddWithValue("@ten", txtHoTen.Text.Trim());
                cmd.Parameters.AddWithValue("@vt", cboVaiTro.SelectedValue);
                cmd.Parameters.AddWithValue("@ck", cboVaiTro.Text == "Bác sĩ" ? cboChuyenKhoa.SelectedValue : DBNull.Value);
                cmd.Parameters.AddWithValue("@user", txtTenDangNhap.Text.Trim());
                cmd.Parameters.AddWithValue("@pass", txtMatKhau.Text.Trim());
                cmd.Parameters.AddWithValue("@sdt", txtSoDienThoai.Text.Trim());
                cmd.Parameters.AddWithValue("@tt", cboTrangThai.SelectedIndex);

                if (tenAnhMoi != null || hanhDong == "THEM")
                    cmd.Parameters.AddWithValue("@anh", (object)tenAnhMoi ?? DBNull.Value);
                // Mã nhân viên chỉ cần thiết khi SỬA
                if (hanhDong == "SUA")
                    cmd.Parameters.AddWithValue("@ma", txtMaNhanVien.Text);

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Lưu dữ liệu thành công!");
                    if (hanhDong == "THEM") btnLamMoi_Click(null, null);
                    else LoadData("");
                }
                catch (SqlException ex)
                {
                    // ex.Number == 2627 là lỗi trùng dữ liệu 
                    string msg = ex.Number == 2627 ? "Tên đăng nhập hoặc Số điện thoại bị trùng!" : "Lỗi SQL: " + ex.Message;
                    MessageBox.Show(msg, "Lỗi hệ thống");
                }
            }
        }
        private string XuLyLuuAnh()
        {
            if (string.IsNullOrEmpty(duongDanAnhGoc) || !File.Exists(duongDanAnhGoc)) return null;
            string tenFileMoi = Guid.NewGuid().ToString() + Path.GetExtension(duongDanAnhGoc);//Guid.NewGuid() tạo ra một chuỗi duy nhất, Path.GetExtension lấy đuôi file gốc (.jpg, .png...)
            File.Copy(duongDanAnhGoc, Path.Combine(thuMucAnh, tenFileMoi), true);                                                     //true: nếu đã tồn tại file cùng tên thì ghi đè lên
            return tenFileMoi;
        }
        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp" })
            {
                //người dùng có bấm "OK / Open"
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    duongDanAnhGoc = ofd.FileName;
                    using (var stream = new FileStream(duongDanAnhGoc, FileMode.Open, FileAccess.Read))
                    {
                        picAvatar.Image = Image.FromStream(stream);
                    }
                    picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }
        private void btnThem_Click(object sender, EventArgs e) 
        {
            if (!string.IsNullOrEmpty(txtMaNhanVien.Text))
            {
                MessageBox.Show("Vui lòng làm mới trước khi thêm!");
                return;
            }
            if (KiemTraNhapLieu()) ThucThiTruyVan("THEM"); 
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNhanVien.Text) || txtMaNhanVien.Text == "Mã nhân viên")
            {
                MessageBox.Show("Vui lòng chọn nhân viên!"); return;
            }
            if (KiemTraNhapLieu()) ThucThiTruyVan("SUA");
        }
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaNhanVien.Text = "";
            duongDanAnhGoc = "";
            picAvatar.Image = Properties.Resources.avatar_macdinh;
            SetupGiaoDien();
            LoadData("");
        }
        private void txtSoDienThoai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; //iscontrol kiểm tra có phải phím điều khiển tab,enter không
        }
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();
            LoadData(string.IsNullOrWhiteSpace(tuKhoa) ? "" : tuKhoa);
        }

    }
}
