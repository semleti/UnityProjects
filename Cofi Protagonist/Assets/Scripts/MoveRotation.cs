using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRotation : MonoBehaviour {
    public float movHorizontalMultiplier = 1.0f;
    public float smoothPoints = 5;
    private Vector3 startPostion;
    private List<float> rotations;
	// Use this for initialization
	void Start () {
        startPostion = transform.position;
        rotations = new List<float>();
	}
	
	// Update is called once per frame
	void Update () {
        if (SystemInfo.supportsAccelerometer
                #if UNITY_EDITOR
                    || UnityEditor.EditorApplication.isRemoteConnected
                #endif
            )
            AddRotation(Input.acceleration.x);
        else
            AddRotation(-(0.5f - Input.mousePosition.x / Screen.width));
        transform.position = new Vector3(startPostion.x + GetRotation() * movHorizontalMultiplier, startPostion.y, startPostion.z);
    }

    private void AddRotation(float rot)
    {
        rotations.Add(Mathf.Clamp(rot,-1,1));
        if (rotations.Count > smoothPoints)
            rotations.RemoveAt(0);
    }

    private float GetRotation()
    {
        float rotation = 0;
        foreach (float rot in rotations)
        {
            rotation += rot;
        }
        rotation /= rotations.Count;
        return rotation;
    }
}
