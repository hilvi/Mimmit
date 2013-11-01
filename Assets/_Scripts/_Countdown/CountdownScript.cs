using UnityEngine;
using System.Collections;

public class CountdownScript : MonoBehaviour {
	
	public float maxSize;
	public float transformSpeed;
	
	void Update () {
		float __w = transform.localScale.x + Time.deltaTime * transformSpeed;
		float __h = transform.localScale.y + Time.deltaTime * transformSpeed;
		
		if (__w < maxSize && __h < maxSize) {
			transform.localScale = new Vector3(__w, __h, 1f);
		}
		
		Color __c = renderer.material.color;
		float __a = __c.a - Time.deltaTime;
		__a = Mathf.Clamp01(__a);
		__c.a = __a;
		renderer.material.color = __c;
		
		if (__a == 0f) {
			Destroy (this.gameObject);
		}
	}
	
	public void SetText(string text) {
		GetComponent<TextMesh>().text = text;
	}
}
