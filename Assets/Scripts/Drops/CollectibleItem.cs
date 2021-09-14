using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectibleItem : MonoBehaviour {

    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(pickupSound != null && collision.tag == "Player") AudioSource.PlayClipAtPoint(pickupSound, transform.position, SoundOptions.GetSFXVolume());
        Debug.Log(pickupSound);
        Effect(collision);
    }

    public abstract void Effect(Collider2D collision);
}
