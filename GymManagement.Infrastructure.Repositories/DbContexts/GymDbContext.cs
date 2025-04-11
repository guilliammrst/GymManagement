using GymManagement.Shared.Core.Configurations;
using GymManagement.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GymManagement.Infrastructure.Repositories.DbContexts
{
    public class GymDbContext(IOptions<DbSettings> _options) : BaseGymDbContext
    {
        private readonly string _connectionString = _options.Value.ConnectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_connectionString);
        }
    }
}
