using BlogService.Application.Interfaces.Services;
using BlogService.Infrastructure;
using AutoMapper;
using BlogService.Application.Mappings;

using BlogApplicationService =
    BlogService.Application.Services.BlogService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddOpenApi();


builder.Services.AddInfrastructure(
    builder.Configuration);


builder.Services.AddScoped<
    IBlogService,
    BlogApplicationService>();


builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<BlogProfile>();
});


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