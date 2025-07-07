using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReelDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject TopSlot;
    [SerializeField] private GameObject MiddleSlot;
    [SerializeField] private GameObject BottomSlot;
    private Image _topSlot;
    private Image _bottomSlot;
    private Image _middleSlot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _topSlot = TopSlot.GetComponent<Image>();
        _bottomSlot = BottomSlot.GetComponent<Image>();
        _middleSlot = MiddleSlot.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayReel(List<Sprite> reel)
    {
        //Debug.Log($"Reel: {string.Join(", ", reel)}");
        _topSlot.sprite = reel[0];
        _middleSlot.sprite = reel[1];
        _bottomSlot.sprite = reel[2];
        
    }
    
}
