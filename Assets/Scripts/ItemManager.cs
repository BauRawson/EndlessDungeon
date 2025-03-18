using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    [SerializeField] WorldItem worldItemPrefab;

    private List<ItemData> itemDataList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadItemData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadItemData()
    {
        itemDataList = new List<ItemData>(Resources.LoadAll<ItemData>("ScriptableObjects/ItemData"));
    }

    public ItemData GetRandomItemData()
    {
        if (itemDataList == null || itemDataList.Count == 0)
        {
            Debug.LogWarning("ItemData list is empty or not loaded.");
            return null;
        }

        int randomIndex = Random.Range(0, itemDataList.Count);
        return itemDataList[randomIndex];
    }

    public WorldItem GetRandomWorldItem()
    {
        WorldItem worldItem = Instantiate(worldItemPrefab);
        worldItem.SetItemInstance(new ItemInstance(GetRandomItemData()));
        return worldItem;
    }
}