using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DaggerManager : MonoBehaviour {

	public static int daggers;
	Text text;

	void Awake () 
	{
		text = GetComponent<Text>();
		daggers = 16;
	}
	
	// Update is called once per frame
	void Update () 
	{
		text.text = "" + daggers;	
	}
}
