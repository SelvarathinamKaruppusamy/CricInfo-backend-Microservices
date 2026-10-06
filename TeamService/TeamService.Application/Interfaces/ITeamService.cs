using System;
using System.Collections.Generic;
using System.Text;
using TeamService.Domain.Entities;

namespace TeamService.Application.Interfaces
{
    public interface ITeamService
    {
        Task<List<Teams>> GetAllAsync();
        Task<Teams?> GetByIdAsync(int id);
        Task<Teams?> CreateAsync(Teams team);
        Task<Teams?> UpdateAsync(int id, Teams team);
        Task<bool> DeleteAsync(int id);
    }
}