[System.Serializable]
class ItemSaveData // Used for Save/Load
{
    public string itemPath;
    public int amount;
    
    public ItemSaveData(ItemInstance instance)
    {
        itemPath = instance.itemData.name;
        amount = instance.amount;
    }
}