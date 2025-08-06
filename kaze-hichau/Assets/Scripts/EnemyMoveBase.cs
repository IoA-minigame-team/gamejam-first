// EnemyMoveBase.cs
using UnityEngine;

// これから作る「動きのスクリプト」は、みんなこの設計図を元に作ってね！っていうお約束だよ
public abstract class EnemyMoveBase : MonoBehaviour
{
    // 敵を動かすための命令文。中身はそれぞれの動きスクリプトで書くよ！
    public abstract void Move(Transform enemyTransform, Transform playerTransform);
}
