using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public float initialSpeed = 10f;
    public float speedMultiplier = 1.1f;
    public float maxSpeed = 30f;

    public float debounce;

    private Vector2 currentDirection;
    public float currentSpeed;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Color color;
    

    public int lastHit;
    public GameManager gameManager;
    //public HitStop hitStop;

    void Awake()
    {
        debounce = 0;
        lastHit = 100;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero * initialSpeed;
        currentSpeed = initialSpeed;
        rb.gravityScale = 0f;
    }


    private void Update()
    {
        debounce += 1f * Time.deltaTime;
        currentDirection = rb.velocity.normalized;
    }

   /* void OnCollisionEnter2D(Collision2D collision)
    {if (collision.gameObject.CompareTag("Wall")||collision.gameObject.CompareTag("Ground"))
        {
            ReflectOffWall(collision);
        }
    }
   */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BallHit(collision);
        /*if (collision.gameObject.CompareTag("Hitbox") && debounce >= 0.12f)
        {
            debounce = 0;
            //hitStop.Stop(currentSpeed / maxSpeed);
            lastHit = collision.gameObject.GetComponentInParent<PlayerInput>().playerID;
            collision.gameObject.SetActive(false);
            ChangeColor();
        }
        else if (collision.gameObject.CompareTag("Hurtbox") && collision.gameObject.GetComponentInParent<PlayerInput>().playerID != lastHit & lastHit != 100)
        {
            //hitStop.Stop(currentSpeed / maxSpeed);
            collision.gameObject.GetComponentInParent<PlayerInput>().PlayerHit();
        }
        */

    }
    void BallHit(Collider2D collision)
    {   
        if(lastHit ==100)
        {
            rb.gravityScale = 1f;
        }
        StartCoroutine(FreezePhysics(collision));
    }
    void BallLaunched(Collider2D collision)
    {
        // Calculate a new random direction within a specified angle range
        collision.gameObject.GetComponent<HitboxParameters>().GenerateAngle();
        currentDirection = collision.gameObject.GetComponent<HitboxParameters>().launchAngle;
        

        // Update the ball's velocity
        rb.velocity = currentDirection * currentSpeed;

        // Increase speed and clamp to maxSpeed
        currentSpeed *= speedMultiplier;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
    }
    /*
       void ReflectOffWall(Collision2D collision)
    {
        // Reflect the direction of the ball off the wall
        currentDirection = Vector2.Reflect(currentDirection, collision.GetContact(0).normal).normalized;
         
        // Update the ball's velocity
        rb.velocity = currentDirection * currentSpeed;

        // Lose a tiny bit of speed when hitting a wall
        currentSpeed *= 0.99f;
    }
    */

    void HitPlayer(Collider2D collision)
    {
        rb.velocity = new Vector2 (-currentDirection.x, -currentDirection.y);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    void ChangeColor()
    {
        if(lastHit == GameManager.playerOne)
        {
            spriteRenderer.color = Color.red;
        }
        else if (lastHit == GameManager.playerTwo)
        {
            spriteRenderer.color = Color.blue;
        }
    }

    IEnumerator FreezePhysics(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hitbox") && debounce >= 0.12f)
        {
            StartCoroutine(collision.gameObject.GetComponentInParent<PlayerInput>().FreezeControls(currentSpeed / maxSpeed));
            debounce = 0;
            //hitStop.Stop(currentSpeed / maxSpeed);
            lastHit = collision.gameObject.GetComponentInParent<PlayerInput>().playerID;
            collision.gameObject.SetActive(false);
            ChangeColor();
            rb.simulated = false;
            yield return new WaitForSecondsRealtime(currentSpeed / maxSpeed);
            rb.simulated = true;
            BallLaunched(collision);
        }
        else if (collision.gameObject.CompareTag("Hurtbox") && collision.gameObject.GetComponentInParent<PlayerInput>().playerID != lastHit & lastHit != 100)
        {
            StartCoroutine(collision.gameObject.GetComponentInParent<PlayerInput>().FreezeControls(currentSpeed / maxSpeed));
            //hitStop.Stop(currentSpeed / maxSpeed);
            collision.gameObject.GetComponentInParent<PlayerInput>().PlayerHit();
            rb.simulated = false;
            yield return new WaitForSecondsRealtime(currentSpeed / maxSpeed);
            rb.simulated = true;
            HitPlayer(collision);
        }
    }
}
