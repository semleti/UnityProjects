using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBonus : BonusUI {
    public float invulnerabilityDuration = 3f;
    public override void Activate()
    {
        Debug.Log("Shield");
        LevelController.instance.player.invulnerableEndTime = Time.time + invulnerabilityDuration;
        base.Activate();
    }
}
