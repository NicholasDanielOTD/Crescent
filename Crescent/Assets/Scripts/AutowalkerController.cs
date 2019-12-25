using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutowalkerController : MonoBehaviour
{
	
	public float speed;
	private Rigidbody2D rb;
	public float detectionRadius;
	
	public LayerMask whatIsPlayer;
	
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
		if(Physics2D.OverlapCircleAll(rb.position, detectionRadius, whatIsPlayer).Length == 0){
			
			rb.velocity = new Vector2(speed, rb.velocity.y);
		}
		else
		{
			Debug.Log("Player detected!");
		}
    }
}
