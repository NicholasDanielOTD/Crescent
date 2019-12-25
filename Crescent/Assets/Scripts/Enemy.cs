using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public float hp;
	
	public float speed;
	public Rigidbody2D rb;
	public float detectionRadius;
	
	public LayerMask whatIsPlayer;
	
    // Start is called before the first frame update
    void Start()
    {
       
    }

	
	public void TakeDamage(float damage)
	{
		hp -= damage;
		Debug.Log("Took damage! Hp is now: " + hp);
	}
	
	
	public virtual void Death()
	{
		Debug.Log("Died a generic death");
	}
	
}
