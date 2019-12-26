using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Hitbox hitbox;
	public float hp;
	public float strength;
	public float attackRadius;
	public float speed;
	public Rigidbody2D rb;
	public float detectionRadius;
	public List<int> hitlist;
	public LayerMask whatIsPlayer;
	public bool dead = false;
	public Dictionary<string, int> attackdict;
	public bool inAnimation;
	public double startTimeBtwAttack;
	public double timeBtwAttack;
	
	
	public void TakeDamage(float damage, int attackid)
	{
		if(!hitlist.Contains(attackid)){ hp -= damage; hitlist.Add(attackid); 
		Debug.Log("Took damage! Hp is now: " + hp);
		}
	}
	
	public void clearHitlist()
	{
		hitlist.Clear();
	}
	
	public bool detectPlayer()
	{
		if(Physics2D.OverlapCircleAll(rb.position, detectionRadius, whatIsPlayer).Length > 0){
			return true;
		}
		return false;
	}
	
	public bool canAttack()
	{
		if(Physics2D.OverlapCircleAll(rb.position, attackRadius, whatIsPlayer).Length > 0 && !inAnimation){
			return true;
		}
		return false;
	}
	
	public void collisionedWith(Collider2D collider, int attackid){
		strength = attackdict[hitbox.attackname];
		collider.GetComponentInParent<Player>().TakeDamage(strength, attackid);
	}
	
	
	public virtual void Death()
	{
		Debug.Log("Died a generic death");
	}
	
}
