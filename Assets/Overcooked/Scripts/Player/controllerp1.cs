using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class controllerp1 : MonoBehaviour
{
    public int playerID;
    public MultiplayerInputManager inputManager;
    InputControls inputControls;

    public Vector2 input;


    public LayerMask solidObjectsLayer;

    public float moveSpeed;
    public float runningSpeed;
    public float walkingSpeed;


    public bool isMoving;
    public Vector2 overlapBoxSize = new Vector2(0.8f, 0.8f); // Customize this to fit your needs

    private BoxCollider2D boxCollider;
    private Transform spriteTransform;
    private Vector2 lastTargetPos; // To store the last target position for visualization

    private void Awake()
    {
        moveSpeed = walkingSpeed;
        boxCollider = GetComponent<BoxCollider2D>();
        spriteTransform = transform.GetChild(0); // Assumes the sprite is the first child
    }

    private void Update()
    {
        if (!isMoving)
        {
            inputManager.onPlayerJoined += AssignInputs;


            //input.x = input.x > 0 ? 1 : (input.x < 0 ? -1 : 0);
            //input.y = input.y > 0 ? 1 : (input.y < 0 ? -1 : 0);

            if (input.x != 0)
            {
                //input.y = 0;
            }

            if (input != Vector2.zero)
            {
                var targetPos = (Vector2)transform.position + input;

                // Snap the target position to the grid
                targetPos = SnapToGrid(targetPos);

                // Rotate the player regardless of movement
                UpdateColliderRotation();

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
                else
                {
                    // Rotate the player even if not walkable
                    UpdateColliderRotation();
                }

                lastTargetPos = targetPos; // Store the target position for visualization
            }
        }
    }

    private IEnumerator Move(Vector2 targetPos)
    {
        isMoving = true;

        while (((Vector2)transform.position - targetPos).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    private bool IsWalkable(Vector2 targetPos)
    {
        // Center the overlap box on the target position
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

    private void RotateCollider(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
        spriteTransform.rotation = Quaternion.Euler(0, 0, 0); // Ensure sprite stays unrotated
    }

    private Vector2 SnapToGrid(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
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
            inputManager.onPlayerJoined -= AssignInputs;
            inputControls = inputManager.players[playerID].playerControls;
            inputControls.MasterControls.Movement.performed += MovementPerformed;
            inputControls.MasterControls.Jump.performed += RunningPerformed;
            inputControls.MasterControls.Jump.canceled += WalkingPerformed;

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


}