using UnityEngine;

public class PlatformUIController : MonoBehaviour
{
    [Header("スマホでのみ表示するUI")]
    public GameObject mobileUIObject; // 仮想スティックのGameObject

    void Awake()
    {
        if (mobileUIObject == null) return;

        // --- ▼ここからが重要な修正▼ ---

#if UNITY_EDITOR
        // Unityエディタ上では、テストしやすいように常に表示しておく（お好みでfalseにもできます）
        mobileUIObject.SetActive(true);

#elif UNITY_STANDALONE
        // PC向けビルドでは非表示
        mobileUIObject.SetActive(false);

#elif UNITY_WEBGL
        // WebGLビルドの場合、モバイル端末かどうかで判定する
        if (Application.isMobilePlatform)
        {
            // スマホやタブレットのブラウザなら表示
            mobileUIObject.SetActive(true);
        }
        else
        {
            // PCのブラウザなら非表示
            mobileUIObject.SetActive(false);
        }
#else
        // AndroidアプリやiOSアプリとしてビルドした場合は表示
        mobileUIObject.SetActive(true);
#endif

        // --- ▲修正ここまで▲ ---
    }
}