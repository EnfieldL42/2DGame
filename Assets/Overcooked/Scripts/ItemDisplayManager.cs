using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    public Transform[] itemPositions;  // Positions for the items to display
    public SpriteRenderer[] itemRenderers;  // SpriteRenderers for each position
    public Sprite[] itemSprites;  // Array of all possible item sprites

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = GetComponentInParent<PlayerInventory>();
        UpdateItemDisplay();
    }

    public void UpdateItemDisplay()
    {
        if (playerInventory == null) return;

        for (int i = 0; i < itemPositions.Length; i++)
        {
            int itemID = playerInventory.GetItemAtIndex(i);
            if (itemID != -1 && itemID < itemSprites.Length)
            {
                itemRenderers[i].sprite = itemSprites[itemID];
                itemRenderers[i].enabled = true;  // Enable the renderer
            }
            else
            {
                itemRenderers[i].enabled = false;  // Disable the renderer if no item is present
            }
        }
    }
}
