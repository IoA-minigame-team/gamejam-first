// EnemyMove_Chase.cs
using UnityEngine;

// 「動きの設計図（EnemyMoveBase）」を元に作るよ！
public class EnemyMove_Chase : EnemyMoveBase
{
    private EnemyCore enemyCore; // 司令塔の情報を入れておく箱

    void Awake()
    {
        // 自分にくっついてる司令塔（EnemyCore）を探しておくの
        enemyCore = GetComponent<EnemyCore>();
    }

    // 設計図にあった「Move」命令の、具体的な中身をここに書くよ！
    public override void Move(Transform enemyTransform, Transform playerTransform)
    {
        // 司令塔が見つからなかったら、お休み
        if (enemyCore == null)
        {
            return;
        }

        // ① 敵さんからプレイヤーさんに向かう「方向」を計算するよ
        Vector3 direction = (playerTransform.position - enemyTransform.position).normalized;

        // ② その方向に、司令塔からもらった「速さ」で進むんだ！
        enemyTransform.position += direction * enemyCore.moveSpeed * Time.deltaTime;
    }
}