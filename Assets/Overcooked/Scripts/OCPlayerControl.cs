using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum Direction
{
    up, down, left, right
}

public class OCPlayerControl : MonoBehaviour
{
    public int playerID;
    InputControls inputControls;

    public OCGameManager gameManager;

    public Vector2 input;


    public LayerMask solidObjectsLayer;

    public float moveSpeed;
    public float runningSpeed;
    public float walkingSpeed;
    public bool canAttack = false;

    public Vector2 overlapBoxSize = new Vector2(0.8f, 0.8f);

    private BoxCollider2D boxCollider;
    private Transform spriteTransform;

    public Transform movePoint;

    private Vector2 lastTargetPos;
    Vector3 TargetPosition;
    Direction Direction;

    private void Start()
    {
        movePoint.parent = null;


        moveSpeed = walkingSpeed;
        boxCollider = GetComponent<BoxCollider2D>();
        spriteTransform = transform.GetChild(0);

        if (MultiplayerInputManager.instance.players.Count > playerID)
        {
            AssignInputs(playerID);
        }
        else
        {
            MultiplayerInputManager.instance.onPlayerJoined += AssignInputs;
        }

        TargetPosition = new Vector2(transform.position.x, transform.position.y);
        Direction = Direction.down;
    }

    private void Update()
    {
        if (Mathf.Abs(input.x) == 1f)
        {
            movePoint.position += new Vector3(input.x, 0f, 0f);
        }
        if (Mathf.Abs(input.y) == 1f)
        {
            movePoint.position += new Vector3(0f, input.y, 0f);
        }
    }
    private Vector2 SnapToGrid(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }


    bool GetCollision
    {
        get
        {
            RaycastHit2D rh;

            Vector2 dir = Vector2.zero;

            if (Direction == Direction.down)
                dir = Vector2.down;

            if (Direction == Direction.left)
                dir = Vector2.left;

            if (Direction == Direction.right)
                dir = Vector2.right;

            if (Direction == Direction.up)
                dir = Vector2.up;

            rh = Physics2D.Raycast(transform.position, dir, 1, solidObjectsLayer);

            return rh.collider != null;
        }
    }


    private bool IsWalkable(Vector2 targetPos)
    {
        Collider2D collider = Physics2D.OverlapBox(targetPos, overlapBoxSize, 0, solidObjectsLayer);
        return collider == null;
    }

    private void UpdateColliderRotation()
    {
        if (input.x > 0)
        {
            RotateCollider(90); // Facing right
        }
        else if (input.x < 0)
        {
            RotateCollider(270); // Facing left
        }
        else if (input.y > 0)
        {
            RotateCollider(180); // Facing up
        }
        else if (input.y < 0)
        {
            RotateCollider(0); // Facing down
        }
    }

    private void Move()
    {

        if (input != Vector2.zero && TargetPosition == transform.position)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {

                if (input.x > 0)
                {
                    Direction = Direction.right;

                    if (!GetCollision)
                        TargetPosition += Vector3.right;
                }
                else
                {
                    Direction = Direction.left;

                    if (!GetCollision)
                        TargetPosition += Vector3.left;
                }



            }
            else
            {
                if (input.y > 0)
                {
                    Direction = Direction.up;

                    if (!GetCollision)
                        TargetPosition += Vector3.up;
                }
                else
                {
                    Direction = Direction.down;

                    if (!GetCollision)
                        TargetPosition += Vector3.down;
                }
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition, moveSpeed * Time.deltaTime);
    
    }

    private void RotateCollider(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
        spriteTransform.rotation = Quaternion.Euler(0, 0, 0); 
    }



    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.red;
        Vector2 direction = (Vector2)transform.position + input - (Vector2)transform.position;
        Gizmos.DrawWireCube((Vector2)transform.position + direction, overlapBoxSize);

        // Draw the overlap box for visualization
        if (lastTargetPos != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(lastTargetPos, overlapBoxSize);
        }
    }


    void AssignInputs(int ID)
    {
        if (playerID == ID)
        {
            MultiplayerInputManager inputManager = MultiplayerInputManager.instance;

            inputManager.onPlayerJoined -= AssignInputs;
            inputControls = inputManager.players[playerID].playerControls;
            inputControls.MasterControls.Movement.performed += MovementPerformed;
            inputControls.MasterControls.Jump.performed += RunningPerformed;
            inputControls.MasterControls.Jump.canceled += WalkingPerformed;
            inputControls.MasterControls.Attack.performed += InteractionPerformed;
        }
    }

    private void OnDisable()
    {
        if (inputControls != null)
        {
            inputControls.MasterControls.Movement.performed -= MovementPerformed;
            inputControls.MasterControls.Jump.performed -= RunningPerformed;
            inputControls.MasterControls.Jump.canceled -= WalkingPerformed;
            inputControls.MasterControls.Attack.performed -= InteractionPerformed;
        }
        else
        {
            MultiplayerInputManager inputManager = MultiplayerInputManager.instance;
            inputManager.onPlayerJoined -= AssignInputs;
        }
    }


    private void MovementPerformed(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }


    private void RunningPerformed(InputAction.CallbackContext context)
    {
        moveSpeed = runningSpeed;
    }

    private void WalkingPerformed(InputAction.CallbackContext context)
    {
        moveSpeed = walkingSpeed;
    }

    private void InteractionPerformed(InputAction.CallbackContext context)
    {
        if (canAttack)
        {
            gameManager.AddScore(playerID, 10); // Increase score by 10 (or any other value)
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Objectives")
        {
            canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Objectives")
        {
            canAttack = false;
        }
    }


}

