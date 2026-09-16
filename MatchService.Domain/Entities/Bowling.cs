namespace MatchService.Domain.Entities
{
    public class Bowling
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public int TeamId { get; set; }

        public int MatchNo { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public decimal Overs { get; set; }

        public int Balls { get; set; }

        public int Maidens { get; set; }

        public int RunsConceded { get; set; }

        public int Wickets { get; set; }

        public decimal Economy { get; set; }
    }
}
