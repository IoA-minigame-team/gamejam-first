// EnemyMove_Lunge.cs
using System.Collections;
using UnityEngine;

public class EnemyMove_Lunge : EnemyMoveBase
{
    [Header("突進の設定")]
    public float lungeDistance = 1.0f;
    public float lungeDuration = 0.2f;

    [Header("警告表示の設定")]
    public GameObject warningMark;
    public float warningDuration = 0.5f;
    public Vector3 warningOffset = new Vector3(0, 1.2f, 0); // ★プレイヤーの頭のどれくらい上に表示するかのズレ

    private bool isLunging = false;

    public override void Move(Transform enemyTransform, Transform playerTransform) { }

    public void PerformLunge(Transform enemyTransform, Transform playerTransform)
    {
        if (isLunging) return;
        StartCoroutine(LungeCoroutine(enemyTransform, playerTransform));
    }

    // ★ここのコルーチンを大きく書き換えるよ！
    private IEnumerator LungeCoroutine(Transform enemyTransform, Transform playerTransform)
    {
        isLunging = true;

        if (warningMark != null)
        {
            // 1. 警告マークを表示する
            warningMark.SetActive(true);
            SEManager.instance.PlayEnemyAlert();

            // 2. 警告の時間だけ、プレイヤーを追いかけさせる
            float warningTimer = 0f;
            while (warningTimer < warningDuration)
            {
                // ★毎フレーム、警告マークをプレイヤーの頭の上に移動させる！
                warningMark.transform.position = playerTransform.position + warningOffset;
                
                warningTimer += Time.deltaTime;
                yield return null; // 次のフレームまで待つ
            }
            
            // 3. 警告マークを隠す
            warningMark.SetActive(false);
        }

        // ここから下の「ヌルっと」動く処理は、今までと全く同じだよ
        float elapsedTime = 0f;
        Vector3 startPosition = enemyTransform.position;
        Vector3 direction = (playerTransform.position - startPosition).normalized;
        Vector3 endPosition = startPosition + direction * lungeDistance;

        while (elapsedTime < lungeDuration)
        {
            enemyTransform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / lungeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemyTransform.position = endPosition;

        isLunging = false;
    }
}