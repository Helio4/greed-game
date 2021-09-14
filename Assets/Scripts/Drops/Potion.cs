using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : CollectibleItem {

    [SerializeField] private int restoredHealth = 50;

    public override void Effect(Collider2D collision)
    {
        playerController player = collision.GetComponent<playerController>();
        if (player != null) {
            player.RestoreHealth(restoredHealth);
            Destroy(gameObject);
        }
    }

}

