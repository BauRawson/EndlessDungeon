using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    [SerializeField] Canvas parentCanvas;
    [SerializeField] Transform ToolTripTransform;
    [SerializeField] TMP_Text title, details;
    [SerializeField] RectTransform background;
    [SerializeField] CanvasGroup ToolTipCanvasGroup;
    bool isShowing;

    public static ToolTipManager Instance;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        ToolTripTransform.position = Input.mousePosition;
    }

    public void Show(ItemInstance itemInstance)
    {
        ToolTipCanvasGroup.alpha = 0;

        title.text = itemInstance.itemData.itemName;
        details.text = itemInstance.itemData.description + "\n\n";

        details.text += "Attack: " + itemInstance.itemData.attack;
        if (itemInstance.attackModifier != 0)
        {
            details.text += "+" + itemInstance.attackModifier + "\n";
        }
        else
        {
            details.text += "\n";
        }

        details.text += "Defense: " + itemInstance.itemData.defense;
        if (itemInstance.defenseModifier != 0)
        {
            details.text += "+" + itemInstance.defenseModifier + "\n";
        }
        else
        {
            details.text += "\n";
        }

        details.text += "Health: " + itemInstance.itemData.health;
        if (itemInstance.healthModifier != 0)
        {
            details.text += "+" + itemInstance.healthModifier + "\n";
        }
        else
        {
            details.text += "\n";
        }

        details.text += "Attack Speed: " + itemInstance.itemData.attackSpeed;
        if (itemInstance.attackSpeedModifier != 0)
        {
            details.text += "+" + itemInstance.attackSpeedModifier + "\n";
        }
        else
        {
            details.text += "\n";
        }

        details.text += "Knockback Power: " + itemInstance.itemData.knockbackPower;
        if (itemInstance.knockbackPowerModifier != 0)
        {
            details.text += "+" + itemInstance.knockbackPowerModifier + "\n";
        }
        else
        {
            details.text += "\n";
        }

        details.text += "Type: " + itemInstance.itemData.itemType.ToString();

        AdjustBackgroundPosition();
        ToolTripTransform.gameObject.SetActive(true);
        isShowing = true;
        StartCoroutine(FadeIn());
    }

    private void AdjustBackgroundPosition()
    {
        float offset = 32f;
        Vector2 mousePosition = Input.mousePosition;
        float screenWidth = Screen.width;

        if (mousePosition.x < screenWidth / 2)
        {
            background.anchoredPosition = new Vector2(background.sizeDelta.x / 2 + offset, -background.sizeDelta.y / 2 + offset);
        }
        else
        {
            background.anchoredPosition = new Vector2(-background.sizeDelta.x / 2 - offset, -background.sizeDelta.y / 2 + offset);
        }
    }

    public void Hide()
    {
        ToolTripTransform.gameObject.SetActive(false);
        isShowing = false;
    }

    private IEnumerator FadeIn()
    {
        float duration = 0.5f;
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            ToolTipCanvasGroup.alpha = Mathf.Lerp(0, 1, counter / duration);
            yield return null;
        }
    }
}
