using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int maxItems = 3;
    public List<int> inventory = new List<int>();
    public int uniqueItem = -1;

    public bool CollectItem(int itemID)
    {
        if (inventory.Count < maxItems)
        {
            inventory.Add(itemID);
            return true;
        }
        return false;
    }

    public void ClearInventory()
    {
        inventory.Clear();
    }

    public void AddUniqueItem(int itemID)
    {
        uniqueItem = itemID;
    }

    public int GetItemAtIndex(int index)
    {
        if (index < inventory.Count)
        {
            return inventory[index];
        }
        return -1;
    }

    public int GetUniqueItem()
    {
        return uniqueItem;
    }

    public bool UseUniqueItem()
    {
        if (uniqueItem != -1)
        {
            uniqueItem = -1;
            return true;
        }
        return false;
    }
}
