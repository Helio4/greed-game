using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private int _score = 0;
    [SerializeField] private GameObject _gold;
    [SerializeField] private GameObject _potion;
    [SerializeField] private GameObject _daggers;
    [SerializeField] private float _timeIncreaseDifficulty = 180.0f;
    [SerializeField] private AudioClip gameOverSound;
    private int difficultyLevel = 0;


    
    // Use this for initialization
    void Start () {
        AudioListener.volume = SoundOptions.GetMasterVolume();
        Debug.Log("Starting New Game...");
        _score = 0;
        difficultyLevel = 0;
        StartCoroutine("IncreaseDifficulty");
        Debug.Log(Time.timeScale);
    }

    private void Awake()
    {
        Messenger.AddListener(GameEvent.GAME_OVER, OnGameOver);
        Messenger<Transform>.AddListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
        Messenger<int>.AddListener(GameEvent.GOLD_GOTTEN, IncreaseScore);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GAME_OVER, OnGameOver);
        Messenger<Transform>.RemoveListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
        Messenger<int>.RemoveListener(GameEvent.GOLD_GOTTEN, IncreaseScore);
    }

    // Update is called once per frame
    void Update () 
    {

	}

    private void OnGameOver() {
        Debug.Log("GameOver!");
        //Do GameOver Stuff
        AudioSource.PlayClipAtPoint(gameOverSound, GameObject.FindGameObjectWithTag("Player").transform.position, SoundOptions.GetSFXVolume());
        UpdateScoreRanking();
        togglePause();
    }

    private void UpdateScoreRanking() {
        int score1st = PlayerPrefs.GetInt("score_first");
        int score2nd = PlayerPrefs.GetInt("score_second");
        int score3rd = PlayerPrefs.GetInt("score_third");

        if (_score > score1st)
        {
            score3rd = score2nd;
            score2nd = score1st;
            score1st = _score;
        }
        else if (_score > score2nd)
        {
            score3rd = score2nd;
            score2nd = _score;
        }
        else if (_score > score3rd) {
            score3rd = _score;
        }

        PlayerPrefs.SetInt("score_first", score1st);
        PlayerPrefs.SetInt("score_second", score2nd);
        PlayerPrefs.SetInt("score_third", score3rd);
    }

    private void OnEnemyDead(Transform bodyTransform)
    {
        float result = Random.Range(0.0f, 1.0f);
        if (result <= 0.6f) {
            //Drop
            result = Random.Range(0.0f, 1.0f);
            GameObject toInstanciate;
            if (result <= 0.6f) toInstanciate = _gold;
            else if (result <= 0.8f) toInstanciate = _potion;
            else toInstanciate = _daggers;
            Instantiate(toInstanciate, bodyTransform.position, bodyTransform.rotation);
        }
    }

    private void IncreaseScore(int increment) {
        _score += increment;
        Debug.Log("Score: " + _score);
        ScoreManager.score = _score;
    }

    private bool togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            return (false);
        }
        else
        {
            Time.timeScale = 0f;
            return (true);
        }
    }

    private IEnumerator IncreaseDifficulty() {
        yield return new WaitForSeconds(_timeIncreaseDifficulty);
        difficultyLevel++;
        Debug.Log("Difficulty: " + difficultyLevel);
        Messenger<int>.Broadcast(GameEvent.INCREASE_DIFFICULTY, difficultyLevel);
        StartCoroutine("IncreaseDifficulty");
    }
}
