using System.Collections;
using System.Net.WebSockets;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levelPrefabs; // Assign in Inspector
    private Level lastLevel;
    private Level currentLevel;
    private Level nextLevel;
    private int currentLevelIndex = 0;
    private int levelHeight = 12;

    private void Start()
    {
        // Load the initial level
        currentLevel = transform.GetChild(0).GetComponent<Level>();

        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        // Randomly select the next level prefab
        currentLevelIndex++;
        int nextLevelIndex = Random.Range(0, levelPrefabs.Length);
        nextLevel = Instantiate(levelPrefabs[nextLevelIndex], transform).GetComponent<Level>();
        nextLevel.transform.position = currentLevel.transform.position + Vector3.up * levelHeight;

        // Connect to the EnemiesDefeated event
        Level nextLevelScript = nextLevel.GetComponent<Level>();
        if (nextLevelScript != null)
        {
            nextLevelScript.PlayerEntered += OnPlayerEnteredLevel;
            nextLevelScript.levelReady = true; // So it doesn't collide automatically with LevelEnterTrigger
            nextLevelScript.InitializeEnemies(currentLevelIndex);
        }
    }

    private void OnPlayerEnteredLevel()
    {
        var lastLevelTemp = lastLevel;
        lastLevel = currentLevel;
        lastLevel.CloseDoor();
        currentLevel = nextLevel;
        nextLevel = null;

        if (lastLevelTemp != null)
        {
            Destroy(lastLevelTemp.gameObject);
        }

        LoadNextLevel();
    }
}