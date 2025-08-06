using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    [Header("敵のステータス")]
    public float moveSpeed = 3f;

    private EnemyMoveBase moveScript;   // 動きの部品
    private EnemyAttackBase attackScript; // ★攻撃の部品を追加！
    private Transform playerTransform;

    void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        moveScript = GetComponent<EnemyMoveBase>();
        attackScript = GetComponent<EnemyAttackBase>(); // ★攻撃の部品も探すようにする
    }

    void Update()
    {
        if (playerTransform == null) return;

        // 動きの部品があったら、動いてもらう
        if (moveScript != null)
        {
            moveScript.Move(transform, playerTransform);
        }

        // ★攻撃の部品があったら、攻撃してもらう
        if (attackScript != null)
        {
            attackScript.Attack(transform, playerTransform);
        }
    }
}