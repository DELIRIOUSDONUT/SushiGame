using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Assertions;

public class LineStrategy : MonoBehaviour
{
    [SerializeField] private List<int> Positions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public decimal GetMultiplier(List<String> grid)
    {
        var symbols = new[] { grid[Positions[0]-1], grid[Positions[1]-1], grid[Positions[2]-1] };
        HashSet<String> set = new HashSet<String>(symbols);

        // Count occurrences for Green and Yellow
        var greenCount = symbols.Count(s => s == "Green");
        var yellowCount = symbols.Count(s => s == "Yellow");

        //Debug.Log($"Symbols [{string.Join(", ", symbols)}], Positions [{string.Join(", ", Positions)}]");
        if(greenCount == 3){return ScoringDictionary.Scores["Green"];}
        if(yellowCount == 3){return ScoringDictionary.Scores["Yellow"];}
        return set.Count switch
        {
            1 => ScoringDictionary.Scores[set.ElementAt(0)],
            2 when !set.Contains("Jackpot") && set.Contains("Green") && greenCount == 2 => ScoringDictionary.Scores["Green_2"],
            2 when !set.Contains("Jackpot") && set.Contains("Yellow") && yellowCount == 2 => ScoringDictionary.Scores["Yellow_2"],
            2 when set.Contains("Jackpot") => ScoringDictionary.Scores[set.First(item => item != "Jackpot")],
            3 when set.Contains("Green") && set.Contains("Yellow") && set.Contains("Jackpot") => 
                ScoringDictionary.Scores["Green-Yellow"],
            _ => 0
        };
    }
}
