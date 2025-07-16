namespace GymManagement.Application.Interfaces.Services.CoachingPlans
{
    public class CoachingPlanCreateDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CoachId { get; set; }
        public int ClubId { get; set; }
    }
}
