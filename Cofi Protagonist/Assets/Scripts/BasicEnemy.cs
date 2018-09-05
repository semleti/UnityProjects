using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : BasicMovingObject {

    public void OnTriggerEnter(Collider collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            OnPlayerTriggerEnter(player);
        }
    }

    virtual public void OnPlayerTriggerEnter(PlayerController player)
    {
        player.LooseLive();
        Destroy(gameObject);
        
    }
}
