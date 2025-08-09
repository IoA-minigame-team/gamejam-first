using UnityEngine;
using UnityEngine.SceneManagement; // �V�[���Ǘ��ɕK�v

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    private AudioSource bgmSource;

    [Header("BGM�N���b�v")]
    public AudioClip titleBGM;     // �^�C�g����ʗp��BGM
    public AudioClip gameplayBGM;  // �Q�[���v���C����BGM
    public AudioClip resultBGM;    // ���U���g��ʗp��BGM

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
        // �ŏ��Ƀ^�C�g��BGM�����[�v�Đ�
        ChangeBGM(titleBGM, true);
    }

    // �V�[�������[�h���ꂽ���ɌĂ΂�郁�\�b�h
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip targetBGM = null;
        bool loop = true; // �f�t�H���g�̓��[�v�Đ�

        // �V�[�����ɉ����čĐ�����BGM�ƃ��[�v�ݒ������
        if (scene.name == "GameScene")
        {
            targetBGM = gameplayBGM;
            loop = true;
        }
        else if (scene.name == "ResultScene")
        {
            targetBGM = resultBGM;
            loop = false; // �����U���g��ʂ̓��[�v���Ȃ�
        }
        else // TitleScene��How-to-playScene�ȂǁA����ȊO�̃V�[��
        {
            targetBGM = titleBGM;
            loop = true;
        }

        // BGM��ύX
        ChangeBGM(targetBGM, loop);
    }

    // BGM��ύX���郁�\�b�h�i���[�v�ݒ�̈�����ǉ��j
    public void ChangeBGM(AudioClip musicClip, bool shouldLoop)
    {
        // �Đ��������N���b�v�����ɍĐ����ŁA���[�v�ݒ�������Ȃ牽�����Ȃ�
        if (musicClip == null || (bgmSource.clip == musicClip && bgmSource.loop == shouldLoop))
        {
            return;
        }

        bgmSource.Stop();
        bgmSource.clip = musicClip;
        bgmSource.loop = shouldLoop; // �����[�v�ݒ�𔽉f
        bgmSource.Play();
    }
}