using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using GymManagement.Shared.Core.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GymManagement.Shared.Core.KeyVaultService
{
    public class KeyVaultService(IOptions<KeyVaultOptions> keyVaultOptions, ILogger<KeyVaultService> _logger)
    {
        private readonly string _keyVaultUrl = keyVaultOptions.Value.Url;

        public string GetValue(string key)
        {
            try
            {
                var secretClient = new SecretClient(new Uri(_keyVaultUrl), new DefaultAzureCredential(true));
                KeyVaultSecret secret = secretClient.GetSecret(key);
                var secretValue = secret.Value;
                _logger.LogInformation($"Secret successfully retrieved from KeyVault: {key}");

                return secretValue;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting value from KeyVault: {ex.Message}");
            }
        }

        public Dictionary<string, string> GetValues(List<string> keys)
        {
            var secretClient = new SecretClient(new Uri(_keyVaultUrl), new DefaultAzureCredential(true));
            var secrets = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                try
                {
                    KeyVaultSecret secret = secretClient.GetSecret(key);
                    secrets.Add(key, secret.Value);
                    _logger.LogInformation($"Secret successfully retrieved from KeyVault: {key}");
                }
                catch (Exception ex)
                {
                    secrets.Add(key, string.Empty);
                    _logger.LogError($"Error while getting value from KeyVault: {ex.Message}");
                }
            }
            return secrets;
        }
    }
}
