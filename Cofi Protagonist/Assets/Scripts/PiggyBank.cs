using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggyBank : BasicMovingObject
{

    public float coinCooldown = 1f;
    private float lastCoinTimer = 0f;
    public float hurtSpeedMultiplier = 1.2f;
    public int numberOfCoins = 10;
    public GameObject coin;
    private AudioSource source;

    public void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void OnMouseDown()
    {
        if(numberOfCoins > 0 && lastCoinTimer + coinCooldown < Time.time)
        {
            lastCoinTimer = Time.time;
            numberOfCoins--;
            Instantiate(coin, new Vector3(LevelController.instance.GetClosestLanePos(transform.position.x), 8, 0), coin.transform.rotation, LevelController.instance.root.transform);
            Debug.Log("oink");
            source.pitch = Random.Range(0.9f, 1.2f);
            source.Play();
            speed.x *= hurtSpeedMultiplier;
        }
        if(numberOfCoins == 0)
        {
            source.Play();
            source.Play(1);
            source.Play(2);
            Debug.Log("oink oink oink !!!");
            Destroy(this.gameObject);
        }
    }
}
