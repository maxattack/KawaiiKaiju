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
	
	public const int kLayer = 8;
	public const int kLayerBit = (1<<8);
	static int kSpeed = Animator.StringToHash("Speed");
	static int kRoar = Animator.StringToHash("Roar");
	static Quaternion kInputBias = Quaternion.AngleAxis(13.5f, Vector3.up);
	
	public float maxSpeed = 1f;
	public float maxRotationSpeed = 90f;
	public AudioSource[] footfalls;
	public AudioSource roarAnticipation;
	public AudioSource roar;
	public Explosion explosionPrefab;
	public FireBreath fireBreath;
	public Transform lazer;

	internal static Dino inst;
	internal Animator anim;
	internal Rigidbody body;
	internal SmoothValue linear = new SmoothValue();
	internal SmoothValue angular = new SmoothValue();

	// getters

	bool IsRoaring() {
		return anim.GetCurrentAnimatorStateInfo(0).shortNameHash == kRoar;
	}
	
	Vector3 GetMoveInput() {
		
		// TODO: gamepad stick?

		var result = Vector3.zero;
		
		// compute camera-local input
		if (Input.GetKey(KeyCode.W)) {
			result.z = 1f;
		} else if (Input.GetKey (KeyCode.S)) {
			result.z = -1f;
		}
		
		if (Input.GetKey(KeyCode.A)) {
			result.x = -1f;
		} else if (Input.GetKey(KeyCode.D)) {
			result.x = 1f;
		}

		if (result.sqrMagnitude < 0.001f * 0.001f) {
			return Vector3.zero;
		} else {
			result.Normalize();
		}

		// get camera directions
		var camFwd = kInputBias * Cam.inst.transform.forward.x0z().normalized;
		var camRight = kInputBias * Cam.inst.transform.right.x0z().normalized;
		
		return camFwd * result.z + camRight * result.x;
	}
	
	bool LazerTriggered() {
		return Input.GetKeyDown(KeyCode.L);
	}
	
	public float GetNormalizedSpeed() {
		return Mathf.Clamp01(body.velocity.x0z().magnitude / maxSpeed);
	}
	
	// callbacks

	void Awake() {
		inst = this;
		anim = GetComponent<Animator>();
		body = GetComponent<Rigidbody>();
		
		lazer.gameObject.SetActive(false);
	}
	
	void OnDestroy() {
		if (inst == this) { inst = null; }
	}
	
	void Update() {
		
		if (LazerTriggered() && !IsRoaring()) {
			roarAnticipation.Play();
			anim.SetTrigger(kRoar);
		}
		
		anim.SetFloat(kSpeed, Mathf.Max (0.95f * linear.speed/maxSpeed, Mathf.Abs(angular.speed/maxRotationSpeed)));
	}
	
	void FixedUpdate () {

		var targetSpeed = 0.0f;
		var targetRot = 0.0f;
		
		if (!IsRoaring()) {

			// sampling input
			var input = GetMoveInput();
			if (input.sqrMagnitude > 0.01f * 0.01f) {
			
				// compare input and forward to determine how much to accelerate/rotate
				var fwd = transform.forward;
				targetSpeed = Mathf.Max(0f, Vector3.Dot (fwd, input)) * maxSpeed;
				
				Vector3 axis; Quaternion.FromToRotation(fwd, input).ToAngleAxis(out targetRot, out axis);
				targetRot = Mathf.Min(targetRot/0.15f, maxRotationSpeed) * axis.y;
			}
		}
		
		var speed = linear.Update (targetSpeed, 0.15f);
		var angSpeed = angular.Update (targetRot, 0.15f);
		body.velocity = speed * transform.forward + Vec(0, body.velocity.y, 0);
		body.angularVelocity = Vec(0f, Mathf.Deg2Rad * angSpeed, 0f);
	}
	
	// animation events
	
	void Footfall() {
		var randomFootfall = Random.Range(0, footfalls.Length);
		footfalls[randomFootfall].Play();
	}
	
	void Roar() {
		Cam.inst.Shake (0.5f);
		roar.Play();
		fireBreath.Burst();
		StartCoroutine(DoLazer());
	}
	
	IEnumerator DoLazer() {
		lazer.gameObject.SetActive(true);
		var len = -1f;
		foreach(var t in Transition(0.5f)) {
			
			var p0 = fireBreath.transform.position;
			var dist = 20f * EaseOut2(t);
			if (len > 0f) {
				dist = len;		
					
			} else {
			
				// sweep for building hit
				RaycastHit hit;
				var r = 0.25f;
				if (Physics.SphereCast(p0, r, transform.forward, out hit, dist - r, Building.kLayerBit)) {
					var bldg =hit.collider.GetComponentInParent<Building>();
					bldg.Detonate(hit.point);
					StartCoroutine(DoExplosions(hit.point));
					dist = len = hit.distance + r;	
				}
					
			}

			lazer.position = p0;
			lazer.rotation = transform.rotation;
			lazer.localScale = Vec(1f, 1f, dist);
			
			yield return null;
		}
		
		var sz = lazer.localScale.z;
		foreach(var t in Transition (0.25f)) {
			lazer.position = fireBreath.transform.position;
			lazer.rotation = transform.rotation;
			lazer.localScale = Vec(1f-t, 1f-t, sz);
			yield return null;
		}
		lazer.gameObject.SetActive(false);
	}
	
	IEnumerator DoExplosions(Vector3 loc) {
		Cam.inst.Shake();
		Cam.inst.Flash();
	
		var explosion = Dup(explosionPrefab, loc, Quaternion.identity);
		explosion.transform.localScale = Vec(0.75f, 0.75f, 0.75f);
		
		yield return new WaitForSeconds(Random.Range (0.05f, 0.15f));
		
		var smallerExplosion = Dup(explosionPrefab, loc + 0.5f * Random.insideUnitSphere, Quaternion.identity);
		smallerExplosion.transform.localScale = Vec(0.35f, 0.35f, 0.35f);
		smallerExplosion.GetComponent<AudioSource>().volume = 0.75f;
		
		if (Random.value > 0.5f) {
			
			yield return new WaitForSeconds(Random.Range (0.05f, 0.15f));
			
			var evenSmallerExplosion = Dup(explosionPrefab, loc + 0.5f * Random.insideUnitSphere, Quaternion.identity);
			evenSmallerExplosion.transform.localScale = Vec(0.25f, 0.25f, 0.25f);
			evenSmallerExplosion.GetComponent<AudioSource>().volume = 0.75f;
			
		}
	}
}




