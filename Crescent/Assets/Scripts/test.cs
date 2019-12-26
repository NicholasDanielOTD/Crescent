using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
	
	public LayerMask hurtboxMask;
	public Vector3 boxSize;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position,new Vector2 (boxSize.x,boxSize.y),0,hurtboxMask);
		Debug.Log(colliders.Length);
    }
}
