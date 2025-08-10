using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager instance;
    private AudioSource seSource;

    [Header("SEの音声クリップ")]
    public AudioClip buttonClickSE;
    public AudioClip playerLightDamageSE; // 弾に当たった時の軽いダメージ音
    public AudioClip playerHeavyDamageSE; // 即死ダメージ音
    public AudioClip enemyAlertSE; // 警告音用のSE

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            seSource = GetComponent<AudioSource>();
            if (seSource == null)
            {
                seSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayButtonClick()
    {
        if (buttonClickSE != null)
        {
            seSource.PlayOneShot(buttonClickSE);
        }
    }

    // --- ▼ここからが今回の重要な修正▼ ---

    // 軽いダメージ音を再生するメソッド
    public void PlayPlayerLightDamage()
    {
        if (playerLightDamageSE != null)
        {
            seSource.PlayOneShot(playerLightDamageSE);
        }
    }

    // 致命的なダメージ音を再生するメソッド
    public void PlayPlayerHeavyDamage()
    {
        if (playerHeavyDamageSE != null)
        {
            seSource.PlayOneShot(playerHeavyDamageSE);
        }
    }
    // --- ▲ここまでが今回の重要な修正▲ ---

    // 警告音を再生するための公開メソッド
    public void PlayEnemyAlert()
    {
        if (enemyAlertSE != null)
        {
            seSource.PlayOneShot(enemyAlertSE);
        }
    }
}