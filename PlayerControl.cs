using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerControl : MonoBehaviour {

	public bool facingRight = true;
	public bool jump = false;

	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 400f;

	private Transform groundCheck;
	private bool grounded = false;
	private Animator anim; 

	// Use this for initialization
	void Start () {

		groundCheck = transform.Find ("groundCheck");
		anim = GetComponent<Animator> ();
	
	}
	
	// Update is called once per frame
	void Update () {

		grounded = Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

		bool btnJump = CrossPlatformInputManager.GetButton ("Jump");

		if (btnJump && grounded)
			jump = true;

	}

	void FixedUpdate(){

		//float h = Input.GetAxis ("Horizontal");

		Vector2 movVec = new Vector2 (CrossPlatformInputManager.GetAxis ("Horizontal"), CrossPlatformInputManager.GetAxis ("Vertical") * 5);
		float h = movVec.x;

		anim.SetFloat ("Speed", Mathf.Abs (h));

		print (h);

		if (h * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * moveForce);

		if (Mathf.Abs (GetComponent<Rigidbody2D> ().velocity.x) > maxSpeed)
			GetComponent<Rigidbody2D> ().velocity = new Vector2 (Mathf.Sign (GetComponent<Rigidbody2D> ().velocity.x) * maxSpeed, GetComponent<Rigidbody2D> ().velocity.y);

		// Si el personaje esta mirando a izq y le pedimos ir a der
		if(h > 0 && !facingRight)
			// ... voltear.
			Flip();
		// Viceversa
		else if(h < 0 && facingRight)
			// ... voltear.
			Flip();


		if (jump) {
			anim.SetTrigger ("Jump");

			GetComponent<Rigidbody2D>().AddForce(new Vector2 (0f, jumpForce));
			jump = false;
		}

	}

	void Flip(){

		// Voltear al personajue
		facingRight = !facingRight;

		// Multiplicamos el eje x local y lo escalamos por -1
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

	}
}



