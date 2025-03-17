using UnityEngine;

[System.Serializable]
class ItemSaveData // Used for Save/Load
{
    public string itemPath;
    public int amount;
    public float attackModifier;
    public float defenseModifier;
    public float healthModifier;
    public float attackSpeedModifier;
    public float knockbackPowerModifier;

    public ItemSaveData(ItemInstance instance)
    {
        itemPath = instance.itemData.name;
        amount = instance.amount;
        attackModifier = instance.attackModifier;
        defenseModifier = instance.defenseModifier;
        healthModifier = instance.healthModifier;
        attackSpeedModifier = instance.attackSpeedModifier;
        knockbackPowerModifier = instance.knockbackPowerModifier;
    }

    public ItemInstance ToItemInstance()
    {
        ItemData itemData = Resources.Load<ItemData>("ScriptableObjects/ItemData/" + itemPath);
        if (itemData == null)
        {
            Debug.LogError("Failed to load ItemData at path: " + itemPath);
            return null;
        }

        ItemInstance itemInstance = new ItemInstance(itemData)
        {
            amount = amount,
            attackModifier = attackModifier,
            defenseModifier = defenseModifier,
            healthModifier = healthModifier,
            attackSpeedModifier = attackSpeedModifier,
            knockbackPowerModifier = knockbackPowerModifier
        };
        return itemInstance;
    }
}