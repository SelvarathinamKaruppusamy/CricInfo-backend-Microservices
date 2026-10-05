using System.Collections.Concurrent;

namespace MatchService.Application.Memory;

public static class BowlingOverStore
{
    public static ConcurrentDictionary<
        string,
        int> OverRunsConceded
    { get; } = new();
}