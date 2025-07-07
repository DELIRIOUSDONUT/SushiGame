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
            child.color = new Color32(106, 89, 0, 255);
            child.text = $"{i+1}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            //Highlight(i+1); // works
        }
    }
    
    public void Highlight(int index)
    {
        _children[index-1].color = new Color32(255, 215, 0, 255);
    }

    public void Unhighlight(int index)
    {
        _children[index - 1].color = new Color32(106, 89, 0, 255);

    }
    
    public void UnhighlightAll()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            _children[i].color = new Color32(106, 89, 0, 255);
        }
    }
}
