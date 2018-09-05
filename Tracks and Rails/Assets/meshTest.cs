using UnityEngine;
using System.Collections;

public class meshTest : MonoBehaviour {
    public Mesh mesh;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnValidate()
    {
        Debug.Log("changed");
    }
}
