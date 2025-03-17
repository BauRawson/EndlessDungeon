using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private InventorySlot[] equipmentSlots;
    [SerializeField] private Player player;

    public void EquipItem(InventoryItem inventoryItem)
    {
        if (inventoryItem.itemInstance.itemData.itemType != ItemData.ItemType.Equipment)
        {
            return;
        }

        foreach (var slot in equipmentSlots)
        {
            if (!slot.HasItem())
            {
                slot.SetItem(inventoryItem);
                UpdatePlayerStats();
                return;
            }
        }
    }

    public void UnequipItem(InventoryItem inventoryItem)
    {
        foreach (var slot in equipmentSlots)
        {
            if (slot.GetItem() == inventoryItem)
            {
                slot.SetItem(null);
                UpdatePlayerStats();
                return;
            }
        }
    }

    public void UpdatePlayerStats()
    {
        Debug.Log("Updating player stats");

        player.ResetStats();
        foreach (var slot in equipmentSlots)
        {
            var item = slot.GetItem();
            if (item != null && item.itemInstance.itemData.itemType == ItemData.ItemType.Equipment)
            {
                player.AddStats(item.itemInstance);
            }
        }
    }
}