using UnityEngine;
using System.Collections;

public class Cam : CustomBehaviour {

	internal static Cam inst;
	internal Vector3 speed;
	internal Vector3 offset;
	
	internal float shakeIntensity = 0f;
	internal float shakeTime = -1f;
	internal Vector2 shakePhase;
	
	internal SpriteRenderer flashCard;
	internal float flashTime = -1f;
	internal float flashDuration = 0f;
	
	public void Shake(float intensity = 1f) {
		shakeIntensity = intensity;
		shakeTime = Time.time;
		shakePhase.x = Mathf.PI * Random.value;
		shakePhase.y = Mathf.PI * Random.value;
	}
	
	public void Flash(float duration = 0.25f) {
		flashTime = Time.time;
		flashDuration = duration;
		flashCard.enabled = true;
		flashCard.color = RGBA(Color.white, 1f);
	}
	
	void Awake() {
		inst = this;
		offset = transform.position;
		flashCard = GetComponentInChildren<SpriteRenderer>();
	}
	
	void OnDestroy() {
		if (inst == this) { inst = null; }
	}
	
	void LateUpdate() {
		
		var cam = Camera.main.transform;
		
		if (Logo.inst == null) {
			
			// smooth tracking of position just ahead of dino
			var prevTrackingLoc = transform.position;
			var targetLoc = Dino.inst.transform.position + 0.75f * Dino.inst.transform.forward;
			var nextTrackingLoc = Vector3.SmoothDamp(transform.position, targetLoc + offset, ref speed, 0.35f); 
			transform.position = nextTrackingLoc;
	
			// leash rotation
			var noShake = cam.parent;
			var camLoc = noShake.position;
			var prevOffset = (prevTrackingLoc - camLoc).x0z();
			var nextOffset = (nextTrackingLoc - camLoc).x0z();
			var rot = Quaternion.FromToRotation(prevOffset, nextOffset).eulerAngles.y;
			transform.Rotate(0, rot, 0, Space.World);
			
		}
		
		// shake
		if (shakeTime > 0f) {
			var duration = 0.75f;
			var dt = (Time.time - shakeTime) / duration;
			if (dt > 1.0f) {
				cam.localPosition = Vector3.zero;
				shakeTime = -1f;
			} else {
				var a = Mathf.SmoothStep(shakeIntensity, 0.0f, dt);
				var ax = a * 0.1666f * Mathf.Cos (shakePhase.x + 8.1f * Mathf.PI * dt);
				var ay = a * 0.1666f * Mathf.Cos (shakePhase.y + 9.7234098f * Mathf.PI * dt);
				cam.localPosition = Vec(ax, ay, 0f);
			}
				
		}
		
		// flash
		if (flashTime > 0f) {
			var dt = (Time.time - flashTime) / flashDuration;
			if (dt > 1.0f) {
				flashCard.enabled = false;
			} else {
				flashCard.color = RGBA(Color.white, 1f - EaseIn2(dt));
			}
		}
	}
}
