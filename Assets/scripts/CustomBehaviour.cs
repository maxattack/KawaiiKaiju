using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomBehaviour : MonoBehaviour {
	
	public static Vector2 Vec(float x, float y) { return new Vector2(x, y); }
	public static Vector3 Vec(float x, float y, float z) { return new Vector3(x, y, z); }
	public static Vector3 Vec(float x, Vector2 yz) { return new Vector3(x, yz.x, yz.y); }
	public static Vector3 Vec(Vector2 v, float z) { return new Vector3(v.x, v.y, z); }
	
	public static IEnumerable<float> Transition(float duration) {
		var k = 1f / duration;
		for(var t=0f; t<duration; t+=Time.deltaTime) {
			yield return k * t;
		}
		yield return 1f;
	}
	
	
	// Some easing functions
	public static float Parabola(float x) { return 1f - (x=1f-x-x)*(x); }
	public static float ParabolaDeriv(float x) { return 4f*(1f-x-x); }
	public static float EaseIn2(float u) { return u*u; }
	public static float EaseIn4(float u) { return u*u*u*u; }
	public static float EaseOut2(float u) { return 1f-(u=1f-u)*u; }
	public static float EaseOut4(float u) { return 1f-(u=1f-u)*u*u*u; }
	
	public static float Modf(float x, float radix) {
		return ((x % radix) + radix) % radix;
	}
	
	public static float EaseInOutBack(float t)  {
		var v = t + t;
		var s = 1.70158f * 1.525f;
		if (v < 1.0f) {
			return 0.5f * (v * v * ((s + 1.0f) * v - s));
		} else {
			v -= 2.0f;
			return 0.5f * (v * v * ((s + 1.0f) * v + s) + 2.0f);
		}
	}
	
	public static float EaseOutBack(float t) { 
		t-=1.0f; 
		return t*t*((1.70158f+1.0f)*t + 1.70158f) + 1.0f; 
	}
	
	public static Vector3 Lerp(Vector3 v0, Vector3 v1, float u) {
		return v0 + u * (v1 - v0);
	}
	
	public static float Expovariate(float dt, float uMin=0.05f, float uMax=0.95f) { 
		return -dt * Mathf.Log(1f-Random.Range(uMin,uMax)); 
	}
	
	public static T Dup<T> (T obj) where T : Object { return Instantiate(obj) as T; }
	public static T Dup<T> (T obj, Vector3 pos) where T : Object { return Instantiate(obj, pos, Quaternion.identity) as T; }
	public static T Dup<T> (T obj, Vector3 pos, Quaternion q) where T : Object {  return Instantiate(obj, pos, q) as T; }
	
	public static Color RGB(float r, float g, float b) { return new Color(r, g, b); }
	public static Color RGBA(float r, float g, float b, float a) { return new Color(r, g, b, a); }
	public static Color RGBA(Color c, float a) { return new Color(c.r, c.g, c.b, a); }	
}
