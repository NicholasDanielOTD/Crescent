using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	
	
    // Start is called before the first frame update
    void Start()
    {
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

		//If can attack
        if(timeBtwAttack <= 0){
			if(Input.GetKey(KeyCode.LeftShift)){
				stab();
			}
			else if(Input.GetKey(KeyCode.LeftControl)){
				swipe();
			}
		}
		else
		{
			timeBtwAttack -= Time.deltaTime;
		}
		
		if(timeBtwAttack < 0)
		{
			hitbox?.stopCheckingCollision();
			inAnimation = false;
		}
		
    }
	  
	public void TakeDamage(float damage, int attackid)
	{
		if(!hitlist.Contains(attackid)){ hp -= damage; hitlist.Add(attackid); 
		Debug.Log("Took damage! Hp is now: " + hp);
		}
	}
	  
	void FixedUpdate(){
		
		if(inAnimation && hitbox.isStateOpen()){
			hitbox.hitboxUpdate();
		}
	}
	  
	
	public void stab(){
		hitbox = transform.Find("stabbox").GetComponent<Hitbox>();
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
		strength = attackdict[hitbox.attackname];
		collider.GetComponentInParent<Enemy>().TakeDamage(strength, attackid);
	}
	
	//This is used to see a red circle around gizmos when drawing them in Unity scene editor
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
//		Gizmos.DrawWireSphere(attackPos.position, attackRange);
	}
}
