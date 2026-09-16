using MatchService.Application.Interfaces;
using MatchService.Application.Interfaces.Repositories;
using MatchService.Application.Interfaces.Services;
using MatchService.Application.Services;
using MatchService.Infrastructure.Persistence;
using MatchService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MatchDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultString"));
});

// Application Services
builder.Services.AddScoped<IMatchService, MatchServiceManager>();

// Repositories
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMatchTeamRepository, MatchTeamRepository>();
builder.Services.AddScoped<IMatchPlayerRepository, MatchPlayerRepository>();
builder.Services.AddScoped<IBattingRepository, BattingRepository>();
builder.Services.AddScoped<IBowlingRepository, BowlingRepository>();

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();