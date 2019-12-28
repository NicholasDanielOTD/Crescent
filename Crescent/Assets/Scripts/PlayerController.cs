using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This file is for the physical controlling (movement and stuff) of the player character

//this is here for legacy purposes

public class PlayerController : MonoBehaviour
{
	//Vars for dashing
	private int dashcount = 0;
	private float dashinterval = .2f;
	
	//Var for blocking
	private bool blocking;
	
	//Vars for movement
	public float speed;
	public float jumpForce;
	private float moveInput;
	
	private Rigidbody2D rb;
	
	//Vars for checking if the player is on the ground and therefore can jump
	private bool isGrounded;
	public Transform groundCheck;
	public float checkRadius;
	public LayerMask whatIsGround;

	private bool facingRight = true;
	
    // Start is called before the first frame update
    void Start(){
		//Initialize the rigidbody so we can give it velocity changes
        rb = GetComponent<Rigidbody2D>();
		blocking = false;
    }

	
	void FixedUpdate(){
		
		//Get raw movement input then set velocity, maintain vertical speed
		if(!blocking){ moveInput = Input.GetAxisRaw("Horizontal"); }
		rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
		if(Input.GetKey(KeyCode.LeftShift))
		{
			blocking = true;
			rb.velocity = Vector2.zero;
		}
		//Check if a circle centered at the GroundCheck gizmo overlaps with anything in the ground layer
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
		
		//Check to see if the player is turned around and flip character model as needed
		if(facingRight == false && moveInput > 0){
			Flip();
		}
		else if(facingRight == true && moveInput < 0){
			Flip();
		}
	}
	
	
    // Update is called once per frame
    void Update()
    {
		//Input checking
		if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
			rb.velocity = Vector2.up * jumpForce;
		}
		
		if(blocking && Input.GetKeyUp(KeyCode.LeftShift)) { blocking=false; }			
		
		if(Input.GetKeyDown(KeyCode.S) && rb.velocity.y < 0){
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*1.6f);
		}
		if(Input.GetKeyDown(KeyCode.A))
		{
			//If have hit A once recently and are within the dashinterval, dash
			if(dashinterval > 0 && dashcount == 1)
			{
				Debug.Log("dashed");
			}
			else
			{
				//Must be first hit of A, set dashcount and start the interval
				dashcount = 1;
				dashinterval = .2f;
			}
		}
		//Decrease dash interval and if interval is over, reset dash counter
		if(dashinterval > 0)
		{
			dashinterval -= Time.deltaTime;
		}
		else
		{
			dashcount = 0;
		}
		
    }
	
	void Flip()
	{
		//Scale the horizontal axis of our transform by -1, flipping the object
		facingRight = !facingRight;
		Vector3 Scaler = transform.localScale;
		Scaler.x *= -1;
		transform.localScale = Scaler;
	}
}