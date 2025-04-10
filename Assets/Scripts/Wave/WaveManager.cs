using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private WaveData[] waves;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float minimumSpawnDistance = 10f;
    [SerializeField] private float maximumSpawnDistance = 15f;

    [Header("Debug")]
    [SerializeField] private bool showSpawnArea = true;

    private int currentWaveIndex = 0;
    private int enemiesAlive = 0;
    private int enemiesLeftToSpawn = 0;
    private bool isSpawning = false;
    private bool isWaveActive = false;

    public int CurrentWave => currentWaveIndex + 1;
    public int TotalWaves => waves.Length;
    
    // Events
    public UnityEvent onWaveStart = new UnityEvent();
    public UnityEvent onWaveComplete = new UnityEvent();
    public UnityEvent onAllWavesComplete = new UnityEvent();

    [Header("Reward System")]
    [SerializeField] private bool waitForRewardSelection = true; // Nova variável
    private bool rewardSelected = false; // Nova variável

    private void Start()
    {
        StartCoroutine(StartFirstWave());
    }

    private IEnumerator StartFirstWave()
    {
        yield return new WaitForSeconds(1f); // Pequeno delay inicial
        StartWave();
    }

    private void StartWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            Debug.Log("All waves complete!");
            onAllWavesComplete.Invoke();
            return;
        }

        WaveData currentWave = waves[currentWaveIndex];
        enemiesLeftToSpawn = CalculateTotalEnemies(currentWave);
        isWaveActive = true;
        isSpawning = true;

        Debug.Log($"Starting Wave {CurrentWave}");
        onWaveStart.Invoke();

        StartCoroutine(SpawnWaveEnemies(currentWave));
    }

    private int CalculateTotalEnemies(WaveData wave)
    {
        int total = 0;
        foreach (var enemyData in wave.enemies)
        {
            total += enemyData.quantity;
        }
        return total;
    }

    private IEnumerator SpawnWaveEnemies(WaveData wave)
    {
        while (enemiesLeftToSpawn > 0 && isSpawning)
        {
            SpawnEnemy(wave);
            enemiesLeftToSpawn--;
            yield return new WaitForSeconds(wave.spawnRate);
        }
        isSpawning = false;
    }

    private void SpawnEnemy(WaveData wave)
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
    
        // Escolhe um tipo de inimigo aleatório da wave
        var enemyData = wave.enemies[Random.Range(0, wave.enemies.Length)];
        GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPosition, Quaternion.identity);
    
        enemiesAlive++;
    }
    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(minimumSpawnDistance, maximumSpawnDistance);
        
        Vector2 offset = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
            Mathf.Sin(angle * Mathf.Deg2Rad) * distance
        );

        return (Vector2)playerPosition + offset;
    }

    public void OnEnemyDefeated()
    {
        enemiesAlive--;
    
        if (enemiesAlive <= 0 && !isSpawning)
        {
            StartCoroutine(PrepareNextWave());
        }
    }
    private IEnumerator PrepareNextWave()
    {
        isWaveActive = false;
        onWaveComplete.Invoke();

        if (currentWaveIndex < waves.Length - 1)
        {
            Debug.Log($"Wave {CurrentWave} complete! Waiting for reward selection...");

            if (waitForRewardSelection)
            {
                // Espera até que uma recompensa seja selecionada
                rewardSelected = false;
                yield return new WaitUntil(() => rewardSelected);
            }

            // Depois da seleção da recompensa, espera o tempo entre waves
            Debug.Log($"Next wave in {timeBetweenWaves} seconds...");
            yield return new WaitForSeconds(timeBetweenWaves);

            currentWaveIndex++;
            StartWave();
        }
        else
        {
            Debug.Log("All waves complete!");
            onAllWavesComplete.Invoke();
        }
    }

    // Novo método para ser chamado quando uma recompensa for selecionada
    public void OnRewardSelected()
    {
        rewardSelected = true;
    }

    // Atualização no Enemy.cs necessária
    public void StopWave()
    {
        isSpawning = false;
    }

    private void OnDrawGizmos()
    {
        if (!showSpawnArea || !Application.isPlaying) return;

        Vector2 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerPosition, minimumSpawnDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerPosition, maximumSpawnDistance);
    }
}