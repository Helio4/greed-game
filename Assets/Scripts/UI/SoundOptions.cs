using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptions : MonoBehaviour {

    public AudioClip testSound;

    public static float GetMasterVolume() {
        float res = 1.0f;
        if (PlayerPrefs.HasKey("master_volume")) {
            res = PlayerPrefs.GetFloat("master_volume");
        }
        return res;
    }

    public static float GetSFXVolume() {
        float res = 1.0f;
        if (PlayerPrefs.HasKey("sfx_volume"))
        {
            res = PlayerPrefs.GetFloat("sfx_volume");
        }
        return res;
    }

    public static float GetMusicVolume()
    {
        float res = 1.0f;
        if (PlayerPrefs.HasKey("music_volume"))
        {
            res = PlayerPrefs.GetFloat("music_volume");
        }
        return res;
    }

    public void SetMasterVolume(Slider slider) {
        if (slider.value == GetMusicVolume()) return;
        Debug.Log("Master");
        PlayerPrefs.SetFloat("master_volume", slider.value);
        AudioListener.volume = GetMasterVolume();
    }

    public void SetSFXVolume(Slider slider)
    {
        if (slider.value == GetSFXVolume()) return;
        Debug.Log("SFX");
        PlayerPrefs.SetFloat("sfx_volume", slider.value);
        AudioSource.PlayClipAtPoint(testSound, GameObject.FindObjectOfType<Camera>().transform.position, GetSFXVolume());
    }

    public void SetMusicVolume(Slider slider)
    {
        if (slider.value == GetMusicVolume()) return;
        Debug.Log("Music");
        PlayerPrefs.SetFloat("music_volume", slider.value);
        Messenger<float>.Broadcast(GameEvent.MUSIC_VOLUME_CHANGED, GetMusicVolume());
    }

}
