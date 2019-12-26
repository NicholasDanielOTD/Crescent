using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHitboxResponder
{
	
	public Hitbox hitbox;
	
	private float timeBtwAttack;
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
		hitbox = GetComponentInChildren<Hitbox>();
		Debug.Log(hitbox);
    }

    // Update is called once per frame
    void Update()
    {
		//If can attack
        if(timeBtwAttack <= 0 && !hitbox.isStateOpen()){
			if(Input.GetKey(KeyCode.LeftShift)){
				stab();
			}
		}
		else
		{
			timeBtwAttack -= Time.deltaTime;
		}
		
		
    }
	  
	  
	void FixedUpdate(){
		
		if(hitbox.isStateOpen()){
			hitbox.hitboxUpdate();
		}
	}
	  
	
	public void stab(){
		timeBtwAttack = startTimeBtwAttack;
		hitbox.useResponder(this);
		hitbox.startCheckingCollision();
		hitbox.name = "stab";
		inAnimation = true;
		Debug.Log("stabbing!");
	}
	
	public void collisionedWith(Collider2D collider, int attackid){
		Debug.Log("collided!");
		collider.GetComponentInParent<Enemy>().TakeDamage(50, attackid);
	}
	
	//This is used to see a red circle around gizmos when drawing them in Unity scene editor
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
//		Gizmos.DrawWireSphere(attackPos.position, attackRange);
	}
}
