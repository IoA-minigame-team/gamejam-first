// EnemySpawner.cs
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("スポナーの設定")]
    public Transform playerTransform;
    // public float spawnRadius = 5.0f; // ← この一行はもういらないから消しちゃう！
    public float minSpawnRadius = 5.0f; // ★最低でもこれだけは離す距離
    public float maxSpawnRadius = 7.0f; // ★最大でもこれ以上は離さない距離

    [Header("生み出す敵のリスト")]
    public List<EnemySpawnData> enemiesToSpawn;

    private float gameTimer;

    void Start()
    {
        // (ここの中身は変更なし！)
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }

        foreach (var enemy in enemiesToSpawn)
        {
            enemy.spawnTimer = enemy.spawnRate;
        }
    }

    void Update()
    {
        // (ここの中身も変更なし！)
        if (playerTransform == null) return;

        gameTimer += Time.deltaTime;

        foreach (var enemy in enemiesToSpawn)
        {
            if (gameTimer >= enemy.activationTime)
            {
                enemy.spawnTimer -= Time.deltaTime;

                if (enemy.spawnTimer <= 0f)
                {
                    enemy.spawnTimer = enemy.spawnRate;
                    SpawnEnemy(enemy.enemyPrefab);
                }
            }
        }
    }

    // ★ここの中身を書き換えるよ！
    void SpawnEnemy(GameObject enemyPrefab)
    {
        // ① ランダムな方向を決めるのは同じ
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // ② 最低距離と最大距離の間で、ランダムな距離を決める！
        float randomDistance = Random.Range(minSpawnRadius, maxSpawnRadius);

        // ③ 方向と距離を組み合わせて、最終的な場所を決める
        Vector3 spawnPos = playerTransform.position + (Vector3)randomDirection * randomDistance;

        // ④ 計算した場所に、敵さんを生み出す！
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}