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
	public Vector3 originalPosition;
	public Vector3 originalDistance;
	public bool isMoving = false;
	public Vector2 _end;
	public double _rate;
	private Transform parentObj;
	
	
	//A state for describing what our collider is doing at the moment
	private ColliderState _state;
	
	void Start()
	{
		inactiveColor = new Color(0,0,255,.5f);
		collisionOpenColor = new Color(255,255,0,.5f);
		collidingColor = new Color(255,0,0,.5f);

	
	}
	
	
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
		Gizmos.color = CheckGizmoColor();
		Gizmos.DrawCube(transform.position, new Vector3(boxSize.x*2, boxSize.y*2, boxSize.z*2));
	}
	
	
	//Sets the hitbox color based on state, currently does not work because gizmos aren't properly configured
	private Color CheckGizmoColor()
	{
		switch(_state) {

		case ColliderState.Closed:

			return inactiveColor;

		case ColliderState.Open:

			return  collisionOpenColor;

	
		case ColliderState.Colliding:

			return  collidingColor;
			
		default:	
			return Color.yellow;
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
	
	public void move(Vector2 end, double rate)
	{
		originalDistance = transform.position - transform.parent.position;
		originalPosition = transform.position;
		isMoving = true;
		parentObj = transform.parent;
		transform.parent = null;
		_rate = rate;
		_end = end;
		
	}

	public void cancelAttack()
	{
		isMoving = false;
		stopCheckingCollision();
	}
	
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
		
		if(isMoving)
		{
			Vector3 pos = new Vector3((float)(transform.position.x+ _end.x/_rate),(float)(transform.position.y + _end.y/_rate),(transform.position.z));
			transform.position = pos;
			
			if(Mathf.Abs(transform.position.x -  (originalPosition.x + _end.x)) < .3 && Mathf.Abs(transform.position.y - (originalPosition.y + _end.y)) < .3)
			{
				isMoving = false;
				transform.parent = parentObj;
				transform.position = transform.parent.position + originalDistance;
				
			}
		}
		
		//Set the state to colliding if colliding else, se it to open
		_state = colliders.Length > 0 ? ColliderState.Colliding : ColliderState.Open;
    }
}
