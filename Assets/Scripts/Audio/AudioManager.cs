using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour {

	public Sound[] sounds;
	public static AudioManager instance;

	void Awake () 
	{
        Messenger<float>.AddListener(GameEvent.MUSIC_VOLUME_CHANGED, OnMusicVolumeChange);
		if(instance == null)
		{
			instance = this;
		}

		else
		{
			Destroy(gameObject);
			return;
		}
	
		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;

			s.source.volume = SoundOptions.GetMusicVolume();
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

    private void OnDestroy()
    {
        Messenger<float>.RemoveListener(GameEvent.MUSIC_VOLUME_CHANGED, OnMusicVolumeChange);
    }

    private void OnMusicVolumeChange(float newVolume)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = newVolume;
        }
    }

    void Start()
	{
		Play("Theme");
        AudioListener.volume = SoundOptions.GetMusicVolume();
    }
	
	public void Play(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		s.source.Play();
	}
}
