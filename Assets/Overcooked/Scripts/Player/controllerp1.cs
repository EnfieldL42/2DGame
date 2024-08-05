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
    private Animator animator;
    private Vector2 lastTargetPos;

    private void Start()
    {
        moveSpeed = walkingSpeed;
        boxCollider = GetComponent<BoxCollider2D>();

        Transform spriteObject = transform.GetChild(0);
        spriteTransform = spriteObject;
        animator = spriteObject.GetComponent<Animator>();

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
                Vector2 filteredInput = FilterInput(input);
                var targetPos = (Vector2)transform.position + filteredInput;
                targetPos = SnapToGrid(targetPos);

                if (IsWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                }

                lastTargetPos = targetPos;
            }
        }
    }

    private Vector2 FilterInput(Vector2 rawInput)
    {
        if (Mathf.Abs(rawInput.x) > Mathf.Abs(rawInput.y))
        {
            return new Vector2(Mathf.Sign(rawInput.x), 0);
        }
        else
        {
            return new Vector2(0, Mathf.Sign(rawInput.y));
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
        Collider2D collider = Physics2D.OverlapBox(targetPos, overlapBoxSize, 0, solidObjectsLayer);
        return collider == null;
    }

    private Vector2 SnapToGrid(Vector2 position)
    {
        position.x = Mathf.Floor(position.x) + 0.5f;
        position.y = Mathf.Floor(position.y) + 0.7f;
        return position;
    }

    private void UpdateAnimatorParameters()
    {
        if (input.x > 0)
        {
            animator.SetBool("isMovingRight", true);
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingUp", false);
            animator.SetBool("isMovingDown", false);
        }
        else if (input.x < 0)
        {
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isMovingLeft", true);
            animator.SetBool("isMovingUp", false);
            animator.SetBool("isMovingDown", false);
        }
        else if (input.y > 0)
        {
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingUp", true);
            animator.SetBool("isMovingDown", false);
        }
        else if (input.y < 0)
        {
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingUp", false);
            animator.SetBool("isMovingDown", true);
        }
        else
        {
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingUp", false);
            animator.SetBool("isMovingDown", false);
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.red;
        Vector2 direction = (Vector2)transform.position + input - (Vector2)transform.position;
        Gizmos.DrawWireCube((Vector2)transform.position + direction, overlapBoxSize);

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
        UpdateAnimatorParameters();
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
                if (playerInventory != null && playerInventory.CollectItem(station.itemID))
                {
                    ItemDisplay itemDisplay = GetComponentInChildren<ItemDisplay>();
                    if (itemDisplay != null)
                    {
                        itemDisplay.UpdateItemDisplay();  // Update item display after collecting an item
                    }
                }
                break;
            }
        }
    }
}
