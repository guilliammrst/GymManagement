using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.KeyVaultService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var builder = DistributedApplication.CreateBuilder(args);

var options = Options.Create(new KeyVaultOptions
{
    Url = "https://kv-gym.vault.azure.net/"
});
var logger = new LoggerFactory().CreateLogger<KeyVaultService>();
var keyVaultService = new KeyVaultService(options, logger);
var mainKeys = new List<string>
{
    KeyVaultKeyNames.GymDbConnection,
    KeyVaultKeyNames.JwtSecretKey,
    KeyVaultKeyNames.JwtIssuer,
    KeyVaultKeyNames.JwtAudience
};
var keyVaultValues = keyVaultService.GetValues(mainKeys);

var apiService = builder.AddProject<Projects.GymManagement_Presentation_Api>(ProjectNames.GymApi)
    .WithEnvironment(KeyVaultKeyNames.GymDbConnection, keyVaultValues[KeyVaultKeyNames.GymDbConnection])
    .WithEnvironment(KeyVaultKeyNames.JwtSecretKey, keyVaultValues[KeyVaultKeyNames.JwtSecretKey])
    .WithEnvironment(KeyVaultKeyNames.JwtIssuer, keyVaultValues[KeyVaultKeyNames.JwtIssuer])
    .WithEnvironment(KeyVaultKeyNames.JwtAudience, keyVaultValues[KeyVaultKeyNames.JwtAudience]);

var authApiService = builder.AddProject<Projects.GymManagement_Presentation_AuthApi>(ProjectNames.AuthApi)
    .WithEnvironment(KeyVaultKeyNames.GymDbConnection, keyVaultValues[KeyVaultKeyNames.GymDbConnection])
    .WithEnvironment(KeyVaultKeyNames.JwtSecretKey, keyVaultValues[KeyVaultKeyNames.JwtSecretKey])
    .WithEnvironment(KeyVaultKeyNames.JwtIssuer, keyVaultValues[KeyVaultKeyNames.JwtIssuer])
    .WithEnvironment(KeyVaultKeyNames.JwtAudience, keyVaultValues[KeyVaultKeyNames.JwtAudience]);

builder.AddProject<Projects.GymManagement_Presentation_WebApp>(ProjectNames.WebApp)
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(authApiService)
    .WaitFor(apiService)
    .WaitFor(authApiService);

builder.Build().Run();
