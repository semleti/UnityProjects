using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour {
    public int currentLives = 0;
    public GameObject heart;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    //necessary to differentiate setting in the editor and gameobjects currently attached
    public int editorMaxLives = 3;
    [HideInInspector]
    public int maxLives = 0;

	// Use this for initialization
	void Start () {
        setMaxLives(editorMaxLives);
        //weird bug requires HUD canvas to be enabled by default to ever render hearts :/
        GameController.instance.HUD.GetComponent<Canvas>().enabled = false;
        currentLives = editorMaxLives;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //only used at start, maybe also ingame thanks to bonus/differet gamemode...
    public void setMaxLives(int lives)
    {
        for(int i = maxLives; i < lives; i++)
        {
            Instantiate(heart).transform.SetParent(gameObject.transform);
        }
        for(int i = maxLives; i > lives; i--)
        {
            Destroy(transform.GetChild(i - 1).gameObject);
        }
        maxLives = lives;
        if (currentLives > maxLives)
            currentLives = maxLives;
    }

    public void setLives(int lives)
    {
        if (lives != currentLives)
        {
            Sprite newSprite = lives > currentLives ? fullHeart : emptyHeart;
            for (int i = Mathf.Max(Mathf.Min(lives, currentLives-1),0); i < Mathf.Max(lives, currentLives); i++)
                transform.GetChild(i).GetComponent<Image>().overrideSprite = newSprite;
            currentLives = lives;
        }
    }
}
