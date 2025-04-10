// WaveData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave Data", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        public int quantity;
    }

    public EnemySpawnData[] enemies;
    public bool isBossWave;
    public bool isMidBossWave;
    public float spawnRate = 1f; // Tempo entre spawns de inimigos
    public float difficultyMultiplier = 1f; // Multiplicador de dificuldade base
}