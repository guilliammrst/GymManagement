using GymManagement.Presentation.WebApp;
using GymManagement.Presentation.WebApp.ApiClients;
using GymManagement.Presentation.WebApp.ApiClients.Gym;
using GymManagement.Presentation.WebApp.Components;
using GymManagement.Shared.Core.AuthManager;
using GymManagement.Shared.Core.Constants;
using GymManagement.Shared.Core.Environments;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

builder.Services.AddScoped<DialogService>();

builder.Services.AddHttpClient<GymAdminApiClient>(client =>
{
    client.BaseAddress = new(EnvironmentVariables.GetEnvironmentVariable(EnvironmentVariables.GymApiUrl));
});

builder.Services.AddHttpClient<AuthApiClient>(client =>
{
    client.BaseAddress = new(EnvironmentVariables.GetEnvironmentVariable(EnvironmentVariables.AuthApiUrl));
});

builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IApiClientHelper, ApiClientHelper>();
builder.Services.AddSingleton<AuthenticatedUser>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();
app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
