using Unity.VisualScripting;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected AttackArea attackArea;
    [SerializeField] protected Health healthComponent;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector2 movementInput;

    // Character stats
    public float baseAttack = 2f;
    public float baseDefense = 0.5f;
    public float baseHealth = 10f;
    public float baseAttackSpeed = 1f;
    public float baseKnockbackPower = 1f;

    public float attack;
    public float defense;
    public float health;
    public float attackSpeed;
    public float knockbackPower;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        ResetStats();
        healthComponent.SetMaxHealth(health);
    }

    protected virtual void Update()
    {
        GetMovementInput();
    }

    protected virtual void FixedUpdate()
    {
        MoveCharacter();
    }

    protected abstract void GetMovementInput();

    protected void MoveCharacter()
    {
        if (movementInput == Vector2.zero)
        {
            rb.mass = 1000; // Ohhh such an ugly but beautiful hack -- Stop pushing me!
        }
        else
        {
            rb.mass = 1;
        }

        rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
        UpdateAttackAreaPosition();
    }

    public void ResetStats()
    {
        attack = baseAttack;
        defense = baseDefense;
        health = baseHealth;
        attackSpeed = baseAttackSpeed;
        knockbackPower = baseKnockbackPower;
    }

    public void AddStats(ItemInstance itemInstance)
    {
        attack += itemInstance.itemData.attack + itemInstance.attackModifier;
        defense += itemInstance.itemData.defense + itemInstance.defenseModifier;
        health += itemInstance.itemData.health + itemInstance.healthModifier;
        attackSpeed += itemInstance.itemData.attackSpeed + itemInstance.attackSpeedModifier;
        knockbackPower += itemInstance.itemData.knockbackPower + itemInstance.knockbackPowerModifier;

        // Ensure stats don't go below minimum values
        attackSpeed = Mathf.Max(attackSpeed, 1f);
        health = Mathf.Max(health, 1f);

        Debug.Log($"{gameObject.name} stats: " +
            "Attack: " + attack +
            "Defense: " + defense +
            "Health: " + health +
            "Attack Speed: " + attackSpeed +
            "Knockback Power: " + knockbackPower);

        // Update health component
        healthComponent.SetMaxHealth(health);
    }

    protected void UpdateAttackAreaPosition()
    {
        if (attackArea == null || movementInput == Vector2.zero)
        {
            return;
        }
            
        float angle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg;
        attackArea.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        Vector3 offset = new Vector3(movementInput.x, movementInput.y, 0).normalized * 0.5f;
        attackArea.transform.position = transform.position + offset;
    }

    public abstract void Attack();
    public virtual void TakeDamage(float damage)
    {
        healthComponent.TakeDamage(damage);
    }

    public virtual void Heal(float amount)
    {
        healthComponent.Heal(amount);
    }
}