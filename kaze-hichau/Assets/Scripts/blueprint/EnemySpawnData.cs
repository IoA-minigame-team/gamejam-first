// EnemySpawnData.cs
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject spawnHeraldPrefab; // ★呼び出す「予告係」のプレハブ
    public float spawnRate;
    public float activationTime = 0f;

    [HideInInspector]
    public float spawnTimer;
}