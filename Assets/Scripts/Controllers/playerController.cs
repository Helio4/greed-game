
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    public float speedX = 4.5f;
    public float speedY = 4.5f;

    private int numMaxDaggers = 99;
    private Rigidbody2D _body;
    private Animator _anim;

    public float attackCooldown = 0.1f;
    private bool daggerInCooldown = false;
    private bool meleeInCooldown = false;
    private bool canMove = true;
    public float knockDownForce = 10.0f;

   
    private bool alive = true;
    private bool isInvincible = false;
    public float invicibilityTime = 1.0f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private Camera _camera;
    [SerializeField] GameObject _dagger;
    [SerializeField] GameObject _sword;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip swordHit;
    [SerializeField] AudioClip daggerThrow;
    [SerializeField] AudioClip footsteps;

    private Transform _transform;
    private LayerMask selectObjectsToHit;

    private bool isMoving = false;

    [SerializeField] private int maxHealth = 100;

     //Cambio private a public para poder acceder al game over
    public int _health;

    //UI componentes
    public Slider healthSlider;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f,0f,0f,0.1f);
    bool damaged;

    public static int numDaggers;
    
    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _transform = GetComponent<Transform>();
        _health = maxHealth;
        alive = true;
        canMove = true;
        Debug.Log("Health: " + _health);
        numDaggers = 16;
        DaggerManager.daggers = numDaggers; //UI TEXT
        InvokeRepeating("PlayFootsteps", 0.0f, 0.5f);
    }

    private void OnMasterChanged(float newVolume) {
        AudioListener.volume = newVolume;
    }

    //Recibir daño y mostrar por consola
    public void Hurt(int damageReceived)
    {
        if (isInvincible) return;
        _health -= damageReceived;
        //Esto es de la UI
        damaged = true;            
        healthSlider.value = _health;
        
        Debug.Log("Health: " + _health);
        if (_health <= 0) {
            Messenger.Broadcast(GameEvent.GAME_OVER);
            alive = false;
        } else
        {
            StartCoroutine("AfterEnemyHitRoutine");  
        }
    }

    public void RestoreHealth(int healthRestored) {
        _health += healthRestored;
        _health = Mathf.Min(_health, maxHealth);
        _health = Mathf.Max(_health, 1);
        //Update UI
        Debug.Log("Health: " + _health);
        healthSlider.value = _health;
    }

    private void PlayFootsteps()
    {
        if (isMoving && canMove)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(footsteps, SoundOptions.GetSFXVolume());
        }
    }

    public void IncreaseDaggers(int daggersGotten) {
        numDaggers += daggersGotten; 
        numDaggers = Mathf.Min(numDaggers, numMaxDaggers);
        numDaggers = Mathf.Max(numDaggers, 0);
        //Update UI
        Debug.Log("Daggers: " + numDaggers);
        DaggerManager.daggers = numDaggers; //UI TEXT
    }

    void Update()
    {
        _camera.transform.position = new Vector3(transform.position.x, transform.position.y, _camera.transform.position.z);
        if (!alive) return;

		//Cuando se pausa el juego, el jugador pasa a estar muerto y no moverse
        /*
        if(PauseMenu.GameIsPaused)
        {
            alive = false;
        }
        else 
        {
            alive = true;
        }
        */
        
        //Attack
        if(PauseMenu.GameIsPaused == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack("Melee");
            }
            else if (Input.GetMouseButtonDown(1)) {
                Attack("Range");
            }
        }
        //Movement
        Move();
        
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    private void Move()
    {
        if (!canMove) {
            isMoving = false;
            return;
        }

        float deltaX = Input.GetAxis("Horizontal") * speedX;
        float deltaY = Input.GetAxis("Vertical") * speedY;

        if (_anim != null)
        {
            _anim.SetFloat("speedX", deltaX); //Mathf.Abs(deltaX)
            _anim.SetFloat("speedY", deltaY); //Mathf.Abs(deltaY)
        }

        //pruebas diagonal
        if (deltaX < 0.2 && deltaY < 0.2)
        {
            _anim.SetBool("Ambas", true);
        }

        if (deltaX > 0.2 && deltaY < 0.2)
        {
            _anim.SetBool("Ambas", true);
        }

        if (deltaX > 0.2 && deltaY > 0.2)
        {
            _anim.SetBool("Ambas", true);
        }

        if (deltaX < 0.2 && deltaY > 0.2)
        {
            _anim.SetBool("Ambas", true);
        }

        Vector2 movement = new Vector2(deltaX, deltaY);
        if (movement.magnitude > 2.0f) isMoving = true;
        else isMoving = false;
        _body.velocity = movement;
    }

    private void Attack(string attackType) {
        Vector3 attackDirection = GetMouseDirection();

        if (attackType == "Melee")
        {
            if (!meleeInCooldown) {
                StartCoroutine("MeleeAttack", attackDirection);
                SetAttackAnimation(attackDirection);
            }
        }
        else if (attackType == "Range") {
            if (numDaggers > 0 && !daggerInCooldown) {
                StartCoroutine("ThrowDagger", attackDirection);
                SetAttackAnimation(attackDirection);
            }
        }
    }

    private Vector3 GetMouseDirection() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f;
        Vector3 mouseDirection = (_camera.ScreenToWorldPoint(mousePos) - _transform.position).normalized;
        return mouseDirection;
    }

    private IEnumerator ThrowDagger(Vector3 attackDirection) {
        canMove = false;
        _body.velocity = new Vector2(0.0f, 0.0f);
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        Instantiate(_dagger, new Vector3(attackDirection.x + _transform.position.x, attackDirection.y + _transform.position.y) , Quaternion.AngleAxis(angle - 90.0f, Vector3.forward));
        audioSource.pitch = 1;
        audioSource.PlayOneShot(daggerThrow, SoundOptions.GetSFXVolume());
        daggerInCooldown = true;
        numDaggers--;
        //Update UI
        Debug.Log(numDaggers);
        DaggerManager.daggers = numDaggers; //UI TEXT
        yield return new WaitForSeconds(attackCooldown);
        daggerInCooldown = false;
        canMove = true;
    }

    private IEnumerator MeleeAttack(Vector3 attackDirection)
    {
        meleeInCooldown = true;
        canMove = false;
        _body.velocity = new Vector2(0.0f, 0.0f);
        Collider2D[] objectsHit = Physics2D.OverlapBoxAll(new Vector2(attackDirection.x + _transform.position.x, attackDirection.y + _transform.position.y), new Vector2(1.0f, 1.0f), 0.0f);
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        Instantiate(_sword, new Vector3(attackDirection.x + _transform.position.x, attackDirection.y + _transform.position.y), Quaternion.AngleAxis(angle, Vector3.forward));
        if (objectsHit.Length > 0)
        {
            bool hasHit = false;
            foreach (Collider2D hit in objectsHit)
            {
                if (hit.tag == "Enemy")
                {
                    hasHit = true;
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null) enemy.OnHit(_damage);
                    Rigidbody2D rigidbody2D = hit.GetComponent<Rigidbody2D>();
                    if (rigidbody2D != null) rigidbody2D.velocity = attackDirection * knockDownForce;
                }
            }
            if (hasHit) {
                audioSource.pitch = 1;
                audioSource.PlayOneShot(swordHit, SoundOptions.GetSFXVolume());
            }
        }
        yield return new WaitForSeconds(attackCooldown);
        meleeInCooldown = false;
        canMove = true;
    }

    private IEnumerator AfterEnemyHitRoutine() {
        isInvincible = true;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.6f);
        yield return new WaitForSeconds(invicibilityTime);
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1f);
        isInvincible = false;
    }

    private void SetAttackAnimation(Vector3 attackDirection) {
        if (Mathf.Abs(attackDirection.x) >= Mathf.Abs(attackDirection.y))
        {
            if (attackDirection.x >= 0)
            {
                _anim.SetTrigger("attackRight");
            }
            else
            {
                _anim.SetTrigger("attackLeft");
            }
        }
        else
        {
            if (attackDirection.y >= 0)
            {
                _anim.SetTrigger("attackUp");
            }
            else
            {
                _anim.SetTrigger("attackDown");
            }
        }
    }
}
