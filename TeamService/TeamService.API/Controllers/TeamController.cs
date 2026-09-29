using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.Pkcs;
using TeamService.Application.Interfaces;
using TeamService.Domain.Entities;

namespace TeamService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService) {
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Teams>>> GetAll() {
            var teams = await _teamService.GetAllAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Teams>> GetById(int id)
        {
            var teams = await _teamService.GetByIdAsync(id);
            if(teams == null)
            {
                return NotFound();
            }
            return Ok(teams);
        }

        [HttpPost]
        public async Task<ActionResult<Teams>> AddAsync(Teams teams) { 
        var team = await _teamService.CreateAsync(teams);
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Teams>> UpdateTeamAsync(int id, Teams teams)
        {
            var team = await _teamService.UpdateAsync(id, teams);
            return Ok(team);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Teams>> DeleteAsync(int id)
        {
            var team=await _teamService.DeleteAsync(id);

            if (team == null)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}
