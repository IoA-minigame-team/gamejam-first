// EnemyAttackBase.cs
using UnityEngine;

// これから作る「攻撃のスクリプト」の設計図だよ！
public abstract class EnemyAttackBase : MonoBehaviour
{
    // 攻撃するための命令文。中身はそれぞれの攻撃スクリプトで書くよ！
    public abstract void Attack(Transform enemyTransform, Transform playerTransform);
}