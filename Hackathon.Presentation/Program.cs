using System.Text.Json.Serialization;
using Hackathon.Application;
using Hackathon.Infrastructure;
using Hackathon.Presentation.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();

var app = builder.Build();

app.UseSwaggerAPI();

app.MapControllers();

app.Run();
