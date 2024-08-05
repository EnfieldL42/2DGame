using System.Collections.Generic;
using UnityEngine;

public class ItemStation : MonoBehaviour
{
    public int itemID; // ID of the item this station provides
    private HashSet<int> collectedByPlayers = new HashSet<int>();
// Track players who have collected from this station

    // Method to attempt collecting an item from this station
    public bool TryCollectItem(int playerID, PlayerInventory playerInventory)
    {
        if (collectedByPlayers.Contains(playerID))
        {
            Debug.Log($"Player {playerID} has already collected from this station.");
            return false; // Player has already collected from this station
        }

        // Try to collect the item if the player can hold more items
        if (playerInventory.CollectItem(itemID))
        {
            collectedByPlayers.Add(playerID); // Mark this player as having collected from the station
            Debug.Log($"Player {playerID} collected item {itemID}.");
            return true;
        }

        return false; // Failed to collect item (inventory might be full)
    }
}