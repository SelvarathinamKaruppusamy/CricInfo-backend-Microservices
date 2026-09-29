using Microsoft.EntityFrameworkCore;
using TeamService.Application.Interfaces;
using TeamService.Application.Services;
using TeamService.Infrastructure.Persistence;
using TeamService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddDbContext<TeamDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultString")));

builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamService, TeamsService>();

var app = builder.Build();


app.UseHttpsRedirection();


app.MapControllers();

app.Run();