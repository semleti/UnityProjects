using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovingObject : MonoBehaviour {
    public Vector2 speed;
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = transform.position;
        newPos.x += speed.x * LevelController.instance.currentRound.gameSpeed * Time.deltaTime;
        newPos.y -= speed.y * LevelController.instance.currentRound.gameSpeed * Time.deltaTime;
        transform.position = newPos;
    }
}
