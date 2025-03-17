using UnityEngine;

public class Player : Character
{
    public Inventory inventory;

    protected override void GetMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(horizontal, vertical).normalized;
    }

    protected override void Update()
    {
        base.Update();
        CheckForAttack();
    }

    private void CheckForAttack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Attack();
                break;
            }
        }
    }

    public override void Attack()
    {
        Debug.Log("Player attacks!");
        // Implement attack logic here
    }
}