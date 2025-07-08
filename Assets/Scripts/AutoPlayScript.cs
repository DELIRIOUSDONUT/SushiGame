using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AutoPlayScript : MonoBehaviour
{
    [SerializeField] private Toggle ThisToggle;
    [SerializeField] ReelManager reelManager;
    [SerializeField] public float WaitTimeSeconds;
    [SerializeField] private LineRenderChecker lineRenderChecker;
    [SerializeField] private MoneyTracker moneyTracker;
    [SerializeField] private ToastEvent ToastMessageAnimator;
    private Coroutine repeatCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ThisToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnToggleValueChanged(bool value)
    {
        //if (lineRenderChecker.isCurrentlyPlaying())
        //{
        //    ThisToggle.isOn = true;
        //    return;
        //}
        if (value)
        {
            if (repeatCoroutine == null)
            {
                repeatCoroutine = StartCoroutine(AutoPlayCoroutine());
            }
            ToastMessageAnimator.SetAnimationLock();
        }
        else
        {
            Debug.Log("Stopping AutoPlay");
            if (repeatCoroutine != null)
            {
                StopCoroutine(repeatCoroutine);
                repeatCoroutine = null;
            }
        }
    }
    
    private IEnumerator AutoPlayCoroutine()
    {
        while (moneyTracker.canRoll())
        {
            Task rollTask = reelManager.AllReelsRollAsync(); 
            yield return WaitForTask(rollTask);

            yield return new WaitForSeconds(WaitTimeSeconds);
        }

        ThisToggle.isOn = false;
        repeatCoroutine = null;
    }
    
    private IEnumerator WaitForTask(Task task)
    {
        while (!task.IsCompleted)
            yield return null;

        if (task.IsFaulted)
            Debug.LogException(task.Exception);
    }


}
