using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TeamService.Application.Interfaces;
using TeamService.Domain.Entities;

namespace TeamService.Application.Services
{
    public class TeamsService : ITeamService
    {
        private readonly ITeamRepository _repository;
        public TeamsService(ITeamRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<Teams>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<Teams> GetByIdAsync(int id) {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<Teams> CreateAsync(Teams teams)
        {
            return await _repository.AddAsync(teams);

        }

        public async Task<Teams> UpdateAsync(int id,Teams teams) { 
            teams.Id=id;
            return await _repository.UpdateAsync(teams);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
