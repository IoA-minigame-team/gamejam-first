// EnemySpawnData.cs
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public float spawnRate;
    public float activationTime = 0f; // ★「何秒後に登場させるか」の項目を追加！

    [HideInInspector]
    public float spawnTimer;
}