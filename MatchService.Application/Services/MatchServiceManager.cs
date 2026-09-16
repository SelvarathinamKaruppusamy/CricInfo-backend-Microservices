using MatchService.Application.DTOs;
using MatchService.Application.Interfaces;
using MatchService.Application.Interfaces.Repositories;
using MatchService.Application.Interfaces.Services;
using MatchService.Domain.Entities;
namespace MatchService.Application.Services;

public class MatchServiceManager : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMatchTeamRepository _matchTeamRepository;
    private readonly IMatchPlayerRepository _matchPlayerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MatchServiceManager(
     IMatchRepository matchRepository,
     IMatchTeamRepository matchTeamRepository,
     IMatchPlayerRepository matchPlayerRepository,
     IUnitOfWork unitOfWork)
    {
        _matchRepository = matchRepository;
        _matchTeamRepository = matchTeamRepository;
        _matchPlayerRepository = matchPlayerRepository;
        _unitOfWork = unitOfWork;
    }
    Task<bool> IMatchService.ChangeBowlerAsync(ChangeBowlerDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<MatchDto?> GetLiveMatchAsync()
    {
        var match = await _matchRepository.GetCurrentLiveMatchAsync();

        if (match == null)
            return null;

        var teams = await _matchTeamRepository
            .GetByMatchNoAsync(match.MatchNo);

        var matchDto = new MatchDto
        {
            MatchNo = match.MatchNo,
            Venue = match.Venue,
            City = match.City,
            Date = match.Date,
            TossWinner = match.TossWinner,
            TossDecision = match.TossDecision,
            Result = match.Result,
            PlayerOfTheMatch = match.PlayerOfTheMatch,
            Status = match.Status,
            CurrentInnings = match.CurrentInnings,
            CurrentBattingTeamIndex = match.CurrentBattingTeamIndex,
            CurrentBowlingTeamIndex = match.CurrentBowlingTeamIndex,
            StrikerPlayerId = match.StrikerPlayerId,
            NonStrikerPlayerId = match.NonStrikerPlayerId,
            CurrentBowlerPlayerId = match.CurrentBowlerPlayerId
        };

        foreach (var team in teams)
        {
            var players = await _matchPlayerRepository
                .GetByTeamAsync(team.TeamId, match.MatchNo);

            var teamDto = new MatchTeamDto
            {
                MatchNo = team.MatchNo,
                TeamId = team.TeamId,
                FullName = team.FullName,
                ShortName = team.ShortName,
                Logo = team.Logo,
                Scores = team.Scores,
                Runs = team.Runs,
                Wickets = team.Wickets,
                Extras = team.Extras,
                Overs = team.Overs,
                Balls = team.Balls,
                MatchStatus = team.MatchStatus
            };

            foreach (var player in players)
            {
                teamDto.Players.Add(new MatchPlayerDto
                {
                    MatchNo = player.MatchNo,
                    TeamId = player.TeamId,
                    PlayerId = player.PlayerId,
                    Name = player.Name,
                    Role = player.Role,
                    Runs = player.Runs,
                    Balls = player.Balls,
                    Fours = player.Fours,
                    Sixes = player.Sixes,
                    StrikeRate = player.StrikeRate,
                    Status = player.Status,
                    Overs = player.Overs,
                    BowlingBalls = player.BowlingBalls,
                    Wickets = player.Wickets,
                    Maidens = player.Maidens,
                    RunsConceded = player.RunsConceded,
                    Economy = player.Economy
                });
            }

            matchDto.Teams.Add(teamDto);
        }

        return matchDto;
    }

    Task<List<MatchDto>> IMatchService.GetUpcomingMatchesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ProcessBallAsync(BallUpdateDto dto)
    {
        // 1. Validate request
        if (string.IsNullOrWhiteSpace(dto.BallResult))
        {
            Console.WriteLine("ERROR: BallResult is empty.");
            return false;
        }

        // 2. Get match
        var match = await _matchRepository
            .GetByMatchNoAsync(dto.MatchNo);

        if (match == null)
        {
            Console.WriteLine(
                $"ERROR: Match {dto.MatchNo} not found.");

            return false;
        }

        Console.WriteLine(
            $"Match found: {match.MatchNo}, Status: {match.Status}");

        // 3. Make sure match is live
        if (!string.Equals(
            match.Status,
            "LIVE",
            StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine(
                $"ERROR: Match {match.MatchNo} is not LIVE.");

            return false;
        }

        // 4. Validate current players
        if (match.StrikerPlayerId == null ||
            match.NonStrikerPlayerId == null ||
            match.CurrentBowlerPlayerId == null)
        {
            Console.WriteLine(
                "ERROR: Striker, NonStriker or Bowler ID is NULL.");

            return false;
        }

        Console.WriteLine(
            $"Batting Team ID: {match.CurrentBattingTeamIndex}");

        Console.WriteLine(
            $"Bowling Team ID: {match.CurrentBowlingTeamIndex}");

        Console.WriteLine(
            $"Striker: {match.StrikerPlayerId}");

        Console.WriteLine(
            $"NonStriker: {match.NonStrikerPlayerId}");

        Console.WriteLine(
            $"Bowler: {match.CurrentBowlerPlayerId}");

        // 5. Get teams
        var teams = await _matchTeamRepository
            .GetByMatchNoAsync(dto.MatchNo);

        Console.WriteLine(
            $"Teams found: {teams.Count}");

        if (teams.Count < 2)
        {
            Console.WriteLine(
                "ERROR: Less than 2 teams found.");

            return false;
        }

        // IMPORTANT:
        // CurrentBattingTeamIndex actually contains TeamId
        var battingTeam = teams.FirstOrDefault(
            x => x.TeamId == match.CurrentBattingTeamIndex);

        // IMPORTANT:
        // CurrentBowlingTeamIndex actually contains TeamId
        var bowlingTeam = teams.FirstOrDefault(
            x => x.TeamId == match.CurrentBowlingTeamIndex);

        if (battingTeam == null)
        {
            Console.WriteLine(
                $"ERROR: Batting team " +
                $"{match.CurrentBattingTeamIndex} not found.");

            return false;
        }

        if (bowlingTeam == null)
        {
            Console.WriteLine(
                $"ERROR: Bowling team " +
                $"{match.CurrentBowlingTeamIndex} not found.");

            return false;
        }

        Console.WriteLine(
            $"Batting Team: {battingTeam.TeamId}");

        Console.WriteLine(
            $"Bowling Team: {bowlingTeam.TeamId}");

        // 6. Get striker
        var striker = await _matchPlayerRepository.GetAsync(
            match.StrikerPlayerId.Value,
            battingTeam.TeamId,
            match.MatchNo);

        // 7. Get non-striker
        var nonStriker = await _matchPlayerRepository.GetAsync(
            match.NonStrikerPlayerId.Value,
            battingTeam.TeamId,
            match.MatchNo);

        // 8. Get bowler
        var bowler = await _matchPlayerRepository.GetAsync(
            match.CurrentBowlerPlayerId.Value,
            bowlingTeam.TeamId,
            match.MatchNo);

        // 9. Validate players
        if (striker == null)
        {
            Console.WriteLine(
                $"ERROR: Striker " +
                $"{match.StrikerPlayerId} not found.");

            return false;
        }

        if (nonStriker == null)
        {
            Console.WriteLine(
                $"ERROR: Non-striker " +
                $"{match.NonStrikerPlayerId} not found.");

            return false;
        }

        if (bowler == null)
        {
            Console.WriteLine(
                $"ERROR: Bowler " +
                $"{match.CurrentBowlerPlayerId} not found.");

            return false;
        }

        // 10. Normalize ball result
        var ballResult =
            dto.BallResult.Trim().ToUpperInvariant();

        Console.WriteLine(
            $"Processing ball: {ballResult}");

        // 11. Process ball
        switch (ballResult)
        {
            case "0":

                ProcessDotBall(
                    battingTeam,
                    striker,
                    bowler);

                break;

            case "1":

                ProcessRuns(
                    battingTeam,
                    striker,
                    bowler,
                    1);

                SwapStrike(match);

                break;

            case "2":

                ProcessRuns(
                    battingTeam,
                    striker,
                    bowler,
                    2);

                break;

            case "3":

                ProcessRuns(
                    battingTeam,
                    striker,
                    bowler,
                    3);

                SwapStrike(match);

                break;

            case "4":

                ProcessRuns(
                    battingTeam,
                    striker,
                    bowler,
                    4);

                striker.Fours++;

                break;

            case "6":

                ProcessRuns(
                    battingTeam,
                    striker,
                    bowler,
                    6);

                striker.Sixes++;

                break;

            case "WD":

                ProcessWide(
                    battingTeam,
                    bowler);

                break;

            case "NB":

                ProcessNoBall(
                    battingTeam,
                    bowler);

                break;

            case "W":

                await ProcessWicket(
                    battingTeam,
                    striker,
                    bowler,
                    match);

                break;

            default:

                Console.WriteLine(
                    $"ERROR: Unsupported ball result: {ballResult}");

                return false;
        }

        // 12. Save everything
        await _unitOfWork.SaveChangesAsync();

        Console.WriteLine(
            $"Ball {ballResult} processed successfully.");

        return true;
    }
    private void ProcessDotBall(
    MatchTeam battingTeam,
    MatchPlayer striker,
    MatchPlayer bowler)
    {
        UpdateLegalBall(
            battingTeam,
            striker,
            bowler);

        UpdateStrikeRate(striker);
        UpdateEconomy(bowler);

        UpdateScore(battingTeam);
    }
    private void ProcessRuns(
    MatchTeam battingTeam,
    MatchPlayer striker,
    MatchPlayer bowler,
    int runs)
    {
        UpdateLegalBall(
            battingTeam,
            striker,
            bowler);

        striker.Runs = (striker.Runs) + runs;

        battingTeam.Runs =
            (battingTeam.Runs ?? 0) + runs;

        bowler.RunsConceded =
            (bowler.RunsConceded) + runs;

        UpdateScore(battingTeam);

        UpdateStrikeRate(striker);

        UpdateEconomy(bowler);
    }
    private void UpdateLegalBall(
        MatchTeam battingTeam,
        MatchPlayer striker,
        MatchPlayer bowler)
    {
        battingTeam.Balls =
            (battingTeam.Balls ?? 0) + 1;

        striker.Balls =
            (striker.Balls) + 1;

        bowler.BowlingBalls =
            (bowler.BowlingBalls) + 1;
    }
    private void UpdateScore(MatchTeam team)
    {
        var runs = team.Runs ?? 0;
        var wickets = team.Wickets ?? 0;

        team.Scores = $"{runs}/{wickets}";

        var balls = team.Balls ?? 0;

        var completedOvers = balls / 6;
        var remainingBalls = balls % 6;

        team.Overs =
            decimal.Parse(
                $"{completedOvers}.{remainingBalls}");
    }
    private void UpdateStrikeRate(MatchPlayer player)
    {
        var runs = player.Runs;
        var balls = player.Balls;

        if (balls == 0)
        {
            player.StrikeRate = 0;
            return;
        }

        player.StrikeRate =
            Math.Round(
                (decimal)runs / balls * 100,
                2);
    }
    private void UpdateEconomy(MatchPlayer player)
    {
        var runsConceded =
            player.RunsConceded;

        var bowlingBalls =
            player.BowlingBalls;

        if (bowlingBalls == 0)
        {
            player.Economy = 0;
            return;
        }

        player.Economy =
            Math.Round(
                (decimal)runsConceded /
                bowlingBalls * 6,
                2);
    }
    private void SwapStrike(Match match)
    {
        var temp = match.StrikerPlayerId;

        match.StrikerPlayerId =
            match.NonStrikerPlayerId;

        match.NonStrikerPlayerId =
            temp;
    }
    private void ProcessWide(
    MatchTeam battingTeam,
    MatchPlayer bowler)
    {
        battingTeam.Runs =
            (battingTeam.Runs ?? 0) + 1;

        battingTeam.Extras =
            (battingTeam.Extras ?? 0) + 1;

        bowler.RunsConceded =
            (bowler.RunsConceded) + 1;

        UpdateScore(battingTeam);
        UpdateEconomy(bowler);
    }
    private void ProcessNoBall(
    MatchTeam battingTeam,
    MatchPlayer bowler)
    {
        battingTeam.Runs =
            (battingTeam.Runs ?? 0) + 1;

        battingTeam.Extras =
            (battingTeam.Extras ?? 0) + 1;

        bowler.RunsConceded =
            (bowler.RunsConceded) + 1;

        UpdateScore(battingTeam);
        UpdateEconomy(bowler);
    }
    private async Task ProcessWicket(
    MatchTeam battingTeam,
    MatchPlayer striker,
    MatchPlayer bowler,
    Match match)
    {
        // Increase team wickets
        battingTeam.Wickets =
            (battingTeam.Wickets ?? 0) + 1;

        // Mark striker as out
        striker.Status = "Out";

        // Increase bowler wickets
        bowler.Wickets =
            (bowler.Wickets) + 1;

        // One legal ball
        battingTeam.Balls =
            (battingTeam.Balls) + 1;

        striker.Balls =
            (striker.Balls) + 1;

        bowler.BowlingBalls =
            (bowler.BowlingBalls) + 1;

        // Update score
        UpdateScore(battingTeam);

        // Update player statistics
        UpdateStrikeRate(striker);
        UpdateEconomy(bowler);

        // Find next batter
        var nextBatter = await _matchPlayerRepository
            .GetNextBatterAsync(
                battingTeam.TeamId,
                match.MatchNo);

        if (nextBatter != null)
        {
            nextBatter.Status = "Batting";

            match.StrikerPlayerId = nextBatter.PlayerId;
        }
    }

    Task<bool> IMatchService.PromoteUpcomingMatchAsync(int matchNo)
    {
        throw new NotImplementedException();
    }

    Task<bool> IMatchService.StartMatchAsync(int matchNo)
    {
        throw new NotImplementedException();
    }

    Task<bool> IMatchService.StartSecondInningsAsync(int matchNo)
    {
        throw new NotImplementedException();
    }

    Task<bool> IMatchService.UpdateMatchAsync(int matchNo, MatchUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    Task<bool> IMatchService.UpdatePlayerOfTheMatchAsync(int matchNo, PlayerOfTheMatchDto dto)
    {
        throw new NotImplementedException();
    }

    Task<bool> IMatchService.UpdateTossAsync(TossDto dto)
    {
        throw new NotImplementedException();
    }
}