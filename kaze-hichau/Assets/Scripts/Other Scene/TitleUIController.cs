using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必須！

public class TitleUIController : MonoBehaviour
{
    // ボタンが押された時に呼び出す関数
    public void OnPlayButtonClicked()
    {
        // "GameScene"という名前のシーンをロードする
        SceneManager.LoadScene("GameScene");
    }
}
