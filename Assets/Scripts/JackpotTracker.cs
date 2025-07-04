using System;
using TMPro;
using UnityEngine;

public class JackpotTracker : MonoBehaviour
{
    [SerializeField] private GameObject JackPotObject;
    [SerializeField] private int StartingPot = 0;
    private TextMeshPro JackPotText;
    
    private int potAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        JackPotText = JackPotObject.GetComponent<TextMeshPro>();
        potAmount = StartingPot;
        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText()
    {
        JackPotText.text = $"{potAmount}";
    }
    public void UpdatePot(int amount)
    {
        potAmount += amount;
        potAmount = Math.Clamp(potAmount, 0, 1000000000);
        UpdateText();
        
    }

    public int JackpotExplosion()
    {
        var amount = potAmount;
        potAmount = 0;
        UpdateText();
        return amount;
    }
}
