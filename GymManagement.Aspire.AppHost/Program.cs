var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.GymManagement_Presentation_Api>("gym-api");
var authApiService = builder.AddProject<Projects.GymManagement_Presentation_AuthApi>("auth-api");

builder.AddProject<Projects.GymManagement_Presentation_WebApp>("web-app")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WithReference(authApiService)
    .WaitFor(apiService)
    .WaitFor(authApiService);

builder.Build().Run();
