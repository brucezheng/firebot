using UnityEngine;
using System.Collections;

public class CritKill : MonoBehaviour {
	private FireEnemyHealth health;
	
	// Use this for initialization
	void Start () {
		health = transform.parent.GetComponent<FireEnemyHealth>();
	}

	// Update is called once per frame
	void Update () {
	
	}

	
	
	void OnCollisionEnter2D (Collision2D col) 
	{
		//DebugLog (col.collider.gameObject.tag);
		if (col.collider.gameObject.tag == "Bullet" && !health.onfire ()) {
			//Destroy (gameObject);
			Destroy (gameObject.transform.parent.gameObject);
			//col.gameObject.GetComponent<FireHealth> ().Hurt (20);
			//Instantiate (steamParticle, transform.position, Quaternion.Euler (0f, 0f, 0f));
			}
	}
}
