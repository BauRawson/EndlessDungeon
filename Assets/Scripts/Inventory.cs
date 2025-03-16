using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public List<ItemInstance> items = new();

    public void AddItem(ItemData itemData, int amount)
    {
        var existingItem = items.FirstOrDefault(i => i.itemData == itemData);
        if (existingItem != null)
            existingItem.amount += amount;
        else
            items.Add(new ItemInstance(itemData));
    }

    public void RemoveItem(ItemData itemData, int amount)
    {
        var existingItem = items.FirstOrDefault(i => i.itemData == itemData);
        if (existingItem != null)
        {
            existingItem.amount -= amount;
            if (existingItem.amount <= 0)
                items.Remove(existingItem);
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
}
