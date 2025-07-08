using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ObjectBoolPair
{
    public GameObject key;
    public bool value;
}

public class TogglerScript : MonoBehaviour
{
    [SerializeField] List<ObjectBoolPair> toggles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var toggle in toggles)
        {
            toggle.value = !toggle.value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleAttachedObjects()
    {
        //Debug.Log("Toggling");
        foreach (var toggle in toggles)
        {
            toggle.value = !toggle.value;
            toggle.key.SetActive(toggle.value);
        }
    }
}
