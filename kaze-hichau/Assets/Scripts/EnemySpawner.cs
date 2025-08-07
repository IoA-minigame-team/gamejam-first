// EnemySpawner.cs
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("スポナーの設定")]
    public Transform playerTransform;
    public float minSpawnRadius = 5.0f;
    public float maxSpawnRadius = 7.0f;

    [Header("難易度の設定")]
    public AnimationCurve difficultyCurve;  // ★難易度を決めるための魔法のカーブ！
    public float minSpawnRate = 0.1f;       // ★どんなに難しくなっても、これよりは早くならないようにする上限

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
        if (playerTransform == null || GameManager.Instance.currentState != GameManager.GameState.Playing) return;

        gameTimer += Time.deltaTime;

        // ★今のスコアから、難易度カーブを読み取って「難しさの倍率」を決める
        float score = GameManager.Instance.score;
        float difficultyMultiplier = difficultyCurve.Evaluate(score);

        foreach (var enemy in enemiesToSpawn)
        {
            if (gameTimer >= enemy.activationTime)
            {
                enemy.spawnTimer -= Time.deltaTime;

                if (enemy.spawnTimer <= 0f)
                {
                    // ★基本のスポーンレートに、さっきの倍率を掛けて、今のスポーンレートを計算する！
                    float currentSpawnRate = enemy.spawnRate * difficultyMultiplier;
                    
                    // ★ただし、早くなりすぎないように下限を決めてあげる
                    currentSpawnRate = Mathf.Max(currentSpawnRate, minSpawnRate);
                    
                    // ★計算した今のスポーンレートをタイマーにセット！
                    enemy.spawnTimer = currentSpawnRate;

                    SpawnEnemy(enemy.enemyPrefab);
                }
            }
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        // (ここの中身は変更なし！)
        Vector2 randomCirclePos = Random.insideUnitCircle.normalized * Random.Range(minSpawnRadius, maxSpawnRadius);
        Vector3 spawnPos = playerTransform.position + new Vector3(randomCirclePos.x, randomCirclePos.y, 0);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}