using UnityEngine;
using TMPro; // TextMeshProを扱うために必要

public class GameManager : MonoBehaviour
{
    // シングルトンのための静的インスタンス
    public static GameManager Instance { get; private set; }

    // ゲームの状態（GameClearを削除）
    public enum GameState
    {
        Ready,    // 準備中
        Playing,  // プレイ中
        GameOver  // ゲームオーバー
    }
    public GameState currentState { get; private set; }

    // --- ここから変更 ---
    [Header("UI参照")]
    public TextMeshProUGUI scoreText; // スコア表示用UIテキスト

    private float score; // スコアを保持する変数
    // --- 変更ここまで ---

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

    void Start()
    {
        currentState = GameState.Ready;
        // 1秒後にゲームを開始する
        Invoke(nameof(StartGame), 1f);
    }

    private void StartGame()
    {
        currentState = GameState.Playing;
        score = 0f; // ゲーム開始時にスコアを0にリセット
        Debug.Log("Game Start!");
    }

    void Update()
    {
        // プレイ中でなければ何もしない
        if (currentState != GameState.Playing) return;

        // --- ここから変更 ---
        // 時間経過でスコアを加算していく（1秒で10点加算されるペース）
        score += Time.deltaTime * 10;
        
        // スコアUIを更新
        UpdateScoreUI();
        // --- 変更ここまで ---
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            // スコアを整数で表示
            scoreText.text = "SCORE: " + score.ToString("F0");
        }
    }
    
    // ゲームオーバー時に呼ばれる関数（中身はStep3で実装）
    public void EndGame()
    {
        if (currentState == GameState.Playing)
        {
            currentState = GameState.GameOver;
            Debug.Log("Game Over! Final Score: " + score.ToString("F0"));
            // ここにリザルト画面への遷移などを追加していく
        }
    }
}