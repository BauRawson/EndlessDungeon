using UnityEngine;

/*
The setup for this code was provided by AI. Didn't want to use much of my time on this!
*/

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    
    // Components
    private Rigidbody2D rb;
    private Animator animator; // Uncomment if you have animations
    
    // Movement input
    private Vector2 movementInput;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Uncomment if you have animations
        
        // Configure Rigidbody2D for top-down movement
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }
    
    private void Update()
    {
        // Get input from keyboard/controller
        GetMovementInput();
        
        // Update animations if needed
        // UpdateAnimations();
    }
    
    private void FixedUpdate()
    {
        // Move character
        MoveCharacter();
    }
    
    private void GetMovementInput()
    {
        // Get input from keyboard (WASD or arrow keys)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        // Normalize diagonal movement
        movementInput = new Vector2(horizontal, vertical).normalized;
    }
    
    private void MoveCharacter()
    {
        // Use MovePosition for physics-based movement with collisions
        rb.velocity = movementInput * moveSpeed;
    }
    
    // Function to receive input from virtual joystick (when implemented)
    public void SetJoystickInput(Vector2 joystickInput)
    {
        // This will be called by your virtual joystick script
        movementInput = joystickInput.normalized;
    }
    
    /* Uncomment this if you need animations
    private void UpdateAnimations()
    {
        if (movementInput != Vector2.zero)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("Horizontal", movementInput.x);
            animator.SetFloat("Vertical", movementInput.y);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }
    */
}