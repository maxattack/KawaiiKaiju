using UnityEngine;
using System.Collections;

public static class CustomExtensions {

	public static Vector2 yx(this Vector2 v) { return new Vector2(v.y, v.x); }
	public static Vector2 xy(this Vector3 v) { return new Vector3(v.x, v.y); }
	public static Vector2 yx(this Vector3 v) { return new Vector3(v.y, v.x); }
	public static Vector2 xz(this Vector3 v) { return new Vector3(v.x, v.z); }
	public static Vector2 yz(this Vector3 v) { return new Vector3(v.y, v.z); }
	public static Vector2 zx(this Vector3 v) { return new Vector3(v.z, v.x); }
	public static Vector2 zy(this Vector3 v) { return new Vector3(v.z, v.y); }
	public static Vector3 xy0(this Vector3 v) { return new Vector3(v.x, v.y, 0); }
	public static Vector3 x0z(this Vector3 v) { return new Vector3(v.x, 0, v.z); }
	
	

}
