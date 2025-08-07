// EnemyAttack_OmniShot.cs
using UnityEngine;

public class EnemyAttack_OmniShot : EnemyAttackBase
{
    [Header("全方位ショットの設定")]
    public GameObject bulletPrefab; // 撃ちだす弾のプレハブ
    public int numberOfBullets = 8; // 同時に発射する弾の数
    public float fireRate = 2f;     // 弾を撃つ間隔（秒）

    private float fireTimer;
    private EnemyMove_Lunge lungeMoveScript; // 「シュッ！」て動く部品を入れておく箱

    void Awake()
    {
        // 自分にくっついてる「シュッ！」て動く部品を探しておくの
        lungeMoveScript = GetComponent<EnemyMove_Lunge>();
    }

    public override void Attack(Transform enemyTransform, Transform playerTransform)
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            fireTimer = fireRate;

            // ① まずは動きの部品に「今だ、動け！」って合図を送る！
            if (lungeMoveScript != null)
            {
                lungeMoveScript.PerformLunge(enemyTransform, playerTransform);
            }

            // ② その後、全方向に弾を撃つ！
            Shoot(enemyTransform);
        }
    }

    void Shoot(Transform enemyTransform)
    {
        if (bulletPrefab == null) return;

        // 弾の数だけ、ぐるっと一周するように角度を計算するよ
        float angleStep = 360f / numberOfBullets;
        float currentAngle = 0f;

        for (int i = 0; i < numberOfBullets; i++)
        {
            // 角度から、弾が飛んでいく方向を計算するんだ
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);

            // 弾を生成して、その方向に飛ばす！
            GameObject bullet = Instantiate(bulletPrefab, enemyTransform.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().SetDirection(direction);

            // 次の弾の角度を計算
            currentAngle += angleStep;
        }
    }
}