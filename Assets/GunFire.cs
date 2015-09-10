using UnityEngine;
using System.Collections;

public class GunFire : MonoBehaviour
{
	public Rigidbody2D streamProjectile;				// Prefab of the rocket.
	public Rigidbody2D semiProjectile;
	public float speed = 40f;				// The speed the rocket will fire at.
	public float force = 3f;
	public float firingrate = 0.1f;
	
	private float lastfired = -100f;
	private float lastSemiFired = -100f;
	private float lastchangedammo = -100f;
	private float lastlock = -100f;
	private PlatformerCharacter2D playerCtrl;		// Reference to the PlayerControl script.
	private Animator anim;					// Reference to the Animator component.

	private bool gunlocked = false;
	private bool semiGunlocked = false;
	public AudioSource[] source;
	//bool waterAudio = false;
	
	void Awake()
	{
		LayerMask layer1 = LayerMask.NameToLayer ("Bullet");
		LayerMask layer2 = LayerMask.NameToLayer ("Bullet Residue");
		LayerMask layer3 = LayerMask.NameToLayer ("Player");
		Physics2D.IgnoreLayerCollision(layer1,layer2);
		Physics2D.IgnoreLayerCollision(layer1,layer3);
		// Setting up the references.
		anim = transform.root.gameObject.GetComponent<Animator>();
		playerCtrl = transform.root.GetComponent<PlatformerCharacter2D>();
	}
	
	
	Vector2 Rotate(Vector2 vec, float degrees) {
		float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
		float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
		float tx = vec.x;
		float ty = vec.y;
		return new Vector2((cos * tx) - (sin * ty),(sin * tx) + (cos * ty));
	}
	
	void Update ()
	{
		Vector2 difference = Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position;
		float scale = Mathf.Min((difference.magnitude)/10f,1);
		difference.Normalize ();
		if (Input.GetButtonDown ("Fire2") && lastSemiFired < Time.time - 2f*firingrate && playerCtrl.getSemiAmmo () > 0) {
			source[0].Play();
			playerCtrl.decrSemiAmmo ();
			lastSemiFired = Time.time;
			Vector2 newPosition = new Vector2 (difference.x + transform.position.x, difference.y + transform.position.y);
			Rigidbody2D bulletInstance = Instantiate (semiProjectile, transform.position, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
			bulletInstance.velocity = difference.normalized * 150;
			playerCtrl.Push (4f*force * (-difference));
			//playerCtrl.Push (force * (-difference));
			//playerCtrl.Push (force * (-difference));
			//Debug.Log (force * (-difference));
		}
		else if (lastSemiFired < Time.time - 15f*firingrate) {
			playerCtrl.setSemiAmmo (1);

		}
		// If the fire button is pressed...
		if (!gunlocked && Input.GetButton ("Fire1") && lastfired < Time.time - firingrate && playerCtrl.getAmmo () > 0) {
			source[1].Play();
			//Debug.Log(playerCtrl.getAmmo());
						playerCtrl.decrAmmo ();
						if(playerCtrl.getAmmo () <= 0) gunlocked = true;
						lastchangedammo = Time.time;
						lastfired = Time.time;
						// ... set the animator Shoot trigger parameter and play the audioclip.
						//anim.SetTrigger("Shoot");
						//audio.Play();
			
						
						Vector2 newPosition = new Vector2 (difference.x + transform.position.x + Random.Range (-.5f,.5f), 
			                                                difference.y + transform.position.y + Random.Range (-.5f,.5f));
			Rigidbody2D bulletInstance = Instantiate (streamProjectile, newPosition, Quaternion.Euler (new Vector3 (0, 0, 0))) as Rigidbody2D;
						bulletInstance.velocity = speed * scale * new Vector2 (difference.x, difference.y);
						playerCtrl.Push (force * (-difference)*(.25f + .75f*scale));
			//Debug.Log (force * (-difference)*(.25f + .75f*scale));
		} else if((gunlocked  || !Input.GetButton ("Fire1")) && lastlock < Time.time - 20*firingrate && lastchangedammo < Time.time - firingrate) {
				if(playerCtrl.getAmmo () < 6) {
				playerCtrl.incrAmmo ();
				if(playerCtrl.getAmmo () >= 6) gunlocked = false;
			}	
				lastchangedammo = Time.time;
				
		}
		if (!Input.GetButton ("Fire1")) {
			
			source[1].Stop();
				}
	}
}
