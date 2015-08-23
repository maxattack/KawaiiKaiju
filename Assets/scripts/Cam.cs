using UnityEngine;
using System.Collections;

public class Cam : CustomBehaviour {

	internal static Cam inst;
	internal Vector3 speed;
	internal Vector3 offset;
	
	Vector3 prevDinoLoc;
	
	void Awake() {
		inst = this;
		offset = transform.position;
	}
	
	void Start() {
		prevDinoLoc = Dino.inst.transform.position;
	}
	
	void OnDestroy() {
		if (inst == this) { inst = null; }
	}
	
	void LateUpdate() {
		
		// leash rotation
		var camLoc = Camera.main.transform.position;
		var nextDinoLoc = Dino.inst.transform.position;
		var prevOffset = (prevDinoLoc - camLoc).x0z();
		var nextOffset = (nextDinoLoc - camLoc).x0z();
		prevDinoLoc = nextDinoLoc;
		var rot = Quaternion.FromToRotation(prevOffset, nextOffset).eulerAngles.y;
		transform.Rotate(0, rot, 0, Space.World);
		
		// smooth tracking
		transform.position = Vector3.SmoothDamp(transform.position, nextDinoLoc + offset, ref speed, 0.25f);
		
	}
}
