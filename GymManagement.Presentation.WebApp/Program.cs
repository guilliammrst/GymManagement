using GymManagement.Presentation.WebApp;
using GymManagement.Presentation.WebApp.ApiClients;
using GymManagement.Presentation.WebApp.ApiClients.Auth;
using GymManagement.Presentation.WebApp.ApiClients.Gym;
using GymManagement.Presentation.WebApp.Components;
using GymManagement.Shared.Core.Constants;
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
    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
    // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
    client.BaseAddress = new("https+http://" + ProjectNames.GymApi);
});

builder.Services.AddHttpClient<AuthApiClient>(client =>
{
    // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
    // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
    client.BaseAddress = new("https+http://" + ProjectNames.AuthApi);
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
