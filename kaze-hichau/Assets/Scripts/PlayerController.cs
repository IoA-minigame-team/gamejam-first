using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    
    void Start()
    {
        // 自分自身に付いているRigidbody 2Dコンポーネントを取得して、変数rbに入れる
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // 入力をVector2にまとめ、長さを1に正規化する（斜め移動が速くなるのを防ぐ）
        moveInput = new Vector2(moveX, moveY).normalized;
    }
    
    void FixedUpdate()
    {
        // Rigidbodyの速度を更新してプレイヤーを動かす
        rb.linearVelocity = moveInput * moveSpeed;
    }
    
    // PlayerController.cs のクラス内に追記
    private void OnTriggerEnter2D(Collider2D other)
    {
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
