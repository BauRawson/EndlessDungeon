using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
class InventorySaveData // Used for Save/Load
{
    public List<ItemSaveData> savedItems = new();
    public List<int> slotIndices = new();

    public InventorySaveData(List<ItemInstance> items, InventorySlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].HasItem())
            {
                savedItems.Add(new ItemSaveData(slots[i].GetItem().itemInstance));
                slotIndices.Add(i);
            }
        }
    }

    public List<ItemInstance> ToItemInstances()
    {
        return savedItems.Select(i => i.ToItemInstance()).Where(i => i != null).ToList();
    }
}