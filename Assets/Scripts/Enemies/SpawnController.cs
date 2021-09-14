using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {

    [SerializeField] private GameObject topLeftCorner;
    [SerializeField] private GameObject bottomRightCorner;
    [SerializeField] private float timeBetweenSpawns = 5.0f;
    [SerializeField] private int maxNumOfEnemies = 10;
    [SerializeField] private int increaseOfEnemiesPerLevel = 5;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject MapGenerator;
    private int numEnemies = 0;
    bool spawningEnemies = false;

	// Use this for initialization
	void Start () {
        numEnemies = 0;
    }

    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.INCREASE_DIFFICULTY, OnIncreaseDifficulty);
        Messenger<Transform>.AddListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
    }

    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.INCREASE_DIFFICULTY, OnIncreaseDifficulty);
        Messenger<Transform>.RemoveListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
    }

    private void Update()
    {
        EdgeCollider2D edgeCollider2D = MapGenerator.GetComponent<EdgeCollider2D>();
        if (edgeCollider2D != null && !spawningEnemies) {
            StartCoroutine("SpawnEnemies");
            spawningEnemies = true;
        }
    }

    private void OnEnemyDead(Transform transformEnemy)
    {
        numEnemies--;
        numEnemies = Mathf.Max(0, numEnemies);
    }

    private void OnIncreaseDifficulty(int level) {
        maxNumOfEnemies = maxNumOfEnemies + increaseOfEnemiesPerLevel;
    }

    private IEnumerator SpawnEnemies() {
        Vector2 spawnPoint, viewportPoint;
        bool validPoint = true;
        do
        {
            spawnPoint = GetRandomPoint();
            viewportPoint = _camera.WorldToViewportPoint(spawnPoint);
            validPoint = Poly.ContainsPoint(MapGenerator.GetComponent<EdgeCollider2D>().points, spawnPoint) && (viewportPoint.x > 1 || viewportPoint.x < 0 || viewportPoint.y > 1 || viewportPoint.y < 0);
        } while (!validPoint);
        int enemyIndex = Random.Range(0, enemies.Length);
        Instantiate(enemies[enemyIndex], spawnPoint, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
        numEnemies++;
        yield return new WaitForSeconds(timeBetweenSpawns);
        while (numEnemies >= maxNumOfEnemies) {
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        StartCoroutine("SpawnEnemies");
    }

    private Vector2 GetRandomPoint() {
        float x1 = topLeftCorner.transform.position.x;
        float y1 = topLeftCorner.transform.position.y;
        float x2 = bottomRightCorner.transform.position.x;
        float y2 = bottomRightCorner.transform.position.y;
        float x = Random.Range(x1, x2);
        float y = Random.Range(y2, y1);
        return new Vector2(x, y);
    }
}
