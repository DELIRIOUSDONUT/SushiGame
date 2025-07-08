using System;
using TMPro;
using UnityEngine;

public class JackpotTracker : MonoBehaviour
{
    [SerializeField] private GameObject JackPotObject;
    [SerializeField] private int StartingPot = 0;
    private TextMeshPro JackPotText;
    [SerializeField] private JackpotWinController JackpotWinningTextObj;
    private int potAmount;
    
    [SerializeField] private bool loadState = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        JackPotText = JackPotObject.GetComponent<TextMeshPro>();
        if (PlayerPrefs.HasKey("Jackpot") && loadState)
        {
            potAmount = PlayerPrefs.GetInt("Jackpot");
        }
        else
        {
            potAmount = StartingPot;
            PlayerPrefs.SetInt("Jackpot", potAmount);
        }
        UpdateText();
        JackpotWinningTextObj.UpdateText(potAmount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText()
    {
        JackPotText.text = $"{potAmount}$";
    }
    public void UpdatePot(int amount)
    {
        potAmount += amount;
        potAmount = Math.Clamp(potAmount, 0, 1000000000);
        UpdateText();
        PlayerPrefs.SetInt("Jackpot", potAmount);
        
    }

    public int JackpotExplosion()
    {
        JackpotWinningTextObj.UpdateText(potAmount);
        JackpotWinningTextObj.PlayWinningAnimation();
        var amount = potAmount;
        potAmount = 0;
        UpdateText();
        PlayerPrefs.SetInt("Jackpot", potAmount);
        return amount;
    }

    public void TestJackPot()
    {
        JackpotExplosion();
    }
}
