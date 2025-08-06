// EnemyAttack_Shoot.cs
using UnityEngine;

public class EnemyAttack_Shoot : EnemyAttackBase
{
    [Header("弾の設定")]
    public GameObject bulletPrefab; // 撃ちだす弾のプレハブ
    public Transform firePoint;     // 弾が発射される場所
    public float fireRate = 1f;     // 弾を撃つ間隔（秒）

    private float fireTimer; // 次に弾を撃つまでのタイマー

    public override void Attack(Transform enemyTransform, Transform playerTransform)
    {
        // タイマーを毎フレーム減らしていく
        fireTimer -= Time.deltaTime;

        // タイマーが0になったら…
        if (fireTimer <= 0f)
        {
            // タイマーをリセット
            fireTimer = fireRate;

            // 弾を撃つ！
            Shoot(playerTransform);
        }
    }

    void Shoot(Transform playerTransform)
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("弾のプレハブか発射地点が設定されてないよ！");
            return;
        }

        // 弾を生成！
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        
        // 弾に「プレイヤーはあっちだよ！」って教えてあげる
        Vector3 direction = (playerTransform.position - firePoint.position).normalized;
        bullet.GetComponent<Bullet>().SetDirection(direction);
    }
}