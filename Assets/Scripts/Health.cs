using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    public event Action<float, float> OnHealthChanged; // damage amount, current health
    public event Action OnDeath;

    private void Awake()
    {
        SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damageAmount)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);
        
        OnHealthChanged?.Invoke(damageAmount, currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        float oldHealth = currentHealth;
        currentHealth += healAmount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        
        float actualHealAmount = currentHealth - oldHealth;
        if (actualHealAmount > 0)
        {
            OnHealthChanged?.Invoke(-actualHealAmount, currentHealth);
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
    }
}