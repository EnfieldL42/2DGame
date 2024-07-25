using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public int playerID;
    public MultiplayerInputManager inputManager;
    InputControls inputControls;
    public Rigidbody2D rb;
    public Animator anim;

    public bool canJump;
    private bool facingRight;
    private float horizontal;
    public float speed = 5f;
    public float jumpForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inputManager.onPlayerJoined += AssignInputs;
     
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
    }

    void AssignInputs(int ID)
    {
        if(playerID == ID)
        {
            inputManager.onPlayerJoined -= AssignInputs;
            inputControls = inputManager.players[playerID].playerControls;
            inputControls.MasterControls.Movement.performed += MovementPerformed;
            inputControls.MasterControls.Jump.performed += JumpPerformed;
            inputControls.MasterControls.Jump.canceled += JumpCanceled;
            inputControls.MasterControls.Attack.performed += AttackPerformed;
        }
    }

    private void AttackPerformed(InputAction.CallbackContext context)
    {
        anim.SetTrigger("Attack");
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
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }
    }
}
