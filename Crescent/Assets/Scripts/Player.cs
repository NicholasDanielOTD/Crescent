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
		//If can attack
        if(timeBtwAttack <= 0){
			if(Input.GetKey(KeyCode.LeftShift)){
				Debug.Log("Attacked!");
				timeBtwAttack = startTimeBtwAttack;
				//Check hittable enemies in a circle around player
				Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
				//For each enemy, do damage
				for(int i = 0; i < enemiesToDamage.Length; i++){
					enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(strength);
				}
			}
			
			
		}
		else
		{
			timeBtwAttack -= Time.deltaTime;
		}
    }
	  
	
	//This is used to see a red circle around gizmos when drawing them in Unity scene editor
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPos.position, attackRange);
	}
}
