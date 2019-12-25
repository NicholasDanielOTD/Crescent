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
	public PolygonCollider2D weapon;
	
	public ContactFilter2D filter;
	public Collider2D[] enemiesToDamage;
	
    // Start is called before the first frame update
    void Start()
    {
		weapon = GetComponent<PolygonCollider2D>();
        strength = 50;
		filter.layerMask = whatIsEnemies;
    }

    // Update is called once per frame
    void Update()
    {
		//If can attack
        if(timeBtwAttack <= 0){
			if(Input.GetKey(KeyCode.LeftShift)){
				Attack();
			}
		}
		else
		{
			timeBtwAttack -= Time.deltaTime;
		}
    }
	  
	  
	void Attack(){
		Debug.Log("Attacked!");
		//Reset timer between attacks
		timeBtwAttack = startTimeBtwAttack;
		//Check hittable enemies in a circle around player
		weapon.OverlapCollider(filter,enemiesToDamage);
		//For each enemy, do damage
		for(int i = 0; i < enemiesToDamage.Length; i++){
			enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(strength);
		}
	}
	
	//This is used to see a red circle around gizmos when drawing them in Unity scene editor
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackPos.position, attackRange);
	}
}
