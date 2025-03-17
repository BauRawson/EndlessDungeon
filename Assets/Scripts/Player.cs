using UnityEngine;

/*
The setup for this code was provided by AI. Didn't want to use much of my time on this!
*/

public class PlayerMovement : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private Inventory inventory;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movementInput;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        SetupInventory();
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

    private void SetupInventory()
    {
        inventory = new Inventory();
    }
}