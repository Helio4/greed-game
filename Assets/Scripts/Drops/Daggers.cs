using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daggers : CollectibleItem {

    [SerializeField] private int numDaggers = 5;

    public override void Effect(Collider2D collision)
    {
        playerController player = collision.GetComponent<playerController>();
        if (player != null) {
            player.IncreaseDaggers(numDaggers);
            Destroy(gameObject);
        }
    }

}
