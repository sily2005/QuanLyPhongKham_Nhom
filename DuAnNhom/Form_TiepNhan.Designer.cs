namespace DuAnNhom
{
    partial class Form_TiepNhan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtSDT = new System.Windows.Forms.TextBox();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.dgvDanhSachCho = new System.Windows.Forms.DataGridView();
            this.lblHoTen = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.lblNgaySinh = new System.Windows.Forms.Label();
            this.dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
            this.lblGioiTinh = new System.Windows.Forms.Label();
            this.cboGioiTinh = new System.Windows.Forms.ComboBox();
            this.lblTieuSu = new System.Windows.Forms.Label();
            this.txtTieuSu = new System.Windows.Forms.TextBox();
            this.lblBacSi = new System.Windows.Forms.Label();
            this.cboBacSi = new System.Windows.Forms.ComboBox();
            this.lblCaTruc = new System.Windows.Forms.Label();
            this.cboCaTruc = new System.Windows.Forms.ComboBox();
            this.lblNgayHen = new System.Windows.Forms.Label();
            this.dtpNgayHen = new System.Windows.Forms.DateTimePicker();
            this.lblPhong = new System.Windows.Forms.Label();
            this.txtPhong = new System.Windows.Forms.TextBox();
            this.btnDangKy = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.colMaPhieuKham = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenBenhNhan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoDienThoai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBacSi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayHen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachCho)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Số điện thoại:";
            // 
            // txtSDT
            // 
            this.txtSDT.Location = new System.Drawing.Point(117, 20);
            this.txtSDT.Name = "txtSDT";
            this.txtSDT.Size = new System.Drawing.Size(100, 22);
            this.txtSDT.TabIndex = 1;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BackColor = System.Drawing.Color.LightBlue;
            this.btnTimKiem.Location = new System.Drawing.Point(232, 20);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(75, 23);
            this.btnTimKiem.TabIndex = 2;
            this.btnTimKiem.Text = "Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // dgvDanhSachCho
            // 
            this.dgvDanhSachCho.AllowUserToAddRows = false;
            this.dgvDanhSachCho.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDanhSachCho.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDanhSachCho.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMaPhieuKham,
            this.colTenBenhNhan,
            this.colSoDienThoai,
            this.colBacSi,
            this.colNgayHen});
            this.dgvDanhSachCho.Location = new System.Drawing.Point(54, 66);
            this.dgvDanhSachCho.MultiSelect = false;
            this.dgvDanhSachCho.Name = "dgvDanhSachCho";
            this.dgvDanhSachCho.ReadOnly = true;
            this.dgvDanhSachCho.RowHeadersWidth = 51;
            this.dgvDanhSachCho.RowTemplate.Height = 24;
            this.dgvDanhSachCho.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDanhSachCho.Size = new System.Drawing.Size(240, 150);
            this.dgvDanhSachCho.TabIndex = 3;
            // 
            // lblHoTen
            // 
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Location = new System.Drawing.Point(371, 23);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Size = new System.Drawing.Size(49, 16);
            this.lblHoTen.TabIndex = 4;
            this.lblHoTen.Text = "Họ tên:";
            // 
            // txtHoTen
            // 
            this.txtHoTen.Location = new System.Drawing.Point(453, 23);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.Size = new System.Drawing.Size(100, 22);
            this.txtHoTen.TabIndex = 5;
            // 
            // lblDiaChi
            // 
            this.lblDiaChi.AutoSize = true;
            this.lblDiaChi.Location = new System.Drawing.Point(368, 55);
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.Size = new System.Drawing.Size(50, 16);
            this.lblDiaChi.TabIndex = 6;
            this.lblDiaChi.Text = "Địa chỉ:";
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.Location = new System.Drawing.Point(453, 55);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(100, 22);
            this.txtDiaChi.TabIndex = 7;
            // 
            // lblNgaySinh
            // 
            this.lblNgaySinh.AutoSize = true;
            this.lblNgaySinh.Location = new System.Drawing.Point(371, 86);
            this.lblNgaySinh.Name = "lblNgaySinh";
            this.lblNgaySinh.Size = new System.Drawing.Size(70, 16);
            this.lblNgaySinh.TabIndex = 8;
            this.lblNgaySinh.Text = "Ngày sinh:";
            // 
            // dtpNgaySinh
            // 
            this.dtpNgaySinh.CustomFormat = "Short";
            this.dtpNgaySinh.Location = new System.Drawing.Point(453, 86);
            this.dtpNgaySinh.Name = "dtpNgaySinh";
            this.dtpNgaySinh.Size = new System.Drawing.Size(200, 22);
            this.dtpNgaySinh.TabIndex = 9;
            // 
            // lblGioiTinh
            // 
            this.lblGioiTinh.AutoSize = true;
            this.lblGioiTinh.Location = new System.Drawing.Point(371, 120);
            this.lblGioiTinh.Name = "lblGioiTinh";
            this.lblGioiTinh.Size = new System.Drawing.Size(57, 16);
            this.lblGioiTinh.TabIndex = 10;
            this.lblGioiTinh.Text = "Giới tính:";
            // 
            // cboGioiTinh
            // 
            this.cboGioiTinh.FormattingEnabled = true;
            this.cboGioiTinh.Items.AddRange(new object[] {
            "Nam",
            "Nữ"});
            this.cboGioiTinh.Location = new System.Drawing.Point(453, 120);
            this.cboGioiTinh.Name = "cboGioiTinh";
            this.cboGioiTinh.Size = new System.Drawing.Size(121, 24);
            this.cboGioiTinh.TabIndex = 11;
            // 
            // lblTieuSu
            // 
            this.lblTieuSu.AutoSize = true;
            this.lblTieuSu.Location = new System.Drawing.Point(371, 152);
            this.lblTieuSu.Name = "lblTieuSu";
            this.lblTieuSu.Size = new System.Drawing.Size(87, 16);
            this.lblTieuSu.TabIndex = 12;
            this.lblTieuSu.Text = "Tiền sử bệnh:";
            // 
            // txtTieuSu
            // 
            this.txtTieuSu.Location = new System.Drawing.Point(453, 150);
            this.txtTieuSu.Multiline = true;
            this.txtTieuSu.Name = "txtTieuSu";
            this.txtTieuSu.Size = new System.Drawing.Size(171, 38);
            this.txtTieuSu.TabIndex = 13;
            // 
            // lblBacSi
            // 
            this.lblBacSi.AutoSize = true;
            this.lblBacSi.Location = new System.Drawing.Point(371, 218);
            this.lblBacSi.Name = "lblBacSi";
            this.lblBacSi.Size = new System.Drawing.Size(49, 16);
            this.lblBacSi.TabIndex = 14;
            this.lblBacSi.Text = "Bác sĩ:";
            // 
            // cboBacSi
            // 
            this.cboBacSi.FormattingEnabled = true;
            this.cboBacSi.Location = new System.Drawing.Point(453, 209);
            this.cboBacSi.Name = "cboBacSi";
            this.cboBacSi.Size = new System.Drawing.Size(121, 24);
            this.cboBacSi.TabIndex = 15;
            // 
            // lblCaTruc
            // 
            this.lblCaTruc.AutoSize = true;
            this.lblCaTruc.Location = new System.Drawing.Point(374, 252);
            this.lblCaTruc.Name = "lblCaTruc";
            this.lblCaTruc.Size = new System.Drawing.Size(51, 16);
            this.lblCaTruc.TabIndex = 16;
            this.lblCaTruc.Text = "Ca trực:";
            // 
            // cboCaTruc
            // 
            this.cboCaTruc.FormattingEnabled = true;
            this.cboCaTruc.Location = new System.Drawing.Point(453, 243);
            this.cboCaTruc.Name = "cboCaTruc";
            this.cboCaTruc.Size = new System.Drawing.Size(121, 24);
            this.cboCaTruc.TabIndex = 17;
            // 
            // lblNgayHen
            // 
            this.lblNgayHen.AutoSize = true;
            this.lblNgayHen.Location = new System.Drawing.Point(374, 300);
            this.lblNgayHen.Name = "lblNgayHen";
            this.lblNgayHen.Size = new System.Drawing.Size(79, 16);
            this.lblNgayHen.TabIndex = 18;
            this.lblNgayHen.Text = "Ngày khám:";
            // 
            // dtpNgayHen
            // 
            this.dtpNgayHen.Location = new System.Drawing.Point(453, 293);
            this.dtpNgayHen.Name = "dtpNgayHen";
            this.dtpNgayHen.Size = new System.Drawing.Size(200, 22);
            this.dtpNgayHen.TabIndex = 19;
            // 
            // lblPhong
            // 
            this.lblPhong.AutoSize = true;
            this.lblPhong.Location = new System.Drawing.Point(371, 338);
            this.lblPhong.Name = "lblPhong";
            this.lblPhong.Size = new System.Drawing.Size(85, 16);
            this.lblPhong.TabIndex = 20;
            this.lblPhong.Text = "Phòng khám:";
            // 
            // txtPhong
            // 
            this.txtPhong.Location = new System.Drawing.Point(453, 331);
            this.txtPhong.Name = "txtPhong";
            this.txtPhong.Size = new System.Drawing.Size(100, 22);
            this.txtPhong.TabIndex = 21;
            // 
            // btnDangKy
            // 
            this.btnDangKy.BackColor = System.Drawing.Color.LightGreen;
            this.btnDangKy.Location = new System.Drawing.Point(371, 377);
            this.btnDangKy.Name = "btnDangKy";
            this.btnDangKy.Size = new System.Drawing.Size(75, 23);
            this.btnDangKy.TabIndex = 22;
            this.btnDangKy.Text = "Đăng ký khám";
            this.btnDangKy.UseVisualStyleBackColor = false;
            this.btnDangKy.Click += new System.EventHandler(this.btnDangKy_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(467, 376);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(75, 23);
            this.btnLamMoi.TabIndex = 23;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(578, 377);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(75, 23);
            this.btnThoat.TabIndex = 24;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // colMaPhieuKham
            // 
            this.colMaPhieuKham.HeaderText = "Mã phiếu";
            this.colMaPhieuKham.MinimumWidth = 6;
            this.colMaPhieuKham.Name = "colMaPhieuKham";
            this.colMaPhieuKham.ReadOnly = true;
            this.colMaPhieuKham.Visible = false;
            // 
            // colTenBenhNhan
            // 
            this.colTenBenhNhan.HeaderText = "Bệnh nhân";
            this.colTenBenhNhan.MinimumWidth = 6;
            this.colTenBenhNhan.Name = "colTenBenhNhan";
            this.colTenBenhNhan.ReadOnly = true;
            this.colTenBenhNhan.Visible = false;
            // 
            // colSoDienThoai
            // 
            this.colSoDienThoai.HeaderText = "SĐT";
            this.colSoDienThoai.MinimumWidth = 6;
            this.colSoDienThoai.Name = "colSoDienThoai";
            this.colSoDienThoai.ReadOnly = true;
            this.colSoDienThoai.Visible = false;
            // 
            // colBacSi
            // 
            this.colBacSi.HeaderText = "Bác sĩ";
            this.colBacSi.MinimumWidth = 6;
            this.colBacSi.Name = "colBacSi";
            this.colBacSi.ReadOnly = true;
            this.colBacSi.Visible = false;
            // 
            // colNgayHen
            // 
            this.colNgayHen.HeaderText = "Ngày hẹn";
            this.colNgayHen.MinimumWidth = 6;
            this.colNgayHen.Name = "colNgayHen";
            this.colNgayHen.ReadOnly = true;
            this.colNgayHen.Visible = false;
            // 
            // Form_TiepNhan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.btnDangKy);
            this.Controls.Add(this.txtPhong);
            this.Controls.Add(this.lblPhong);
            this.Controls.Add(this.dtpNgayHen);
            this.Controls.Add(this.lblNgayHen);
            this.Controls.Add(this.cboCaTruc);
            this.Controls.Add(this.lblCaTruc);
            this.Controls.Add(this.cboBacSi);
            this.Controls.Add(this.lblBacSi);
            this.Controls.Add(this.txtTieuSu);
            this.Controls.Add(this.lblTieuSu);
            this.Controls.Add(this.cboGioiTinh);
            this.Controls.Add(this.lblGioiTinh);
            this.Controls.Add(this.dtpNgaySinh);
            this.Controls.Add(this.lblNgaySinh);
            this.Controls.Add(this.txtDiaChi);
            this.Controls.Add(this.lblDiaChi);
            this.Controls.Add(this.txtHoTen);
            this.Controls.Add(this.lblHoTen);
            this.Controls.Add(this.dgvDanhSachCho);
            this.Controls.Add(this.btnTimKiem);
            this.Controls.Add(this.txtSDT);
            this.Controls.Add(this.label1);
            this.Name = "Form_TiepNhan";
            this.Text = "Form1_TiepNhan";
            this.Load += new System.EventHandler(this.Form_TiepNhan_Load);
            this.Click += new System.EventHandler(this.Form_TiepNhan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDanhSachCho)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.DataGridView dgvDanhSachCho;
        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblNgaySinh;
        private System.Windows.Forms.DateTimePicker dtpNgaySinh;
        private System.Windows.Forms.Label lblGioiTinh;
        private System.Windows.Forms.ComboBox cboGioiTinh;
        private System.Windows.Forms.Label lblTieuSu;
        private System.Windows.Forms.TextBox txtTieuSu;
        private System.Windows.Forms.Label lblBacSi;
        private System.Windows.Forms.ComboBox cboBacSi;
        private System.Windows.Forms.Label lblCaTruc;
        private System.Windows.Forms.ComboBox cboCaTruc;
        private System.Windows.Forms.Label lblNgayHen;
        private System.Windows.Forms.DateTimePicker dtpNgayHen;
        private System.Windows.Forms.Label lblPhong;
        private System.Windows.Forms.TextBox txtPhong;
        private System.Windows.Forms.Button btnDangKy;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaPhieuKham;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenBenhNhan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoDienThoai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBacSi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayHen;
    }
}