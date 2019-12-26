using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public Collider collider;
	public Vector3 boxSize;
	public float rotation;
	public Color inactiveColor;
	public Color collisionOpenColor;
	public Color collidingColor;
	
	private ColliderState _state = ColliderState.Open;
	
	
	public enum ColliderState{
		Closed,
		Open,
		Colliding
		}
	
	
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawCube(transform.position, new Vector3(boxSize.x*2, boxSize.y*2, boxSize.z*2));
	}
	
}
