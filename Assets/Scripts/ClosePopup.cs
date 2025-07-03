using UnityEngine;

public class ClosePopup : MonoBehaviour
{
    [SerializeField] private GameObject MainGameFacade;

    [SerializeField] private GameObject LineSelectPopup;
    private SelectionTracker selectionTracker;
    private Animator mainGameAnimator;
    private Animator lineSelectAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectionTracker = LineSelectPopup.GetComponent<SelectionTracker>();
        mainGameAnimator = MainGameFacade.GetComponent<Animator>();
        lineSelectAnimator = LineSelectPopup.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Close()
    {
        if (selectionTracker.GetNumSelections() > 0)
        {
            mainGameAnimator.SetTrigger("DoSlide");
            lineSelectAnimator.SetTrigger("DoSlide");
        }
    }
    
    
}
