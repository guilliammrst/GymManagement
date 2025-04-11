using System.Text.RegularExpressions;

namespace GymManagement.Shared.Core.Regexs
{
    public partial class UserRegexs
    {
        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        public static partial Regex ValidEmail();
        [GeneratedRegex(@"^\d{10}$")]
        public static partial Regex ValidPhoneNumber();
    }
}
