using UnityEngine;
using System;

public class Level : MonoBehaviour
{
    private Door door;
    private Enemy[] enemies;
    public event Action PlayerEntered;
    private bool enteredLevel = false;
    public bool levelReady = false;

    private void Start()
    {
        door = GetComponentInChildren<Door>(); // Disgusting but I have no timeeeeeeee
    }

    public void InitializeEnemies(int difficultyMultiplier)
    {
        enemies = GetComponentsInChildren<Enemy>();

        foreach (var enemy in enemies)
        {
            enemy.Defeated += OnEnemyDefeated;
            enemy.Initialize(difficultyMultiplier);
        }
    }

    private void OnEnemyDefeated()
    {
        int remainingEnemies = 0;

        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                remainingEnemies++;
            }
        }

        if (remainingEnemies <= 1) // The last enemy isn't null yet but will be right after this.
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        door.Open();
    }

    public void CloseDoor()
    {
        door.Close();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (PlayerEntered != null && !enteredLevel && levelReady)
            {
                PlayerEntered.Invoke();
                enteredLevel = true;
                ActivateEnemies();
            }
        }
    }

    private void ActivateEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.sleeping = false;
        }
    }
}