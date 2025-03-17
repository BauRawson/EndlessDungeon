using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Equipment,
        Consumable,
        Enhancer
    }
    public string itemName;
    public Sprite sprite;
    [TextArea]
    public string description;

    public float attack;
    public float defense;
    public float health;
    public float attackSpeed;
    public float knockbackPower;

    public ItemType itemType;
}