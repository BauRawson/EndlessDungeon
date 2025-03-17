using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed = 3f;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector2 movementInput;

    // Character stats
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
    }

    public void ResetStats()
    {
        attack = 0;
        defense = 0;
        health = 0;
        attackSpeed = 0;
        knockbackPower = 0;
    }

    public void AddStats(ItemInstance itemInstance)
    {
        attack += itemInstance.itemData.attack + itemInstance.attackModifier;
        defense += itemInstance.itemData.defense + itemInstance.defenseModifier;
        health += itemInstance.itemData.health + itemInstance.healthModifier;
        attackSpeed += itemInstance.itemData.attackSpeed + itemInstance.attackSpeedModifier;
        knockbackPower += itemInstance.itemData.knockbackPower + itemInstance.knockbackPowerModifier;

        Debug.Log($"{gameObject.name} stats: " +
            "Attack: " + attack +
            "Defense: " + defense +
            "Health: " + health +
            "Attack Speed: " + attackSpeed +
            "Knockback Power: " + knockbackPower);
    }

    public abstract void Attack();
}