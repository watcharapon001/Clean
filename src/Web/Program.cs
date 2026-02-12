using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Serilog;
using Web.Middlewares;

// Serilog — bootstrap logger for startup errors
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{

var builder = WebApplication.CreateBuilder(args);

// Serilog — read config from appsettings
builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// ========================================
// Register services (Clean Architecture)
// ========================================
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Controllers
builder.Services.AddControllers();

// OpenAPI / Swagger
builder.Services.AddOpenApi();

// CORS — allow Angular dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ========================================
// Middleware Pipeline
// ========================================

// Global exception handler (must be first)
app.UseExceptionHandling();

// Serilog — structured HTTP request logging
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Seed development data
    await app.Services.SeedDevelopmentDataAsync();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularDev");

app.UseAuthorization();
app.MapControllers();

app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
