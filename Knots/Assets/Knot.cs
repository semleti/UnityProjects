using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Knot : MonoBehaviour {
	public GameObject prefab;
	public static List<Knot> knots = new List<Knot>();
	public List<GameObject> points = new List<GameObject>();
	public bool drawFlatBool = true;
	public bool draw3DCrossingBool = true;
	public bool first3dDraw = true;
	public int indexToDraw = 0;

	// Use this for initialization
	void OnEnable () {

		OnSceneGUICatcher.list.Add (this.DrawOnSceneGUI);
		OnSceneGUICatcher.list.Add (this.DrawOnSceneGUIOptional);
		Knot.knots.Add (this);

		Vector3 outahere;
		Debug.Log (segmentIntersectTriangle (new Vector3(-0.5f,-1.1f,48.5f),new Vector3(12.4f,0,21.3f),new Vector3(-38.2f,0, 0.6f),new Vector3(11.1f,0,5.8f),new Vector3(17.1f,0,-4.4f),out outahere).ToString() +" hj");
	}

	void OnValidate()
	{
		Debug.Log ("changed");
	}

	void OnDestroy()
	{
		OnSceneGUICatcher.list.Remove (this.DrawOnSceneGUI);
		Knot.knots.Remove (this);
	}

	public void DrawOnSceneGUI()
	{
		//Draw here
		for (int i=0;i<points.Count;i++) {
			if(points[i]==null)
			{
				points.RemoveAt(i);
				i--;
			}
			else{
				points[i].name="Point " + (i+1).ToString();
			}
		}
		for (int i=0;i<points.Count;i++) {
			Handles.DrawLine (points[i].transform.position, points[mod ((i+1),points.Count)].transform.position);
		}
	}

	public void DrawOnSceneGUIOptional()
	{
		if(drawFlatBool)
		{
			DrawFlat(this,new Vector3(0,10,0));
		}
		if (draw3DCrossingBool)
		{
			Draw3DCrossing(this, mod(indexToDraw,points.Count));
		}
	}
	//---------------------------------------to finish
	public void Draw3DCrossing(Knot target,int index)
	{
		List<Vector3> newPoints = new List<Vector3>();
		int count = target.points.Count;
		int numberOfIntersections = 0;
		for(int i=0; i<count;i++)
		{
			//Debug.Log ((mod ((index-i),count)).ToString());
			if(mod ((index-i),count)>=2)
			{
				for(int j=0;j<count;j++)
				{
					//Debug.Log ("j");
					if(mod ((index-j),count)>=2 && mod ((i-j),count)>2 && mod ((j-i),count)>2)
					{
						//segmentIntersectTriangle(target.points[j],target.points[mod((j+1),count]),)
						Vector3 intersection = Vector3.zero;
						if(segmentIntersectTriangle(target.points[j].transform.position,target.points[mod ((j+1),count)].transform.position,target.points[index].transform.position,target.points[i].transform.position,target.points[mod ((i+1),count)].transform.position,out intersection))
						{
							numberOfIntersections++;
							Gizmos.color = new Color(100f/255f,150f/255f,100f/255f,100f/255f);
							Gizmos.DrawSphere(intersection,1f);
							Gizmos.color = new Color(100f/255f,100f/255f,150f/255f,50f/255f);
							Mesh mesh = new Mesh ();
							mesh.vertices = new Vector3[]{target.points[index].transform.position,target.points[i].transform.position,target.points[mod ((i+1),count)].transform.position};
							mesh.triangles = new int[]{0,1,2,0,2,1};
							mesh.normals = new Vector3[]{Vector3.zero,Vector3.zero,Vector3.zero};
							//Gizmos.DrawMesh (mesh);
							Gizmos.color = Color.grey;
							//Gizmos.Draw
							/*if(first3dDraw)
							{
								Debug.Log ((j+1).ToString() +" "+(mod ((j+1),count)+1).ToString() +" "+(index+1).ToString() +" "+(i+1).ToString() +" "+(mod ((i+1),count)+1).ToString());
								Debug .Log (target.points[j].transform.position.ToString() +" "+ target.points[mod ((j+1),count)].transform.position.ToString() +" "+ target.points[index].transform.position.ToString() +" "+ target.points[i].transform.position.ToString() +" "+ target.points[mod ((i+1),count)].transform.position.ToString());
							}*/
						}
					}
				}
			}
		}
		if (first3dDraw) {
			Debug.Log ("intersections: " + numberOfIntersections.ToString());
		}
		first3dDraw = false;
	}

	static public void DrawFlat(Knot target, Vector3 observingPoint)
	{
		List<Vector3> newPoints = new List<Vector3>();
		Vector3 center = target.transform.position;
		Vector3 distance = observingPoint - center;
		/*Debug.Log ("normal: " + normal.ToString ());
		Debug.Log ("observing point: " + observingPoint.ToString ());
		Debug.Log ("normal normalized: " + normal.normalized.ToString ());
		Debug.Log ("center: " + center.ToString ());*/
		foreach(GameObject point in target.points)
		{
			newPoints.Add(Vector3.ProjectOnPlane(point.transform.position,distance)+observingPoint);
			//Debug.Log ( newPoints[newPoints.Count-1].ToString()+" dot: " + (-Vector3.Dot(normal.normalized, (point.transform.position - observingPoint))).ToString());
		}

		Mesh mesh = new Mesh ();
		mesh.vertices = new Vector3[]{Vector3.ProjectOnPlane(new Vector3(-10,0,-10)+center,distance)+observingPoint,Vector3.ProjectOnPlane(new Vector3(-10,0,10)+center,distance)+observingPoint,Vector3.ProjectOnPlane(new Vector3(10,0,-10)+center,distance)+observingPoint,Vector3.ProjectOnPlane(new Vector3(10,0,10)+center,distance)+observingPoint};
		mesh.triangles = new int[]{0,1,2,1,3,2};
		mesh.normals = new Vector3[]{observingPoint,observingPoint,observingPoint,observingPoint};
		Gizmos.DrawMesh (mesh);

		Gizmos.color = Color.red;
		for (int i=0;i<newPoints.Count;i++) {
			//Debug.Log("draw red line " + newPoints[i].ToString() +" " + newPoints[mod((i+1),newPoints.Count)].ToString());
			Gizmos.DrawLine (newPoints[i], newPoints[mod ((i+1),newPoints.Count)]);

		}
		Gizmos.color = Color.white;

		int index = 0;
		foreach (Vector3 point in newPoints)
		{
			int index2 = 0;
			foreach(Vector3 point3 in newPoints)
			{
				if(mod ((index2-index),newPoints.Count)>=2)
				{
					Vector3 intersectionPoint;
					if(intersectionSegments(newPoints[index],newPoints[mod ((index+1),newPoints.Count)],newPoints[index2],newPoints[mod ((index2+1),newPoints.Count)],out intersectionPoint))
					{
						Gizmos.DrawSphere(intersectionPoint,.1f);
					}
					else
					{
						//Gizmos.DrawSphere(intersectionPoint,.1f);
					}
				}
				index2++;
			}
			index++;
		}



	}

	static public bool intersectionLines(Vector3 a,Vector3 b,Vector3 A,Vector3 B, out Vector3 intersection)
	{
		Vector3 ab = b - a;
		Vector3 AB = B - A;
		
		float k = Vector3.Cross(A-a,AB).magnitude/Vector3.Cross(ab,AB).magnitude ;
		float j = Vector3.Cross(a-A,ab).magnitude/Vector3.Cross(AB,ab).magnitude ;
		
		intersection = ((a + k * ab) + (A + j * AB))/2;
		if (((a + k * ab) - (A + j * AB)).magnitude <= 0.0001f) {
			return true;
		}
		return false;
	}

	static public bool intersectionSegments(Vector3 a,Vector3 b,Vector3 A,Vector3 B, out Vector3 intersection)
	{
		if (intersectionLines(a,b,A,B,out intersection)) {
			/*if(((a-b).magnitude>(a-intersection).magnitude)&&((b-a).magnitude>(b-intersection).magnitude)&&((A-B).magnitude>(A-intersection).magnitude)&&((B-A).magnitude>(B-intersection).magnitude))
			{
				//Gizmos.DrawSphere (intersection, .1f);
				return true;
			}*/
			if((((intersection-(a+b)/2)).magnitude<(a-b).magnitude/2) && (((intersection-(A+B)/2)).magnitude<(A-B).magnitude/2))
			{
				return true;
			}
		}

		return false;
	}

	static public bool lineIntersectPlane(Vector3 pointA, Vector3 pointB, Vector3 planePoint, Vector3 planeNormal, out Vector3 intersection)
	{
		intersection = new Vector3(-123,456,-789);
		if(Vector3.Dot(planeNormal,(pointB-pointA)) != 0)
		{
			//float s = -(planeNormal.x*pointA.x + planeNormal.y*pointA.y + planeNormal.z*pointA.z)/(Vector3.Dot(planeNormal,(pointB-pointA)));
			float s = Vector3.Dot ((planePoint-pointA),planeNormal)/Vector3.Dot ((pointB-pointA),planeNormal);
			intersection = pointA + (pointB - pointA) * s;
			return true;
		}
		return false;
	}

	static public bool segmentIntersectPlane(Vector3 pointA, Vector3 pointB, Vector3 planePoint, Vector3 planeNormal, out Vector3 intersection)
	{
		/*intersection = new Vector3(-123,456,-789);
		if(Vector3.Dot(planeNormal,(pointB-pointA)) != 0)
		{
			//float s = -(planeNormal.x*pointA.x + planeNormal.y*pointA.y + planeNormal.z*pointA.z)/(Vector3.Dot(planeNormal,(pointB-pointA)));
			float s = Vector3.Dot ((-planePoint-pointA),planeNormal)/Vector3.Dot ((pointB-pointA),planeNormal);
			intersection = pointA + (pointB - pointA) * s;
			if (0 < s && s < 1) {
				return true;
			}
		}
		return false;*/
		if(lineIntersectPlane(pointA, pointB, planePoint, planeNormal, out intersection))
		{
			if((intersection-pointA).magnitude<(pointB-pointA).magnitude)
			{
				return true;
			}
		}
		return false;
	}

	public static bool lineIntersectTriangle(Vector3 segmentA, Vector3 segmentB, Vector3 triangleA, Vector3 triangleB, Vector3 triangleC, out Vector3 intersection)
	{
		intersection = Vector3.zero;
		return false;
	}

	public static bool segmentIntersectTriangle(Vector3 segmentA, Vector3 segmentB, Vector3 triangleA, Vector3 triangleB, Vector3 triangleC, out Vector3 intersection)
	{
		Vector3 outA;
		Vector3 outB;
		Vector3 outC;
		if(segmentIntersectPlane(segmentA, segmentB, triangleA, Vector3.Cross(triangleB-triangleA,triangleC-triangleA),out intersection))
		{
			if((intersectionLines(triangleA,intersection,triangleB,triangleC, out outA)&&((triangleA-intersection).magnitude>(triangleA-outA).magnitude)) || (intersectionLines(triangleB,intersection, triangleC, triangleA, out outB) && ((triangleB-intersection).magnitude>(triangleB-outB).magnitude)) || (intersectionLines(triangleC, intersection, triangleA, triangleB, out outC) && ((triangleC-intersection).magnitude>(triangleC-outC).magnitude)))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public static int mod(int x, int m) {
		int r = x%m;
		return r<0 ? r+m : r;
	}

}
