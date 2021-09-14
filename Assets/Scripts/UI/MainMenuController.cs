using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour 
{

    private void Start()
    {
        AudioListener.volume = SoundOptions.GetMasterVolume();
        Debug.Log(AudioListener.volume);
    }

    public void Play()
	{
		SceneManager.LoadScene("TestMap");
	}

	public void HighScore()
	{
		SceneManager.LoadScene("ScoreScene");
	}

	public void QuitGame()
	{
		Debug.Log("Quitting game...");
		Application.Quit();
	}

}
