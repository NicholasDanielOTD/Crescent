using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutowalkerController : Enemy
{
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(speed, rb.velocity.y);
    }
	
	void Update()
    {
        if(hp <= 0 && !dead){
			Death();
		}
		
		//Enemy detectoin, if too close then stop moving
		if(Physics2D.OverlapCircleAll(rb.position, detectionRadius, whatIsPlayer).Length == 0){
			
			rb.velocity = new Vector2(speed, rb.velocity.y);
		}
		else
		{
			//Debug.Log("Player detected!");
		}
    }

	
	public override void Death()
	{
		dead = true;
		Debug.Log("Died in override");
	}
}
