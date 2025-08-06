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
}
