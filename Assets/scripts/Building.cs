using UnityEngine;
using System.Collections;

public class Building : CustomBehaviour {
	
	public const int kLayer = 9;
	public const int kLayerBit = (1<<9);
	
	public void Detonate(Vector3 impactPoint) {
		var box = GetComponentInChildren<BoxCollider>();
		var scale = transform.localScale;
		var size = box.size;
		var worldSize = Vec(scale.x * size.x, scale.y * size.y, scale.z * size.z);
		var debrisDim = 0.1666f;
		var nx = Mathf.CeilToInt(worldSize.x / debrisDim);
		var ny = Mathf.CeilToInt(worldSize.y / debrisDim);
		var nz = Mathf.CeilToInt(worldSize.z / debrisDim);
		
		var debrisSize = Vec(worldSize.x / (float)nx, worldSize.y / (float)ny, worldSize.z / (float)nz);
		transform.localScale = Vector3.one;
		
		var physMat = box.sharedMaterial;
		
		for(int x=0; x<nx; ++x)
		for(int y=0; y<ny; ++y)
		for(int z=0; z<nz; ++z) {
			var debrisGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
			var debrisXf = debrisGo.transform;
			debrisXf.parent = transform;
			debrisXf.localScale = 0.9f * debrisSize;
			debrisXf.localRotation = Quaternion.identity;
			debrisXf.localPosition = Vec(
				debrisSize.x * x + 0.5f * debrisSize.x - 0.5f * worldSize.x,
				debrisSize.y * y + 0.5f * debrisSize.y,
				debrisSize.z * z + 0.5f * debrisSize.z - 0.5f * worldSize.z
			);
			debrisXf.parent = null;
			debrisGo.hideFlags = HideFlags.HideInHierarchy;
			var body = debrisGo.AddComponent<Rigidbody>();
			body.AddExplosionForce(3.333f, impactPoint, 0.5f * scale.y, 0f, ForceMode.VelocityChange);
			body.angularVelocity = Mathf.PI * Random.insideUnitSphere;
			body.drag = 0.05f;
			body.angularDrag = 0.05f;
			debrisGo.GetComponent<BoxCollider>().sharedMaterial = physMat;
		}
		
		Destroy(gameObject);
	}
	
}
