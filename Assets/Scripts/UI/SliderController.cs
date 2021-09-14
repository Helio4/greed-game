using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {

    private Slider slider;
    public string type = "";

	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        if (slider != null || type != "") {
            float currentValue = 1.0f;
            switch (type) {
                case "sfx":
                    if (PlayerPrefs.HasKey("sfx_volume")) currentValue = PlayerPrefs.GetFloat("sfx_volume");
                    break;
                case "music":
                    if(PlayerPrefs.HasKey("music_volume")) currentValue = PlayerPrefs.GetFloat("music_volume");
                    break;
                case "master":
                    if (PlayerPrefs.HasKey("master_volume")) currentValue = PlayerPrefs.GetFloat("master_volume");
                    break;
            }
            slider.value = currentValue;
        }
	}
	
}
