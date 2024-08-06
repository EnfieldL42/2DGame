using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : MonoBehaviour
{
    private Dictionary<HashSet<int>, int> craftingRecipes = new Dictionary<HashSet<int>, int>(HashSet<int>.CreateSetComparer())
    {
        { new HashSet<int> { 0, 1, 2 }, 1 },
        { new HashSet<int> { 0, 1, 3 }, 2 },
        { new HashSet<int> { 0, 2, 3 }, 3 },
        { new HashSet<int> { 1, 2, 3 }, 4 },

    };

    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInventory playerInventory = collision.GetComponent<PlayerInventory>();
        if (playerInventory != null && playerInventory.inventory.Count == playerInventory.maxItems && playerInventory.uniqueItem == -1)
        {
            int uniqueItemID;
            if (TryCraftItem(playerInventory.inventory, out uniqueItemID))
            {
                playerInventory.ClearInventory();
                playerInventory.AddUniqueItem(uniqueItemID);
                Debug.Log($"Player {playerInventory.gameObject.name} crafted item {uniqueItemID}.");
            }
        }
    }

    public bool TryCraftItem(List<int> ingredients, out int uniqueItemID)
    {
        uniqueItemID = -1;
        HashSet<int> ingredientSet = new HashSet<int>(ingredients);
        return craftingRecipes.TryGetValue(ingredientSet, out uniqueItemID);
    }
}