using System.Text.Json.Serialization;
using Hackathon.Application;
using Hackathon.Infrastructure;
using Hackathon.Presentation.Extentions;
using Hackathon.Presentation.Middleware;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = null;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104_857_600;
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJwtServices(builder.Configuration);
builder.Services.ConfigureRateLimiter();

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseCors();

// Rate limiting must run early (after CORS, before auth/endpoints) to shield the app from DDoS/abuse.
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwaggerAPI();

app.MapControllers();

app.Run();
