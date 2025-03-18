using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldItem : MonoBehaviour
{
    private ItemInstance itemInstance;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float pickUpRange = 1f;
    [SerializeField] GameObject InventoryItemPrefab;
    
    // Optional: Add a small bounce effect when dropped
    [SerializeField] float bounceHeight = 0.5f;
    [SerializeField] float bounceDuration = 0.5f;
    
    private Player player;
    private Vector3 startPosition;
    private float dropTime;
    private bool isDropped = false;

    public void SetItemInstance(ItemInstance p_itemInstance)
    {
        this.itemInstance = p_itemInstance;
        sprite.sprite = itemInstance.itemData.sprite;
        player = FindObjectOfType<Player>();
        
        startPosition = transform.position;
        dropTime = Time.time;
        isDropped = true;

        StartCoroutine(BounceEffect());
    }

    public void SetItemInstance(ItemInstance p_itemInstance, Vector2 p_startPosition)
    {
        this.itemInstance = p_itemInstance;
        sprite.sprite = itemInstance.itemData.sprite;
        player = FindObjectOfType<Player>();
        
        startPosition = p_startPosition;
        dropTime = Time.time;
        isDropped = true;

        StartCoroutine(BounceEffect());
    }

    private IEnumerator BounceEffect()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < bounceDuration)
        {
            float t = elapsedTime / bounceDuration;
            float height = bounceHeight * Mathf.Sin(t * Mathf.PI);
            transform.position = new Vector3(startPosition.x, startPosition.y + height, startPosition.z);
            
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure final position reset
        transform.position = startPosition;
        isDropped = false;
    }

    private void TryPickUp()
    {
        InventoryItem inventoryItem = CreateInventoryItem();
        if (player.inventory != null && player.inventory.CanAddItem(inventoryItem))
        {
            player.inventory.AddItem(inventoryItem);
            Destroy(gameObject);
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

    private void OnMouseDown()
    {
        if (player != null && Vector2.Distance(transform.position, player.transform.position) <= pickUpRange)
        {
            TryPickUp();            
        }
    }
}