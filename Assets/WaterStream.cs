using UnityEngine;
using System.Collections;

public class WaterStream : MonoBehaviour 
{
	public Rigidbody2D impactParticle;		// Prefab of explosion effect.
	public Rigidbody2D trailParticle;	
	public GameObject steamParticle;	
	public float reflectCoeff = 0.15f;
	private float lastTrailPart = -100f;
	public float firingRate = 0.25f;
	
	void Start () 
	{
		//LayerMask layer1 = LayerMask.NameToLayer ("Bullet");
		//LayerMask layer2 = LayerMask.NameToLayer ("Bullet Residue");
		//LayerMask layer3 = LayerMask.NameToLayer ("Player");
		//Physics2D.IgnoreLayerCollision(layer1,layer2);
		//Physics2D.IgnoreLayerCollision(layer1,layer3);
		// Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
		Destroy(gameObject, 20);
	}

	Vector2 Rotate(Vector2 vec, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
		float tx = vec.x;
		float ty = vec.y;
		return new Vector2((cos * tx) - (sin * ty),(sin * tx) + (cos * ty));
	}

	
	void FixedUpdate () {
		if (lastTrailPart < Time.time - (firingRate*Random.Range (0.1f,1.9f))) {
			lastTrailPart= Time.time;
			Rigidbody2D trail = Instantiate(trailParticle, transform.position, Quaternion.Euler (0f, 0f, Random.Range (0f, 360f))) as Rigidbody2D;
			trail.velocity = Rotate(rigidbody2D.velocity,Random.Range (-15f,15f)) * .5f*Random.Range (0.5f,1.5f);
		}
	}
	
	void OnExplode(Vector2 normal, Vector2 inc)
	{
		float speed = (inc).magnitude;
		normal.Normalize();
		inc.Normalize ();
		Vector2 reflect = 2f * normal + inc;
		//Debug.Log (speed);

		for (int i = 0; i < 4; ++i) {
			Rigidbody2D impact = Instantiate(impactParticle, transform.position, Quaternion.Euler (0f, 0f, Random.Range (0f, 360f))) as Rigidbody2D;
			impact.velocity = reflectCoeff * speed*Random.Range (0.75f,1.25f) * Rotate(reflect,Random.Range (-15f,15f));
		}

	}

	//void OnCollisionStay2D (Collision2D col) {
		//Debug.Log (col.collider.gameObject.tag);
	//}

	void OnCollisionEnter2D (Collision2D col) 
	{
		//Debug.Log (col.collider.gameObject.tag);
		if (col.collider.gameObject.tag == "Fire") {
			Destroy (gameObject);
			col.gameObject.GetComponent<FireHealth>().Hurt(20);
			Instantiate (steamParticle,transform.position,Quaternion.Euler (0f,0f,0f));
		}
		else if (col.collider.gameObject.tag == "Fire Enemy") {
			Destroy (gameObject);
			col.gameObject.GetComponent<FireEnemyHealth>().Hurt(20);
			Instantiate (steamParticle,transform.position,Quaternion.Euler (0f,0f,0f));
		}
		else if(col.collider.gameObject.tag != "Player" && col.collider.gameObject.tag != "Water")
		{
			Vector2 norm = col.contacts[0].normal;
			Vector2 inc = col.relativeVelocity;
			// Instantiate the explosion and destroy the rocket.
			OnExplode(norm,inc);
			Destroy (gameObject);
		}
	}
}
