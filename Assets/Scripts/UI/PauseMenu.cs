using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public static bool GameIsPaused = false;
	
	public GameObject pauseMenuUI;
	public GameObject userUI;
	public GameObject optionsMenu;

	public GameObject gameOverUI;
	//[SerializeField] AudioSource audioSource;
	//[SerializeField] AudioClip pressButton;

	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(GameIsPaused)
			{
				Resume();
				optionsMenu.SetActive(false);
			}
			else
			{
				Pause();
			}
		}
	}

	public void Resume()
	{
		userUI.SetActive(true);
		pauseMenuUI.SetActive(false);
		gameOverUI.SetActive(true);
		Time.timeScale = 1f;
		GameIsPaused = false;
    }

  void Pause()
	{
		userUI.SetActive(false);
		pauseMenuUI.SetActive(true);
		gameOverUI.SetActive(false);
		Time.timeScale = 0f;
		GameIsPaused = true;
    }

  public void Restart()
	{
		userUI.SetActive(true);
		Time.timeScale = 1f;
		GameIsPaused = false;
		SceneManager.LoadScene("TestMap");
  }


    public void LoadMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
	}
}
