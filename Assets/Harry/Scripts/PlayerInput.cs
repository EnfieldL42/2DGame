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
        ball = FindObjectOfType<BallPhysics>();
        gameManager = FindObjectOfType<GameManager>();

        col = GetComponent<Collider2D>();

        rb = GetComponent<Rigidbody2D>();
        rb.excludeLayers.Equals("Player");
        rb.mass = 1f;
        rb.gravityScale = 8;

        anim = GetComponent<Animator>();
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

    public void PlayerHit()
    {
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
    /*IEnumerator DisableAttack()
    {
        inputControls.BallControls.Attack.performed -= AttackPerformed;
        yield return new WaitForSeconds(0.4f);
        inputControls.BallControls.Attack.performed += AttackPerformed;
    }*/
}
