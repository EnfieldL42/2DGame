using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class controllerp1 : MonoBehaviour
{
    public int playerID;
    private InputControls inputControls;
    public PlayerInventory playerInventory;
    public ItemDisplay itemDisplay;

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

    Animator animator;
    public RuntimeAnimatorController[] characterControllers;
    private Vector2 lastMoveDirection;

    public int interactCount = 0;
    public int interactionsNeeded;



    public CharacterDatabase characterDB;
    public SpriteRenderer artworkSprite;
    public int[] selectedOptions;


    [SerializeField] private Sprite[] characterHeadSprites;
    [SerializeField] private Image playerCharacterImage;


    private void Start()
    {
        moveSpeed = walkingSpeed;
        boxCollider = GetComponent<BoxCollider2D>();

        Transform spriteObject = transform.GetChild(0);
        spriteTransform = spriteObject;
        animator = spriteObject.GetComponent<Animator>();

        selectedOptions = new int[4];

        Load();

        UpdateCharacter(selectedOptions[playerID]);


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
        ProcessInputs();
        Animate();

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

        if(interactCount == interactionsNeeded)
        {
            interactCount = 0;
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

    private void ProcessInputs()
    {

        float moveX = input.x;
        float moveY = input.y;

        if((input.x != 0 || input.y != 0))
        {
            lastMoveDirection = input;
        }
    }

    void Animate()
    {
        //if (isMoving)
        //{
        //    animator.SetBool("isMoving", true);
        //}
        //else
        //{
        //    animator.SetBool("isMoving", false);
        //}

        animator.SetFloat("MoveX", input.x);
        animator.SetFloat("MoveY", input.y);
        animator.SetFloat("MoveMagnitude", input.magnitude);
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
    }

    private void UpdateCharacter(int selectedOption)
    {
        if (selectedOption >= 0 && selectedOption < characterControllers.Length)
        {
            animator.runtimeAnimatorController = characterControllers[selectedOption];
        }


        Character character = characterDB.GetCharacter(selectedOption);
        artworkSprite.sprite = character.characterSprite;

        if (playerCharacterImage != null && selectedOption >= 0 && selectedOption < characterHeadSprites.Length)
        {
            playerCharacterImage.sprite = characterHeadSprites[selectedOption];
        }


    }

    private void Load()
    {
        for (int i = 0; i < selectedOptions.Length; i++)
        {
            if (PlayerPrefs.HasKey("SelectedOption_Player" + i))
            {
                selectedOptions[i] = PlayerPrefs.GetInt("SelectedOption_Player" + i);
            }
            else
            {
                selectedOptions[i] = 0; 
            }
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
            ItemStation itemStation = collider.GetComponent<ItemStation>();
            if (itemStation != null)
            {
                interactCount++;

                if(interactCount == interactionsNeeded)
                {
                    PlayerInventory playerInventory = GetComponent<PlayerInventory>();
                    if (playerInventory != null && itemStation.TryCollectItem(playerID, playerInventory))
                    {
                        ItemDisplay itemDisplay = GetComponentInChildren<ItemDisplay>();
                        if (itemDisplay != null)
                        {
                            itemDisplay.UpdateItemDisplay();
                        }
                    }
                    break;
                }

            }

            CraftingStation craftingStation = collider.GetComponent<CraftingStation>();
            if (craftingStation != null)
            {
                PlayerInventory playerInventory = GetComponent<PlayerInventory>();
                if (playerInventory != null)
                {

                    interactCount++;

                    if (interactCount == interactionsNeeded)
                    {

                        int uniqueItemID;
                        if (craftingStation.TryCraftItem(playerInventory, out uniqueItemID))
                        {
                            ItemDisplay itemDisplay = GetComponentInChildren<ItemDisplay>();
                            if (itemDisplay != null)
                            {
                                itemDisplay.UpdateItemDisplay();
                            }

                        }
                    }
                    break;
                }

            }

            DummyArea triggerArea = collider.GetComponent<DummyArea>();
            if (triggerArea != null && playerInventory.GetUniqueItem() != -1)
            {
                int uniqueItemID = playerInventory.GetUniqueItem();

                if (playerInventory.UseUniqueItem())
                {
                    gameManager.AddScore(playerID, uniqueItemID);
                    itemDisplay.UpdateItemDisplay();


                    ItemStation[] itemStations = FindObjectsOfType<ItemStation>();
                    foreach (ItemStation stationToReset in itemStations)
                    {
                        stationToReset.ResetCollectionStatus(playerID);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        ItemStation itemStation = collider.GetComponent<ItemStation>();
        if (itemStation != null)
        {
            interactCount = 0;
        }

        CraftingStation craftingStation = collider.GetComponent<CraftingStation>();
        if (craftingStation != null)
        {
            interactCount = 0;
        }
    }

}
