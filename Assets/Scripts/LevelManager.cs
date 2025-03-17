using System.Collections;
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
        LoadNextLevel();
        //StartCoroutine(TransitionLevel());
    }

    private IEnumerator TransitionLevel()
    {
        // Pause the game
        Time.timeScale = 0f;

        float duration = 1f;
        Vector3 targetPosition = transform.position - Vector3.up * levelHeight;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsed / duration);
            yield return null;
        }

        // Destroy the last level if it exists
        if (lastLevel != null)
        {
            Destroy(lastLevel);
        }

        // Update level references
        lastLevel = currentLevel;
        currentLevel = nextLevel;
        nextLevel = null;
        currentLevelIndex++;

        // Resume the game
        Time.timeScale = 1f;

        // Load the next level
        LoadNextLevel();
    }
}