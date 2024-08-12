using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int maxItems = 3;
    public List<int> inventory = new List<int>();
    public int uniqueItem = -1;

    // Method to collect an item
    public bool CollectItem(int itemID)
    {
        if (inventory.Count < maxItems)
        {
            inventory.Add(itemID);
            return true;
        }
        return false;
    }

    // Method to clear the inventory
    public void ClearInventory()
    {
        inventory.Clear();
    }

    // Method to add a unique item
    public void AddUniqueItem(int itemID)
    {
        uniqueItem = itemID;
    }

    // Method to get an item at a specific index
    public int GetItemAtIndex(int index)
    {
        if (index < inventory.Count)
        {
            return inventory[index];
        }
        return -1;
    }

    // Method to get the unique item
    public int GetUniqueItem()
    {
        return uniqueItem;
    }

    public bool UseUniqueItem()
    {
        if (uniqueItem != -1)
        {
            uniqueItem = -1; // Reset unique item
            return true;
        }
        return false;
    }
}
