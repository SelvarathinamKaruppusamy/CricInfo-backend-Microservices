using Microsoft.EntityFrameworkCore;
using QuizService.Domain.Entities;

namespace QuizService.Infrastructure.Persistence;

public class QuizDbContext : DbContext
{
    public QuizDbContext(
        DbContextOptions<QuizDbContext> options)
        : base(options)
    {
    }

    public DbSet<QuizQuestion>
        QuizQuestions
    { get; set; }

    public DbSet<QuizResult>
        QuizResults
    { get; set; }
}