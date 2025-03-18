using UnityEngine;
using System;
using System.Collections;

public class Enemy : Character
{
    public event Action Defeated;
    private bool canAttack = true;
    public bool sleeping = true;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected override void GetMovementInput()
    {
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            movementInput = direction;
        }
    }

    protected override void Update()
    {
        if (sleeping)
        {
            return;
        }

        base.Update();
    }

    public override void Attack()
    {
        if (!canAttack)
        {
            return;
        }

        Debug.Log("Enemy attacks!");
        attackArea.Attack();
        canAttack = false;
        StartCoroutine(HandleAttackCooldown());

        // Deal damage to the player if in the attack area
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackArea.transform.position, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Player player = hitCollider.GetComponent<Player>();
                if (player != null)
                {
                    float damage = attack - player.defense;
                    damage = Mathf.Max(damage, 0); // Ensure damage is not negative
                    player.TakeDamage(damage);
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
        if (healthComponent.GetCurrentHealth() <= 0)
        {
            OnDefeated();
        }
    }

    public void OnDefeated()
    {
        Debug.Log("Enemy defeated!");
        if (Defeated != null)
        {
            Defeated.Invoke();
        }
        
        Destroy(gameObject);
    }
}