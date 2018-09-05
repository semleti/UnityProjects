using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionMenuCharacter : MonoBehaviour {

    [System.Serializable]
    public class CharachterData
    {
        public Sprite sprite;
        public string name;
    }

    public Image img;
    public Text txt;

    public void SetData(CharachterData data)
    {
        img.sprite = data.sprite;
        txt.text = data.name;
    }

}
