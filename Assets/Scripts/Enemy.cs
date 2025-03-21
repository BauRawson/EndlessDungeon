using UnityEngine;
using System;
using System.Collections;

public class Enemy : Character
{
    public event Action Defeated;
    private bool canAttack = true;
    public bool sleeping = true;
    private Player player;
    private float distanceThreshold = 0.8f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void Initialize(int p_difficultyMultiplier)
    {
        float difficultyMultiplier = p_difficultyMultiplier / 10.0f;
        attack = baseAttack + baseAttack * difficultyMultiplier;
        defense = baseDefense + baseDefense * difficultyMultiplier;
        health = baseHealth + baseHealth * difficultyMultiplier;
        attackSpeed = baseAttackSpeed + baseAttackSpeed * difficultyMultiplier;
        
        healthComponent.SetMaxHealth(health);     
    }

    protected override void GetMovementInput()
    {
        if (player != null)
        {
            Vector2 distanceToPlayer = player.transform.position - transform.position;

            if (distanceToPlayer.magnitude <= distanceThreshold)
            {
                movementInput = Vector2.zero;
                return;
            }

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
        CheckForAttack();
    }

    private void CheckForAttack()
    {
        Vector2 attackPosition = (Vector2)transform.position + movementInput * 0.1f; // Adjust the distance as needed
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPosition, 1f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player") && canAttack)
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
        Debug.Log("Enemy defeated!");
        if (Defeated != null)
        {
            Defeated.Invoke();
        }

        if (UnityEngine.Random.Range(0f, 1f) < 0.2f)
        {
            SpawnWorldItem();
        }

        Destroy(gameObject);
    }

    public void SpawnWorldItem()
    {
        AudioManager.Instance.PlaySound(AudioManager.SoundEffect.ItemPickup);
        WorldItem worldItem = ItemManager.Instance.GetRandomWorldItem(transform.position);
    }
}