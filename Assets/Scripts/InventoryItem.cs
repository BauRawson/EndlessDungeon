using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemInstance itemInstance;
    [HideInInspector] public Transform parentAfterDrag;
    [SerializeField] Image image;

    // filepath: e:\Unity\Projects\InventoryTest\Assets\Scripts\InventoryItem.cs
    public void SetItemInstance(ItemInstance p_itemInstance)
    {
        itemInstance = p_itemInstance;

        if (image == null)
        {
            image = GetComponent<Image>();
        }

        if (itemInstance != null && itemInstance.itemData != null)
        {
            image.sprite = itemInstance.itemData.sprite;
            Debug.Log("SetItemInstance: " + itemInstance.itemData.itemName);
        }
        else
        {
            Debug.LogError("ItemInstance or ItemData is null");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root, true);
        transform.SetAsLastSibling(); // So it renders on top of other UI elements
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        // If it's dragged on top of an InventorySlot, InventorySlot.OnDrop() will handle the rest.
    }

    public void Enhance(ItemData enhancementData)
    {
        itemInstance.attackModifier += enhancementData.attack;
        itemInstance.defenseModifier += enhancementData.defense;
        itemInstance.healthModifier += enhancementData.health;
        itemInstance.attackSpeedModifier += enhancementData.attackSpeed;
        itemInstance.knockbackPowerModifier += enhancementData.knockbackPower;
    }
}