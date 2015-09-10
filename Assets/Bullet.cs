using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	void Start () 
	{
		Destroy(gameObject, 2);
	}
	void OnCollisionEnter2D (Collision2D col) 
	{
				Destroy (gameObject,.25f);
		}
}
