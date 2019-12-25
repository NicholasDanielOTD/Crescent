﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This interface allows for the inclusion of the collisionedWith void into other classes
public interface IHitboxResponder {
		void collisionedWith(Collider collider);
	}


public class Hitbox : MonoBehaviour
{
	//This creates the responder
	private IHitboxResponder _responder = null;
	
	public Vector3 boxSize;
	public float rotation;
	public LayerMask mask;
	public Color inactiveColor;
	public Color collisionOpenColor;
	public Color collidingColor;

	private ColliderState _state;
	
	//Adds a ColliderState type with several possible states
	public enum ColliderState{
		Closed,
		Open,
		Colliding
		}
		
	//Gives a callable to set local responder of Hitbox
	public void useResponder(IHitboxResponder responder){
		_responder = responder;
	}
	
	
	//Makes the hitbox in the Unity screen
	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(transform.position, new Vector3(boxSize.x*2, boxSize.y*2, boxSize.z*2));
	}
	
	
	//Sets the hitbox color based on state
	private void CheckGizmoColor()
	{
		switch(_state) {

		case ColliderState.Closed:

			Gizmos.color = inactiveColor;

			break;

		case ColliderState.Open:

			Gizmos.color = collisionOpenColor;

			break;
	
		case ColliderState.Colliding:

			Gizmos.color = collidingColor;

			break;
		}
	}

	
	//Turn on and off collision checking when hitbox is out
	public void startCheckingCollision()
	{
		_state = ColliderState.Open;
	}
	
	public void stopCheckingCollision()
	{
		_state = ColliderState.Closed;
	}
	
	

    // Update is called once per frame
    void hitboxUpdate()
    {
		//If no hitbox out, do nothing
        if (_state == ColliderState.Closed) {return;}
		
		//Get list of colliding entities
		Collider[] colliders = Physics.OverlapBox(transform.position,boxSize,transform.rotation,mask);
		
		//For each collision, if it has a responder, report that the collision has happened
		for (int i = 0; i < colliders.Length; i++)
		{
			Collider aCollider = colliders[i];
			_responder?.collisionedWith(aCollider);
		}
		
		//Set the state to colliding if colliding else, se it to open
		_state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
    }
}