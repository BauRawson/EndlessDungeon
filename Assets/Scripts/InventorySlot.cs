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
        
        item = null;
        otherSlot.item = null;
        
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
                else
                {
                    SwapItem(previousSlot);
                }
            }
            else
            {
                previousSlot.item = null;
                SetItem(inventoryItem);
            }
            
            var inventory = GetComponentInParent<Inventory>();
            if (inventory != null)
            {
                inventory.UpdateItemList();
                inventory.SaveItemsList();
            }
        }
    }
}