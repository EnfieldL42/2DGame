using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    public Transform[] ingredientPositions;
    public SpriteRenderer[] ingredientRenderers;
    public Sprite[] ingredientSprites;

    public Transform uniqueItemPosition;
    public SpriteRenderer uniqueItemRenderer;
    public Sprite[] uniqueItemSprites;

    public SpriteRenderer progressBarRenderer;
    public Sprite[] progressBar;

    private PlayerInventory playerInventory;
    public controllerp1 controller;



    private void Start()
    {
        playerInventory = GetComponentInParent<PlayerInventory>();
        UpdateItemDisplay();
    }

    private void Update()
    {
        UpdateInteractionProgress();

    }

    public void UpdateItemDisplay()
    {
        if (playerInventory == null) return;

        for (int i = 0; i < ingredientPositions.Length; i++)
        {
            int itemID = playerInventory.GetItemAtIndex(i);
            if (itemID != -1 && itemID < ingredientSprites.Length)
            {
                ingredientRenderers[i].sprite = ingredientSprites[itemID];
                ingredientRenderers[i].enabled = true;
            }
            else
            {
                ingredientRenderers[i].enabled = false;
            }
        }

        int uniqueItemID = playerInventory.GetUniqueItem();
        if (uniqueItemID != -1 && uniqueItemID < uniqueItemSprites.Length)
        {
            uniqueItemRenderer.sprite = uniqueItemSprites[uniqueItemID];
            uniqueItemRenderer.enabled = true;
        }
        else
        {
            uniqueItemRenderer.enabled = false;
        }
    }

    public void UpdateInteractionProgress()
    {
        int interactCount = controller.interactCount;
        int interactionsNeeded = controller.interactionsNeeded;

        if (interactCount > 0 && interactCount < interactionsNeeded)
        {
            progressBarRenderer.sprite = progressBar[interactCount];
        }
        else
        {
            progressBarRenderer.enabled = false;


            Debug.Log($"InteractCount: {interactCount}, InteractionsNeeded: {interactionsNeeded}");

            progressBarRenderer.enabled = true;
        }
    }
}
