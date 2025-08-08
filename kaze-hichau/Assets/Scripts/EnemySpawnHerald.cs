// EnemySpawnHerald.cs
using UnityEngine;

public class EnemySpawnHerald : MonoBehaviour
{
    [Header("予告の設定")]
    public GameObject enemyToSpawn; // この予告の後に生まれてくる敵のプレハブ
    public float spawnDelay = 1.0f; // 予告が出てから、敵が生まれるまでの時間（秒）

    void Start()
    {
        // もし呼び出す敵が設定されてなかったら、エラーを教えてあげる
        if (enemyToSpawn == null)
        {
            Debug.LogError("呼び出す敵が設定されていません！", this.gameObject);
            return;
        }
        
        // spawnDelay秒後に、SpawnEnemyメソッドを呼び出す予約をする
        Invoke(nameof(SpawnEnemy), spawnDelay);

        // 予告エフェクト自体も、敵が生まれたら消えるようにする
        // 少しだけ長めに時間をとって、敵の出現と同時くらいに消えるようにする
        Destroy(gameObject, spawnDelay + 0.1f);
    }

    // 予約された時間になると、この関数が呼ばれる
    private void SpawnEnemy()
    {
        // 自分のいた場所に、設定された敵を生み出す！
        Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
    }
}