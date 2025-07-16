namespace GymManagement.Application.Interfaces.Controllers.DTOs
{
    public class CreateCoachingPlanDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CoachId { get; set; }
        public int ClubId { get; set; }
    }
}
