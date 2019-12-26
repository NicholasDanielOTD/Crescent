using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float hp;
	
	public float speed;
	public Rigidbody2D rb;
	public float detectionRadius;
	public List<int> hitlist;
	
	public LayerMask whatIsPlayer;
	
	
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
	
	
	public virtual void Death()
	{
		Debug.Log("Died a generic death");
	}
	
}
