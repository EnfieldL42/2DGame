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
                Debug.Log($"Setting ingredient sprite for position {i} to itemID {itemID}");
                ingredientRenderers[i].sprite = ingredientSprites[itemID];
                ingredientRenderers[i].enabled = true;
            }
            else
            {
                Debug.Log($"Disabling ingredient renderer at position {i}");
                ingredientRenderers[i].enabled = false;
            }
        }

        // Update unique item display
        int uniqueItemID = playerInventory.GetUniqueItem();
        if (uniqueItemID != -1 && uniqueItemID < uniqueItemSprites.Length)
        {
            Debug.Log($"Setting unique item sprite to itemID {uniqueItemID}");
            uniqueItemRenderer.sprite = uniqueItemSprites[uniqueItemID];
            uniqueItemRenderer.enabled = true;
        }
        else
        {
            Debug.Log($"Disabling unique item renderer");
            uniqueItemRenderer.enabled = false;
        }
    }
}
