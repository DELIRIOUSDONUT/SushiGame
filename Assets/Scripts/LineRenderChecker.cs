using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;


public class LineRenderChecker : MonoBehaviour
{
    private List<GameObject> strategyGObjects;
    
    // Cache the component references
    private List<LineStrategy> strategyComponents;

    [SerializeField] private List<GameObject> Lines;
    [SerializeField] private GameObject SelectionTrackerObj;
    
    [SerializeField] private float displayDelay = 0.1f;
    [SerializeField] private GameObject NumberControllerObj;
    
    [SerializeField] AudioManagerScript AudioManager;
    [SerializeField] float PitchGrowth = 0.01f;
    
    private NumberController numberController;
    
    private SelectionTracker selectionTracker;

    private bool currentlyPlaying = false;

    private float currentLinePitch;
    

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
    
    public async Task<decimal> DoScoreCheck(List<String> grid)
    {
        currentlyPlaying = true;
        //Debug.Log($"Checking for lines: {string.Join(", ", grid)}");
        //if(selectionTracker == null){return 0;}
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
        
        var tcs = new TaskCompletionSource<bool>();
        AudioManager.ResetPitch();
        currentLinePitch = AudioManager.GetPitch();
        StartCoroutine(ActivateLinesSequentially(scoringStratsIndices, selections, tcs));
        await tcs.Task;
        
        currentlyPlaying = false;
        if (totalMult > 0)
        {
            return totalMult;
        }

        return 0;

    }
    
    private IEnumerator ActivateLinesSequentially(List<int> indices, List<bool> selections, 
                                                    TaskCompletionSource<bool> tcs)
    {
        // First get number of activated lines from selections
        float scaledDelay = displayDelay / (2 * selections.Count(x => x));
        yield return new WaitForSeconds(scaledDelay);
        foreach (var index in indices)
        {
            if (selections[index])
            {
                AudioManager.PlayAudio("LineScoreAudio", currentLinePitch);
                numberController.Highlight(index+1);
                strategyGObjects[index].SetActive(true);
                currentLinePitch += PitchGrowth;
                yield return new WaitForSeconds(scaledDelay);
            }
        }
        
        tcs.SetResult(true);
    }

    public bool isCurrentlyPlaying()
    {
        return currentlyPlaying;
    }
    
    public float GetDisplayDelay()
    {
        return displayDelay;
    }

    public void SetDisplayDelay(float delay)
    {
        displayDelay = delay;
    }

    public void TurnOffLines()
    {
        for (int i = 0; i < strategyGObjects.Count; i++)
        {
            strategyGObjects[i].SetActive(false);
        }
        numberController.UnhighlightAll();
    }


}
