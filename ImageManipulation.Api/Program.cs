using ImageManipulation.Application.Handlers;
using ImageManipulation.Domain.Services;
using ImageManipulation.Infrastructure.Services;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(ImageDistortionHandler).Assembly);
builder.Services.AddTransient<IImageService, ImageService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();