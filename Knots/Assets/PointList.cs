using UnityEngine;
using System;
using System.Collections.Generic; // Import the System.Collections.Generic class to give us access to List<>

public class PointList : MonoBehaviour {

	//This is our list we want to use to represent our class as an array.
	public List<GameObject> list = new List<GameObject>();

	public void Add(int index){
		//Add a new index position to the end of our list
		GameObject newPoint = new GameObject ();
		newPoint.transform.position = list [(index - 1) % list.Count].transform.position;
		newPoint.AddComponent (typeof(Point));
		list.Insert(index,newPoint);
	}
	
	public void Remove(int index){
		//Remove an index position from our list at a point in our list array
		list.RemoveAt(index);
	}
}