using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SelectionTracker : MonoBehaviour
{
    private List<bool> _enabledSelections;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enabledSelections = Enumerable.Repeat(false, 20).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ToggleSelection(int index)
    {
        _enabledSelections[index-1] = !_enabledSelections[index-1]; // -1 since indexing
        //Debug.Log($"Selections: [{string.Join(", ", _enabledSelections)}]");
    }

    public void ResetSelections()
    {
        _enabledSelections = Enumerable.Repeat(false, 20).ToList();
    }
    
    public int GetNumSelections()
    {
        return _enabledSelections.Count(item => item);
    }

    public List<bool> GetSelections()
    {
        return _enabledSelections;
    }
}