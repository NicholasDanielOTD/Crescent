using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutowalkerController : Enemy, IHitboxResponder
{
	
    // Start is called before the first frame update
    void Start()
    {
		attackdict = new Dictionary<string, int>();
		attackdict.Add("swing",50);
        rb = GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(speed, rb.velocity.y);
    }
	
	void Update()
    {
        if(hp <= 0 && !dead){
			Death();
		}
		
		if(timeBtwAttack < 0)
		{
			hitbox?.stopCheckingCollision();
			inAnimation = false;
		}
		
		//Enemy detectoin, if too close then stop moving
		if(canAttack())
		{
			swingAttack();
			timeBtwAttack = startTimeBtwAttack;
		}
		else if(detectPlayer()){
			if(!inAnimation){rb.velocity = new Vector2(speed, rb.velocity.y);}
			if(timeBtwAttack > 0){timeBtwAttack -= Time.deltaTime;}
		}
    }
	
	void FixedUpdate(){
		
		if(inAnimation && hitbox.isStateOpen()){
			hitbox.hitboxUpdate();
		}
	}

	
	public void swingAttack(){
		hitbox = transform.Find("swingbox").GetComponent<Hitbox>();
		timeBtwAttack = .5;
		hitbox.useResponder(this);
		hitbox.startCheckingCollision();
		hitbox.attackname = "swing";
		inAnimation = true;
		Debug.Log("swingin!");
	}
	
	public override void Death()
	{
		dead = true;
		Debug.Log("Died in override");
	}
}
