namespace GymManagement.Application.Interfaces.Repositories.Users
{
    public class UserRepositoryFilter
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? Email { get; set; }
    }
}
