using TMPro;
using UnityEngine;

public class MoneyTracker : MonoBehaviour
{
    [SerializeField] private int StartingMoney;
    [SerializeField] private GameObject MoneyText;
    [SerializeField] private GameObject LineSelectManager;
    [SerializeField] private JackpotTracker JackpotTracker;
    private TextMeshPro moneyText;

    private int currentBet;
    private int currentEarnings;
    private SelectionTracker selectionTracker;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moneyText = MoneyText.GetComponent<TextMeshPro>();
        moneyText.text = $"{StartingMoney}";
        currentBet = 0;

        if (PlayerPrefs.HasKey("PlayerMoney"))
        {
            currentEarnings = PlayerPrefs.GetInt("PlayerMoney");
            UpdateMoney(currentEarnings);
        }
        else
        {
            PlayerPrefs.SetInt("PlayerMoney", StartingMoney);
            currentEarnings = StartingMoney;
        }
        
        selectionTracker = LineSelectManager.GetComponent<SelectionTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateMoney(int amount)
    {
        moneyText.text = $"{amount}";
    }

    public void SetBet(int amount)
    {
        if (selectionTracker == null)
        { return; }
        currentBet = amount * selectionTracker.GetNumSelections();
    }

    public void UpdateEarnings(decimal multiplier)
    {
        Debug.Log($"Scaled multiplier: {multiplier}");
        Debug.Log($"Current Earnings: {currentEarnings}");
        Debug.Log($"Current Bet: {currentBet}");
        currentEarnings -= currentBet;
        int currEarnings = 0;
        if (multiplier < 0)
        {
            // Jackpot case
            currentEarnings += JackpotTracker.JackpotExplosion();
        }
        else
        {
            currEarnings = (int)(currentBet * multiplier);
            JackpotTracker.UpdatePot(currentBet - currEarnings);
            currentEarnings += currEarnings;
        }
        currentEarnings = Mathf.Clamp(currentEarnings, 0, 100000000);
        UpdateMoney(currentEarnings);
        PlayerPrefs.SetInt("PlayerMoney", currentEarnings);
    }

    public int getCurrentEarnings()
    {
        return currentEarnings;
    }

    public int getCurrentBet()
    {
        return currentBet;
    }

    public bool canRoll()
    {
        return currentEarnings >= currentBet;
    }
}
