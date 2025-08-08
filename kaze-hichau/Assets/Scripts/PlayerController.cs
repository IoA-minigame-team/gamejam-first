using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動の設定")]
    public float moveSpeed = 5f;
    
    [Header("ダッシュの設定")] // ★ここからダッシュ用の設定を追加
    public float dashSpeed = 10f;       // ダッシュの速さ
    public float dashDuration = 0.2f;   // ダッシュしてる時間
    public float dashCooldown = 1f;     // ダッシュ後の待ち時間
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection; // ★最後に動いた方向を覚えておく

    private bool isDashing = false;    // ★今、ダッシュ中かどうか
    private float dashCooldownTimer = 0f; // ★次のダッシュまでの時間を計るタイマー
  
    public ParticleSystem dashParticles; // ★パーティクルシステムを入れておく箱
    
    void Start()
    {
        // 自分自身に付いているRigidbody 2Dコンポーネントを取得して、変数rbに入れる
        rb = GetComponent<Rigidbody2D>();
        // ★もし見つからなかったら、エラーを出す（設定ミスを防ぐため）
        if (dashParticles == null)
        {
            Debug.LogError("DashParticles という名前のパーティクルシステムがプレイヤーの子オブジェクトに見つかりません！");
        }
    }
    
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        // 入力をVector2にまとめ、長さを1に正規化する（斜め移動が速くなるのを防ぐ）
        moveInput = new Vector2(moveX, moveY).normalized;
        // ★もし移動入力があったら、その方向を覚えておく
        if (moveInput.magnitude > 0)
        {
            lastMoveDirection = moveInput;
        }
        
        // --- ▼ダッシュの処理▼ ---

        // ★ダッシュのクールダウンタイマーを進める
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // ★Shiftキーが押されて、ダッシュ中でなくて、クールダウンが終わってたら…
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownTimer <= 0)
        {
            StartCoroutine(DashCoroutine());
        }
    }
    
    void FixedUpdate()
    {
        // ★ダッシュ中は、普通の移動を止める
        if (isDashing)
        {
            return;
        }
        // Rigidbodyの速度を更新してプレイヤーを動かす
        rb.linearVelocity = moveInput * moveSpeed;
    }
    
    // ★これがダッシュ処理の本体（コルーチン）だよ！
    private IEnumerator DashCoroutine()
    {
        isDashing = true; // ダッシュ開始！
        dashCooldownTimer = dashCooldown; // クールダウンタイマーをリセット

        // ★一瞬だけ、物理的な力を加えてシュッと動かす！
        rb.linearVelocity = Vector2.zero; // 一旦停止して…
        rb.AddForce(lastMoveDirection * dashSpeed, ForceMode2D.Impulse);

        // ★ダッシュが始まったらパーティクルを再生！
        if (dashParticles != null)
        {
            // ① ダッシュと"逆"方向の角度を計算する
            //    lastMoveDirection にマイナス(-)を付けて逆向きにしてるのがポイント！
            float angle = Mathf.Atan2(-lastMoveDirection.y, -lastMoveDirection.x) * Mathf.Rad2Deg;

            // ② パーティクルシステムの向きを、その角度にクルッと回転させる！
            //    z軸の回転だけ変えればOK
            dashParticles.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            // ③ その向きでパーティクルを再生！
            dashParticles.Play();
        }
        yield return new WaitForSeconds(dashDuration);

        // ★ダッシュが終わったらパーティクルを停止！
        if (dashParticles != null)
        {
            dashParticles.Stop();
        }
        isDashing = false;
    }

    // PlayerController.cs のクラス内に追記
    private void OnTriggerEnter2D(Collider2D other)
    {
        // ★ダッシュ中は無敵にする！
        if (isDashing)
        {
            return;
        }
        
        // もし接触した相手のタグが、池本君と決めた「合言葉」だったら
        if (other.CompareTag("Death"))
        {
            // GameManagerのEndGame関数を呼び出す
            GameManager.Instance.EndGame();

            // プレイヤー自身を画面から消す（非アクティブにする）
            gameObject.SetActive(false);
        }
    }
}
