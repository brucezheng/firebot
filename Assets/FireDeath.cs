using UnityEngine;
using System.Collections;

public class FireDeath : MonoBehaviour {
	private FireEnemyHealth health;

	public void Death() {
		Destroy (gameObject);
	}

}
