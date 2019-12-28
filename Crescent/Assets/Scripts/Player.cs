using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles all of the 'moves' and 'attacks' a player should have

public class Player : MonoBehaviour, IHitboxResponder
{
	
	//Vars for dashing
	private int dashcount = 0;
	private float dashinterval = .2f;
	
	//Var for blocking
//	private bool blocking;
	
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
	
	public Hitbox hitbox;
	public float hp;
	private bool dead;
	private double attackDuration;
	public bool inAnimation;
	public float strength;
	private Dictionary<string, int> attackdict;
	public LayerMask whatIsEnemies;
	public List<int> hitlist;
	private bool blocking;
	
    // Start is called before the first frame update
    void Start()
    {
		//Initialize the rigidbody so we can give it velocity changes
        rb = GetComponent<Rigidbody2D>();
		blocking = false;
		//Initializes attacks and their strength
		attackdict = new Dictionary<string, int>();
		attackdict.Add("stab",50);
		attackdict.Add("swipe",30);
		
    }

    // Update is called once per frame
    void Update()
    {
		//Checks for movement
		controllerUpdate();
		//Death check
		if(hp <= 0 && !dead){
			Death();
		}

		if(attackDuration > 0)
		{
			attackDuration -= Time.deltaTime;
		}
		//If the attack is over, turn off collision detection and set inAnimation
		else if(attackDuration < 0)
		{
			hitbox?.endAttack();
			inAnimation = false;
		}
		
    }
	  
	void FixedUpdate(){
		movementFixedUpdate();
		//If we are attacking, call hitboxUpdate to check for hits
		if(inAnimation && hitbox.isStateOpen()){
			hitbox.hitboxUpdate();
		}
	}

	
	void movementFixedUpdate(){
		
		//Get raw movement input then set velocity, maintain vertical speed
		if(!blocking){ moveInput = Input.GetAxisRaw("Horizontal"); }
		rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
		if(Input.GetKey(KeyCode.LeftShift) && !inAnimation)
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
    void controllerUpdate()
    {
		//Basic movement
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
		
		//ATTACKS
		if(attackDuration <= 0 && !inAnimation){
			if(Input.GetKeyDown(KeyCode.Q)){
				stab();
			}
			else if(Input.GetKeyDown(KeyCode.LeftControl)){
				swipe();
			}
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
	  
	  
	public void TakeDamage(float damage, int attackid)
	{
		//Verify this attack has not already hit the enemy, if not take the damage and add the attackid.
		if(!hitlist.Contains(attackid) && !blocking){hp -= damage; hitlist.Add(attackid); 
		Debug.Log("Took damage! Hp is now: " + hp);
		}
	}
	  
	
	  
	//Attacks below create an instance of the attack hitbox, initialize attack vars, then enable collision
	
	public void stab(){
		hitbox = transform.Find("stabbox").GetComponent<Hitbox>();
		hitbox.startWithDelay(.5);
		attackDuration = .7;
		hitbox.useResponder(this);
		hitbox.attackname = "stab";
		inAnimation = true;
		Debug.Log("stabbing!");
	}
	
	public void swipe(){
		hitbox = transform.Find("swipebox").GetComponent<Hitbox>();
		attackDuration = .5;
		hitbox.useResponder(this);
		hitbox.startCheckingCollision();
		hitbox.attackname = "swipe";
		inAnimation = true;
		Debug.Log("swiping!");
		
	}
	
	public void Death()
	{
		Debug.Log("You died");
		dead = true;
	}
	
	public void collisionedWith(Collider2D collider, int attackid){
		//Called when a hitbox lands on the enemy, finds the damage that needs to be dealt and deals it
		strength = attackdict[hitbox.attackname];
		collider.GetComponentInParent<Enemy>().TakeDamage(strength, attackid);
	}
	
	//This is used to see a red circle around gizmos when drawing them in Unity scene editor
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
//		Gizmos.DrawWireSphere(attackPos.position, attackRange);
	}
}
