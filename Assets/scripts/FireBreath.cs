using UnityEngine;
using System.Collections;

public class FireBreath : CustomBehaviour {
	
	public ParticleSystem mainParticles;
	public ParticleSystem secondaryParticles;
	float mainEmission;
	
	
	void Awake() {
		mainEmission = mainParticles.emissionRate;
	}
	
	public void Burst() {
		StartCoroutine(DoBurst());
	}
	
	IEnumerator DoBurst() {
		mainParticles.emissionRate = mainEmission;
		mainParticles.Play();
		secondaryParticles.Play();
		yield return new WaitForSeconds(0.5f);
		secondaryParticles.Stop();
		foreach(var t in Transition (0.75f)) {
			mainParticles.emissionRate = (1f-t) * mainEmission;
			yield return null;
		}
	}
}
