using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class LineRenderChecker : MonoBehaviour
{
    private List<GameObject> strategyGObjects;
    
    // Cache the component references
    private List<LineStrategy> strategyComponents;

    [SerializeField] private List<GameObject> Lines;
    [SerializeField] private GameObject SelectionTrackerObj;
    
    [SerializeField] private float displayDelay = 0.1f;
    [SerializeField] private GameObject NumberControllerObj;
    
    private NumberController numberController;
    
    private SelectionTracker selectionTracker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        numberController = NumberControllerObj.GetComponent<NumberController>();
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

    private (List<int>, decimal) GetScoredStrategiesIndices(List<String> grid, List<bool> selections)
    {
        var strategyGObjectsIndices = new List<int>();
        decimal totalMult = 0;
        // Iterate through each line
        // Each line has a GameObject (for rendering) and a script (score, shape, position)
        // Strategy(Grid) => Score (0 if no score, else score)
        for (int i = 0; i < strategyComponents.Count; i++)
        {
            if(!selections[i]){continue;}
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

        return (strategyGObjectsIndices, totalMult / selections.Count(x => x));
    }
    
    public decimal DoScoreCheck(List<String> grid)
    {
        //Debug.Log($"Checking for lines: {string.Join(", ", grid)}");
        if(selectionTracker == null){return 0;}
        List<bool> selections = selectionTracker.GetSelections();
        if (strategyComponents == null || strategyComponents.Count == 0 || strategyGObjects == null || strategyGObjects.Count == 0)
        {
            return 0;
        }
        var (scoringStratsIndices, totalMult) = GetScoredStrategiesIndices(grid, selections);

        for (int i = 0; i < strategyGObjects.Count; i++)
        {
            numberController.Unhighlight(i+1);
            strategyGObjects[i].SetActive(false);
        }

        StartCoroutine(ActivateLinesSequentially(scoringStratsIndices, selections));
        if (totalMult > 0)
        {
            return totalMult;
        }

        return 0;

    }
    
    private IEnumerator ActivateLinesSequentially(List<int> indices, List<bool> selections)
    {
        // First get number of activated lines from selections
        float scaledDelay = displayDelay / (2 * selections.Count(x => x));
        yield return new WaitForSeconds(scaledDelay);
        foreach (var index in indices)
        {
            if (selections[index])
            {
                numberController.Highlight(index+1);
                strategyGObjects[index].SetActive(true);
                yield return new WaitForSeconds(scaledDelay);
            }
        }
    }



}
