using System;
using System.Collections.Generic;
using System.Text;
using TeamService.Domain.Entities;


namespace TeamService.Application.Interfaces
{
    public interface ITeamRepository
        {
            Task<List<Teams>> GetAllAsync();
            Task<Teams?> GetByIdAsync(int id);
            Task<Teams> AddAsync(Teams team);
            Task<Teams> UpdateAsync(Teams team);
            Task<bool> DeleteAsync(int id);
        }
    }

