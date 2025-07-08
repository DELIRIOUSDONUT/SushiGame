using System.Collections.Generic;

public static class ScoringDictionary
{
    // Global dictionary accessible from anywhere
    public static readonly IReadOnlyDictionary<string, decimal> Scores = new Dictionary<string, decimal>
    {
        ["Green_2"] = 0.4m,
        ["Green"] = 3m,
        ["Yellow_2"] = 0.8m,
        ["Yellow"] = 8m,
        ["Pink"] = 20m,
        ["Orange"] = 40m,
        ["Red"] = 85m,
        ["Jackpot"] = -1m,
        ["Green-Yellow"] = 1.2m
    };
}

