using UnityEngine;
using System.Collections;

public class FireBullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Destroy (gameObject,2);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D col) 
	{
		//Debug.Log (col.collider.gameObject.tag);
		if (col.collider.gameObject.tag != "Bullet") {
			Destroy (gameObject);
		}
	}
}
