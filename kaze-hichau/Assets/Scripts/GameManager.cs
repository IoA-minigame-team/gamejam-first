using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // ★シーン管理に必要
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public Transform playerTransform;　//プレイヤーの情報

    public enum GameState
    {
        Ready,
        Playing,
        GameOver
    }
    public GameState currentState { get; private set; }

    [Header("UI参照")]
    public TextMeshProUGUI scoreText;

    public float score;

    // --- ▼ここからがリトライ対応のための修正▼ ---

    private void OnEnable()
    {
        // シーンがロードされた時にOnSceneLoadedメソッドを呼ぶように登録します
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // オブジェクトが破棄される際に登録を解除します（お作法です）
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーンがロードされるたびに、このメソッドが自動的に呼ばれます
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // もしロードされたのが「GameScene」なら、ゲームをリセットします
        if (scene.name == "GameScene")
        {
            // ★シーンがロードされるたびに、新しいプレイヤーを探して覚え直す！
            var playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
            }
            // 新しいシーンにあるUIを再取得するため、タグで見つけます
            // ※Unityエディタでスコア表示UIに"ScoreTextUI"タグを設定してください
            var scoreUIObject = GameObject.FindWithTag("ScoreTextUI");
            if (scoreUIObject != null)
            {
                scoreText = scoreUIObject.GetComponent<TextMeshProUGUI>();
            }
            
            StartGame(); // ゲームを初期化して開始
        }
    }
    
    // --- ▲ここまでがリトライ対応のための修正▲ ---

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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

    private void StartGame()
    {
        currentState = GameState.Playing;
        score = 0f;
        Debug.Log("Game Start!");
    }

    void Update()
    {
        if (currentState != GameState.Playing) return;

        score += Time.deltaTime * 10;
        
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score.ToString("F0");
        }
    }
    
    public void EndGame()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.GameOver;
            Debug.Log("Game Over! Final Score: " + score.ToString("F0"));
            
            LoadResultSceneWithDelay().Forget();
        }
    }

    private async UniTaskVoid LoadResultSceneWithDelay()
    {
        await UniTask.Delay(1000, ignoreTimeScale: true);
        SceneManager.LoadScene("ResultScene");
    }
}