using UnityEngine;
using System.Collections.Generic;

public class ToolBoxScriptableObject : ScriptableObject {
	private ToolBox ToolBoxReference = null;
	public List<int> test = new List<int>(){1,2,3,5,6,7};
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setToolBoxReference(ToolBox reference)
	{
		ToolBoxReference = reference;
	}
	
	public ToolBox getToolBoxReference()
	{
		return ToolBoxReference;
	}
}
