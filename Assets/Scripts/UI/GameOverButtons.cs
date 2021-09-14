using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverButtons : MonoBehaviour {

	public void Replay()
	{
		SceneManager.LoadScene("TestMap");	
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
