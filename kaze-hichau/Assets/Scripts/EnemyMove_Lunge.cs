using System.Collections; // ★コルーチンを使うために、これを追加してね！
using UnityEngine;

public class EnemyMove_Lunge : EnemyMoveBase
{
    [Header("突進の設定")]
    public float lungeDistance = 1.0f; // どれくらい近づくか
    public float lungeDuration = 0.2f; // ★どれくらいの時間をかけて「ヌルっと」動くか

    private bool isLunging = false; // ★今「ヌルっと」移動中かどうかを覚えておくための旗

    // 普段はずっとお休みしてるから、Moveの中身は空っぽでOK！
    public override void Move(Transform enemyTransform, Transform playerTransform)
    {
        // 何もしない
    }

    // 攻撃スクリプトから「今だ、動け！」って呼ばれるのは変わらないよ
    public void PerformLunge(Transform enemyTransform, Transform playerTransform)
    {
        // もし、まだ前の「ヌルっと」が終わってなかったら、何もしない
        if (isLunging)
        {
            return;
        }
        
        // ★コルーチン（ヌルっと動かす処理）をスタートさせる！
        StartCoroutine(LungeCoroutine(enemyTransform, playerTransform));
    }

    // ★ここが「ヌルっと」動かす処理の本体（コルーチン）だよ！
    private IEnumerator LungeCoroutine(Transform enemyTransform, Transform playerTransform)
    {
        // ヌルっと移動開始！
        isLunging = true;

        float elapsedTime = 0f;
        Vector3 startPosition = enemyTransform.position;
        Vector3 direction = (playerTransform.position - startPosition).normalized;
        Vector3 endPosition = startPosition + direction * lungeDistance;

        // lungeDurationの時間になるまで、少しずつ動かすのを繰り返すよ
        while (elapsedTime < lungeDuration)
        {
            // Lerpっていう魔法で、スタートとゴールの間の位置を計算するの
            enemyTransform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / lungeDuration);
            
            // 時間を進めて…
            elapsedTime += Time.deltaTime;
            
            // 次のフレームまで待ってね、っていう合図
            yield return null;
        }

        // ぴったりゴールの位置に合わせる
        enemyTransform.position = endPosition;

        // ヌルっと移動おわり！
        isLunging = false;
    }
}