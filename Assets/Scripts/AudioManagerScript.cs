using System;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    [SerializeField] private AudioSource LineScoreAudio;
    [SerializeField] private AudioSource SlotAudio;
    [SerializeField] private AudioSource DrummingAudio;
    [SerializeField] private AudioSource SingleRollAudio;

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
            case "SlotAudio":
                SlotAudio.Play();
                break;
            case "DrummingAudio":
                DrummingAudio.Play();
                break;
            case "SingleRollAudio":
                SingleRollAudio.PlayOneShot(SingleRollAudio.clip);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(audioName), audioName, null);
                // "Audio not found"
        }
    }

    public void StopAudio(String audioName)
    {
        switch (audioName)
        {
            case "LineScoreAudio":
                LineScoreAudio.Stop();
                break;
            case "SlotAudio":
                SlotAudio.Stop();
                break;
            case "DrummingAudio":
                DrummingAudio.Stop();
                break;
            case "SingleRollAudio":
                SingleRollAudio.Stop();
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
