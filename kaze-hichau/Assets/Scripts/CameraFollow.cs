using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // 追跡するターゲット（プレイヤー）をUnityエディタから設定するための変数
    public Transform target;

    // カメラとターゲットの相対的な位置を保つためのオフセット
    // 2Dの場合、カメラは通常Z軸が-10なので、それを維持する
    public Vector3 offset = new Vector3(0, 0, -10);

    // LateUpdateは、全てのUpdate処理が終わった後に呼ばれる
    // プレイヤーが移動した「後」にカメラが動くので、カクつきがなくなる
    void LateUpdate()
    {
        // ターゲットが設定されていなければ、何もしない
        if (target == null) return;

        // カメラの位置を「ターゲットの位置 + オフセット」に更新する
        transform.position = target.position + offset;
    }
}
