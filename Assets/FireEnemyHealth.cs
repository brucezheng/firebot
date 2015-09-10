using UnityEngine;
using System.Collections;

public class FireEnemyHealth : MonoBehaviour {
	public int health = 200;
	public bool invincible = false;
	public GameObject explosion;
	public Sprite nextSprite;

	private bool fire = true;

	public bool onfire() { return fire; }

	// Use this for initialization
	void Start () {
		fire = true;
	}
	
	public void Hurt(int i){
		if(!invincible)
			health = health - i;
		if(health <= 0)
			StopFire();
	}
	
	private void StopFire(){
		fire = false;
		GetComponent<SpriteRenderer> ().sprite = nextSprite;
	}

	public void Death() {

	}
	
}
