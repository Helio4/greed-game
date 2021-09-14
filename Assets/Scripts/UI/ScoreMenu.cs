using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMenu : MonoBehaviour {

    [SerializeField] private Text textFirst;
    [SerializeField] private Text textSecond;
    [SerializeField] private Text textThird;

    // Use this for initialization
    void Start () {
        if (PlayerPrefs.HasKey("score_first")) {
            int score1st = PlayerPrefs.GetInt("score_first");
            textFirst.text = textFirst.text.Substring(0, textFirst.text.Length - score1st.ToString().Length) + score1st.ToString();
        }
        if (PlayerPrefs.HasKey("score_second"))
        {
            int score2nd = PlayerPrefs.GetInt("score_second");
            textSecond.text = textSecond.text.Substring(0, textSecond.text.Length - score2nd.ToString().Length) + score2nd.ToString();
        }
        if (PlayerPrefs.HasKey("score_third"))
        {
            int score3rd = PlayerPrefs.GetInt("score_third");
            textThird.text = textThird.text.Substring(0, textThird.text.Length - score3rd.ToString().Length) + score3rd.ToString();
        }

    }

}
