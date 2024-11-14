using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject player;
    public int waveNumber = 1;
    public TextMeshProUGUI scoreText;
    public bool isGameActive = false;
    public GameObject titleScreen;

    private int score;
    private float spawnTimer = 0.0f;
    private float spawnInterval = 0.5f;
    private int enemiesToSpawnPerWave = 40;
    private int enemiesSpawnedThisWave = 0;
    private float spawnDistance = 30.0f;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    void Update()
    {
        if (isGameActive) // Only spawn enemies if the game is active
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval && enemiesSpawnedThisWave < enemiesToSpawnPerWave)
            {
                Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
                enemiesSpawnedThisWave++;
                spawnTimer = 0.0f;
            }

            if (FindObjectsOfType<Enemy>().Length == 0 && enemiesSpawnedThisWave == enemiesToSpawnPerWave)
            {
                waveNumber++;
                enemiesSpawnedThisWave = 0;
                SpawnEnemyWave(waveNumber);
            }
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        Enemy.speed += 1f;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        Vector3 randomDirection = Random.onUnitSphere;
        randomDirection.y = 0;
        return player.transform.position + randomDirection * spawnDistance;
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void StartGame()
    {
        isGameActive = true;
        score = 0;
        enemiesSpawnedThisWave = 0;
        waveNumber = 1;
        UpdateScore(0); // Reset the score display

        // Immediately start the first wave
        SpawnEnemyWave(waveNumber);

        titleScreen.gameObject.SetActive(false);
    }
}