using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    public float initialSpeed = 10f;
    public float speedMultiplier = 1.1f;
    public float maxSpeed = 30f;
    public float currentSpeed;
    public float gravityScale;
    public float deceleration;


    private Vector2 currentDirection;
    public float hitSpeed;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem actionLines;
    public ParticleSystem hitParticle;
    

    public int lastHit;
    public GameManager gameManager;
    //public HitStop hitStop;

    void Awake()
    {
        lastHit = 100;
        rb.velocity = Vector2.zero * initialSpeed;
        hitSpeed = initialSpeed;
    }


    private void Update()
    {
        currentDirection = rb.velocity.normalized;
        
        if(currentDirection != Vector2.zero )
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, currentDirection );
        }

        //StartCoroutine(SpeedCalc());
        if(lastHit != 100)
        {
            rb.gravityScale = gravityScale;
            gravityScale = ((-2*currentSpeed / 75) + 2);
            //Mathf.Clamp(gravityScale, 0f, 1f);
        }

        gravityScale = Mathf.Clamp(gravityScale, 0f, 2f);
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);


    }
    IEnumerator Decelerate()
    {
        while( lastHit != 100 )
        {
            yield return new WaitForSeconds(0.1f);
            currentSpeed -= deceleration;
            
            Debug.Log("Decelerate");
        }
        yield return null;
    }
    IEnumerator SpeedCalc()
    {
        Vector3 lastPos = transform.position;
        yield return new WaitForFixedUpdate();
        currentSpeed = (lastPos - transform.position).magnitude /Time.deltaTime;

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
            StartCoroutine(FreezePhysics(collision));
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            AudioManager.instance.PlaySFX("Ball Bounce", 4);
            ReflectOffWall(collision);
        }
    }
    /*void BallHit(Collider2D collision)
    {   
        if(lastHit ==100)
        {
            rb.gravityScale = 1f;
        }
        StartCoroutine(FreezePhysics(collision));
    } */

    void BallLaunched(Collider2D collision)
    {
        // Calculate a new random direction within a specified angle range
        collision.gameObject.GetComponent<HitboxParameters>().GenerateAngle();
        currentDirection = collision.gameObject.GetComponent<HitboxParameters>().launchAngle;
        

        // Update the ball's velocity
        rb.velocity = currentDirection * hitSpeed;

        // Increase speed and clamp to maxSpeed
        hitSpeed *= speedMultiplier;
        hitSpeed = Mathf.Min(hitSpeed, maxSpeed);
        currentSpeed = hitSpeed;
    }
    
       void ReflectOffWall(Collision2D collision)
    {
        // Reflect the direction of the ball off the wall
        currentDirection = Vector2.Reflect(currentDirection, collision.GetContact(0).normal).normalized;
         
        // Update the ball's velocity
        rb.velocity = currentDirection * currentSpeed;

        // Lose a tiny bit of speed when hitting a wall
        //currentSpeed *= 0.99f;
    }
    

    void HitPlayer(Collider2D collision)
    {
        rb.velocity = new Vector2 (-currentDirection.x, -currentDirection.y);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    void ChangeColor()
    {
        switch (lastHit)
        { 
            case 0:
                spriteRenderer.color = Color.red;
                break;
            case 1:
                spriteRenderer.color = Color.blue; 
                break;
            case 2:
                spriteRenderer.color = Color.green;
                break;
            case 3:
                spriteRenderer.color = Color.yellow;
                break;
        }
    }

    IEnumerator FreezePhysics(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hitbox"))
        {
            if (hitSpeed <= 75)
            {
                AudioManager.instance.PlaySFX("Ball Hit", 4);
            }
            else if (hitSpeed >=75)
            {
                AudioManager.instance.PlaySFX("Ball Hit Fast", 4);
            }
            if (lastHit == 100)
            {
                lastHit = collision.gameObject.GetComponentInParent<PlayerInput>().playerID;
                StartCoroutine(Decelerate());
            }
            actionLines.gameObject.SetActive(true);
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            StartCoroutine(collision.gameObject.GetComponentInParent<PlayerInput>().FreezeControls(hitSpeed / maxSpeed));
            //hitStop.Stop(currentSpeed / maxSpeed);
            lastHit = collision.gameObject.GetComponentInParent<PlayerInput>().playerID;
            collision.gameObject.SetActive(false);
            ChangeColor();
            rb.simulated = false;
            yield return new WaitForSecondsRealtime(hitSpeed / maxSpeed);
            actionLines.gameObject.SetActive(false);
            rb.simulated = true;
            BallLaunched(collision);
        }
        else if (collision.gameObject.CompareTag("Hurtbox") && collision.gameObject.GetComponentInParent<PlayerInput>().playerID != lastHit & lastHit != 100)
        {
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            StartCoroutine(collision.gameObject.GetComponentInParent<PlayerInput>().FreezeControls(hitSpeed / maxSpeed));
            //hitStop.Stop(currentSpeed / maxSpeed);
            collision.gameObject.GetComponentInParent<PlayerInput>().PlayerHit();
            rb.simulated = false;
            yield return new WaitForSecondsRealtime(hitSpeed / maxSpeed);
            rb.simulated = true;
            HitPlayer(collision);
        }
    }
}
