using UnityEngine;
using Cysharp.Threading.Tasks; // UniTaskを使用

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
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        if(animator != null)
        {
            animator.SetFloat("Horizontal", moveInput.x);
            animator.SetFloat("Vertical", moveInput.y);
            animator.SetFloat("Speed", moveInput.sqrMagnitude);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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