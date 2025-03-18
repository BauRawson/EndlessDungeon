using System.Collections;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] private string targetTag = "Enemy";
    [SerializeField] Animator animator;
    [SerializeField] Collider2D attackCollider;
    private Character owner;
    
    private void Awake()
    {
        owner = GetComponentInParent<Character>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            Character character = other.GetComponent<Character>();

            if (character != null)
            {
                float finalDamage = 0f;
                if (owner != null)
                {
                    finalDamage += owner.attack;
                    
                    Character targetCharacter = other.GetComponent<Character>();
                    if (targetCharacter != null)
                    {
                        finalDamage = Mathf.Max(1, finalDamage - targetCharacter.defense);
                    }
                }
                
                character.TakeDamage(finalDamage);
                
                if (owner != null && owner.knockbackPower > 0)
                {
                    Rigidbody2D targetRb = other.GetComponent<Rigidbody2D>();
                    if (targetRb != null)
                    {
                        Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                        targetRb.mass = 1;
                        targetRb.AddForce(knockbackDirection * owner.knockbackPower * 10, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        attackCollider.enabled = true;
        AudioManager.Instance.PlaySound(AudioManager.SoundEffect.Attack);
        StartCoroutine(HandleAttackCooldown());
    }

    private IEnumerator HandleAttackCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        attackCollider.enabled = false;
    }
}