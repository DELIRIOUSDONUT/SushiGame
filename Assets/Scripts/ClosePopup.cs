using TMPro;
using UnityEngine;
using UnityEngine.WSA;

public class ClosePopup : MonoBehaviour
{
    [SerializeField] private GameObject MainGameFacade;

    [SerializeField] private GameObject LineSelectPopup;

    [SerializeField] private GameObject TextToUpdate;
    [SerializeField] private ToastEvent ToastMessageAnimator;
    
    private SelectionTracker selectionTracker;
    private Animator mainGameAnimator;
    private Animator lineSelectAnimator;
    private TextMeshPro textToUpdate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectionTracker = LineSelectPopup.GetComponent<SelectionTracker>();
        mainGameAnimator = MainGameFacade.GetComponent<Animator>();
        lineSelectAnimator = LineSelectPopup.GetComponent<Animator>();
        textToUpdate = TextToUpdate.GetComponent<TextMeshPro>();
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
            textToUpdate.text = $"{selectionTracker.GetNumSelections()} DÒNG";
        }
        else
        {
            ToastMessageAnimator.SetAnimationLock();
        }
    }
    
    
}
