using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles all of the 'moves' and 'attacks' a player should have

public class Player : MonoBehaviour, IHitboxResponder
{
	
	public Hitbox hitbox;
	public float hp;
	private bool dead;
	private double timeBtwAttack;
	public float startTimeBtwAttack;
	public bool inAnimation;
	public float strength;
	private Dictionary<string, int> attackdict;
	public LayerMask whatIsEnemies;
	public List<int> hitlist;
	private bool blocking;
	
    // Start is called before the first frame update
    void Start()
    {
		//Initializes attacks and their strength
		attackdict = new Dictionary<string, int>();
		attackdict.Add("stab",50);
		attackdict.Add("swipe",30);
		
    }

    // Update is called once per frame
    void Update()
    {
		if(hp <= 0 && !dead){
			Death();
		}
		
		//If can attack, allow attack inputs
        if(timeBtwAttack <= 0 && !inAnimation){
			if(Input.GetKeyDown(KeyCode.LeftAlt)){
				stab();
			}
			else if(Input.GetKeyDown(KeyCode.LeftControl)){
				swipe();
			}
		}
		else //decrease time
		{
			timeBtwAttack -= Time.deltaTime;
		}
		//If the attack is over, turn off collision detection and set inAnimation
		if(timeBtwAttack < 0)
		{
			hitbox?.stopCheckingCollision();
			inAnimation = false;
		}
		
    }
	  
	public void TakeDamage(float damage, int attackid)
	{
		//Verify this attack has not already hit the enemy, if not take the damage and add the attackid.
		if(!hitlist.Contains(attackid) && !blocking){hp -= damage; hitlist.Add(attackid); 
		Debug.Log("Took damage! Hp is now: " + hp);
		}
	}
	  
	void FixedUpdate(){
		//If we are attacking, call hitboxUpdate to check for hits
		if(inAnimation && hitbox.isStateOpen()){
			hitbox.hitboxUpdate();
		}
	}
	  
	//Attacks below create an instance of the attack hitbox, initialize attack vars, then enable collision
	
	public void stab(){
		hitbox = transform.Find("stabbox").GetComponent<Hitbox>();
		hitbox.hitDelay = .1;
		timeBtwAttack = .3;
		hitbox.useResponder(this);
		hitbox.startCheckingCollision();
		hitbox.attackname = "stab";
		inAnimation = true;
		Debug.Log("stabbing!");
	}
	
	public void swipe(){
		hitbox = transform.Find("swipebox").GetComponent<Hitbox>();
		timeBtwAttack = .5;
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
