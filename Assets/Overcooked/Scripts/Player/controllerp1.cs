using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;


public class controllerp1 : MonoBehaviour
{
    public int playerID;
    private InputControls inputControls;
    public PlayerInventory playerInventory;
    public ItemDisplay itemDisplay;
    private ItemStation station;
    public Tutorial tut;


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

    public PauseMenu pauseMenu;


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
        InteractionAnimation();


        if (!isMoving)
        {
            //AudioManager.instance.sfxSource.pitch = 1f;

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
        else if (isMoving)
        {
            if(moveSpeed >= 10)
            {
                AudioManager.instance.FastSFXOnce("Footstep", 2.5f);
            }
            else
            {
                AudioManager.instance.FastSFXOnce("Footstep", 1.5f);

            }
        }

        if(interactCount == interactionsNeeded)
        {
            interactCount = 0;
        }

        if(interactCount > 0 && station != null)
        {
            station.ResetTimer();
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

    void InteractionAnimation()
    {
        if(!isMoving && interactCount > 0)
        {
            animator.SetBool("interact", true);
        }
        else
        {
            animator.SetBool("interact", false);
        }
    }

    void AttackAnimation()
    {

        animator.SetFloat("MoveMagnitude", input.magnitude);
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
        if (playerInventory.uniqueItem == 0)
        {
            animator.SetTrigger("BowAttack");
        }
        else if (playerInventory.uniqueItem == 1)
        {
            animator.SetTrigger("ShieldAttack");
        }
        else if (playerInventory.uniqueItem == 2)
        {
            animator.SetTrigger("SwordAttack");
        }
        else if (playerInventory.uniqueItem == 3)
        {
            animator.SetTrigger("StaffAttack");
        }
    }

    public IEnumerator attackwait()
    {
        DisableInputs();
        AttackAnimation();
        yield return new WaitForSecondsRealtime(0.5f);
        AssignInputs(playerID);

    }

    void Animate()
    {
        if (input.x != 0 || input.y != 0)
        {
            animator.SetFloat("MoveX", input.x);
            animator.SetFloat("MoveY", input.y);
        }
        animator.SetFloat("MoveMagnitude", input.magnitude);
        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
        animator.SetBool("isMoving", isMoving);
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
            inputControls.MasterControls.Pause.performed += PausePerformed;

        }
        DisableDash();
    }

    public void DisableInputs()
    {
        if (inputControls != null)
        {
            inputControls.MasterControls.Movement.performed -= MovementPerformed;
            inputControls.MasterControls.Jump.performed -= RunningPerformed;
            inputControls.MasterControls.Jump.canceled -= WalkingPerformed;
            inputControls.MasterControls.Attack.performed -= InteractionPerformed;
            inputControls.MasterControls.Pause.performed += PausePerformed;

        }
        else
        {
            MultiplayerInputManager inputManager = MultiplayerInputManager.instance;
            inputManager.onPlayerJoined -= AssignInputs;
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
            inputControls.MasterControls.Pause.performed += PausePerformed;

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

        AudioManager.instance.PlaySFX("Sprint", playerID, 1f);
    }

    private void WalkingPerformed(InputAction.CallbackContext context)
    {
        moveSpeed = walkingSpeed;
    }

    private void PausePerformed(InputAction.CallbackContext context)
    {
        pauseMenu.PauseGame();
    }

    private void InteractionPerformed(InputAction.CallbackContext context)
    {

        tut.SkipTutorial();

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, overlapBoxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            ItemStation itemStation = collider.GetComponent<ItemStation>();
            if (itemStation != null)
            {
                AudioManager.instance.SFXOnce("Grabing Ingredients", playerID); //need this to not be play once

                interactCount++;

                if (interactCount == interactionsNeeded)
                {
                    PlayerInventory playerInventory = GetComponent<PlayerInventory>();
                    if (playerInventory != null && itemStation.TryCollectItem(playerID, playerInventory))
                    {

                        ItemDisplay itemDisplay = GetComponentInChildren<ItemDisplay>();
                        if (itemDisplay != null)
                        {
                            itemDisplay.UpdateItemDisplay();
                            AudioManager.instance.PlaySFX("Item Pickup", playerID);
                        }
                    }
                    else
                    {
                        AudioManager.instance.PlaySFX("Fail to Interact", playerID);
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
                    AudioManager.instance.SFXOnce("Crafting", playerID);

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
                                AudioManager.instance.PlaySFX("Crafting Finished", playerID);
                            }

                        }
                        else
                        {
                            AudioManager.instance.PlaySFX("Fail to Interact", playerID);
                        }
                    }
                    break;
                }

            }

            DummyArea triggerArea = collider.GetComponent<DummyArea>();
            if (triggerArea != null && playerInventory.GetUniqueItem() != -1)
            {
                
                StartCoroutine(attackwait());
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

    private void DisableDash()
    {
        if(playerID == OCGameManager.nerfedPlayer && inputControls != null)
        {
            inputControls.MasterControls.Jump.performed -= RunningPerformed;
        }
        else
        {
            inputControls.MasterControls.Jump.performed += RunningPerformed;
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


        if (collider.CompareTag("Station"))
        {
            if (station == collider.GetComponent<ItemStation>())
            {
                station = null;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Station"))
        {
            station = collision.GetComponent<ItemStation>();
        }
    }

}
