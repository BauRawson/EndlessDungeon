using UnityEngine;
using System.Collections;

public class Player : Character
{
    public Inventory inventory;
    private bool canAttack = true;

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
        Vector2 attackPosition = (Vector2)transform.position + movementInput * 0.5f; // Adjust the distance as needed
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPosition, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") && canAttack)
            {
                Attack();
                break;
            }
        }
    }

    public override void Attack()
    {
        if (!canAttack)
        {
            return;
        }

        attackArea.Attack();
        canAttack = false;
        Debug.Log("Player attacks!");
        StartCoroutine(HandleAttackCooldown());

        // Deal damage to enemies in the attack area
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackArea.transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    float damage = attack - enemy.defense;
                    damage = Mathf.Max(damage, 0); // Ensure damage is not negative
                    enemy.TakeDamage(damage);
                }
            }
        }
    }

    private IEnumerator HandleAttackCooldown()
    {
        yield return new WaitForSeconds(1f / attackSpeed - 0.1f);
        canAttack = true;
    }

    public void TakeDamage(float damage)
    {
        healthComponent.TakeDamage(damage);
    }
}