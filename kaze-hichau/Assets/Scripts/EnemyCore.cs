// EnemyCore.cs (スッキリバージョン！)
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    [Header("敵のステータス")]
    public float moveSpeed = 3f; // 移動する速さだけ残したよ！

    private EnemyMoveBase moveScript; // 動き専門のスクリプト（部品）を入れる箱だよ
    private Transform playerTransform;    // プレイヤーさんの場所を覚えておくため！

    void Awake()
    {
        // 自分にくっついてる「動きのスクリプト」を探して、moveScriptに入れておくの
        moveScript = GetComponent<EnemyMoveBase>();

        // ゲームが始まったら、"Player"タグがついたオブジェクト（プレイヤー）を探しておくんだ
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        // 安全のため、プレイヤーか動きのスクリプトが見つからなかったら何もしないようにするよ
        if (playerTransform == null || moveScript == null)
        {
            return;
        }
        
        // 毎フレーム、動きの専門家さんにお願いして、プレイヤーに向かって動いてもらう！
        moveScript.Move(transform, playerTransform);
    }
}
