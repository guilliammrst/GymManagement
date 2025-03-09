namespace GymManagement.Core.PasswordExt
{
    public static class PasswordExt
    {
        public static string Hash(this string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool Verify(this string? password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
