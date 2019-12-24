using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	
	private float timeBtwAttack;
	public float startTimeBtwAttack;
	
	public float strength;
	
	public Transform attackPos;
	public float attackRange;
	public LayerMask whatIsEnemies;
	
    // Start is called before the first frame update
    void Start()
    {
        strength = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBtwAttack <= 0){
			
			if(Input.GetKey(KeyCode.LeftShift)){
				Debug.Log("Attacked!");
				timeBtwAttack = startTimeBtwAttack;
				Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
				for(int i = 0; i < enemiesToDamage.Length; i++){
					Debug.Log("Got autowalker component");
					enemiesToDamage[i].GetComponent<Autowalker>().TakeDamage(strength);
				}
			}
			
			
		}
		else
		{
			timeBtwAttack -= Time.deltaTime;
		}
    }
	
	void Attack(){
		
	}
	
	
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPos.position, attackRange);
	}
}
