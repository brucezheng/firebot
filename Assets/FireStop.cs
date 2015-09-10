using UnityEngine;
using System.Collections;

public class FireStop : MonoBehaviour {
	private FireEnemyHealth health;

	// Use this for initialization
	void Start () {
		health = transform.parent.GetComponent<FireEnemyHealth>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!health.onfire())
			Destroy (gameObject);
	}
}
