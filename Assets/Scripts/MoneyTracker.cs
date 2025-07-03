using TMPro;
using UnityEngine;

public class MoneyTracker : MonoBehaviour
{
    [SerializeField] private int StartingMoney;
    [SerializeField] private GameObject MoneyText;
    private TextMeshPro moneyText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moneyText = MoneyText.GetComponent<TextMeshPro>();
        moneyText.text = $"{StartingMoney}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateMoney(int amount)
    {
        moneyText.text = $"{amount}";
    }
}
