using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldItem : MonoBehaviour
{
    private ItemInstance itemInstance;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float pickUpRange = 2f;
    [SerializeField] GameObject InventoryItemPrefab;

    private Player player;

    private void Start()
    {
        itemInstance = new ItemInstance(ItemManager.Instance.GetRandomItemData());

        sprite.sprite = itemInstance.itemData.sprite;
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (player != null && Vector2.Distance(transform.position, player.transform.position) <= pickUpRange)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryPickUp();
            }
        }
    }

    private void TryPickUp()
    {
        InventoryItem inventoryItem = CreateInventoryItem();
        if (player.inventory != null && player.inventory.CanAddItem(inventoryItem))
        {
            player.inventory.AddItem(inventoryItem);
            Destroy(gameObject); // Destroy the world item after picking it up
        }
        else
        {
            Debug.Log("No available slot in inventory.");
            Destroy(inventoryItem.gameObject);
        }
    }

    private InventoryItem CreateInventoryItem()
    {
        GameObject inventoryItemObject = Instantiate(InventoryItemPrefab);
        InventoryItem inventoryItem = inventoryItemObject.GetComponent<InventoryItem>();
        inventoryItem.SetItemInstance(itemInstance);
        return inventoryItem;
    }
}