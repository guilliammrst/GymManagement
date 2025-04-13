using System.ComponentModel.DataAnnotations.Schema;
using GymManagement.Shared.Core.Enums;

namespace GymManagement.Infrastructure.Persistence.Models
{
    [Table("addresses")]
    public class AddressModel : BaseModel
    {
        [Column("country")]
        public Country Country { get; set; } = Country.None;

        [Column("city")]
        public string City { get; set; } = string.Empty;

        [Column("street")]
        public string Street { get; set; } = string.Empty;

        [Column("postal_code")]
        public string PostalCode { get; set; } = string.Empty;

        [Column("house_number")]
        public string Number { get; set; } = string.Empty;
    }
}
