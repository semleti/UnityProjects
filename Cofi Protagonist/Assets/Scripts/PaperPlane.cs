using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlane : BasicMovingObject
{
    private bool used = false;
    public float hurtSpeedMultiplier = 3f;
    public GameObject bonus;

    public void OnMouseDown()
    {
        if (!used)
        {
            Instantiate(bonus, new Vector3(LevelController.instance.GetClosestLanePos(transform.position.x), 8, 0), bonus.transform.rotation, LevelController.instance.root.transform);
            GetComponent<AudioSource>().Play();
            used = true;
            speed.x *= hurtSpeedMultiplier;
        }
    }
}
