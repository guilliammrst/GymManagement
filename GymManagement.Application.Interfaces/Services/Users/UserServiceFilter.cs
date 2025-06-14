namespace GymManagement.Application.Interfaces.Services.Users
{
    public class UserServiceFilter
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? Email { get; set; }
    }
}
