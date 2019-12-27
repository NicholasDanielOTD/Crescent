using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script sets up a very basic enemy that walks forward and swings when in range

public class AutowalkerController : Enemy, IHitboxResponder
{
	
    // Start is called before the first frame update
    void Start()
    {
		//Establish the attack by name and their damage
		attackdict = new Dictionary<string, int>();
		attackdict.Add("swing",50);
		attackdict.Add("gpound",75);
		//Get the rigidbody and set a velocity
        rb = GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(speed, rb.velocity.y);
    }
	
	void Update()
    {
		//Call death if alive but should be dead
        if(hp <= 0 && !dead){
			Death();
		}
		
		//If the duration of the attack is over, the attack shouild end so disable collision checking and set bool
		if(timeBtwAttack < 0)
		{
			hitbox?.stopCheckingCollision();
			inAnimation = false;
		}
		
		//Check if within attack range, then attack
		if(canAttack())
		{
			gPound();
		}
		//If can't attack because currently attacking or not in range, if within walking range of player, walk, if currently attacking decrease time
		else if(detectPlayer()){
			if(!inAnimation){rb.velocity = new Vector2(speed, rb.velocity.y);}
			if(timeBtwAttack > 0){timeBtwAttack -= Time.deltaTime;}
		}
    }
	
	void FixedUpdate(){
		//If attacking, call hitboxUpdate to check for collisions
		if(inAnimation && hitbox.isStateOpen()){
			hitbox.hitboxUpdate();
		}
	}

	
	public void swingAttack(){
		//Find the hitbox attached to the character
		hitbox = transform.Find("swingbox").GetComponent<Hitbox>();
		//set required variables
		timeBtwAttack = .5;
		hitbox.useResponder(this);
		hitbox.startCheckingCollision();
		hitbox.attackname = "swing";
		inAnimation = true;
		Debug.Log("swingin!");
	}
	
	public void gPound(){
		hitbox = transform.Find("gp").GetComponent<Hitbox>();
		timeBtwAttack = 3;
		hitbox.useResponder(this);
		hitbox.startCheckingCollision();
		hitbox.attackname = "gpound";
		inAnimation = true;
		Debug.Log("poundin!");
		hitbox.move(new Vector2(-3.5f, 0), timeBtwAttack*50);
		
		
	}
	
	
	
	public override void Death()
	{
		dead = true;
		Debug.Log("Died in override");
	}
}
