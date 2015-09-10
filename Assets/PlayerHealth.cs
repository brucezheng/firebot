using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	public int health = 30;
	private float timeLeft = 10;
	private float lastHit = -100f;
	private float hitFreq = .1f;
	private Vector3 originPoint;
	private Quaternion originOrientation;
	
	// Use this for initialization
	void Start () {
		originPoint = transform.position;
		originOrientation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if(timeLeft <= 0 && health < 100){
			Recover();
			timeLeft = 10;
		}
	}
	
	void Recover(){
		health += 10;
	}	
	
	public void Hurt(int i){
		if (lastHit < Time.time - hitFreq) {
			AudioSource source = GetComponent<AudioSource>();
			source.Play ();
			lastHit = Time.time;
						health -= i;
						if (health <= 0)
								Death ();
				}
	}
	
	public void Death(){
		Debug.Log ("You died lol");
		transform.position = originPoint;
		transform.rotation = originOrientation;
		health = 50;
	}
}
