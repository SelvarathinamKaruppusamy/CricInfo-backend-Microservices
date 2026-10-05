using System.Collections.Concurrent;

namespace MatchService.Application.Memory;

public static class MatchBallStore
{
    public static ConcurrentDictionary<int, List<string>>
        FirstInningsBalls
    { get; } = new();

    public static ConcurrentDictionary<int, List<string>>
        SecondInningsBalls
    { get; } = new();
}