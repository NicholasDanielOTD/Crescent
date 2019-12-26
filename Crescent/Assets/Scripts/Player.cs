using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHitboxResponder
{
	
	public Hitbox hitbox;
	
	private double timeBtwAttack;
	public float startTimeBtwAttack;
	public bool inAnimation;
	public float strength;

	public float attackRange;
	public LayerMask whatIsEnemies;
	
	public ContactFilter2D filter;
	public Collider2D[] enemiesToDamage;
	
	
    // Start is called before the first frame update
    void Start()
    {
        strength = 50;
		filter.layerMask = whatIsEnemies;
		
    }

    // Update is called once per frame
    void Update()
    {
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
	  
	  
	void FixedUpdate(){
		
		if(inAnimation && hitbox.isStateOpen()){
			hitbox.hitboxUpdate();
		}
	}
	  
	
	public void stab(){
		Debug.Log(transform.Find("stabbox"));
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
	
	
	public void collisionedWith(Collider2D collider, int attackid){
		collider.GetComponentInParent<Enemy>().TakeDamage(50, attackid);
	}
	
	//This is used to see a red circle around gizmos when drawing them in Unity scene editor
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
//		Gizmos.DrawWireSphere(attackPos.position, attackRange);
	}
}
