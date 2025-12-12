using CodeOptimizer.API.Extensions;
using OpenTelemetry.Logs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerAuth();
builder.Services.AddOpenTelemetryService();
builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());
builder.Services.AddDependencies();
builder.Services.AddJwtAuthenticationScheme(builder.Configuration);
builder.Services.AddCorsPolicy();
builder.Services.AddAutoMapper();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseServiceDefaults();
app.MapControllers();
app.Run();
