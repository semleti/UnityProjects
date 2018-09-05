using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusUI : MonoBehaviour {
    public AudioClip audioClip;
	virtual public void Activate()
    {
        LevelController.instance.player.audioSource.PlayOneShot(audioClip);
        RemoveFromDisplay();
    }

    public void RemoveFromDisplay()
    {
        GameController.instance.bonusDisplay.numOfBonuses--;
        Destroy(gameObject);
    }
}
