using GoldenRaspberryAwards.Api.Application.Handlers;
using GoldenRaspberryAwards.Api.Data;
using GoldenRaspberryAwards.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("GoldenRaspberryDb"));

builder.Services.AddScoped<IntervalService>();
builder.Services.AddScoped<CsvLoader>();
builder.Services.AddScoped<GetProducersIntervalsHandler>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var loader = scope.ServiceProvider.GetRequiredService<CsvLoader>();
loader.Load(Path.Combine(app.Environment.ContentRootPath, "Resources", "movielist.csv"));

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

public partial class Program { }
