using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour 
{
	//public AudioMixer audioMixer;

	public void SetMasterVolume(float volume)
	{
		//audioMixer.SetFloat("MasterVolume", volume);
		Debug.Log("Master volume " + volume);
		SetMusicVolume(volume);
		SetEffectsVolume(volume);
	}

	public void SetMusicVolume(float volume)
	{
		Debug.Log("Music volume " + volume);
	}

	public void SetEffectsVolume(float volume)
	{
		Debug.Log("Effects volume " + volume);
	}
}
