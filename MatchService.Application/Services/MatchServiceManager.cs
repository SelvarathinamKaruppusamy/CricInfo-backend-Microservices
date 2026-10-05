using MatchService.Application.DTOs;
using MatchService.Application.Interfaces;
using MatchService.Application.Interfaces.Repositories;
using MatchService.Application.Interfaces.Services;
using MatchService.Application.Memory;
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
    public async Task<bool> ChangeBowlerAsync(ChangeBowlerDto dto)
    {
        var match = await _matchRepository.GetByMatchNoAsync(dto.MatchNo);

        if (match == null)
        {
            return false;
        }
        if (match.CurrentBowlingTeamIndex == null)
        {
            return false;
        }

        var bowlingTeamId = match.CurrentBowlingTeamIndex.Value;
        var bowlingTeam = await _matchTeamRepository.GetAsync(
            bowlingTeamId,
            match.MatchNo);

        if (bowlingTeam == null)
        {
            return false;
        }

        var bowler = await _matchPlayerRepository.GetAsync(
            dto.BowlerPlayerId,
            bowlingTeam.TeamId,
            match.MatchNo);

        if (bowler == null)
        {
            return false;
        }

      
        if (!string.Equals(bowler.Role, "Bowler",
                StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(bowler.Role, "All-Rounder",
                StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(bowler.Role, "All Rounder",
                StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

       
        match.CurrentBowlerPlayerId = bowler.PlayerId;
        await _unitOfWork.SaveChangesAsync();
        return true;
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
     
        if (string.IsNullOrWhiteSpace(dto.BallResult))
        {
            return false;
        }
        var match = await _matchRepository
            .GetByMatchNoAsync(dto.MatchNo);

        if (match == null)
        {
            return false;
        }
        if (!string.Equals(
            match.Status,
            "LIVE",
            StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        int innings = match.CurrentInnings.Value;

        if (match.StrikerPlayerId == null ||
            match.NonStrikerPlayerId == null ||
            match.CurrentBowlerPlayerId == null)
        {
           
            return false;
        }
 
        var teams = await _matchTeamRepository
            .GetByMatchNoAsync(dto.MatchNo);

        if (teams.Count < 2)
        {
            Console.WriteLine(
                "ERROR: Less than 2 teams found.");

            return false;
        }

      
        var battingTeam = teams.FirstOrDefault(
            x => x.TeamId == match.CurrentBattingTeamIndex);

        var bowlingTeam = teams.FirstOrDefault(
            x => x.TeamId == match.CurrentBowlingTeamIndex);

        if (battingTeam == null)
        {
            return false;
        }

        if (bowlingTeam == null)
        {
            return false;
        }

        var striker = await _matchPlayerRepository.GetAsync(
            match.StrikerPlayerId.Value,
            battingTeam.TeamId,
            match.MatchNo);

       
        var nonStriker = await _matchPlayerRepository.GetAsync(
            match.NonStrikerPlayerId.Value,
            battingTeam.TeamId,
            match.MatchNo);

        
        var bowler = await _matchPlayerRepository.GetAsync(
            match.CurrentBowlerPlayerId.Value,
            bowlingTeam.TeamId,
            match.MatchNo);

        
        if (striker == null)
        {
            
            return false;
        }

        if (nonStriker == null)
        {
           
            return false;
        }

        if (bowler == null)
        {
            return false;
        }

        
        var ballResult =
            dto.BallResult.Trim().ToUpperInvariant();

        
        switch (ballResult)
        {
            case "0":

                ProcessDotBall(
                    battingTeam,
                    striker,
                    bowler);

                HandleOverCompletion(
                    match,
                    battingTeam,
                    bowler);

                break;

            case "1":

                ProcessRuns(
                    match,
                    battingTeam,
                    striker,
                    bowler,
                    1);

                SwapStrike(match);

                HandleOverCompletion(
                    match,
                    battingTeam,
                    bowler);

                break;
            case "2":

                ProcessRuns(
                    match,
                    battingTeam,
                    striker,
                    bowler,
                    2);

                HandleOverCompletion(
                    match,
                    battingTeam,
                    bowler);

                break;

            case "3":

                ProcessRuns(
                    match,
                    battingTeam,
                    striker,
                    bowler,
                    3);

                SwapStrike(match);

                HandleOverCompletion(
                    match,
                    battingTeam,
                    bowler);

                break;

            case "4":

                ProcessRuns(
                    match,
                    battingTeam,
                    striker,
                    bowler,
                    4);

                striker.Fours++;

                HandleOverCompletion(
                    match,
                    battingTeam,
                    bowler);

                break;
            case "6":

                ProcessRuns(
                    match,
                    battingTeam,
                    striker,
                    bowler,
                    6);

                striker.Sixes++;

                HandleOverCompletion(
                    match,
                    battingTeam,
                    bowler);

                break;
            case "WD":

                ProcessWide(
                    match,
                    battingTeam,
                    bowler);

                break;

            case "NB":

                ProcessNoBall(
                    match,
                    battingTeam,
                    bowler);

                break;

            case "W":

                await ProcessWicket(
                    battingTeam,
                    striker,
                    bowler,
                    match);

                HandleOverCompletion(
                    match,
                    battingTeam,
                    bowler);

                break;

            default:

                Console.WriteLine(
                    $"ERROR: Unsupported ball result: {ballResult}");

                return false;
        }

        StoreBall(
    match.MatchNo,
    innings,
    ballResult);

        if (match.CurrentInnings == 1 &&
            IsFirstInningsCompleted(battingTeam))
        {
            var result = await StartSecondInningsAsync(match.MatchNo);

            if (!result)
                return false;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        // Second innings completed
        if (match.CurrentInnings == 2 &&
            IsMatchCompleted(match, battingTeam, bowlingTeam))
        {
            FinishMatch(
                match,
                battingTeam,
                bowlingTeam);
        }

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    private void StoreBall(
    int matchNo,
    int innings,
    string ball)
    {
        if (innings == 1)
        {
            if (!MatchBallStore.FirstInningsBalls
                .TryGetValue(matchNo, out var balls))
            {
                balls = new List<string>();

                MatchBallStore.FirstInningsBalls[matchNo] = balls;
            }

            balls.Add(ball);
        }
        else if (innings == 2)
        {
            if (!MatchBallStore.SecondInningsBalls
                .TryGetValue(matchNo, out var balls))
            {
                balls = new List<string>();

                MatchBallStore.SecondInningsBalls[matchNo] = balls;
            }

            balls.Add(ball);
        }
    }
    private bool IsFirstInningsCompleted(MatchTeam battingTeam)
    {
        return (battingTeam.Wickets ?? 0) >= 10 ||
               (battingTeam.Balls ?? 0) >= 120;
    }
    private void HandleOverCompletion(
     Match match,
     MatchTeam battingTeam,
     MatchPlayer bowler)
    {
        int balls =
            battingTeam.Balls ?? 0;

        if (balls == 0 || balls % 6 != 0)
            return;

        string key =
            $"{match.MatchNo}_{bowler.PlayerId}";

        int overRuns =
            BowlingOverStore.OverRunsConceded
                .GetValueOrDefault(key, 0);

        if (overRuns == 0)
        {
            bowler.Maidens =
                (bowler.Maidens) + 1;
        }

        BowlingOverStore.OverRunsConceded
            .TryRemove(key, out _);

        UpdateBowlerOvers(bowler);

        SwapStrike(match);
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
    Match match,
    MatchTeam battingTeam,
    MatchPlayer striker,
    MatchPlayer bowler,
    int runs)
    {
        UpdateLegalBall(
            battingTeam,
            striker,
            bowler);

        striker.Runs =
            (striker.Runs) + runs;

        battingTeam.Runs =
            (battingTeam.Runs ?? 0) + runs;

        bowler.RunsConceded =
            (bowler.RunsConceded) + runs;

        AddOverRuns(
            match,
            bowler,
            runs);

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

        UpdateBowlerOvers(bowler);
    }
    private void AddOverRuns(
    Match match,
    MatchPlayer bowler,
    int runs)
    {
        string key =
            $"{match.MatchNo}_{bowler.PlayerId}";

        BowlingOverStore.OverRunsConceded.AddOrUpdate(
            key,
            runs,
            (_, currentRuns) => currentRuns + runs);
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
     Match match,
     MatchTeam battingTeam,
     MatchPlayer bowler)
    {
        battingTeam.Runs =
            (battingTeam.Runs ?? 0) + 1;

        battingTeam.Extras =
            (battingTeam.Extras ?? 0) + 1;

        bowler.RunsConceded =
            (bowler.RunsConceded) + 1;

        AddOverRuns(
            match,
            bowler,
            1);

        UpdateScore(battingTeam);

        UpdateEconomy(bowler);
    }
    private void ProcessNoBall(
     Match match,
     MatchTeam battingTeam,
     MatchPlayer bowler)
    {
        battingTeam.Runs =
            (battingTeam.Runs ?? 0) + 1;

        battingTeam.Extras =
            (battingTeam.Extras ?? 0) + 1;

        bowler.RunsConceded =
            (bowler.RunsConceded) + 1;

        AddOverRuns(
            match,
            bowler,
            1);

        UpdateScore(battingTeam);

        UpdateEconomy(bowler);
    }
    private async Task ProcessWicket(
     MatchTeam battingTeam,
     MatchPlayer striker,
     MatchPlayer bowler,
     Match match)
    {
        battingTeam.Wickets =
            (battingTeam.Wickets ?? 0) + 1;

        striker.Status = "Out";

        bowler.Wickets =
            (bowler.Wickets) + 1;

        battingTeam.Balls =
            (battingTeam.Balls ?? 0) + 1;

        striker.Balls =
            (striker.Balls) + 1;

        bowler.BowlingBalls =
            (bowler.BowlingBalls) + 1;

        UpdateBowlerOvers(bowler);

        UpdateScore(battingTeam);

        UpdateStrikeRate(striker);

        UpdateEconomy(bowler);

        var nextBatter =
            await _matchPlayerRepository.GetNextBatterAsync(
                battingTeam.TeamId,
                match.MatchNo);

        if (nextBatter != null)
        {
            nextBatter.Status = "Batting";

            match.StrikerPlayerId =
                nextBatter.PlayerId;
        }
    }

    Task<bool> IMatchService.PromoteUpcomingMatchAsync(int matchNo)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> StartMatchAsync(int matchNo)
    {
        
        // 1. Get Match
        var match = await _matchRepository.GetByMatchNoAsync(matchNo);

        if (match == null)
        {
            return false;
        }

        // 2. Check Toss
        if (string.IsNullOrWhiteSpace(match.TossWinner) ||
            string.IsNullOrWhiteSpace(match.TossDecision))
        {
            return false;
        }

        // 3. Get Teams
        var teams = await _matchTeamRepository.GetByMatchNoAsync(matchNo);
        if (teams.Count != 2)
        {
            return false;
        }

        MatchTeam? battingTeam;
        MatchTeam? bowlingTeam;

        // 4. Determine batting / bowling team from toss
        if (match.TossDecision.Equals(
                "Bat",
                StringComparison.OrdinalIgnoreCase))
        {
            battingTeam = teams.FirstOrDefault(x =>
                string.Equals(
                    x.ShortName?.Trim(),
                    match.TossWinner.Trim(),
                    StringComparison.OrdinalIgnoreCase));

            bowlingTeam = teams.FirstOrDefault(x =>
                !string.Equals(
                    x.ShortName?.Trim(),
                    match.TossWinner.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }
        else if (match.TossDecision.Equals(
                     "Bowl",
                     StringComparison.OrdinalIgnoreCase))
        {
            bowlingTeam = teams.FirstOrDefault(x =>
                string.Equals(
                    x.ShortName?.Trim(),
                    match.TossWinner.Trim(),
                    StringComparison.OrdinalIgnoreCase));

            battingTeam = teams.FirstOrDefault(x =>
                !string.Equals(
                    x.ShortName?.Trim(),
                    match.TossWinner.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            return false;
        }

        // 5. Validate teams
        if (battingTeam == null)
        {
            return false;
        }

        if (bowlingTeam == null)
        {
            
            return false;
        }
        // 6. Get batting players
        var battingPlayers =
            await _matchPlayerRepository.GetByTeamAsync(
                battingTeam.TeamId,
                matchNo);

      

        // 7. Get bowling players
        var bowlingPlayers =
            await _matchPlayerRepository.GetByTeamAsync(
                bowlingTeam.TeamId,
                matchNo);

        // 8. Validate players
        if (battingPlayers.Count < 2)
        {
            
            return false;
        }

        if (bowlingPlayers.Count < 1)
        {
           
            return false;
        }

        // 9. Opening batsmen
        var striker = battingPlayers[0];
        var nonStriker = battingPlayers[1];

        // 10. Opening bowler
        var bowler = bowlingPlayers.FirstOrDefault(x =>
            string.Equals(
                x.Role?.Trim(),
                "Bowler",
                StringComparison.OrdinalIgnoreCase)
            ||
            string.Equals(
                x.Role?.Trim(),
                "All Rounder",
                StringComparison.OrdinalIgnoreCase)
            ||
            string.Equals(
                x.Role?.Trim(),
                "All-Rounder",
                StringComparison.OrdinalIgnoreCase));

        if (bowler == null)
        {
            
            return false;
        }

        
        // 11. Update match
        match.CurrentBattingTeamIndex = battingTeam.TeamId;
        match.CurrentBowlingTeamIndex = bowlingTeam.TeamId;

        match.StrikerPlayerId = striker.PlayerId;
        match.NonStrikerPlayerId = nonStriker.PlayerId;
        match.CurrentBowlerPlayerId = bowler.PlayerId;

        match.CurrentInnings = 1;
        match.Status = "LIVE";

        // 12. Update player status
        striker.Status = "Batting";
        nonStriker.Status = "Batting";
        bowler.Status = "Bowling";

        // 13. Save
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    public async Task<bool> StartSecondInningsAsync(int matchNo)
    {
        var match = await _matchRepository.GetByMatchNoAsync(matchNo);

        if (match == null)
            return false;

        if (match.CurrentInnings == 2)
            return true;

        if (match.CurrentBattingTeamIndex == 0 ||
            match.CurrentBowlingTeamIndex == 0)
            return false;

        var firstInningsBattingTeam =
            await _matchTeamRepository.GetAsync(
                match.CurrentBattingTeamIndex.Value,
                match.MatchNo);

        var firstInningsBowlingTeam =
            await _matchTeamRepository.GetAsync(
                match.CurrentBowlingTeamIndex.Value,
                match.MatchNo);

        if (firstInningsBattingTeam == null ||
            firstInningsBowlingTeam == null)
            return false;

        // Swap batting and bowling teams
        var temp = match.CurrentBattingTeamIndex;

        match.CurrentBattingTeamIndex =
            match.CurrentBowlingTeamIndex;

        match.CurrentBowlingTeamIndex = temp;

        // Start second innings
        match.CurrentInnings = 2;

        var battingTeam =
            await _matchTeamRepository.GetAsync(
                match.CurrentBattingTeamIndex.Value,
                match.MatchNo);

        var bowlingTeam =
            await _matchTeamRepository.GetAsync(
                match.CurrentBowlingTeamIndex.Value,
                match.MatchNo);

        if (battingTeam == null || bowlingTeam == null)
            return false;

        // Get opening batsmen
        var battingPlayers =
            await _matchPlayerRepository.GetByTeamAsync(
                battingTeam.TeamId,
                match.MatchNo);

        if (battingPlayers.Count < 2)
            return false;

        var striker = battingPlayers[0];
        var nonStriker = battingPlayers[1];

        striker.Status = "Batting";
        nonStriker.Status = "Batting";

        match.StrikerPlayerId = striker.PlayerId;
        match.NonStrikerPlayerId = nonStriker.PlayerId;

        // Get opening bowler
        var bowlingPlayers =
            await _matchPlayerRepository.GetByTeamAsync(
                bowlingTeam.TeamId,
                match.MatchNo);

        var openingBowler = bowlingPlayers.FirstOrDefault(
            x => x.Role == "Bowler" ||
                 x.Role == "All Rounder" ||
                 x.Role == "All-Rounder");

        if (openingBowler == null)
            return false;

        match.CurrentBowlerPlayerId =
            openingBowler.PlayerId;

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    private bool IsMatchCompleted(
    Match match,
    MatchTeam battingTeam,
    MatchTeam bowlingTeam)
    {
        if (match.CurrentInnings != 2)
            return false;

        int target = (bowlingTeam.Runs ?? 0) + 1;

        return (battingTeam.Runs ?? 0) >= target ||
               (battingTeam.Wickets ?? 0) >= 10 ||
               (battingTeam.Balls ?? 0) >= 120;
    }
    private void FinishMatch(
    Match match,
    MatchTeam battingTeam,
    MatchTeam bowlingTeam)
    {
        battingTeam.MatchStatus ??= string.Empty;
        bowlingTeam.MatchStatus ??= string.Empty;

        if ((battingTeam.Runs ?? 0) > (bowlingTeam.Runs ?? 0))
        {
            int wicketsLeft =
                10 - (battingTeam.Wickets ?? 0);

            match.Result =
                $"{battingTeam.ShortName} won by {wicketsLeft} wickets";

            battingTeam.MatchStatus = "true";
            bowlingTeam.MatchStatus = "false";
        }
        else if ((battingTeam.Runs ?? 0) <
                 (bowlingTeam.Runs ?? 0))
        {
            int runsMargin =
                (bowlingTeam.Runs ?? 0) -
                (battingTeam.Runs ?? 0);

            match.Result =
                $"{bowlingTeam.ShortName} won by {runsMargin} runs";

            battingTeam.MatchStatus = "false";
            bowlingTeam.MatchStatus = "true";
        }
        else
        {
            match.Result = "Match Tied";

            battingTeam.MatchStatus = "false";
            bowlingTeam.MatchStatus = "false";
        }

        match.Status = "COMPLETED";
    }
    private void UpdateBowlerOvers(MatchPlayer bowler)
    {
        int bowlingBalls = bowler.BowlingBalls;

        int completedOvers = bowlingBalls / 6;
        int remainingBalls = bowlingBalls % 6;

        bowler.Overs =
            completedOvers + (remainingBalls / 10m);
    }

    Task<bool> IMatchService.UpdateMatchAsync(int matchNo, MatchUpdateDto dto)
    {
        throw new NotImplementedException();
    }

    Task<bool> IMatchService.UpdatePlayerOfTheMatchAsync(int matchNo, PlayerOfTheMatchDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateTossAsync(TossDto dto)
    {
        var match = await _matchRepository.GetByMatchNoAsync(dto.MatchNo);

        if (match == null)
        {
            Console.WriteLine($"ERROR: Match {dto.MatchNo} not found.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(dto.TossWinner))
        {
            Console.WriteLine("ERROR: Toss winner is empty.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(dto.TossDecision))
        {
            Console.WriteLine("ERROR: Toss decision is empty.");
            return false;
        }

        match.TossWinner = dto.TossWinner;
        match.TossDecision = dto.TossDecision;

        await _unitOfWork.SaveChangesAsync();

        Console.WriteLine(
            $"Toss updated. Winner: {dto.TossWinner}, Decision: {dto.TossDecision}");

        return true;
    }
    public async Task<bool> CompleteMatchAsync(CompletedMatchDto dto)
    {
        var match = await _matchRepository
            .GetByMatchNoAsync(dto.MatchNo);

        if (match == null)
            return false;

        match.PlayerOfTheMatch = dto.PlayerOfTheMatch;
        match.Status = "COMPLETED";

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    
}