using ImageManipulation.Application.Handlers;
using ImageManipulation.Domain.Services;
using ImageManipulation.Infrastructure.Services;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddHttpLogging(_ => { });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(ImageDistortionHandler).Assembly);
builder.Services.AddTransient<IDistortionService, DistortionService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpLogging();
app.MapControllers();
app.Run();