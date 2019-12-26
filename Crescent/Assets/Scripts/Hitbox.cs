using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script intended to define what a hitbox is for more robust use of hitboxes

//This interface allows for the inclusion of the collisionedWith void into other classes
public interface IHitboxResponder {
		void collisionedWith(Collider2D collider, int attackid);
	}


public class Hitbox : MonoBehaviour
{
	//This creates the responder
	private IHitboxResponder _responder = null;
	
	public string attackname;
	private int attackID;
	public Vector3 boxSize;
	public float rotation;
	public LayerMask hurtboxMask;
	public Color inactiveColor;
	public Color collisionOpenColor;
	public Color collidingColor;
	public double hitDelay;
	
	//A state for describing what our collider is doing at the moment
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
	
	
	//Sets the hitbox color based on state, currently does not work because gizmos aren't properly configured
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

	
	//Turn on and off collision checking when hitbox is out, assign an attackid when a hitbox opens
	public void startCheckingCollision()
	{
		_state = ColliderState.Open;
		attackID = (int) Mathf.Round(Random.value*1000);
	}
	
	public void stopCheckingCollision()
	{
		_state = ColliderState.Closed;
	}
	
	//a callable check to see if colliding is okay
	public bool isStateOpen(){
		return _state == ColliderState.Open || _state == ColliderState.Colliding;
	}
	

    // Update is called once per frame
    public void hitboxUpdate()
    {
		//Debug.Log("Updating");
		//If no hitbox out, do nothing
        if (_state == ColliderState.Closed || hitDelay > 0) { hitDelay -= Time.deltaTime; return;}
		
		//Get list of colliding entities
		Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position,new Vector2 (boxSize.x,boxSize.y),0,hurtboxMask);
		//For each collision, if it has a responder, report that the collision has happened
		for (int i = 0; i < colliders.Length; i++)
		{
			Collider2D aCollider = colliders[i];
			_responder?.collisionedWith(aCollider, attackID);
			
		}
		
		
		//Set the state to colliding if colliding else, se it to open
		_state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
    }
}
