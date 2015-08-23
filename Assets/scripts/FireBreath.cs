using UnityEngine;
using System.Collections;

public class FireBreath : CustomBehaviour {
	
	public ParticleSystem mainParticles;
	public ParticleSystem secondaryParticles;
	public Light highlight;
	float mainEmission;
	float lightIntensity;
	
	void Awake() {
		mainEmission = mainParticles.emissionRate;
		lightIntensity = highlight.intensity;
	}
	
	public void Burst() {
		StartCoroutine(DoBurst());
	}
	
	IEnumerator DoBurst() {
		mainParticles.Clear();
		secondaryParticles.Clear();
		mainParticles.emissionRate = mainEmission;
		mainParticles.Play();
		secondaryParticles.Play();
		highlight.intensity = 0f;
		highlight.enabled = true;
		foreach(var t in Transition (0.25f)) {
			yield return null;
			highlight.intensity = lightIntensity * EaseOut2(t);
		}
		secondaryParticles.Stop();
		foreach(var t in Transition (0.75f)) {
			mainParticles.emissionRate = (1f-t) * mainEmission;
			highlight.intensity = lightIntensity * (1f-EaseIn2(t));
			yield return null;
		}
		mainParticles.Stop();
		highlight.enabled = false;
	}
}
