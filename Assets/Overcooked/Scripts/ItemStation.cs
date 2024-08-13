using System.Collections.Generic;
using UnityEngine;

public class ItemStation : MonoBehaviour
{
    public int itemID; 
    private HashSet<int> collectedByPlayers = new HashSet<int>();

    public bool TryCollectItem(int playerID, PlayerInventory playerInventory)
    {
        if (playerInventory.uniqueItem == -1)
        {
            if (collectedByPlayers.Contains(playerID))
            {
                Debug.Log($"Player {playerID} has already collected from this station.");
                return false;
            }

            if (playerInventory.CollectItem(itemID)) // Tries to collect the item only if the player can hold more items

            {
                collectedByPlayers.Add(playerID); // Mark this player as having collected from the station
                Debug.Log($"Player {playerID} collected item {itemID}.");
                return true;
            }
        }
        return false;
    }

    public void ResetCollectionStatus(int playerID)
    {
        if (collectedByPlayers.Contains(playerID))
        {
            collectedByPlayers.Remove(playerID);
            Debug.Log($"Player {playerID} can now collect from this station again.");
        }
    }
}
