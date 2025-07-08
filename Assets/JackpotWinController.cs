using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class JackpotWinController : MonoBehaviour
{
    [SerializeField] private TextMeshPro JackpotWinningText;
    [SerializeField] private Animator JackpotWinningAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public async void PlayWinningAnimation()
    {
        Debug.Log("Animation started");
        JackpotWinningAnimator.SetTrigger("Win");
        await WaitForAnimation("Win");
        Debug.Log("Animation finished");
    }
    
    public async Task WaitForAnimation(string stateName, int layer = 0)
    {
        // Wait until the animator is in the desired state
        while (!JackpotWinningAnimator.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
        {
            await Task.Yield(); // wait one frame
        }

        // Then wait until the animation finishes
        while (JackpotWinningAnimator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1f)
        {
            await Task.Yield();
        }
    }
    public void UpdateText(int amount)
    {
        JackpotWinningText.text = $"{amount}";
    }
    public void TestJackpot()
    {}

    public void OnAnimationEnd()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    
    public void OnAnimationStart()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
