using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AutoPlayScript : MonoBehaviour
{
    [SerializeField] private Toggle ThisToggle;
    [SerializeField] ReelManager reelManager;
    [SerializeField] private float WaitTimeSeconds;
    [SerializeField] private LineRenderChecker lineRenderChecker;
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
        if (lineRenderChecker.isCurrentlyPlaying())
        {
            ThisToggle.isOn = false;
            return;
            
        }
        if (value)
        {
            if (repeatCoroutine == null)
            {
                repeatCoroutine = StartCoroutine(AutoPlayCoroutine());
            }
        }
        else
        {
            if (repeatCoroutine != null)
            {
                StopCoroutine(repeatCoroutine);
                repeatCoroutine = null;
            }
        }
    }
    
    private IEnumerator AutoPlayCoroutine()
    {
        while (true)
        {
            reelManager.AllReelsRoll();
            yield return new WaitForSeconds(WaitTimeSeconds);
        }
    }
}
