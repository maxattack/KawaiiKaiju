using UnityEngine;
using System.Collections;

public class Logo : CustomBehaviour {
	
	public Transform wobbler;
	public GameObject miscText;
	
	internal static Logo inst;
	
	
	
	void Awake() {
		inst = this;
	}
	
	void OnDestroy() {
		if (inst == this) { inst = null; }
	}
	
	public void Clear() {
		StartCoroutine(DoClear());
	}
	
	void Update() {
		wobbler.localRotation = Quaternion.Euler(5f * Mathf.Sin (Time.time), 0f, 0f);
	}
	
	IEnumerator DoClear() {
		Destroy(miscText);
		var p0 = transform.position;
		foreach(var t in Transition (1f)) {
			transform.position = p0 + Vec(0, EaseIn2(t) * 5.0f, 0f);
			yield return null;
		}
		Destroy(gameObject);
	}
	
}
