using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour 
{
    public AudioClip[] audioClips;
    public int ClipIndexToPlay;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        PlayAudioClip(ClipIndexToPlay);
    }

    public void PlayAudioClip(int ClipIndexToPlay)
    {
        source.PlayOneShot(audioClips[ClipIndexToPlay], SoundOptions.GetSFXVolume());
        source.Play();
    }
}
