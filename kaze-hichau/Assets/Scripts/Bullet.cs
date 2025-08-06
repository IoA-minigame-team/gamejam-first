// Bullet.cs
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;       // 弾の速さ
    public float lifetime = 5f;     // 弾が消えるまでの時間

    private Vector3 moveDirection;

    void Start()
    {
        // lifetime秒後に自動で消えるようにする
        Destroy(gameObject, lifetime);
    }

    // 敵から方向を受け取るための命令だよ
    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    void Update()
    {
        // もらった方向にまっすぐ進んでいく！
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    // 壁とかプレイヤーに当たったら消えるようにするともっと良いよ！
    void OnTriggerEnter2D(Collider2D other)
    {
        // ここにプレイヤーにダメージを与える処理とかを書く！
        // if (other.CompareTag("Player")) { ... }

        Destroy(gameObject); // とりあえず何かに当たったら消える
    }
}