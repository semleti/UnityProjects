using UnityEngine;
using System.Collections;

public interface ToolInterface
{
	Texture Icon {get;set;}
	void execute();
}

public class Tool1 :  ToolInterface {
	
	public Texture icon;
	
	
	void ToolInterface.execute()
	{
		Debug.Log("tool1 is working");
	}
	
	Texture ToolInterface.Icon
	{
		get
		{
			return icon;
		}
		set
		{
			icon = value;
		}
	}
}
