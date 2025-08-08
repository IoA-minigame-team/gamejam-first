using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("アイテム設定")]
    public GameObject maskPrefab; // マスクのPrefab
    public float spawnInterval = 10f; // 出現間隔（秒）

    [Header("出現範囲")]
    public Vector2 spawnAreaMin; // 範囲の左下座標
    public Vector2 spawnAreaMax; // 範囲の右上座標

    private float spawnTimer;

    void Start()
    {
        // 最初の出現までの時間を設定
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        // ゲーム中でなければ何もしない
        if (GameManager.Instance.currentState != GameManager.GameState.Playing) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnItem();
            spawnTimer = spawnInterval; // タイマーをリセット
        }
    }

    void SpawnItem()
    {
        // 指定された範囲内のランダムな位置を計算
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        Vector2 spawnPosition = new Vector2(randomX, randomY);

        // マスクを生成
        Instantiate(maskPrefab, spawnPosition, Quaternion.identity);
        if (maskPrefab != null)
        {
            Instantiate(maskPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("マスクが出現しました！");
        }
        else
        {
            Debug.LogWarning("maskPrefab is not assigned in the Inspector. Cannot spawn mask.");
        }
    }
}