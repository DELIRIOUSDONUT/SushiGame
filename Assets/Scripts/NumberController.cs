using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class NumberController : MonoBehaviour
{
    [SerializeField] private List<GameObject> childObjs;
    private List<TextMeshProUGUI> _children;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _children = new List<TextMeshProUGUI>(childObjs.Count);
        foreach (var childObj in childObjs)
        {
            _children.Add(childObj.GetComponent<TextMeshProUGUI>());
        }
        for (int i = 0; i < _children.Count; i++)
        {
            var child = _children[i];
            child.text = $"<color=#6A5900>{i+1}</color>";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Highlight(int index)
    {
        _children[index-1].color = new Color32(255, 215, 0, 255);
    }
}
