using UnityEngine;
using TMPro; // TextMeshProを扱うために必要
using UnityEngine.SceneManagement; // シーン管理に必要
using Cysharp.Threading.Tasks; // UniTaskを使用する場合

public class GameManager : MonoBehaviour
{
    // シングルトンのための静的インスタンス
    public static GameManager Instance { get; private set; }

    // ゲームの状態
    public enum GameState
    {
        Ready,    // 準備中
        Playing,  // プレイ中
        GameOver  // ゲームオーバー
    }
    public GameState currentState { get; private set; }

    [Header("UI参照")]
    public Transform playerTransform; // プレイヤーのTransformを保持する公開変数
    public TextMeshProUGUI scoreText; // スコア表示用UIテキスト

    public float score; // スコアを保持する変数

    #region Unity Lifecycle Methods

    // このオブジェクトが有効になるたびに呼ばれる
    private void OnEnable()
    {
        // シーンがロードされた時にOnSceneLoadedメソッドを呼ぶように登録します
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // このオブジェクトが無効になるたびに呼ばれる
    private void OnDisable()
    {
        // オブジェクトが破棄される際に登録を解除します（お作法です）
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // GameManagerが最初に作られたこのタイミングでも、UIを探しに行きます
            FindScoreTextUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start()は最初の起動時のみの役割になります
    void Start()
    {
        // ゲームがGameSceneから直接始まった場合のみ、StartGameを呼びます
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            StartGame();
        }
    }

    void Update()
    {
        if (currentState != GameState.Playing) return;

        score += Time.deltaTime * 10;
        
        UpdateScoreUI();
    }

    #endregion

    #region Game Flow Methods

    // シーンがロードされるたびに、このメソッドが自動的に呼ばれます
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // もしロードされたのが「GameScene」なら、ゲームをリセットします
        if (scene.name == "GameScene")
        {
            // 新しいシーンにあるUIを再取得します
            FindScoreTextUI();
            StartGame(); // ゲームを初期化して開始
        }
    }
    
    private void StartGame()
    {
        currentState = GameState.Playing;
        score = 0f;
        Debug.Log("Game Start!");
    }
    
    public void EndGame()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.GameOver;
            Debug.Log("Game Over! Final Score: " + score.ToString("F0"));
            
            // UniTaskを使い、より安全にシーンをロードします
            LoadResultSceneWithDelay().Forget();
        }
    }

    private async UniTaskVoid LoadResultSceneWithDelay()
    {
        // Time.timeScaleの影響を受けない1秒待機
        await UniTask.Delay(1000, ignoreTimeScale: true);
        SceneManager.LoadScene("ResultScene");
    }

    #endregion

    #region UI Methods

    // UIを探して、scoreText変数を設定する処理
    private void FindScoreTextUI()
    {
        // ※Unityエディタでスコア表示UIに"ScoreTextUI"タグを設定してください
        var scoreUIObject = GameObject.FindWithTag("ScoreTextUI");
        if (scoreUIObject != null)
        {
            scoreText = scoreUIObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            // もし見つからなかった場合、エラーログを出しておくとデバッグが楽になります
            Debug.LogWarning("ScoreTextUIタグを持つオブジェクトが見つかりません");
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score.ToString("F0");
        }
    }

    #endregion
}