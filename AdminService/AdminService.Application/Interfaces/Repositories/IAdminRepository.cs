using AdminService.Domain.Entities;

namespace AdminService.Application.Interfaces.Repositories;

public interface IAdminRepository
{
    Task<Admin?> GetByUserNameAsync(
        string userName);

    Task<Admin?> GetByIdAsync(
        int id);

    Task<bool> ExistsByUserNameAsync(
        string userName);

    Task UpdateAsync(
        Admin admin);

    Task AddAsync(
        Admin admin);
}