using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUp : MonoBehaviour
{
    [SerializeField] private Toggle ThisToggle;
    [SerializeField] private LineRenderChecker lineRenderChecker;
    [SerializeField] private float speedUpFactor = 1.5f;
    [SerializeField] private AutoPlayScript autoPlayScript;
    [SerializeField] private List<ReelRoller> reelRollers;
    
    private float _defaultDisplayDelay;
    private float _defaultWaitTimeSeconds;

    private float _defaultReelMoveDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ThisToggle.onValueChanged.AddListener(OnToggleValueChanged);
        _defaultDisplayDelay = lineRenderChecker.GetDisplayDelay();
        _defaultWaitTimeSeconds = autoPlayScript.WaitTimeSeconds;
        _defaultReelMoveDuration = reelRollers[0].MoveDuration;
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
            foreach (var reelRoller in reelRollers)
            {
                reelRoller.MoveDuration = _defaultReelMoveDuration / speedUpFactor;
            }
        }
        else
        {
            lineRenderChecker.SetDisplayDelay(_defaultDisplayDelay);
            autoPlayScript.WaitTimeSeconds = _defaultWaitTimeSeconds;
            foreach (var reelRoller in reelRollers)
            {
                reelRoller.MoveDuration = _defaultReelMoveDuration;
            }
        }
    }
}
