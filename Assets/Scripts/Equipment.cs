using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private InventorySlot[] equipmentSlots;
    [SerializeField] private Player player;
    [SerializeField] private GameObject InventoryItemPrefab;

    private void Start()
    {
        LoadEquipment();
        
        // Subscribe to SlotUpdated events for all equipment slots
        foreach (var slot in equipmentSlots)
        {
            slot.SlotUpdated += OnSlotUpdated;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from all events when the component is destroyed
        foreach (var slot in equipmentSlots)
        {
            if (slot != null)
            {
                slot.SlotUpdated -= OnSlotUpdated;
            }
        }
    }

    // Event handler for SlotUpdated
    private void OnSlotUpdated(InventorySlot slot)
    {
        UpdatePlayerStats();
        SaveEquipment();
    }

    public void EquipItem(InventoryItem inventoryItem)
    {
        if (inventoryItem.itemInstance.itemData.itemType != ItemData.ItemType.Equipment)
        {
            return;
        }

        UpdatePlayerStats();
        SaveEquipment();
    }

    public void UnequipItem(InventoryItem inventoryItem)
    {
        foreach (var slot in equipmentSlots)
        {
            if (slot.GetItem() == inventoryItem)
            {
                slot.SetItem(null);
                break;
            }
        }

        UpdatePlayerStats();
        SaveEquipment();
    }

    public void UpdatePlayerStats()
    {
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

    public void SaveEquipment()
    {
        var json = JsonUtility.ToJson(new InventorySaveData(GetEquipmentItems(), equipmentSlots));
        PlayerPrefs.SetString("Equipment", json);
        PlayerPrefs.Save();
    }

    public void LoadEquipment()
    {
        if (PlayerPrefs.HasKey("Equipment"))
        {
            var json = PlayerPrefs.GetString("Equipment");
            var data = JsonUtility.FromJson<InventorySaveData>(json);
            var itemInstances = data.ToItemInstances();

            for (int i = 0; i < data.slotIndices.Count; i++)
            {
                int slotIndex = data.slotIndices[i];
                if (slotIndex >= 0 && slotIndex < equipmentSlots.Length)
                {
                    var inventoryItem = CreateInventoryItem(itemInstances[i]);
                    equipmentSlots[slotIndex].SetItem(inventoryItem);
                }
            }
        }
    }

    private List<ItemInstance> GetEquipmentItems()
    {
        List<ItemInstance> items = new List<ItemInstance>();
        foreach (var slot in equipmentSlots)
        {
            var inventoryItem = slot.GetItem();
            if (inventoryItem != null)
            {
                items.Add(inventoryItem.itemInstance);
            }
        }
        return items;
    }

    private InventoryItem CreateInventoryItem(ItemInstance itemInstance)
    {
        var inventoryItemObject = Instantiate(InventoryItemPrefab);
        var inventoryItem = inventoryItemObject.GetComponent<InventoryItem>();
        inventoryItem.SetItemInstance(itemInstance);
        return inventoryItem;
    }
}