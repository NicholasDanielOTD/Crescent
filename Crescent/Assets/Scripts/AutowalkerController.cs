using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutowalkerController : MonoBehaviour
{
	
	public float speed;
	private Rigidbody2D rb;
	
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }
}
