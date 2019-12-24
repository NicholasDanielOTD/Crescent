using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	
	public float speed;
	public float jumpForce;
	private float moveInput;
	
	private Rigidbody2D rb;
	
	private bool isGrounded;
	public Transform groundCheck;
	public float checkRadius;
	public LayerMask whatIsGround;
	
	private bool facingRight = true;
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

	
	void FixedUpdate(){
		
		moveInput = Input.GetAxisRaw("Horizontal");
		rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
		
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
		
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
		
		if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
			rb.velocity = Vector2.up * jumpForce;
		}
			
        if(Input.GetKeyDown(KeyCode.S) && rb.velocity.y < 0){
			rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*1.6f);
		}
    }
	
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 Scaler = transform.localScale;
		Scaler.x *= -1;
		transform.localScale = Scaler;
	}
}