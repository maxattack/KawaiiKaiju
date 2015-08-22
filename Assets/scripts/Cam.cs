using UnityEngine;
using System.Collections;

public class Cam : CustomBehaviour {

	internal static Cam inst;
	internal Vector3 speed;
	internal Vector3 offset;
	
	void Awake() {
		inst = this;
		offset = transform.position;
	}
	
	void OnDestroy() {
		if (inst == this) { inst = null; }
	}
	
	void LateUpdate() {
		var targetPosition = Dino.inst.transform.position + offset;
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref speed, 0.25f);
		
	}
}
