using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBonus : BonusUI {
    public float bombRadius = 2;
    override public void Activate()
    {
        Collider[] things = Physics.OverlapSphere(LevelController.instance.player.transform.position, bombRadius);
        foreach(Collider thing in things)
        {
            if(thing.GetComponent<BasicEnemy>() != null)
            {
                Destroy(thing.gameObject);
            }
        }
        base.Activate();
    }
}
