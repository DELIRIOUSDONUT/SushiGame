using System;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderChecker : MonoBehaviour
{
    [Header("Diagonal + Horizontal Lines")] [SerializeField]
    private GameObject LRDownDiagTop;

    [SerializeField] private GameObject LRDownDiagBottom;
    [SerializeField] private GameObject LRUpDiagTop;
    [SerializeField] private GameObject LRUpDiagBottom;
    [SerializeField] private GameObject DiagDownLRTop;
    [SerializeField] private GameObject DiagDownLRBottom;
    [SerializeField] private GameObject DiagUpLRTop;
    [SerializeField] private GameObject DiagUpLRBottom;

    [Header("Tick Line")] [SerializeField] private GameObject TickV;

    [Header("Long V Lines")] [SerializeField]
    private GameObject LongDownV;

    [SerializeField] private GameObject LongUpV;

    [Header("Short V Lines")] [SerializeField]
    private GameObject ShortDownVTop;

    [SerializeField] private GameObject ShortDownVBottom;
    [SerializeField] private GameObject ShortUpVTop;
    [SerializeField] private GameObject ShortUpVBottom;

    [Header("Straight Lines")] [SerializeField]
    private GameObject StraightRowTop;

    [SerializeField] private GameObject StraightRowMiddle;
    [SerializeField] private GameObject StraightRowBottom;

    private List<GameObject> strategyGObjects;
    
    // Cache the component references
    private List<LineStrategy> strategyComponents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        strategyGObjects = new List<GameObject>
        {
            LRDownDiagTop,
            LRDownDiagBottom,
            LRUpDiagTop,
            LRUpDiagBottom,
            DiagDownLRTop,
            DiagDownLRBottom,
            DiagUpLRTop,
            DiagUpLRBottom,
            TickV,
            LongDownV,
            LongUpV,
            ShortDownVTop,
            ShortDownVBottom,
            ShortUpVTop,
            ShortUpVBottom,
            StraightRowTop,
            StraightRowMiddle,
            StraightRowBottom
        };

        // Cache all strategy components during initialization
        strategyComponents = new List<LineStrategy>(strategyGObjects.Count);
        foreach (var strategyObject in strategyGObjects)
        {
            // Using GetComponent during initialization is fine
            // as it only happens once
            var strategyComponent = strategyObject.GetComponent<LineStrategy>();
            if (strategyComponent != null)
            {
                strategyComponents.Add(strategyComponent);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Get a specific strategy by index
    private GameObject GetStrategy(int index)
    {
        if (index >= 0 && index < strategyGObjects.Count)
        {
            return strategyGObjects[index];
        }

        return null;
    }

    private (List<int>, decimal) GetScoredStrategiesIndices(List<String> grid)
    {
        var strategyGObjectsIndices = new List<int>();
        decimal totalMult = 0;
        // Iterate through each line
        // Each line has a GameObject (for rendering) and a script (score, shape, position)
        // Strategy(Grid) => Score (0 if no score, else score)
        for (int i = 0; i < strategyComponents.Count; i++)
        {
            var mult = strategyComponents[i].GetMultiplier(grid);
            if (mult != 0)
            {
                strategyGObjectsIndices.Add(i);;
                if (mult == -1)
                {
                    // Jackpot -- handle here
                    break;
                }

                totalMult += mult;
            }
        }

        return (strategyGObjectsIndices, totalMult);
    }
    
    public decimal DoScoreCheck(List<String> grid, uint betAmount)
    {
        var (scoringStratsIndices, totalMult) = GetScoredStrategiesIndices(grid);
        if (totalMult > 0)
        {
            return betAmount * totalMult;
        }

        return 0;

    }

}
