using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;


    Vector2 movement;

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.x = movement.x > 0 ? 1 : (movement.x < 0 ? -1 : 0);
        movement.y = movement.y > 0 ? 1 : (movement.y < 0 ? -1 : 0);

        if (movement.x != 0)
        {
            movement.y = 0;
        }

    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);


    }

}
