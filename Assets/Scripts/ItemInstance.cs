using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemData itemData;
    public int amount;

    public ItemInstance(ItemData p_itemData)
    {
        itemData = p_itemData;
    }
}