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


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Dino : CustomBehaviour {


	public float maxSpeed = 1f;
	public float maxRotationSpeed = 90f;
	public AudioSource[] footfalls;
	public AudioSource roarAnticipation;
	public AudioSource roar;

	internal static Dino inst;
	internal Animator anim;
	internal Rigidbody body;
	internal SmoothValue linear = new SmoothValue();
	internal SmoothValue angular = new SmoothValue();
	
	static int kSpeed = Animator.StringToHash("Speed");
	static int kRoar = Animator.StringToHash("Roar");
	
	// callbacks

	void Awake() {
		inst = this;
		anim = GetComponent<Animator>();
		body = GetComponent<Rigidbody>();
	}
	
	void OnDestroy() {
		if (inst == this) { inst = null; }
	}
	
	void Update() {
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			roarAnticipation.Play();
			anim.SetTrigger(kRoar);
		}
		
		anim.SetFloat(kSpeed, Mathf.Max (linear.speed, Mathf.Abs(angular.speed/maxRotationSpeed)));
	}
	
	void FixedUpdate () {

		
		var targetSpeed = Input.GetKey(KeyCode.UpArrow) ? maxSpeed : 0f;
		if (anim.GetCurrentAnimatorStateInfo(0).shortNameHash == kRoar)
			targetSpeed = 0f;
		var speed = linear.Update (targetSpeed, 0.15f);
		body.MovePosition(body.position + speed * transform.forward * Time.fixedDeltaTime);
		
		var steering = Input.GetKey(KeyCode.RightArrow) ? maxRotationSpeed : Input.GetKey(KeyCode.LeftArrow) ? -maxRotationSpeed : 0f;
		var angSpeed = angular.Update (steering, 0.15f);
		body.MoveRotation(body.rotation * Quaternion.AngleAxis(angSpeed * Time.fixedDeltaTime, Vector3.up));
		
	}
	
	// animation events
	
	void Footfall() {
		var randomFootfall = Random.Range(0, footfalls.Length);
		footfalls[randomFootfall].Play();
	}
	
	void Roar() {
		roar.Play();
	}
}
