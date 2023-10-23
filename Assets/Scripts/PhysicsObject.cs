using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float gravityModifier = 1f;

    protected Rigidbody2D rb2d;
    protected Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
		velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 move = Vector2.up * deltaPosition.y;
        Movement(move);
	}

    void Movement(Vector2 move)
    {
		rb2d.position += move;
	}
}
