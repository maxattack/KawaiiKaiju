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
		
		// smooth tracking
		var prevTrackingLoc = transform.position;
		var nextTrackingLoc = Vector3.SmoothDamp(transform.position, Dino.inst.transform.position + offset, ref speed, 0.25f); 
		transform.position = nextTrackingLoc;

		// leash rotation
		var camLoc = Camera.main.transform.position;
		var prevOffset = (prevTrackingLoc - camLoc).x0z();
		var nextOffset = (nextTrackingLoc - camLoc).x0z();
		var rot = Quaternion.FromToRotation(prevOffset, nextOffset).eulerAngles.y;
		transform.Rotate(0, rot, 0, Space.World);
		
		
	}
}
