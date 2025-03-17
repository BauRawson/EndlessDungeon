using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ItemDatabaseImporter : MonoBehaviour
{
    // Only use this class if we are within Unity Editor
    #if UNITY_EDITOR
    private const string jsonFilePath = "Assets/Scripts/ItemsDatabase.json"; // Update with your actual JSON path
    private const string savePath = "Assets/Resources/ScriptableObjects/ItemData/";
    private const string spritePath = "Assets/Sprites/Icons/";

    [System.Serializable]
    private class Item
    {
        public string itemName;
        public string spriteName;
        public string description;
        public float attack;
        public float defense;
        public float health;
        public float attackSpeed;
        public float knockbackPower;
        public string itemType;
    }

    [System.Serializable]
    private class ItemList
    {
        public List<Item> items;
    }

    [MenuItem("Tools/Import Items from JSON")]
    public static void ImportItems()
    {
        Debug.Log("Starting Import...");
        if (!File.Exists(jsonFilePath))
        {
            Debug.LogError("JSON file not found at " + jsonFilePath);
            return;
        }

        string jsonText = File.ReadAllText(jsonFilePath);
        Debug.Log("JSON Content: " + jsonText);

        ItemList itemList = JsonUtility.FromJson<ItemList>(jsonText);

        if (itemList == null || itemList.items == null || itemList.items.Count == 0)
        {
            Debug.LogError("Failed to parse JSON or JSON is empty");
            return;
        }

        Debug.Log("Parsed " + itemList.items.Count + " items");

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        foreach (var item in itemList.items)
        {   
            Debug.Log("Processing item: " + item.itemName);

            ItemData newItem = ScriptableObject.CreateInstance<ItemData>();
            newItem.itemName = item.itemName;
            newItem.description = item.description;
            newItem.attack = item.attack;
            newItem.defense = item.defense;
            newItem.health = item.health;
            newItem.attackSpeed = item.attackSpeed;
            newItem.knockbackPower = item.knockbackPower;
            newItem.itemType = ParseItemType(item.itemType);

            string spriteFilePath = spritePath + item.spriteName + ".png";
            newItem.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spriteFilePath);

            if (newItem.sprite == null)
            {
                Debug.LogWarning("Sprite not found: " + spriteFilePath);
            }

            string assetPath = savePath + item.itemName.Replace(" ", "_") + ".asset";
            AssetDatabase.CreateAsset(newItem, assetPath);
            Debug.Log("Created ScriptableObject: " + assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Import completed.");
    }

    private static ItemData.ItemType ParseItemType(string type)
    {
        return type switch
        {
            "Equipment" => ItemData.ItemType.Equipment,
            "Consumable" => ItemData.ItemType.Consumable,
            "Enhancer" => ItemData.ItemType.Enhancer,
            _ => ItemData.ItemType.Equipment // Default fallback
        };
    }
    #endif
}
