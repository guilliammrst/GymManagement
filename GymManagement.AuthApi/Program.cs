using GymManagement.Core.CacheManager;
using GymManagement.Core.KeyVaultService;
using GymManagement.Services.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.RegisterKeyVaultService(builder.Configuration);
builder.Services.RegisterAuthService(builder.Configuration);
builder.Services.RegisterCachingOptions(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMemoryCache();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
