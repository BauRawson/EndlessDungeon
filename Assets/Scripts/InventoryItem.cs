using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemInstance itemInstance;
    [HideInInspector] public Transform parentAfterDrag;
    [SerializeField] Image image;
    [SerializeField] GameObject worldItemPrefab; // Reference to the WorldItem prefab

    private bool droppedOutsideUI = false;
    private InventorySlot originalSlot;

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
        }
        else
        {
            Debug.LogError("ItemInstance or ItemData is null");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        originalSlot = parentAfterDrag.GetComponent<InventorySlot>();
        transform.SetParent(transform.root, true);
        transform.SetAsLastSibling(); // So it renders on top of other UI elements
        image.raycastTarget = false;
        droppedOutsideUI = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>() != null ||
                eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<InventorySlot>() != null)
            {
                droppedOutsideUI = false;
            }
        }

        if (IsOnTopOfPlayer())
        {
            droppedOutsideUI = false;
        }
        else if (droppedOutsideUI)
        {
            DropItemToWorld();
        }
        else
        {
            transform.SetParent(parentAfterDrag);
        }
    }

    private void DropItemToWorld()
    {
        if (worldItemPrefab != null)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                GameObject worldItemObj = Instantiate(worldItemPrefab, player.transform.position, Quaternion.identity);
                WorldItem worldItem = worldItemObj.GetComponent<WorldItem>();
                AudioManager.Instance.PlaySound(AudioManager.SoundEffect.ItemPickup);
                
                if (worldItem != null)
                {
                    worldItem.SetItemInstance(itemInstance);
                    
                    Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
                    worldItemObj.transform.position += new Vector3(randomOffset.x, randomOffset.y, 0);

                    if (originalSlot != null)
                    {
                        originalSlot.RemoveItem();
                    }
                    
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogError("WorldItem component not found on prefab");
                    transform.SetParent(parentAfterDrag);
                }
            }
            else
            {
                Debug.LogError("Player not found in the scene");
                transform.SetParent(parentAfterDrag);
            }
        }
        else
        {
            Debug.LogError("WorldItem prefab not assigned in InventoryItem");
            transform.SetParent(parentAfterDrag);
        }
    }

    public void Enhance(ItemData enhancementData)
    {
        itemInstance.attackModifier += enhancementData.attack;
        itemInstance.defenseModifier += enhancementData.defense;
        itemInstance.healthModifier += enhancementData.health;
        itemInstance.attackSpeedModifier += enhancementData.attackSpeed;
        itemInstance.knockbackPowerModifier += enhancementData.knockbackPower;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipManager.Instance.Show(itemInstance);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance.Hide();
    }

    public bool IsOnTopOfPlayer()
    {
        if (itemInstance != null && itemInstance.itemData.itemType == ItemData.ItemType.Consumable)
        {
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            float circleRadius = 1.0f;
            
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(mouseWorldPosition, circleRadius);
            
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    Player player = hitCollider.GetComponent<Player>();
                    if (player != null)
                    {
                        player.Heal(itemInstance.itemData.health);
                        AudioManager.Instance.PlaySound(AudioManager.SoundEffect.ItemConsume);
                        player.inventory.Invoke("UpdateItemList", 0.1f);
                        player.inventory.Invoke("SaveItemsList", 0.1f);
                        Destroy(gameObject);
                        return true;
                    }
                }
            }
        }

        return false;
    }
}