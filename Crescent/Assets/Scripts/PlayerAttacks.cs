using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour, IHitboxResponder
{	

	public Hitbox hitbox;

	
	
	public void stab(){
		hitbox.useResponder(this);
	}
	
	public void collisionedWith(Collider collider){
		
	}
}
