using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int maxItems = 3;  // Maximum number of items a player can hold
    public List<int> inventory = new List<int>();  // List of item IDs
    public int uniqueItem = -1;  // ID of the unique item (-1 if no unique item)

    public bool CollectItem(int itemID)
    {
        if (inventory.Count < maxItems && !inventory.Contains(itemID))
        {
            inventory.Add(itemID);
            return true;  // Item successfully added
        }
        return false;  // Item could not be added (either max items or already collected)
    }

    public void RemoveItem(int itemID)
    {
        if (inventory.Contains(itemID))
        {
            inventory.Remove(itemID);
        }
    }

    public void ClearInventory()
    {
        inventory.Clear();
    }

    public bool AddUniqueItem(int itemID)
    {
        if (uniqueItem == -1)
        {
            uniqueItem = itemID;
            return true;
        }
        return false;
    }

    public int GetItemAtIndex(int index)
    {
        if (index >= 0 && index < inventory.Count)
        {
            return inventory[index];
        }
        return -1;  // Return -1 if index is out of range
    }

    public int GetUniqueItem()
    {
        return uniqueItem;
    }
}
