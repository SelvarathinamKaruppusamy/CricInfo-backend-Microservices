using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TeamService.Domain.Entities;
using TeamService.Infrastructure.Persistence;
using TeamService.Application.Interfaces;



namespace TeamService.Infrastructure.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly TeamDbContext _Context;

        public TeamRepository(TeamDbContext context)
        {
            _Context = context;
        }

        public async Task<List<Teams>> GetAllAsync()
        {
            return await _Context.Teams.AsNoTracking().ToListAsync();
        }

        public async Task<Teams?> GetByIdAsync(int id)
        {
            return await _Context.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
        }


        public async Task<Teams> AddAsync(Teams teams) {
            await _Context.Teams.AddAsync(teams);
            await _Context.SaveChangesAsync();  

            return teams;
        }

        public async Task<bool> DeleteAsync(int id) { 
           var team = await _Context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
            {
                return false;
             
            }
            _Context.Teams.Remove(team);
            await _Context.SaveChangesAsync();
            return true;


        }

        public async Task<Teams> UpdateAsync(Teams teams) { 
        
        var existingTeam= await _Context.Teams.FirstOrDefaultAsync(t=>t.Id== teams.Id);
            if (existingTeam == null) {
                return null;
            }

            _Context.Entry(existingTeam).CurrentValues.SetValues(teams);

            await _Context.SaveChangesAsync();

            return existingTeam;
        
        }

      
    }
}
