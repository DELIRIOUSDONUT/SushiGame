using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionToggler : MonoBehaviour
{
    [SerializeField] List<GameObject> Selections;
    bool isEvenOn = false;
    bool isOddOn = false;
    bool isAllOn = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        ToggleNone();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToggleOdd()
    {
        isOddOn = !isOddOn;
        foreach (var selection in Selections)
        {
            // First check the name of the game object if it has an odd number
            // Game object name is "Opt[number]", eg Opt1 up to Opt20
            if (int.Parse(selection.name.Substring(3)) % 2 == 1)
            {
                var IconNum = selection.transform.GetChild(0);
                var Line = selection.transform.GetChild(1);
                
                IconNum.GetComponent<Toggle>().isOn = isOddOn;
                Line.GetComponent<Toggle>().isOn = isOddOn;
            }
        }
    }
    
    public void ToggleEven()
    {
        isEvenOn = !isEvenOn;
        foreach (var selection in Selections)
        {
            // First check the name of the game object if it has an odd number
            // Game object name is "Opt[number]", eg Opt1 up to Opt20
            if (int.Parse(selection.name.Substring(3)) % 2 == 0)
            {
                var IconNum = selection.transform.GetChild(0);
                var Line = selection.transform.GetChild(1);

                IconNum.GetComponent<Toggle>().isOn = isEvenOn;
                Line.GetComponent<Toggle>().isOn = isEvenOn;
            }
        }
    }

    public void ToggleAll()
    {
        isAllOn = !isAllOn;
        foreach (var selection in Selections)
        {
            var IconNum = selection.transform.GetChild(0);
            var Line = selection.transform.GetChild(1);
            
            IconNum.GetComponent<Toggle>().isOn = isAllOn;
            Line.GetComponent<Toggle>().isOn = isAllOn;
        }
    }

    public void ToggleNone()
    {
        isAllOn = true;
        isEvenOn = false;
        isOddOn = false;
        ToggleAll();
    }
}
