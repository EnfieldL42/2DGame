using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public float initialSpeed = 10f;
    public float speedMultiplier = 1.1f;
    public float maxSpeed = 30f;
    private Vector2 currentDirection;
    private float currentSpeed;

    void Start()
    {
        print("refeclt");

        currentSpeed = initialSpeed;
        currentDirection = Vector2.right; // Initial direction
    }

    void Update()
    {
        transform.Translate(currentDirection * currentSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BounceOffPlayer(collision);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            ReflectOffWall(collision);
        }
    }

    void BounceOffPlayer(Collision2D collision)
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
        print("refeclt");
        currentDirection = Vector2.Reflect(currentDirection, collision.contacts[0].normal);
    }
}
