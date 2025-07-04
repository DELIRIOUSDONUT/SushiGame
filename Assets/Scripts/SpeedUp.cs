using UnityEngine;
using UnityEngine.UI;

public class SpeedUp : MonoBehaviour
{
    [SerializeField] private Toggle ThisToggle;
    [SerializeField] private LineRenderChecker lineRenderChecker;
    [SerializeField] private float speedUpFactor = 1.5f;
    [SerializeField] private AutoPlayScript autoPlayScript;

    private float _defaultDisplayDelay;
    private float _defaultWaitTimeSeconds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ThisToggle.onValueChanged.AddListener(OnToggleValueChanged);
        _defaultDisplayDelay = lineRenderChecker.GetDisplayDelay();
        _defaultWaitTimeSeconds = autoPlayScript.WaitTimeSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnToggleValueChanged(bool value)
    {
        if (value)
        {
            lineRenderChecker.SetDisplayDelay(_defaultDisplayDelay / speedUpFactor);
            autoPlayScript.WaitTimeSeconds = _defaultWaitTimeSeconds / speedUpFactor;
        }
        else
        {
            lineRenderChecker.SetDisplayDelay(_defaultDisplayDelay);
            autoPlayScript.WaitTimeSeconds = _defaultWaitTimeSeconds;
        }
    }
}
