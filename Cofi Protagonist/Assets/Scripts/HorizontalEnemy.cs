using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalEnemy : BasicEnemy
{
    private float _direction;
    public float direction
    {
        get{
            return _direction;
        }
        set
        {
            _direction = value;
            speed.x *= _direction;
        }
    }
}
