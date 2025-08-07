using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultUIController : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        // シーンをまたいで生き残っているGameManagerからスコアを取得して表示
        float finalScore = GameManager.Instance.score; // ※GameManagerのscore変数をpublicにする必要があります
        finalScoreText.text = "SCORE: " + finalScore.ToString("F0");
    }

    public void OnRetryButtonClicked()
    {
        // ゲームシーンを再読み込み
        SceneManager.LoadScene("GameScene");
    }
}