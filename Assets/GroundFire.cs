using UnityEngine;
using System.Collections;

public class GroundFire : MonoBehaviour {
	
	public Rigidbody2D projectile;	
	private float lastSpawn = -100f;
	public float firingRate = 0.75f;

	// Use this for initialization
	void Start () {
	
	}
	
	Vector2 Rotate(Vector2 vec, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
		float tx = vec.x;
		float ty = vec.y;
		return new Vector2((cos * tx) - (sin * ty),(sin * tx) + (cos * ty));
	}
	
	
	void Update () {
		if (lastSpawn < Time.time - (firingRate*Random.Range (0.5f,1.5f))) {
			lastSpawn= Time.time;
			Vector2 newposition;
			newposition.y = transform.position.y + 1f;
			newposition.x = transform.position.x + Random.Range (-.5f,.5f);
			Rigidbody2D trail = Instantiate(projectile, newposition, Quaternion.Euler (0f, 0f, Random.Range (0f, 360f))) as Rigidbody2D;
			trail.velocity = Rotate(new Vector2(0,6),Random.Range (-60f,60f)) * Random.Range (0.75f,1.25f);
		}
	}
}
