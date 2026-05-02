using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DuAnNhom
{
    public partial class ucQuanLyCaTruc : UserControl
    {
        public string ConnectionString { get; set; }
        // Biến lưu ID để biết đang ở chế độ Thêm hay Sửa
        private int idDangChon = -1;
        // Thêm tham số string connStr vào hàm
        public ucQuanLyCaTruc(string connStr)
        {
            InitializeComponent();

            this.ConnectionString = connStr;

            SetPlaceholder(txtPhongKham, "Nhập phòng khám...");
            SetPlaceholder(txtGhiChu, "Nhập ghi chú...");
        }
        public void LoadDauTien()
        {
            DinhDangNgayThang();

            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, 1, 1);
            dtpDenNgay.Value = DateTime.Now;

            LoadComboBoxes();
            LoadDataDanhSach();
            ResetGiaoDien();
        }

        private void DinhDangNgayThang()
        {
            DateTimePicker[] dtps = { dtpNgayLam, dtpTuNgay, dtpDenNgay };
            foreach (var dtp in dtps)
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "dd/MM/yyyy";
            }
        }

        private void LoadComboBoxes()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();

                    string sqlNV = @"
                    SELECT 
                        NV.MaNhanVien,
                        NV.TenNhanVien + '-nv' + CAST(NV.MaNhanVien AS VARCHAR) AS TenHienThi
                    FROM NhanVien NV
                    JOIN VaiTro VT ON NV.MaVaiTro = VT.MaVaiTro
                    WHERE NV.TrangThai = 1 AND VT.TenVaiTro NOT LIKE N'%Quản trị%'";

                    SqlDataAdapter daNV = new SqlDataAdapter(sqlNV, conn);
                    DataTable dtNV = new DataTable();
                    daNV.Fill(dtNV);

                    cbNhanVien.DataSource = dtNV;
                    cbNhanVien.DisplayMember = "TenHienThi";
                    cbNhanVien.ValueMember = "MaNhanVien";

                    SqlDataAdapter daCa = new SqlDataAdapter("SELECT MaCaTruc, TenCaTruc FROM CaTruc", conn);
                    DataTable dtCa = new DataTable();
                    daCa.Fill(dtCa);

                    cbCaTruc.DataSource = dtCa;
                    cbCaTruc.DisplayMember = "TenCaTruc";
                    cbCaTruc.ValueMember = "MaCaTruc";

                    // Combobox lọc
                    DataTable dtLoc = dtNV.Copy();
                    DataRow r = dtLoc.NewRow();
                    r["MaNhanVien"] = 0;
                    r["TenHienThi"] = "-- Tất cả nhân viên --";
                    dtLoc.Rows.InsertAt(r, 0);

                    cbLocNhanVien.DataSource = dtLoc;
                    cbLocNhanVien.DisplayMember = "TenHienThi";
                    cbLocNhanVien.ValueMember = "MaNhanVien";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
                }
            }
        }

        public void LoadDataDanhSach()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT 
                                    L.MaLichLam,
                                    L.MaNhanVien AS [Mã NV],
                                    L.NgayLam AS [Ngày làm],
                                    NV.TenNhanVien AS [Nhân viên],
                                    C.TenCaTruc AS [Ca trực],
                                    L.PhongKham AS [Phòng khám],
                                    L.GhiChu AS [Ghi chú]
                                FROM LichLamViec L
                                JOIN NhanVien NV ON L.MaNhanVien = NV.MaNhanVien
                                JOIN CaTruc C ON L.MaCaTruc = C.MaCaTruc
                                WHERE L.NgayLam BETWEEN @Tu AND @Den";

                if (cbLocNhanVien.SelectedValue != null && (int)cbLocNhanVien.SelectedValue > 0)
                    query += " AND L.MaNhanVien = @MaNV";

                query += " ORDER BY L.NgayLam DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Tu", dtpTuNgay.Value.Date);
                cmd.Parameters.AddWithValue("@Den", dtpDenNgay.Value.Date);

                if (query.Contains("@MaNV"))
                    cmd.Parameters.AddWithValue("@MaNV", cbLocNhanVien.SelectedValue);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvDanhSach.DataSource = dt;
                FormatGrid();
            }
        }

        private void FormatGrid()
        {
            dgvDanhSach.ReadOnly = true;
            dgvDanhSach.AllowUserToAddRows = false;
            dgvDanhSach.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (dgvDanhSach.Columns["MaLichLam"] != null)
                dgvDanhSach.Columns["MaLichLam"].Visible = false;

            if (dgvDanhSach.Columns["MaNhanVien"] != null)
                dgvDanhSach.Columns["MaNhanVien"].Visible = false;

            if (dgvDanhSach.Columns["Ngày làm"] != null)
                dgvDanhSach.Columns["Ngày làm"].DefaultCellStyle.Format = "dd/MM/yyyy";

            if (dgvDanhSach.Columns["colXoa"] != null)
                dgvDanhSach.Columns["colXoa"].DisplayIndex = dgvDanhSach.Columns.Count - 1;

            dgvDanhSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvDanhSach.Rows[e.RowIndex];
            string colName = dgvDanhSach.Columns[e.ColumnIndex].Name;

            if (colName == "colXoa")
            {
                XửLyXoa(row);
                return;
            }

            idDangChon = Convert.ToInt32(row.Cells["MaLichLam"].Value);

            // 🔥 dùng ID -> không lỗi trùng tên
            cbNhanVien.SelectedValue = row.Cells["MaNhanVien"].Value;
            cbCaTruc.Text = row.Cells["Ca trực"].Value.ToString();

            dtpNgayLam.Value = Convert.ToDateTime(row.Cells["Ngày làm"].Value);
            txtPhongKham.Text = row.Cells["Phòng khám"].Value.ToString();
            txtGhiChu.Text = row.Cells["Ghi chú"].Value.ToString();

            btnSua.Enabled = true;
            btnThemLich.Enabled = false;
        }

        private void XửLyXoa(DataGridViewRow row)
        {
            DateTime ngayTruc = Convert.ToDateTime(row.Cells["Ngày làm"].Value);

            if (ngayTruc < DateTime.Now.Date)
            {
                MessageBox.Show("Không thể xóa ca trực trong quá khứ!");
                return;
            }

            if (MessageBox.Show("Xác nhận xóa?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM LichLamViec WHERE MaLichLam=@id", conn);
                    cmd.Parameters.AddWithValue("@id", row.Cells["MaLichLam"].Value);
                    cmd.ExecuteNonQuery();
                }

                LoadDataDanhSach();
                ResetGiaoDien();
            }
        }

        private void btnThemLich_Click(object sender, EventArgs e)
        {
            ThucThiSQL("INSERT INTO LichLamViec (MaNhanVien, NgayLam, MaCaTruc, PhongKham, GhiChu) VALUES (@MaNV, @Ngay, @MaCa, @Phong, @Ghi)");
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (idDangChon == -1) return;

            ThucThiSQL("UPDATE LichLamViec SET MaNhanVien=@MaNV, NgayLam=@Ngay, MaCaTruc=@MaCa, PhongKham=@Phong, GhiChu=@Ghi WHERE MaLichLam=@ID");
        }

        private bool KiemTraTrungLich(SqlConnection conn)
        {
            string sql = @"SELECT COUNT(*) FROM LichLamViec 
                           WHERE MaNhanVien=@MaNV AND NgayLam=@Ngay AND MaCaTruc=@MaCa";

            if (idDangChon != -1)
                sql += " AND MaLichLam <> @ID";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaNV", cbNhanVien.SelectedValue);
            cmd.Parameters.AddWithValue("@Ngay", dtpNgayLam.Value.Date);
            cmd.Parameters.AddWithValue("@MaCa", cbCaTruc.SelectedValue);

            if (idDangChon != -1)
                cmd.Parameters.AddWithValue("@ID", idDangChon);

            return (int)cmd.ExecuteScalar() > 0;
        }

        private void ThucThiSQL(string sql)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (cbNhanVien.SelectedValue == null || cbCaTruc.SelectedValue == null)
                    {
                        MessageBox.Show("Vui lòng chọn nhân viên và ca trực!");
                        return;
                    }

                    if (dtpNgayLam.Value.Date < DateTime.Now.Date)
                    {
                        MessageBox.Show("Không thể thêm/sửa ngày quá khứ!");
                        return;
                    }

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@MaNV", cbNhanVien.SelectedValue);
                    cmd.Parameters.AddWithValue("@Ngay", dtpNgayLam.Value.Date);
                    cmd.Parameters.AddWithValue("@MaCa", cbCaTruc.SelectedValue);
                    cmd.Parameters.AddWithValue("@Phong", txtPhongKham.Text.Trim());
                    cmd.Parameters.AddWithValue("@Ghi", txtGhiChu.Text.Trim());

                    if (sql.Contains("@ID"))
                        cmd.Parameters.AddWithValue("@ID", idDangChon);

                    conn.Open();

                    if (KiemTraTrungLich(conn))
                    {
                        MessageBox.Show("Nhân viên đã có lịch ca này rồi!");
                        return;
                    }

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Thao tác thành công!");
                    ResetGiaoDien();
                    LoadDataDanhSach();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            cbLocNhanVien.SelectedIndex = 0;
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDenNgay.Value = DateTime.Now;

            ResetGiaoDien();
            LoadDataDanhSach();
        }

        private void ResetGiaoDien()
        {
            idDangChon = -1;
            btnSua.Enabled = false;
            btnThemLich.Enabled = true;
            txtPhongKham.Clear();
            txtGhiChu.Clear();
            dtpNgayLam.Value = DateTime.Now;
        }
        private void btnLoc_Click(object sender, EventArgs e)
        {
            LoadDataDanhSach();
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
    }
}


