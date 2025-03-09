using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using GymManagement.Core.Configurations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace GymManagement.Core.KeyVaultService
{
    public class KeyVaultService (IOptions<KeyVaultOptions> keyVaultOptions, IOptions<CachingOptions> cachingOptions, IMemoryCache _cache) : IKeyVaultService
    {
        private readonly KeyVaultOptions _keyVaultOptions = keyVaultOptions.Value;
        private readonly CachingOptions _cachingOptions = cachingOptions.Value;

        public string GetValue(string key)
        {
            if (_cache.TryGetValue(key, out string? value) && !string.IsNullOrWhiteSpace(value))
                return value;

            try
            {
                var secretClient = new SecretClient(new Uri(_keyVaultOptions.Url), new DefaultAzureCredential(true));
                KeyVaultSecret secret = secretClient.GetSecret(key);
                var secretValue = secret.Value;
                
                if (!string.IsNullOrWhiteSpace(secretValue))
                    _cache.Set(key, secretValue, TimeSpan.FromSeconds(_cachingOptions.CacheDurationInSeconds));

                return secretValue;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting value from KeyVault: {ex.Message}");
            }
        }
    }
}
