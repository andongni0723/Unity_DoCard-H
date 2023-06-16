using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : Singleton<SoundManager>
{
    AudioSource audioSource;
    public AudioClip playCardSE;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    #region Event

    private void OnEnable()
    {
        EventHanlder.ChangeVolume += OnChangeVolume; // Set audio volume
    }

    private void OnDisable()
    {
        EventHanlder.ChangeVolume -= OnChangeVolume;
    }

    private void OnChangeVolume(float value)
    {
        audioSource.volume = value;
    }

    #endregion 

    public void PlaySound(AudioClip sound)
    {
        //audioSource.clip = sound;
        audioSource.PlayOneShot(sound);
    }
}
