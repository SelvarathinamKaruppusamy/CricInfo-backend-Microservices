namespace MatchService.Domain.Entities
{
   public class Batting
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public int TeamId { get; set; }

        public int MatchNo { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int Runs { get; set; }

        public int Balls { get; set; }

        public int Fours { get; set; }

        public int Sixes { get; set; }

        public decimal StrikeRate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
