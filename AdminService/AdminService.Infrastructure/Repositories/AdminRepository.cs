using AdminService.Application.Interfaces.Repositories;
using AdminService.Domain.Entities;
using AdminService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Infrastructure.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly AdminDbContext _context;

    public AdminRepository(
        AdminDbContext context)
    {
        _context = context;
    }

    public async Task<Admin?> GetByUserNameAsync(
        string userName)
    {
        return await _context.Admins
            .FirstOrDefaultAsync(
                x => x.UserName == userName);
    }

    public async Task<Admin?> GetByIdAsync(
        int id)
    {
        return await _context.Admins
            .FirstOrDefaultAsync(
                x => x.Id == id);
    }

    public async Task<bool> ExistsByUserNameAsync(
        string userName)
    {
        return await _context.Admins
            .AnyAsync(
                x => x.UserName == userName);
    }

    public async Task UpdateAsync(
        Admin admin)
    {
        _context.Admins.Update(admin);

        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(
        Admin admin)
    {
        await _context.Admins.AddAsync(admin);

        await _context.SaveChangesAsync();
    }
}