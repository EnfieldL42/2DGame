using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int maxItems = 3; // Maximum number of items a player can hold
    public List<int> inventory = new List<int>(); // List to store item IDs

    // Method to add an item to the inventory
    public bool CollectItem(int itemID)
    {
        if (inventory.Count >= maxItems)
        {
            Debug.Log("Inventory is full.");
            return false;
        }

        if (!inventory.Contains(itemID))
        {
            inventory.Add(itemID);
            return true;
        }
        return false; // Item already in inventory
    }
}