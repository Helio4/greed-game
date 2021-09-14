using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : CollectibleItem {

    [SerializeField] private int score = 100;

    public override void Effect(Collider2D collision)
    {
        if (collision.tag == "Player") {
            Messenger<int>.Broadcast(GameEvent.GOLD_GOTTEN, score);
            Destroy(gameObject);
        }
    }

}
