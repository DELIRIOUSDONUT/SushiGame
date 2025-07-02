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
        HashSet<String> set = new HashSet<String>(new []
        {
            grid[Positions[0]], grid[Positions[1]], grid[Positions[2]]
        });

        return set.Count switch
        {
            1 => ScoringDictionary.Scores[set.ElementAt(0)],
            2 when !set.Contains("Jackpot") && set.Contains("Green") => ScoringDictionary.Scores["Green_2"],
            2 when !set.Contains("Jackpot") && set.Contains("Yellow") => ScoringDictionary.Scores["Yellow_2"],
            2 when set.Contains("Jackpot") => ScoringDictionary.Scores[set.First(item => item != "Jackpot")],
            3 when set.Contains("Green") && set.Contains("Yellow") && set.Contains("Jackpot") => 
                ScoringDictionary.Scores["Green-Yellow"],
            _ => 0
        };

    }
}
