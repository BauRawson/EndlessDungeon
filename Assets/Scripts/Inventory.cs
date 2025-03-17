using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    private List<ItemInstance> items = new();
    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] GameObject inventoryItemPrefab;

    private void Start()
    {
        DebugFillInventory();
    }

    private void DebugFillInventory()
    {
        for (int i = 0; i < 4; i++)
        {
            var itemData = ScriptableObject.CreateInstance<ItemData>();
            itemData.itemName = "Debug Item " + i;
            var inventoryItem = CreateInventoryItem(itemData, 1);
            AddItem(inventoryItem);
        }
        
        SaveItemsList();
    }

    private InventoryItem CreateInventoryItem(ItemData itemData, int amount)
    {
        var inventoryItemObject = Instantiate(inventoryItemPrefab);
        var inventoryItem = inventoryItemObject.GetComponent<InventoryItem>();
        inventoryItem.itemInstance = new ItemInstance(itemData) { amount = amount };
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
        var json = JsonUtility.ToJson(new InventorySaveData(items));
        PlayerPrefs.SetString("Inventory", json);
        PlayerPrefs.Save();
    }

    public void LoadItemsList()
    {
        if (PlayerPrefs.HasKey("Inventory"))
        {
            var json = PlayerPrefs.GetString("Inventory");
            var data = JsonUtility.FromJson<InventorySaveData>(json);
            items = data.ToItemInstances();
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
}