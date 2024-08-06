using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    public Transform[] ingredientPositions;  // Positions for the ingredients to display
    public SpriteRenderer[] ingredientRenderers;  // SpriteRenderers for each ingredient position
    public Sprite[] ingredientSprites;  // Array of all possible ingredient sprites

    public Transform uniqueItemPosition;  // Position for the unique item to display
    public SpriteRenderer uniqueItemRenderer;  // SpriteRenderer for the unique item position
    public Sprite[] uniqueItemSprites;  // Array of all possible unique item sprites

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = GetComponentInParent<PlayerInventory>();
        UpdateItemDisplay();
    }

    public void UpdateItemDisplay()
    {
        if (playerInventory == null) return;

        // Update ingredient display
        for (int i = 0; i < ingredientPositions.Length; i++)
        {
            int itemID = playerInventory.GetItemAtIndex(i);
            if (itemID != -1 && itemID < ingredientSprites.Length)
            {
                ingredientRenderers[i].sprite = ingredientSprites[itemID];
                ingredientRenderers[i].enabled = true;  // Enable the renderer
            }
            else
            {
                ingredientRenderers[i].enabled = false;  // Disable the renderer if no item is present
            }
        }

        // Update unique item display
        int uniqueItemID = playerInventory.GetUniqueItem();
        if (uniqueItemID != -1 && uniqueItemID < uniqueItemSprites.Length)
        {
            uniqueItemRenderer.sprite = uniqueItemSprites[uniqueItemID];
            uniqueItemRenderer.enabled = true;  // Enable the renderer
        }
        else
        {
            uniqueItemRenderer.enabled = false;  // Disable the renderer if no unique item is present
        }
    }
}
