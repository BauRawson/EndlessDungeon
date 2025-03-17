using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movementInput;

    // Player stats
    public float attack;
    public float defense;
    public float health;
    public float attackSpeed;
    public float knockbackPower;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        GetMovementInput();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void GetMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(horizontal, vertical).normalized;
    }

    private void MoveCharacter()
    {
        rb.velocity = movementInput * moveSpeed;
    }

    public void SetJoystickInput(Vector2 joystickInput)
    {
        movementInput = joystickInput.normalized;
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

        Debug.Log("Player stats: " +
            "Attack: " + attack +
            "Defense: " + defense +
            "Health: " + health +
            "Attack Speed: " + attackSpeed +
            "Knockback Power: " + knockbackPower);
    }
}