using UnityEngine;

public class ItemController : MonoBehaviour
{
    [Header("アイテムの設定")]
    public float lifeTime = 7f; // アイテムが存在する時間（秒）
    
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}