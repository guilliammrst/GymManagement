using GymManagement.Api.Configurations;
using GymManagement.Core.CacheManager;
using GymManagement.Core.KeyVaultService;
using GymManagement.Services.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.RegisterServices();
builder.Services.RegisterKeyVaultService(builder.Configuration);
builder.Services.RegisterCachingOptions(builder.Configuration);
builder.Services.ConfigureSecurity(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
