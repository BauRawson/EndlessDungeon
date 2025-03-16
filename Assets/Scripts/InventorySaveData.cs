using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
class InventorySaveData // Used for Save/Load
{
    public List<ItemSaveData> savedItems = new();

    public InventorySaveData(List<ItemInstance> items)
    {
        savedItems = items.Select(i => new ItemSaveData(i)).ToList();
    }

    public List<ItemInstance> ToItemInstances()
    {
        return savedItems.Select(i => new ItemInstance(Resources.Load<ItemData>(i.itemPath))).ToList();
    }
}