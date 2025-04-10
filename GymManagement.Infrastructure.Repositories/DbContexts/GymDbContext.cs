using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.KeyVaultService;
using GymManagement.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GymManagement.Infrastructure.Repositories.DbContexts
{
    public class GymDbContext(IKeyVaultService _keyVaultService, IOptions<KeyVaultOptions> options) : BaseGymDbContext
    {
        private readonly KeyVaultOptions _keyVaultSettings = options.Value;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_keyVaultService.GetValue(_keyVaultSettings.GymDb));
        }
    }
}
