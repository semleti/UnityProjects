using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBonus : BonusUI {

    public override void Activate( )
    {
        LevelController.instance.player.AddLive();
        base.Activate();
    }
}
