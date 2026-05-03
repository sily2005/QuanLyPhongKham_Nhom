using System;

namespace DuAnNhom
{
    public static class Session
    {
        public static int MaNhanVien { get; set; }
        public static int MaVaiTro { get; set; }
        public static string TenNhanVien { get; set; }

        // thêm trạng thái đăng nhập (rất nên có)
        public static bool IsLoggedIn => MaNhanVien > 0;

        public static void Clear()
        {
            MaNhanVien = 0;
            MaVaiTro = 0;
            TenNhanVien = null;
        }
    }
}