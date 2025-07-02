using UnityEngine;

public class Disabler : MonoBehaviour
{
    [SerializeField] GameObject ParentToDisable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ParentToDisable.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
