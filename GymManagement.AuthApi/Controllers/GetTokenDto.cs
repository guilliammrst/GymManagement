namespace GymManagement.AuthApi.Controllers
{
    public class GetTokenDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
