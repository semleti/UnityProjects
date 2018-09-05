using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
	private float rotAngle = 0;
	private Vector2 pivotPoint;
	void OnGUI() {
		pivotPoint = new Vector3(Screen.width / 2, Screen.height / 2, 1000);
		GUIUtility.RotateAroundPivot(rotAngle, pivotPoint);
		if (GUI.Button(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 50, 50, 50), "Rotate+"))
			rotAngle += 10;
		pivotPoint = new Vector3(Screen.width / 2, Screen.height / 2, 1000);
		GUIUtility.RotateAroundPivot(0, pivotPoint);
		if (GUI.Button(new Rect(Screen.width / 2 - 25, Screen.height / 2 + 0, 50, 50), "Rotate-"))
			rotAngle -= 10;
		
	}
}