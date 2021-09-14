using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip swordSound;

    // Use this for initialization
    void Awake () {
        StartCoroutine("DestroySword");	
	}

    private IEnumerator DestroySword() {
        audioSource.PlayOneShot(swordSound, SoundOptions.GetSFXVolume());
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
