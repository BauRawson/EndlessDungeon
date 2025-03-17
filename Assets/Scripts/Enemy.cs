using UnityEngine;
using System;

public class Enemy : Character
{
    public event Action Defeated;

    protected override void GetMovementInput()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            movementInput = direction;
        }
    }

    public override void Attack()
    {
        Debug.Log("Enemy attacks!");
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