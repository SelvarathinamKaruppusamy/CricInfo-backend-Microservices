using Microsoft.EntityFrameworkCore;
using QuizService.Application.Interfaces.Repositories;
using QuizService.Application.Interfaces.Services;
using QuizService.Application.Services;
using QuizService.Infrastructure.Persistence;
using QuizService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<QuizDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultString")));


// Repository
builder.Services.AddScoped<
    IQuizRepository,
    QuizRepository>();

// Service
builder.Services.AddScoped<
    IQuizService,
    QuizService.Application.Services.QuizService>();

builder.Services.AddAuthorization();

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

app.UseAuthorization();

app.MapControllers();

app.Run();