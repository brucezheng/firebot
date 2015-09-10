using UnityEngine;
using System.Collections;

public class WaterStreamSmall : MonoBehaviour 
{
	public GameObject steamParticle;
	void Start () 
	{
		StartCoroutine(Fade ());
	}

	IEnumerator Fade() {
		float t = .001f;
		float f = 1f;
		while(f >= 0) {
			Color c = renderer.material.color;
			c.a = f;
			renderer.material.color = c;
			f -= t;
			t *= 1.15f;
			yield return null;
		}
		if(f <= 0f) Destroy(gameObject);
	}
	
	void OnCollisionEnter2D (Collision2D col) 
	{
			//Debug.Log (col.collider.gameObject.tag);
		if (col.collider.gameObject.tag == "Fire") {
			Instantiate (steamParticle,transform.position,Quaternion.Euler (0f,0f,0f));
			col.gameObject.GetComponent<FireHealth>().Hurt(4);
			Destroy (gameObject);
			}
	}
}
