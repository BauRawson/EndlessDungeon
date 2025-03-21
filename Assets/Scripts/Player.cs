using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
        Vector2 attackPosition = (Vector2)transform.position + movementInput * 0.25f; // Adjust the distance as needed
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
        StartCoroutine(HandleAttackCooldown());
    }

    private IEnumerator HandleAttackCooldown()
    {
        yield return new WaitForSeconds(1f / attackSpeed - 0.1f);
        canAttack = true;
    }

    public override void TakeDamage(float damage)
    {
        healthComponent.TakeDamage(damage);

        if (healthComponent.GetCurrentHealth() <= 0)
        {
            OnDefeated();
        }
    }

    public void OnDefeated()
    {
        // Reload the scene
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}