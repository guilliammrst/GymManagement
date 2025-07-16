using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagement.Infrastructure.Persistence.Models
{
    [Table("coaching-plans")]
    public class CoachingPlanModel : BaseModel
    {
        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("price")]
        public decimal Price { get; set; }

        [Column("is_valid")]
        public bool IsValid { get; set; } = false;

        [Column("coach_id")]
        public int CoachId { get; set; }
        public UserModel? Coach { get; set; }

        [Column("club_id")]
        public int ClubId { get; set; }
        public ClubModel? Club { get; set; }

        public ICollection<CoachingModel> Coachings { get; set; } = [];
    }
}
