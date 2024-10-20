using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public int playerID;
    //public MultiplayerInputManager inputManager;
    InputControls inputControls;
    public Rigidbody2D rb;
    public Animator anim;
    public GameManager gameManager;
    public BallPhysics ball;
    public Collider2D col;
    public SpriteRenderer spriteRenderer;

    public bool canJump;
    public Vector2 stickCoord;
    public bool facingRight;
    private float horizontal;
    public float speed = 5f;
    public float jumpForce = 10f;
    bool playerActive;

    // Start is called before the first frame update
    void Awake()
    {
        rb.excludeLayers.Equals("Player");
        rb.mass = 1f;
        rb.gravityScale = 8;

        if(playerID == 0)
        {
            playerID = GameManager.playerOne;
        }
        else
        {
            playerID = GameManager.playerTwo;

        }

        if (MultiplayerInputManager.instance.players.Count > playerID)
        {
            AssignInputs(playerID);
        }
        else
        {
            MultiplayerInputManager.instance.onPlayerJoined += AssignInputs;
        }

        SetColor();
    }
    private void Update()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (!facingRight && horizontal < 0f)
        {
            Flip();
        }
        else if (facingRight && horizontal > 0f)
        {
            Flip();
        }

        if(playerActive == true)
        {
            stickCoord = inputControls.BallControls.Movement.ReadValue<Vector2>();
            anim.SetFloat("StickX", stickCoord.x);
            anim.SetFloat("StickY", stickCoord.y);
        }

        if(stickCoord.x !=0 && canJump == true) 
        {
            AudioManager.instance.FastSFXOnce("Footstep", 1.5f);
        }
        
    }

    void AssignInputs(int ID)
    {
        if(playerID == ID)
        {
            MultiplayerInputManager inputManager = MultiplayerInputManager.instance;

            inputManager.onPlayerJoined -= AssignInputs;
            inputControls = inputManager.players[playerID].playerControls;
            inputControls.BallControls.Movement.performed += MovementPerformed;
            inputControls.BallControls.Jump.performed += JumpPerformed;
            inputControls.BallControls.Jump.canceled += JumpCanceled;
            inputControls.BallControls.Attack.performed += AttackPerformed;

            playerActive = true;
        }
    }

    private void AttackPerformed(InputAction.CallbackContext context)
    {
        AudioManager.instance.PlaySFX("Attack", playerID);
        anim.SetTrigger("Attack");
        //StartCoroutine(DisableAttack());
    }

    private void JumpCanceled(InputAction.CallbackContext context)
    {
        if (rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void JumpPerformed(InputAction.CallbackContext context)
    {
        AudioManager.instance.PlaySFX("Jump", playerID);
        if (canJump == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void MovementPerformed(InputAction.CallbackContext context)
    {

            horizontal = context.ReadValue<Vector2>().x;
        
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            anim.SetBool("Airborne", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
            anim.SetBool("Airborne", true);
        }
    }
    void SetColor()
    {
        switch(playerID)
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

    public void PlayerHit()
    {
        AudioManager.instance.PlaySFX("Death", playerID);
        rb.mass = 0;
        rb.gravityScale = 0;
        rb.velocity = ball.rb.velocity * ball.initialSpeed;
        col.enabled = false;
        int id = playerID;
        gameManager.GameEnded(id);
    }

    public void DisableControls()
    {
       playerActive = false;
       inputControls.BallControls.Movement.performed -= MovementPerformed;
       inputControls.BallControls.Jump.performed -= JumpPerformed;
       inputControls.BallControls.Jump.canceled -= JumpCanceled;
       inputControls.BallControls.Attack.performed -= AttackPerformed;
       inputControls.Disable();
       
    }
    public void EnableControls()
    {
        playerActive = true;
        inputControls.BallControls.Movement.performed += MovementPerformed;
        inputControls.BallControls.Jump.performed += JumpPerformed;
        inputControls.BallControls.Jump.canceled += JumpCanceled;
        inputControls.BallControls.Attack.performed += AttackPerformed;
        inputControls.Enable();
        
    }
    /*IEnumerator DisableAttack()
    {
        inputControls.BallControls.Attack.performed -= AttackPerformed;
        yield return new WaitForSeconds(0.4f);
        inputControls.BallControls.Attack.performed += AttackPerformed;
    }*/

    public IEnumerator FreezeControls(float duration)
    {
        rb.simulated = false;
        DisableControls();
        
        yield return new WaitForSecondsRealtime(duration);
        rb.simulated = true;
        EnableControls();
    }
}
