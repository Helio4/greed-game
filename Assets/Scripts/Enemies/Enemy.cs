using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    [SerializeField] protected int _BaseHealth = 50;
    [SerializeField] protected int _healthIncrement = 10;
    protected int _health;
    [SerializeField] protected int _attackIncrement = 5;
    [SerializeField] protected int _BaseAttack = 10;
    static public int difficultyLevel;
    protected int _attack;
    protected bool _alive = true;
    private bool canMove = true;
    [SerializeField] protected float _speed = 4.0f;
    protected Transform target;
    protected Vector2 randomLocation;

    void Start()
    {
        
        difficultyLevel = 0;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine("ChangeLocation");
    }

    private IEnumerator ChangeLocation()
    {
        randomLocation = new Vector2(Random.Range(transform.position.x - 5.0f, transform.position.x + 5.0f), Random.Range(transform.position.y - 5.0f, transform.position.y + 5.0f));
        yield return new WaitForSeconds(2.0f);
        StartCoroutine("ChangeLocation");
    }

    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.INCREASE_DIFFICULTY, OnIncreaseDifficulty);
        _health = _BaseHealth + difficultyLevel * _healthIncrement;
        _attack = _BaseAttack + difficultyLevel * _attackIncrement;
    }

    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.INCREASE_DIFFICULTY, OnIncreaseDifficulty);
    }

    private void OnIncreaseDifficulty(int newLevel) {
        difficultyLevel = newLevel;   
    }

    void Update()
    {

        if (Vector2.Distance(transform.position, target.position) < 8 && canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
        }
        else {
            transform.position = Vector2.MoveTowards(transform.position, randomLocation, (_speed * 0.75f) * Time.deltaTime);
        }
    }

    public virtual void OnHit(int damage) {
        _health -= damage;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Debug.Log("Enemy Health: " + _health); //Update the UI
        if (_health <= 0)
        {
            Die();
            return;
        }
        StartCoroutine("SetColorBackToWhite");
    }

    public abstract void AttackPlayer();

    public void SetAlive(bool alive) {
        _alive = alive;
    }

    public virtual void Die() {
        Destroy(gameObject);
        Messenger<Transform>.Broadcast(GameEvent.ENEMY_DEAD, transform);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        playerController player = other.gameObject.GetComponent<playerController>();
        if (player != null)
        {
            player.Hurt(_attack);
        }
        if(other.gameObject.tag == "pared" || other.gameObject.tag == "Enemy")
        {
            randomLocation = new Vector2(Random.Range(transform.position.x - 5.0f, transform.position.x + 5.0f), Random.Range(transform.position.y - 5.0f, transform.position.y + 5.0f));
        }
    }

    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "pared")
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    private IEnumerator SetColorBackToWhite() {
        canMove = false;
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }

}
