using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour 
{
	bool facingRight = true;							// For determining which way the player is currently facing.

	public float moveAccel = 1f;
	[SerializeField] float maxRunSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float maxSpeed = 15f;				// The fastest the player can travel in the x axis.
	[SerializeField] float maxYSpeed = 20f;				// The fastest the player can travel in the x axis.

	[Range(0, 1)]
	
	public bool airControl = true;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	Transform ceilingCheck;								// A position marking where to check for ceilings
	float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	float ammo = 6;
	float semiAmmo = 1;
	Animator anim;										// Reference to the player's animator component.
	bool togglelowfriction = false;

	Transform playerGraphics;

	bool touchWall;

	bool isFacingRight() { return facingRight; }

	public float getAmmo() { return ammo; }

	public void incrAmmo() {
		++ammo;
	}

	public void decrAmmo() {
		--ammo;
	}

	public float getSemiAmmo() { return semiAmmo; }
	
	public void setSemiAmmo(float set) {
		semiAmmo = set;
	}
	
	public void decrSemiAmmo() {
		--semiAmmo;
	}

    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		anim = GetComponent<Animator>();
		playerGraphics = transform.FindChild("Graphics");
	}


	void FixedUpdate()
	{
		int groundLayer = 1 << 15;
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, groundLayer);
		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);


	}

	public void Push(Vector2 dir) {
		float currX = rigidbody2D.velocity.x+dir.x;
		float currY = rigidbody2D.velocity.y+dir.y;
		if (currX > 0)
			currX = Mathf.Min (maxSpeed, currX);
		else if (currX < 0)
			currX = Mathf.Max (-maxSpeed, currX);
		currY = Mathf.Min (maxYSpeed, currY);
		if (currY > 10)
						currY = 10f + .75f * (currY - 10);
		rigidbody2D.velocity = new Vector2(currX, currY);
		//Debug.Log (rigidbody2D.velocity);
		togglelowfriction = true;
	}
	
	void OnCollisionEnter2D(Collision2D col){
		if(col.collider.gameObject.tag == "Fire"){
			Debug.Log("HIT FIRE");
			gameObject.GetComponent<PlayerHealth>().Hurt(10);
			Vector2 inc = col.relativeVelocity;
			inc.Normalize();
			if(grounded)
			Push (-inc*8f);
			else 
				Push (-inc*3f);
		}
	}
	
	void OnCollisionStay2D(Collision2D col){
		if(col.collider.gameObject.tag == "Fire"){
			Debug.Log("HIT FIRE");
			gameObject.GetComponent<PlayerHealth>().Hurt(10);
			Vector2 inc = col.relativeVelocity;
			inc.Normalize();
			if (inc.magnitude < 1) inc = new Vector2(0,1);
			//Push (-inc*6f);
		}
	}
	
	public void Move(float move, bool crouch, bool jump)
	{
		int groundLayer = 1 << 15;
		Vector2 facing = (move > 0) ? Vector2.right : -Vector2.right;
		Vector2 position = new Vector2(groundCheck.position.x, groundCheck.position.y);
		RaycastHit2D hit = Physics2D.Raycast(position, facing, Mathf.Infinity, groundLayer);
		if(hit.collider != null)
		{
			float distToWall = ( hit.point - (Vector2) transform.position ).magnitude;
			//Debug.Log (distToWall);
			if (distToWall < 1.3) {
				touchWall = true;
				
			}
			else touchWall = false;
		}
		else touchWall = false;

		//only control the player if grounded or airControl is turned on
		if(grounded || !grounded) //grounded || airControl)
		{
			// If the input is moving the player right and the player is facing left...
			if(move > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(move < 0 && facingRight)
				// ... flip the player.
				Flip();

			float newX, currX = rigidbody2D.velocity.x;
			float currXSqr = Mathf.Abs(currX)*Mathf.Sqrt(Mathf.Abs(currX));
			//if(currXSqr > 0)
			//Debug.Log (currXSqr);
			float coeff = 0.2f;
			if ((move > 0 && facingRight || move < 0 && !facingRight) && touchWall) {
				rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
				//Debug.Log ("A");
			} else if (currX > -0.1 && currX < 0.1) {
				//Debug.Log ("E");
				if(move > 0) {
					rigidbody2D.velocity = new Vector2(moveAccel, rigidbody2D.velocity.y);
				}
				else if(move < 0) {
					rigidbody2D.velocity = new Vector2(-moveAccel, rigidbody2D.velocity.y);
				}
			} else if (move == 0) {
				//Debug.Log ("B");
				if (!grounded) coeff *= .5f;
				if (currX > maxSpeed)
					rigidbody2D.velocity = new Vector2(maxSpeed-coeff*currXSqr*moveAccel, rigidbody2D.velocity.y);
				else if (currX < -maxSpeed) {
					rigidbody2D.velocity = new Vector2(-maxSpeed+coeff*currXSqr*moveAccel, rigidbody2D.velocity.y);
				} else if (currX < 0) {
					rigidbody2D.velocity = new Vector2(Mathf.Min (currX+coeff*currXSqr*moveAccel,0), rigidbody2D.velocity.y);
				} else {
					rigidbody2D.velocity = new Vector2(Mathf.Max (currX-coeff*currXSqr*moveAccel,0), rigidbody2D.velocity.y);
				}
			} else if (currX > 0) {
				//Debug.Log ("C");
				if(move > 0) {
					if (currX > maxSpeed)
						rigidbody2D.velocity = new Vector2(maxSpeed, rigidbody2D.velocity.y);
					else if (currX < maxRunSpeed)
						rigidbody2D.velocity = new Vector2(Mathf.Min (currX+moveAccel,maxRunSpeed), rigidbody2D.velocity.y);
				} else {
					if (currX > maxSpeed)
						rigidbody2D.velocity = new Vector2(maxSpeed-coeff*currXSqr*moveAccel, rigidbody2D.velocity.y);
					else
						rigidbody2D.velocity = new Vector2(currX-coeff*currXSqr*moveAccel, rigidbody2D.velocity.y);
				}
			} else if (currX < 0){
				//Debug.Log ("D");
				if(move < 0) {
					if (currX < -maxSpeed)
						rigidbody2D.velocity = new Vector2(-maxSpeed, rigidbody2D.velocity.y);
					else if (currX > -maxRunSpeed)
						rigidbody2D.velocity = new Vector2(Mathf.Max (currX-moveAccel,-maxRunSpeed), rigidbody2D.velocity.y);
				} else {
					if (currX < -maxSpeed)
						rigidbody2D.velocity = new Vector2(-maxSpeed+coeff*currXSqr*moveAccel, rigidbody2D.velocity.y);
					else
						rigidbody2D.velocity = new Vector2(currX+coeff*currXSqr*moveAccel, rigidbody2D.velocity.y);
				}
			} 

			// The Speed animator parameter is set to the absolute value of the horizontal input.
			//anim.SetFloat("Speed", Mathf.Abs(move));
			
			// Move the character
			//rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
		}

        // If the player should jump...
        if (grounded && jump) {
            // Add a vertical force to the player.
			anim.SetBool("Ground", false);
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y+14f);
			//rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
	}

	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = playerGraphics.localScale;
		theScale.x *= -1;
		playerGraphics.localScale = theScale;
	}
}
