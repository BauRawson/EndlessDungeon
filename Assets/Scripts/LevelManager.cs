using System.Collections;
using System.Net.WebSockets;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levelPrefabs; // Assign in Inspector
    private GameObject lastLevel;
    private GameObject currentLevel;
    private GameObject nextLevel;
    private int currentLevelIndex = 0;
    private int levelHeight = 12;

    private void Start()
    {
        // Load the initial level
        currentLevel = transform.GetChild(0).gameObject;
        Level currentLevelScript = currentLevel.GetComponent<Level>();

        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        Debug.Log("Load Next Level");
        // Randomly select the next level prefab
        int nextLevelIndex = Random.Range(0, levelPrefabs.Length);
        nextLevel = Instantiate(levelPrefabs[nextLevelIndex], transform);
        nextLevel.transform.position = currentLevel.transform.position + Vector3.up * levelHeight;

        // Connect to the EnemiesDefeated event
        Level nextLevelScript = nextLevel.GetComponent<Level>();
        if (nextLevelScript != null)
        {
            nextLevelScript.PlayerEntered += OnPlayerEnteredLevel;
            nextLevelScript.levelReady = true; // So it doesn't collide automatically with LevelEnterTrigger
        }
    }

    private void OnPlayerEnteredLevel()
    {
        Debug.Log("HOLA");
        var lastLevelTemp = lastLevel;
        lastLevel = currentLevel;
        currentLevel = nextLevel;
        nextLevel = null;
        Destroy(lastLevelTemp);
        LoadNextLevel();
    }
}