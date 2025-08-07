// EnemyCore.cs (デスポーン機能つき！)
using UnityEngine;

public class EnemyCore : MonoBehaviour
{
    [Header("敵のステータス")]
    public float moveSpeed = 3f;
    public float lifetime = 10f; // ★敵さんが画面にいる時間（秒）を追加したよ！

    private EnemyMoveBase moveScript;
    private EnemyAttackBase attackScript;
    private Transform playerTransform;

    void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        moveScript = GetComponent<EnemyMoveBase>();
        attackScript = GetComponent<EnemyAttackBase>();
    }

    // ★Start()を追加したよ！
    void Start()
    {
        // lifetime秒後に、この敵さんを自動でデストロイ（消滅）させるおまじないだよ！
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (moveScript != null)
        {
            moveScript.Move(transform, playerTransform);
        }

        if (attackScript != null)
        {
            attackScript.Attack(transform, playerTransform);
        }
    }
}