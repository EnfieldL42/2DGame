using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class controllerp1 : MonoBehaviour
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

    public bool isMoving;
    public Vector2 overlapBoxSize = new Vector2(0.8f, 0.8f);

    private BoxCollider2D boxCollider;
    private Transform spriteTransform;
    private Vector2 lastTargetPos;

    private void Start()
    {
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
    }

    private void Update()
    {
        if (!isMoving)
        {
            if (input != Vector2.zero)
            {
                var targetPos = (Vector2)transform.position + input;

                targetPos = SnapToGrid(targetPos);

                UpdateColliderRotation();

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }
                else
                {
                    UpdateColliderRotation();
                }

                lastTargetPos = targetPos;
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
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, overlapBoxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            ItemStation station = collider.GetComponent<ItemStation>();
            if (station != null)
            {
                PlayerInventory playerInventory = GetComponent<PlayerInventory>();
                if (playerInventory != null && playerInventory.inventory.Count < playerInventory.maxItems)
                {
                    playerInventory.CollectItem(station.itemID);
                }
                break;
            }
        }
    }
}