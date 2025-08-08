using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [Header("スポナーの設定")]
    public float minSpawnRadius = 5.0f;
    public float maxSpawnRadius = 7.0f;

    [Header("難易度の設定")]
    public AnimationCurve difficultyCurve;
    public float minSpawnRate = 0.1f;

    [Header("生み出す敵のリスト")]
    public List<EnemySpawnData> enemiesToSpawn;

    private float gameTimer;
    

    void Update()
    {
        // ★★★ GameManagerから、今の正しいプレイヤー情報を教えてもらう！
        Transform currentPlayerTransform = GameManager.Instance.playerTransform;
        
        // ★★★ ぬるぽ対策と、ゲーム中かのチェック
        if (currentPlayerTransform == null || GameManager.Instance.currentState != GameManager.GameState.Playing) return;

        gameTimer += Time.deltaTime;
        
        float score = GameManager.Instance.score;
        float difficultyMultiplier = difficultyCurve.Evaluate(score);

        foreach (var enemy in enemiesToSpawn)
        {
            if (gameTimer >= enemy.activationTime)
            {
                enemy.spawnTimer -= Time.deltaTime;

                if (enemy.spawnTimer <= 0f)
                {
                    float currentSpawnRate = enemy.spawnRate * difficultyMultiplier;
                    currentSpawnRate = Mathf.Max(currentSpawnRate, minSpawnRate);
                    enemy.spawnTimer = currentSpawnRate;
                    
                    // ★★★ 正しいプレイヤー情報をSpawnEnemyに渡してあげる
                    SpawnEnemy(enemy.enemyPrefab, currentPlayerTransform);
                }
            }
        }
    }

    // ★★★ 引数でプレイヤー情報を受け取るように変更
    void SpawnEnemy(GameObject enemyPrefab, Transform player)
    {
        Vector2 randomCirclePos = Random.insideUnitCircle.normalized * Random.Range(minSpawnRadius, maxSpawnRadius);
        Vector3 spawnPos = player.position + new Vector3(randomCirclePos.x, randomCirclePos.y, 0);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}