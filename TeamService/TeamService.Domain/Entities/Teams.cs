using System;
using System.Collections.Generic;
using System.Text;

namespace TeamService.Domain.Entities
{
    public class Teams
    {
        public int? Id { get; set; }
        public int? TeamId { get; set; }
        public int? MatchNo { get; set; }

        public string? FullName { get; set; }
        public string? ShortName { get; set; }
        public string? Logo { get; set; }
        public string? Scores { get; set; }
        public int? Runs { get; set; }
        public int? Wickets { get; set; }
        public int? Extras { get; set; }
        public decimal? Overs { get; set; }
        public int? Balls { get; set; }
        public int? WinCount { get; set; }
        public int? LossCount { get; set; }
        public int? TotalMatch { get; set; }
        public string? MatchStatus { get; set; }

    }
}