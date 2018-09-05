using UnityEngine;
using System.Collections;

public class ID{
	public int id;
	public static bool operator == (ID id1, ID b) 
	{
		return (id1.id == b.id );

	}
	public static bool operator != (ID id1, ID b) 
	{
		return (id1.id != b.id);
	}
}
