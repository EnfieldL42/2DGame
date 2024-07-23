using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballphysic : MonoBehaviour
{
    public float initialSpeed = 10f;
    public float speedMultiplier = 1.1f;
    public float maxSpeed = 30f;
    private Vector2 currentDirection;
    private float currentSpeed;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.right * initialSpeed;
        currentSpeed = initialSpeed;
        currentDirection = rb.velocity.normalized;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void FixedUpdate()
    {
        rb.velocity = currentDirection * currentSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Collided with Wall");
            ReflectOffWall(collision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with Player");
            BounceOffPlayer(collision);
        }
    }

    void BounceOffPlayer(Collider2D collision)
    {
        // Calculate a new random direction within a specified angle range
        float randomAngle = Random.Range(-45f, 45f);
        currentDirection = Quaternion.Euler(0, 0, randomAngle) * -currentDirection;

        // Increase speed and clamp to maxSpeed
        currentSpeed *= speedMultiplier;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
    }

    void ReflectOffWall(Collision2D collision)
    {
        // Reflect the direction of the ball off the wall
        currentDirection = Vector2.Reflect(currentDirection, collision.contacts[0].normal).normalized;
    }
}
