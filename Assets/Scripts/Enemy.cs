using UnityEngine;

public class Enemy : Character
{
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
}