using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemData itemData;
    public int amount = 1;

    // ToDo: Create a 'Stats' struct to hold these values
    public float attackModifier;
    public float defenseModifier;
    public float healthModifier;
    public float attackSpeedModifier;
    public float knockbackPowerModifier;

    public ItemInstance(ItemData p_itemData)
    {
        itemData = p_itemData;
    }
}