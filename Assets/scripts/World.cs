using UnityEngine;
using System.Collections;

public class World : CustomBehaviour {
	
	public Building buildingPrefab;
	
	void Start () {
		
		// randomly place buildings
		var radius = (transform.position - Dino.inst.transform.position).x0z().magnitude - 4f;
		
		var density = 0.12f;
		var area = Mathf.PI * radius * radius;
		var total = Mathf.CeilToInt(density * area);
		
		for(int i=0; i<total; ++i) {
			
			
			var uniformSample = Random.insideUnitCircle;
			var weight = EaseIn2(uniformSample.magnitude);
			var weightedSample = radius * uniformSample.normalized * weight;
			
			var inst = Dup(buildingPrefab, transform.position + Vec(weightedSample.x, 0f, weightedSample.y));
			
			var extraHeight = Random.Range(0.75f, 2f);
			var extraBase = Random.Range(1f, 3f);
			var adjustedWeight = Random.Range(1f - weight, 1.5f * (1f - weight));
			
			var baseScale = inst.transform.localScale + Vec(0, adjustedWeight, 0);
			baseScale.x *= extraBase;
			baseScale.y *= extraHeight;
			baseScale.z *= extraBase;
			inst.transform.localScale = baseScale;
		
		}
		
	}
}
