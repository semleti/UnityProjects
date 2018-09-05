using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour {
    private float endTime;
    private Text counter;

	// Use this for initialization
	void Start () {
        counter = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        counter.text = (System.Math.Round(Mathf.Max(endTime - Time.time,0),2)).ToString();
	}

    public void StartTimer(float seconds)
    {
        endTime = Time.time + seconds;
    }
}
