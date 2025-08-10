using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager instance;
    private AudioSource seSource;

    [Header("SE�̉����N���b�v")]
    public AudioClip buttonClickSE;
    public AudioClip playerLightDamageSE; // �e�ɓ����������̌y���_���[�W��
    public AudioClip playerHeavyDamageSE; // �����_���[�W��
    public AudioClip enemyAlertSE; // �x�����p��SE

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

    // --- ���������炪����̏d�v�ȏC���� ---

    // �y���_���[�W�����Đ����郁�\�b�h
    public void PlayPlayerLightDamage()
    {
        if (playerLightDamageSE != null)
        {
            seSource.PlayOneShot(playerLightDamageSE);
        }
    }

    // �v���I�ȃ_���[�W�����Đ����郁�\�b�h
    public void PlayPlayerHeavyDamage()
    {
        if (playerHeavyDamageSE != null)
        {
            seSource.PlayOneShot(playerHeavyDamageSE);
        }
    }
    // --- �������܂ł�����̏d�v�ȏC���� ---

    // �x�������Đ����邽�߂̌��J���\�b�h
    public void PlayEnemyAlert()
    {
        if (enemyAlertSE != null)
        {
            seSource.PlayOneShot(enemyAlertSE);
        }
    }
}