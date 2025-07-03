using System;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderChecker : MonoBehaviour
{
    private List<GameObject> strategyGObjects;
    
    // Cache the component references
    private List<LineStrategy> strategyComponents;

    [SerializeField] private List<GameObject> Lines;
    [SerializeField] private GameObject SelectionTrackerObj;
    
    private SelectionTracker selectionTracker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        strategyGObjects = Lines;
        selectionTracker = SelectionTrackerObj.GetComponent<SelectionTracker>();
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
        Debug.Log($"Checking for lines: {string.Join(", ", grid)}");
        if(selectionTracker == null){return 0;}
        List<bool> selections = selectionTracker.GetSelections();
        if (strategyComponents == null || strategyComponents.Count == 0 || strategyGObjects == null || strategyGObjects.Count == 0)
        {
            return 0;
        }
        var (scoringStratsIndices, totalMult) = GetScoredStrategiesIndices(grid);

        foreach (var line in strategyGObjects)
        { 
            line.SetActive(false);
        }

        foreach (var index in scoringStratsIndices)
        {
            if (selections[index])
            {
                strategyGObjects[index].SetActive(true);
            }
        }
        if (totalMult > 0)
        {
            return betAmount * totalMult;
        }

        return 0;

    }

}
