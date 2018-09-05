using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : BasicMovingObject {
    public GameObject bonusUI;
    public AudioClip collectAudio;

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            LevelController.instance.player.audioSource.PlayOneShot(collectAudio);
            GameController.instance.bonusDisplay.AddBonus(bonusUI);
            Destroy(gameObject);
        }
    }
}
