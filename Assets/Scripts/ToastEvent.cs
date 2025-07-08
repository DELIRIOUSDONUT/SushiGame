using UnityEngine;

public class ToastEvent : MonoBehaviour
{
    [SerializeField] private Animator ToastAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAnimationLock()
    {
        ToastAnimator.SetBool("doShow", true);
    }
    
    public void ReleaseAnimationLock()
    {
        ToastAnimator.SetBool("doShow", false);
    }
}
