using UnityEngine;
using Cysharp.Threading.Tasks; // UniTaskを使用
using System.Collections;// ★コルーチン（ダッシュ）のために必要！

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;

    // --- ▼ここからが今回の重要な修正▼ ---
    private float originalMoveSpeed; // 本来の移動速度を保存しておく変数
    private bool isSlowed = false;    // 減速中かどうかを管理するフラグ

    [Header("減速デバフの設定")]
    public float slowMultiplier = 0.5f; // 移動速度の倍率（0.5 = 50%）
    public float slowDuration = 2f;     // 減速効果の時間（秒）
    // --- ▲ここまでが今回の重要な修正▲ ---
    // --- ▼★ここからダッシュ機能を追加！▼ ---
    [Header("ダッシュの設定")]
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public ParticleSystem dashParticles;

    private Vector2 lastMoveDirection;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;
    // --- ▲★ここまでダッシュ機能を追加！▲ ---
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // --- ▼修正点▼ ---
        // 起動時に本来の移動速度を記憶しておく
        originalMoveSpeed = moveSpeed;
        // --- ▲修正点▲ ---

        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerTransform = this.transform;
        }
    }

    void Update()
    {
        // ★ダッシュ中は移動入力を受け付けないようにする
        if (isDashing)
        {
            moveInput = Vector2.zero;
        }
        else
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(moveX, moveY).normalized;
        }
        // もし、横方向の入力があったら…
        if (moveInput.x != 0)
        {
            // 右を向いていたら(xがプラス)、反転しない (flipX = false)
            // 左を向いていたら(xがマイナス)、反転する (flipX = true)
            spriteRenderer.flipX = moveInput.x > 0;
        }

        // ★最後に動いた方向を覚えておく（ダッシュ方向を決めるため）
        if (moveInput.magnitude > 0)
        {
            lastMoveDirection = moveInput;
        }

        // Animatorに今の状態を教える
        if(animator != null)
        {
            // animator.SetFloat("Horizontal", moveInput.x); // ←もしBlendTreeを使うならこっち
            // animator.SetFloat("Vertical", moveInput.y);
            animator.SetFloat("MoveSpeed", moveInput.sqrMagnitude);
        }

        // ★ダッシュのクールダウンタイマーを進める
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // ★Shiftキーが押されて、ダッシュ中でなくて、クールダウンが終わってたらダッシュ！
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownTimer <= 0)
        {
            StartCoroutine(DashCoroutine());
        }
    }
    void FixedUpdate()
    {
        // ★ダッシュ中は物理的な移動をさせない
        if (isDashing)
        {
            return;
        }
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ★★★ マスクの無敵か、ダッシュ中の無敵、どっちかが有効なら当たり判定を無視！
        if (isInvincible || isDashing)
        {
            return;
        }
        // --- ▼ここからが今回の重要な修正▼ ---

        // もし接触した相手が「Bullet」タグなら
        if (other.CompareTag("Bullet"))
        {
            // 減速デバフ処理を呼び出す
            ApplySlowDebuff();
            // 弾を消す
            Destroy(other.gameObject);
        }
        // マスク取得の処理はそのまま
        else if (other.CompareTag("Mask"))
        {
            ActivateInvincibility(5f).Forget();
            Destroy(other.gameObject);
        }
        // 敵の攻撃に当たった時の処理（"Death"タグに変更）
        else if (other.CompareTag("Death") && !isInvincible)
        {
            GameManager.Instance.EndGame();
            gameObject.SetActive(false);
        }
        // --- ▲ここまでが今回の重要な修正▲ ---
    }
    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        dashCooldownTimer = dashCooldown;
        rb.AddForce(lastMoveDirection * dashSpeed, ForceMode2D.Impulse);

        if (dashParticles != null)
        {
            float angle = Mathf.Atan2(-lastMoveDirection.y, -lastMoveDirection.x) * Mathf.Rad2Deg;
            dashParticles.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            dashParticles.Play();
        }
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
    // --- ▼ここからが今回の重要な修正▼ ---
    // 減速デバフを適用する非同期メソッド
    private async UniTaskVoid ApplySlowDebuff()
    {
        // すでに減速中でなければ
        if (!isSlowed)
        {
            isSlowed = true;
            moveSpeed = originalMoveSpeed * slowMultiplier; // 速度を遅くする
            spriteRenderer.color = Color.cyan; // 色を変えてデバフを分かりやすくする

            // 指定された時間、待機する
            await UniTask.Delay((int)(slowDuration * 1000));

            // 元の状態に戻す
            moveSpeed = originalMoveSpeed;
            spriteRenderer.color = Color.white;
            isSlowed = false;
        }
    }

    // --- ▲ここまでが今回の重要な修正▲ ---

    private async UniTaskVoid ActivateInvincibility(float duration)
    {
        isInvincible = true;
        Debug.Log("無敵状態になりました！");

        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            await UniTask.Delay(100);
            spriteRenderer.color = Color.white;
            await UniTask.Delay(100);
        }
        
        isInvincible = false;
        Debug.Log("無敵状態が終了しました。");
    }
}