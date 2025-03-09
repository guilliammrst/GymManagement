using GymManagement.Core.Configurations;
using GymManagement.Core.KeyVaultService;
using GymManagement.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GymManagement.Repositories.DbContexts
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
