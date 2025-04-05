using BCrypt.Net;

namespace WebsiteBanXeMay___Coding.Helpers
{
    public static class PasswordHelper
    {
        // Hàm băm mật khẩu trước khi lưu vào DB
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Hàm kiểm tra mật khẩu nhập vào có khớp với mật khẩu đã mã hóa không
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
