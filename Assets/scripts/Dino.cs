using UnityEngine;
using System.Collections;

public struct SmoothValue {
	
	internal float speed;
	internal float accel;
	
	public float Update(float target, float time)
	{
		speed = Mathf.SmoothDamp(speed, target, ref accel, time);
		return speed;
	}
}


[RequireComponent(typeof(Rigidbody))]
public class Dino : CustomBehaviour {


	public float maxSpeed = 1f;
	public float maxRotationSpeed = 90f;

	internal static Dino inst;
	internal Rigidbody body;
	internal SmoothValue linear = new SmoothValue();
	internal SmoothValue angular = new SmoothValue();
	
	// callbacks

	void Awake() {
		inst = this;
		body = GetComponent<Rigidbody>();
	}
	
	void OnDestroy() {
		if (inst == this) { inst = null; }
	}
	
	void Update() {
		
	}
	
	void FixedUpdate () {

		var targetSpeed = Input.GetKey(KeyCode.UpArrow) ? maxSpeed : Input.GetKey(KeyCode.DownArrow) ? -maxSpeed : 0f;
		var speed = linear.Update (targetSpeed, 0.15f);
		body.MovePosition(body.position + speed * transform.forward * Time.fixedDeltaTime);
		
		var steering = Input.GetKey(KeyCode.RightArrow) ? maxRotationSpeed : Input.GetKey(KeyCode.LeftArrow) ? -maxRotationSpeed : 0f;
		var angSpeed = angular.Update (steering, 0.15f);
		body.MoveRotation(body.rotation * Quaternion.AngleAxis(angSpeed * Time.fixedDeltaTime, Vector3.up));
		
	}
}
