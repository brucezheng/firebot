using UnityEngine;
using System.Collections;

public class FireHealth : MonoBehaviour {
	public int health = 100;
	public bool invincible = false;
	public GameObject explosion;
	
	// Use this for initialization
	void Start () {

	}
	
	public void Hurt(int i){
		if(!invincible)
		health = health - i;
		if(health <= 0)
			Death();
	}
	
	public void Death(){
		Destroy(gameObject);
	}
	
}
