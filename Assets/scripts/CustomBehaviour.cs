using UnityEngine;
using System.Collections;

public class CustomBehaviour : MonoBehaviour {
	
	public static Vector2 Vec(float x, float y) { return new Vector2(x, y); }
	public static Vector3 Vec(float x, float y, float z) { return new Vector3(x, y, z); }
	public static Vector3 Vec(float x, Vector2 yz) { return new Vector3(x, yz.x, yz.y); }
	public static Vector3 Vec(Vector2 v, float z) { return new Vector3(v.x, v.y, z); }
	
	
}
