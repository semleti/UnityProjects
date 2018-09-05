using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : BasicMovingObject {
    public AudioClip audioClip;

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            LevelController.instance.player.audioSource.PlayOneShot(audioClip);
            GameController.instance.coinCounter.value++;
            Destroy(gameObject);
        }
    }
}
