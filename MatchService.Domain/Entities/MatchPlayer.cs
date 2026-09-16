

namespace MatchService.Domain.Entities
{
    public class MatchPlayer
    {
        public int MatchNo { get; set; }

        public int TeamId { get; set; }

        public int PlayerId { get; set; }

        // Snapshot of Player master data
        public string Name { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        // Batting statistics
        public int Runs { get; set; }

        public int Balls { get; set; }

        public int Fours { get; set; }

        public int Sixes { get; set; }

        public decimal StrikeRate { get; set; }

        public string Status { get; set; } = string.Empty;

        // Bowling statistics
        public decimal Overs { get; set; }

        public int BowlingBalls { get; set; }

        public int Wickets { get; set; }

        public int Maidens { get; set; }

        public int RunsConceded { get; set; }

        public decimal Economy { get; set; }
    }
}
