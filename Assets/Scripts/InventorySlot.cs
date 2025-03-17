using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    private InventoryItem item;

    public void SetItem(InventoryItem p_item)
    {
        item = p_item;
        if (p_item != null)
        {
            p_item.transform.SetParent(transform, false);
            p_item.transform.localPosition = Vector3.zero;
            p_item.parentAfterDrag = transform;

            var inventory = GetComponentInParent<Inventory>();
            if (inventory != null)
            {
                inventory.UpdateItemList();
                inventory.SaveItemsList();
            }
            else
            {
                var equipment = GetComponentInParent<Equipment>();
                if (equipment != null)
                {
                    equipment.UpdatePlayerStats();
                }
            }
        }
    }

    public InventoryItem GetItem()
    {
        return item;
    }

    public bool HasItem()
    {
        return item != null;
    }

    public void SwapItem(InventorySlot otherSlot)
    {
        var myItem = item;
        var otherItem = otherSlot.GetItem();

        RemoveItem();
        otherSlot.RemoveItem();

        if (otherItem != null)
        {
            SetItem(otherItem);
        }

        if (myItem != null)
        {
            otherSlot.SetItem(myItem);
        }

        var inventory = GetComponentInParent<Inventory>();
        if (inventory != null)
        {
            inventory.UpdateItemList();
            inventory.SaveItemsList();
        }
        else
        {
            var equipment = GetComponentInParent<Equipment>();
            if (equipment != null)
            {
                equipment.UpdatePlayerStats();
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();

        if (inventoryItem != null)
        {
            InventorySlot previousSlot = inventoryItem.parentAfterDrag.GetComponent<InventorySlot>();

            if (HasItem())
            {
                if (item.itemInstance.itemData == inventoryItem.itemInstance.itemData)
                {
                    item.itemInstance.amount += inventoryItem.itemInstance.amount;
                    Destroy(inventoryItem.gameObject);
                    previousSlot.item = null;
                }
                else if (inventoryItem.itemInstance.itemData.itemType == ItemData.ItemType.Enhancer && item.itemInstance.itemData.itemType == ItemData.ItemType.Equipment)
                {
                    item.Enhance(inventoryItem.itemInstance.itemData);
                    Destroy(inventoryItem.gameObject);
                    previousSlot.item = null;
                }
                else
                {
                    SwapItem(previousSlot);
                }
            }
            else
            {
                previousSlot.RemoveItem(); // Ensure stats are updated when moving from equipment to inventory
                previousSlot.item = null;
                SetItem(inventoryItem);
            }

            var inventory = GetComponentInParent<Inventory>();
            if (inventory != null)
            {
                inventory.UpdateItemList();
                inventory.SaveItemsList();
            }
            else
            {
                var equipment = GetComponentInParent<Equipment>();
                if (equipment != null)
                {
                    equipment.UpdatePlayerStats();
                }
            }
        }
    }

    public void RemoveItem()
    {
        if (item != null)
        {
            var equipment = GetComponentInParent<Equipment>();
            if (equipment != null)
            {
                equipment.UnequipItem(item);
            }
            item = null;
        }
    }
}