using AdminService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Infrastructure.Persistence;

public class AdminDbContext : DbContext
{
    public AdminDbContext(
        DbContextOptions<AdminDbContext> options)
        : base(options)
    {
    }

    public DbSet<Admin> Admins { get; set; }
}