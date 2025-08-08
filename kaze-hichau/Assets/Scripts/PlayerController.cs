using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false;
    private Coroutine invincibilityCoroutine; // コルーチンの参照を保持

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // nullの場合がある
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        // Animatorが存在し、かつAnimatorControllerが設定されている場合のみアニメーション処理を実行
        if (animator != null && animator.runtimeAnimatorController != null)
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
        Debug.Log($"衝突検出: {other.gameObject.name}, タグ: {other.tag}"); // デバッグログ追加

        if (other.CompareTag("Mask"))
        {
            ActivateInvincibility(3f); // 3秒に短縮
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Death") && !isInvincible)
        {
            Debug.Log("敵の攻撃に当たりました！"); // デバッグログ追加
            GameManager.Instance.EndGame();
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Death") && !isInvincible) // 敵本体との衝突も追加
        {
            Debug.Log("敵本体に当たりました！"); // デバッグログ追加
            GameManager.Instance.EndGame();
            gameObject.SetActive(false);
        }
    }

    private void ActivateInvincibility(float duration)
    {
        // 既存の無敵状態がある場合は停止
        if (invincibilityCoroutine != null)
        {
            StopCoroutine(invincibilityCoroutine);
        }

        invincibilityCoroutine = StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        Debug.Log("無敵状態になりました！");

        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        isInvincible = false;
        spriteRenderer.color = Color.white; // 確実に元の色に戻す
        invincibilityCoroutine = null;
        Debug.Log("無敵状態が終了しました。");
    }

    void OnDestroy()
    {
        // オブジェクト破棄時にコルーチンを停止
        if (invincibilityCoroutine != null)
        {
            StopCoroutine(invincibilityCoroutine);
        }
    }
}
