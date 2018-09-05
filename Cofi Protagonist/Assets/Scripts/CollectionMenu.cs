using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionMenu : MonoBehaviour {

    

    public CollectionMenuCharacter characterPrefab;
    public LayoutGroup container;

    //only for testing
    //might use smthg similar for production for the shop
    public CollectionMenuCharacter.CharachterData[] characters;

    // Use this for initialization
    void Start () {
		foreach(var c in characters)
        {
            CollectionMenuCharacter v = Instantiate(characterPrefab, container.gameObject.transform);
            v.SetData(c);
        }
	}

}
