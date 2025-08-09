using UnityEngine;
using UnityEngine.SceneManagement; // シーン管理に必要

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    private AudioSource bgmSource;

    [Header("BGMクリップ")]
    public AudioClip titleBGM;     // タイトル画面用のBGM
    public AudioClip gameplayBGM;  // ゲームプレイ中のBGM
    public AudioClip resultBGM;    // リザルト画面用のBGM

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        bgmSource = GetComponent<AudioSource>();
        // 最初にタイトルBGMをループ再生
        ChangeBGM(titleBGM, true);
    }

    // シーンがロードされた時に呼ばれるメソッド
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip targetBGM = null;
        bool loop = true; // デフォルトはループ再生

        // シーン名に応じて再生するBGMとループ設定を決定
        if (scene.name == "GameScene")
        {
            targetBGM = gameplayBGM;
            loop = true;
        }
        else if (scene.name == "ResultScene")
        {
            targetBGM = resultBGM;
            loop = false; // ★リザルト画面はループしない
        }
        else // TitleSceneやHow-to-playSceneなど、それ以外のシーン
        {
            targetBGM = titleBGM;
            loop = true;
        }

        // BGMを変更
        ChangeBGM(targetBGM, loop);
    }

    // BGMを変更するメソッド（ループ設定の引数を追加）
    public void ChangeBGM(AudioClip musicClip, bool shouldLoop)
    {
        // 再生したいクリップが既に再生中で、ループ設定も同じなら何もしない
        if (musicClip == null || (bgmSource.clip == musicClip && bgmSource.loop == shouldLoop))
        {
            return;
        }

        bgmSource.Stop();
        bgmSource.clip = musicClip;
        bgmSource.loop = shouldLoop; // ★ループ設定を反映
        bgmSource.Play();
    }
}