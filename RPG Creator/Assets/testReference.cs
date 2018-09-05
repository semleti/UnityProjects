using UnityEngine;
using System.Collections;

public class testReference : MonoBehaviour {

	public int textInt;
	public int target;
	public int intAccessor 
	{
		get 
		{ 
			return target; 
		}
		set 
		{
			target = value; 
		}
	}
	public delegate int testInt2( int value);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
