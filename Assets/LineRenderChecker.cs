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
        foreach(var strategyGObject in strategyGObjects)
        {
            strategyGObject.SetActive(false);
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    // Get a specific strategy by index
    public GameObject GetStrategy(int index)
    {
        if (index >= 0 && index < strategyGObjects.Count)
        {
            return strategyGObjects[index];
        }

        return null;
    }

    public List<int> GetScoredStrategiesIndices<T>(List<T> grid) where T : IComparable<T>
    {
        var strategyGObjectsIndices = new List<int>();
        // Iterate through each line
        // Each line has a GameObject (for rendering) and a script (score, shape, position)
        // Strategy(Grid) => Score (0 if no score, else score)
        for (int i = 0; i < strategyComponents.Count; i++)
        {
            if (strategyComponents[i].GetScore(grid) > 0)
            {
                strategyGObjectsIndices.Add(i);;
            }
        }

        return strategyGObjectsIndices;
    }

    public uint GetTotalScore(List<int> strategyIndices)
    {
        uint score = 0;
        foreach (var index in strategyIndices)
        {
            score += strategyComponents[index].GetLineStrategyScore();
        }
        return score;
    }

    public void DoScoreCheck<T>(List<T> grid) where T : IComparable<T>
    {
        List<int> strategyIndices = GetScoredStrategiesIndices(grid);
        uint score = GetTotalScore(strategyIndices);
        foreach (var index in strategyIndices)
        {
            strategyGObjects[index].SetActive(true);
        }
    }

}
