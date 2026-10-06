using PlayersService.Application.Interfaces;
using PlayersService.Application.Interfaces.Repositories;
using PlayersService.DTOs;

namespace PlayersService.Application.Services;

public class CompletedPlayerService : ICompletedPlayerService
{
    private readonly IBattingRepository _battingRepository;
    private readonly IBowlingRepository _bowlingRepository;

    public CompletedPlayerService(
        IBattingRepository battingRepository,
        IBowlingRepository bowlingRepository)
    {
        _battingRepository = battingRepository;
        _bowlingRepository = bowlingRepository;
    }

    public async Task<List<CompletedPlayerDto>> GetCompletedPlayersAsync(int matchNo)
    {
        // Get all batting records for this match
        var battingRecords =
            await _battingRepository.GetByMatchNoAsync(matchNo);

        // Get all bowling records for this match
        var bowlingRecords =
            await _bowlingRepository.GetByMatchNoAsync(matchNo);

        var players = new Dictionary<(int playerId, int teamId, int matchNo), CompletedPlayerDto>();

        // Add batting information
        foreach (var batting in battingRecords)
        {
            var key = (batting.playerId, batting.TeamId, batting.matchNo);

            if (!players.TryGetValue(key, out var player))
            {
                player = new CompletedPlayerDto
                {
                    playerId = batting.playerId,
                    TeamId = batting.TeamId,
                    matchNo = batting.matchNo,
                    name = batting.name,
                    role = batting.role
                };

                players[key] = player;
            }

            player.Batting = new BattingDto
            {
                playerId = batting.playerId,
                TeamId = batting.TeamId,
                matchNo = batting.matchNo,
                name = batting.name,
                role = batting.role,
                runs = batting.runs,
                balls = batting.balls,
                fours = batting.fours,
                sixes = batting.sixes,
                strikeRate = batting.strikeRate,
                status = batting.status
            };
        }

        // Add bowling information
        foreach (var bowling in bowlingRecords)
        {
            var key = (bowling.playerId, bowling.TeamId, bowling.matchNo);

            if (!players.TryGetValue(key, out var player))
            {
                player = new CompletedPlayerDto
                {
                    playerId = bowling.playerId,
                    TeamId = bowling.TeamId,
                    matchNo = bowling.matchNo,
                    name = bowling.name,
                    role = bowling.role
                };

                players[key] = player;
            }

            player.Bowling = new BowlingDto
            {
                playerId = bowling.playerId,
                TeamId = bowling.TeamId,
                matchNo = bowling.matchNo,
                name = bowling.name,
                role = bowling.role,
                overs = bowling.overs,
                balls = bowling.balls,
                maidens = bowling.maidens,
                runsConceded = bowling.runsConceded,
                wickets = bowling.wickets,
                economy = bowling.economy
            };
        }

        return players.Values
            .OrderBy(x => x.TeamId)
            .ThenBy(x => x.playerId)
            .ToList();
    }
}