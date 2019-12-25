using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : IHitboxResponder
{	

	public Hitbox hitbox;

	
	
	public void stab(){
		hitbox.useResponder(this);
		hitbox.startCheckingCollision();
		Debug.Log("stabbing!");
	}
	
	public void collisionedWith(Collider collider){
		//collider[i] .takedamage
	}
}
