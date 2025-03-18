using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    private List<ItemInstance> items = new();
    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] Equipment equipment; // Reference to the Equipment component

    private void Start()
    {
        LoadItemsList();
        
        foreach (var slot in inventorySlots)
        {
            slot.SlotUpdated += OnSlotUpdated;
        }
    }

    private void OnDestroy()
    {
        foreach (var slot in inventorySlots)
        {
            if (slot != null)
            {
                slot.SlotUpdated -= OnSlotUpdated;
            }
        }
    }

    private void OnSlotUpdated(InventorySlot slot)
    {
        UpdateItemList();
        SaveItemsList();
    }

    private InventoryItem CreateInventoryItem(ItemInstance itemInstance)
    {
        var inventoryItemObject = Instantiate(inventoryItemPrefab);
        var inventoryItem = inventoryItemObject.GetComponent<InventoryItem>();
        inventoryItem.SetItemInstance(itemInstance);
        return inventoryItem;
    }

    public void AddItem(InventoryItem inventoryItem)
    {
        var existingItem = items.FirstOrDefault(i => i.itemData == inventoryItem.itemInstance.itemData);
        if (existingItem != null)
        {
            existingItem.amount += inventoryItem.itemInstance.amount;
        }
        else
        {
            items.Add(inventoryItem.itemInstance);
            AssignToSlot(inventoryItem);
        }

        SaveItemsList();
    }

    public void RemoveItem(InventoryItem inventoryItem, int amount)
    {
        var existingItem = items.FirstOrDefault(i => i.itemData == inventoryItem.itemInstance.itemData);
        if (existingItem != null)
        {
            existingItem.amount -= amount;
            if (existingItem.amount <= 0)
            {
                items.Remove(existingItem);
                RemoveFromSlot(inventoryItem);
            }
        }

        SaveItemsList();
    }

    private void AssignToSlot(InventoryItem inventoryItem)
    {
        foreach (var slot in inventorySlots)
        {
            if (!slot.HasItem())
            {
                slot.SetItem(inventoryItem);
                break;
            }
        }
    }

    private void RemoveFromSlot(InventoryItem inventoryItem)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.GetItem() == inventoryItem)
            {
                slot.SetItem(null);
                break;
            }
        }
    }

    public void SaveItemsList()
    {
        var json = JsonUtility.ToJson(new InventorySaveData(items, inventorySlots));
        PlayerPrefs.SetString("Inventory", json);
        PlayerPrefs.Save();

        if (equipment != null)
        {
            equipment.SaveEquipment(); // Save equipment as well
        }
    }

    public void LoadItemsList()
    {
        if (PlayerPrefs.HasKey("Inventory"))
        {
            var json = PlayerPrefs.GetString("Inventory");
            var data = JsonUtility.FromJson<InventorySaveData>(json);
            var itemInstances = data.ToItemInstances();

            for (int i = 0; i < data.slotIndices.Count; i++)
            {
                int slotIndex = data.slotIndices[i];
                if (slotIndex >= 0 && slotIndex < inventorySlots.Length)
                {
                    var inventoryItem = CreateInventoryItem(itemInstances[i]);
                    inventorySlots[slotIndex].SetItem(inventoryItem);
                }
            }
        }
    }

    public void UpdateItemList()
    {
        items.Clear();
        foreach (var slot in inventorySlots)
        {
            var inventoryItem = slot.GetItem();
            if (inventoryItem != null)
            {
                items.Add(inventoryItem.itemInstance);
            }
        }
    }

    public bool CanAddItem(InventoryItem inventoryItem)
    {
        foreach (var slot in inventorySlots)
        {
            if (!slot.HasItem())
            {
                return true;
            }
            else if (slot.GetItem().itemInstance.itemData == inventoryItem.itemInstance.itemData)
            {
                return true; // Stack!
            }
        }
        return false;
    }
}