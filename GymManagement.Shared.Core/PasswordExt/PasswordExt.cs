namespace GymManagement.Shared.Core.PasswordExt
{
    public static class PasswordExt
    {
        public const string SPECIAL_CHARS = "!@#$%^&*()-_=+[]{};:,.<>?";

        public static string Hash(this string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool Verify(this string? password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }

        public static bool IsValidPassword(this string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSpecial = false;
            bool hasNoSpace = !password.Contains(' ');

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (SPECIAL_CHARS.Contains(c)) hasSpecial = true;
            }

            return hasUpper && hasLower && hasDigit && hasSpecial && hasNoSpace;
        }
    }
}
