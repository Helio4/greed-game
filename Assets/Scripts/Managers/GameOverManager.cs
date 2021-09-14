using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour {

	Animator anim;
	public playerController player;
	public GameObject playerHUD;
	public GameObject gameOverMenu;

	//Activa botones
	public GameObject replayButton;
	public GameObject menuButton;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Update () {
		if(player._health <= 0)
		{
			anim.SetTrigger("GameOver");
			gameOverMenu.SetActive(true);
			playerHUD.SetActive(false);
			
			//Activo botones en game over
			replayButton.SetActive(true);
			menuButton.SetActive(true);
		}
	}
}
