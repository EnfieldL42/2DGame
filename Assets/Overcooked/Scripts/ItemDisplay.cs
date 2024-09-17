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

        if (interactCount <= 0)
        {
            progressBarRenderer.enabled = false;

        }

        if( interactCount == 2)
        {
            progressBarRenderer.enabled = true;
            progressBarRenderer.sprite = progressBar[0];
        }

        if (interactCount == 4)
        {
            progressBarRenderer.sprite = progressBar[1];
        }

        if (interactCount == 6)
        {
            progressBarRenderer.sprite = progressBar[2];
        }


    }
}
