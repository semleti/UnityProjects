using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCounter : MonoBehaviour {
    public string textLeft;
    public string textRight;
    private int _value = 0;
    private Text textField;
    public int value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            textField.text = textLeft + _value + textRight;
        }
    }
    // Use this for initialization
    void Start()
    {
        textField = GetComponent<Text>();
    }
}
