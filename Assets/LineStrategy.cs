using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class LineStrategy : MonoBehaviour
{
    [SerializeField] private List<int> Positions;
    [SerializeField] private uint Score = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public uint GetScore<T>(List<T> grid) where T : IComparable<T>
    {
        // If the three corresponding grid positions have the same elements
        if (grid[Positions[0]].Equals(grid[Positions[1]]) && 
            grid[Positions[0]].Equals(grid[Positions[2]]))
        {
            return Score;
        }
        return 0;
    }

    public uint GetLineStrategyScore()
    {
        return Score;
    }
}
