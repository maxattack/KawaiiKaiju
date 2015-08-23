using UnityEngine;
using System.Collections;

public class Explosion : CustomBehaviour {

	IEnumerator Start () {
		
		var mat = GetComponent<Renderer>().material; // implicit dup
		var startScale = transform.localScale;
		var endScale = startScale + Vec(0.5f, 0.5f, 0.5f);
		foreach(var t in Transition (0.5f)) {
			
			// scale up		
			transform.localScale = Lerp(startScale, endScale, EaseOut2(t));
			
			// fade out
			mat.color = RGBA(mat.color, 1f - EaseIn2(t));
			
			yield return null;
		}
		Destroy(gameObject);
					
	}
	
}
