using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerController : MonoBehaviour {

    Rigidbody2D _rigidbody2D;
    [SerializeField] private float _speed = 10.0f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip daggerSound;

	// Use this for initialization
	void Start () {
        _rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        _rigidbody2D.velocity = transform.up * _speed;    	
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnHit(5);
            audioSource.PlayOneShot(daggerSound, SoundOptions.GetSFXVolume());
        } else if (other.tag == "pared")
        {
            Destroy(gameObject);
        }
        
    }
}
