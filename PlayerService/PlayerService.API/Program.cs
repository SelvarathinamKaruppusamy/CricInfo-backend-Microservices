using Microsoft.EntityFrameworkCore;
using PlayersService.Application.Interfaces;
using PlayersService.Application.Interfaces.Repositories;
using PlayersService.Application.Services;
using PlayersService.Infrastructure.Persistence;
using PlayersService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// OpenAPI
builder.Services.AddOpenApi();

// Database
builder.Services.AddDbContext<PlayersDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IBattingRepository, BattingRepository>();
builder.Services.AddScoped<IBowlingRepository, BowlingRepository>();
builder.Services.AddScoped<ICompletedPlayerService, CompletedPlayerService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");

app.MapControllers();

app.Run();