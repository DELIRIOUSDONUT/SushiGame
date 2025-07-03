using System.Collections.Generic;
using UnityEngine;

public class ReelDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject TopSlot;
    [SerializeField] private GameObject MiddleSlot;
    [SerializeField] private GameObject BottomSlot;
    private SpriteRenderer _topSlot;
    private SpriteRenderer _bottomSlot;
    private SpriteRenderer _middleSlot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _topSlot = TopSlot.GetComponent<SpriteRenderer>();
        _bottomSlot = BottomSlot.GetComponent<SpriteRenderer>();
        _middleSlot = MiddleSlot.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayReel(List<Sprite> reel)
    {
        Debug.Log($"Reel: {string.Join(", ", reel)}");
        _topSlot.sprite = reel[0];
        _middleSlot.sprite = reel[1];
        _bottomSlot.sprite = reel[2];
        
    }
    
}
