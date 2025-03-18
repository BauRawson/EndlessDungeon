using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipManager : MonoBehaviour
{
    [SerializeField] Canvas parentCanvas;
    [SerializeField] Transform ToolTipTransform;
    [SerializeField] TMP_Text title, details;
    [SerializeField] RectTransform background;
    [SerializeField] CanvasGroup ToolTipCanvasGroup;
    [SerializeField] Color plusColor;
    [SerializeField] Color equipmentColor;
    [SerializeField] Color consumableColor;
    [SerializeField] Color enhancerColor;
    bool isShowing;

    public static ToolTipManager Instance;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (isShowing)
        {
            ToolTipTransform.position = Input.mousePosition;
            AdjustBackgroundPosition();
        }
    }

    public void Show(ItemInstance itemInstance)
{
    ToolTipCanvasGroup.alpha = 0;

    title.text = itemInstance.itemData.itemName;
    details.text = itemInstance.itemData.description + "\n\n";

    details.text += "Attack: " + itemInstance.itemData.attack;
    if (itemInstance.attackModifier != 0)
    {
        details.text += "<color=#" + ColorUtility.ToHtmlStringRGB(plusColor) + ">+" + itemInstance.attackModifier + "</color>\n";
    }
    else
    {
        details.text += "\n";
    }

    details.text += "Defense: " + itemInstance.itemData.defense;
    if (itemInstance.defenseModifier != 0)
    {
        details.text += "<color=#" + ColorUtility.ToHtmlStringRGB(plusColor) + ">+" + itemInstance.defenseModifier + "</color>\n";
    }
    else
    {
        details.text += "\n";
    }

    details.text += "Health: " + itemInstance.itemData.health;
    if (itemInstance.healthModifier != 0)
    {
        details.text += "<color=#" + ColorUtility.ToHtmlStringRGB(plusColor) + ">+" + itemInstance.healthModifier + "</color>\n";
    }
    else
    {
        details.text += "\n";
    }

    details.text += "Attack Speed: " + itemInstance.itemData.attackSpeed;
    if (itemInstance.attackSpeedModifier != 0)
    {
        details.text += "<color=#" + ColorUtility.ToHtmlStringRGB(plusColor) + ">+" + itemInstance.attackSpeedModifier + "</color>\n";
    }
    else
    {
        details.text += "\n";
    }

    details.text += "Knockback Power: " + itemInstance.itemData.knockbackPower;
    if (itemInstance.knockbackPowerModifier != 0)
    {
        details.text += "<color=#" + ColorUtility.ToHtmlStringRGB(plusColor) + ">+" + itemInstance.knockbackPowerModifier + "</color>\n";
    }
    else
    {
        details.text += "\n";
    }

    // Color the item type based on its value
    Color typeColor;
    switch (itemInstance.itemData.itemType)
    {
        case ItemData.ItemType.Equipment:
            typeColor = equipmentColor;
            break;
        case ItemData.ItemType.Consumable:
            typeColor = consumableColor;
            break;
        case ItemData.ItemType.Enhancer:
            typeColor = enhancerColor;
            break;
        default:
            typeColor = Color.white;
            break;
    }

    details.text += "Type: <color=#" + ColorUtility.ToHtmlStringRGB(typeColor) + ">" + 
                    itemInstance.itemData.itemType.ToString() + "</color>";

    ToolTipTransform.gameObject.SetActive(true);
    isShowing = true;
    StartCoroutine(FadeIn());
}

    private void AdjustBackgroundPosition()
    {
        float offset = 32f;
        Vector2 mousePosition = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        // Determine if tooltip should be on left or right side based on mouse position
        if (mousePosition.x < screenWidth / 2)
        {
            background.anchoredPosition = new Vector2(background.sizeDelta.x / 2 + offset, background.anchoredPosition.y);
        }
        else
        {
            background.anchoredPosition = new Vector2(-background.sizeDelta.x / 2 - offset, background.anchoredPosition.y);
        }
        
        float tooltipHeight = background.rect.height;
        float halfTooltipHeight = tooltipHeight / 2;

        float verticalPosition = mousePosition.y;

        // Clamp the vertical position to ensure the tooltip stays within the screen bounds
        verticalPosition = Mathf.Clamp(verticalPosition, halfTooltipHeight, screenHeight - halfTooltipHeight);

        background.position = new Vector3(background.position.x, verticalPosition, background.position.z);
    }

    public void Hide()
    {
        ToolTipTransform.gameObject.SetActive(false);
        isShowing = false;
    }

    private IEnumerator FadeIn()
    {
        float duration = 1.0f;
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            ToolTipCanvasGroup.alpha = Mathf.Lerp(0, 1, counter / duration);
            yield return null;
        }
    }
}