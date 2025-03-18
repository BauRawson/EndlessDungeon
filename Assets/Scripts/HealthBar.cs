using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer fill;
    [SerializeField] private float lerpSpeed = 5f; // Adjustable speed for smoothness

    private float fullWidth;
    private Coroutine lerpCoroutine;

    private void Start()
    {
        if (fill == null || background == null || health == null)
        {
            Debug.LogError("HealthBar is missing references!");
            return;
        }

        fullWidth = background.size.x;
        health.OnHealthChanged += OnHealthChanged;
        
        // Initialize health bar
        UpdateHealthBarInstantly();
    }

    private void OnHealthChanged(float _, float currentHealth)
    {
        if (lerpCoroutine != null)
            StopCoroutine(lerpCoroutine);

        lerpCoroutine = StartCoroutine(LerpHealthBar(health.GetHealthPercentage()));
    }

    private IEnumerator LerpHealthBar(float targetPercent)
    {
        float startWidth = fill.size.x;
        float targetWidth = fullWidth * targetPercent;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * lerpSpeed;
            float newWidth = Mathf.Lerp(startWidth, targetWidth, elapsedTime);
            AdjustFillSizeAndPosition(newWidth);
            yield return null;
        }

        AdjustFillSizeAndPosition(targetWidth); // Ensure final position is exact
    }

    private void UpdateHealthBarInstantly()
    {
        AdjustFillSizeAndPosition(fullWidth * health.GetHealthPercentage());
    }

    private void AdjustFillSizeAndPosition(float newWidth)
    {
        fill.size = new Vector2(newWidth, fill.size.y);
        float offset = (fullWidth - newWidth) / 2f;
        fill.transform.localPosition = new Vector3(-offset, 0, 0);
    }
}
