using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Thêm 2 dòng quan trọng này
using ReaLTaiizor.Forms;
using ReaLTaiizor.Manager;
namespace DuAnNhom
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e){
        }
        private void AddUserControl(UserControl uc)
        {
            uc.Dock = DockStyle.Fill;
            pnlMain.Controls.Clear(); // Xóa nội dung cũ trong vùng xám
            pnlMain.Controls.Add(uc); // Đổ nội dung mới vào
            uc.BringToFront();
        }

        // Đây là sự kiện khi bấm nút nhân sự
        private void btnQuanLyNhanSu_Click(object sender, EventArgs e)
        {
            ucQuanLyNhanSu ucNhanSu = new ucQuanLyNhanSu();
            AddUserControl(ucNhanSu);
        }
    }
}
