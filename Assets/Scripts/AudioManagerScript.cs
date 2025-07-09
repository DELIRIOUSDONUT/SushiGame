using System;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [SerializeField] private AudioSource LineScoreAudio;


    private float _defaultPitch;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _defaultPitch = LineScoreAudio.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(String audioName, float pitch)
    {
        switch (audioName)
        {
            case "LineScoreAudio":
                LineScoreAudio.pitch = pitch;
                LineScoreAudio.Play();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(audioName), audioName, null);
                // "Audio not found"
        }
    }

    public float GetPitch()
    {
        return LineScoreAudio.pitch;
    }
    
    public void ResetPitch()
    {
        LineScoreAudio.pitch = _defaultPitch;
    }
}
