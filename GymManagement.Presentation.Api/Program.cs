using GymManagement.Shared.Core.CacheManager;
using GymManagement.Application.Services;
using GymManagement.Infrastructure.Repositories;
using GymManagement.Shared.Web.Core.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure();

builder.Services.ConfigureCachingOptions(builder.Configuration);
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
