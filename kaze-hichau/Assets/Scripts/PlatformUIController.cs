using UnityEngine;

public class PlatformUIController : MonoBehaviour
{
    [Header("�X�}�z�ł̂ݕ\������UI")]
    public GameObject mobileUIObject; // ���z�X�e�B�b�N��GameObject

    void Awake()
    {
        if (mobileUIObject == null) return;

        // --- ���������炪�d�v�ȏC���� ---

#if UNITY_EDITOR
        // Unity�G�f�B�^��ł́A�e�X�g���₷���悤�ɏ�ɕ\�����Ă����i���D�݂�false�ɂ��ł��܂��j
        mobileUIObject.SetActive(true);

#elif UNITY_STANDALONE
        // PC�����r���h�ł͔�\��
        mobileUIObject.SetActive(false);

#elif UNITY_WEBGL
        // WebGL�r���h�̏ꍇ�A���o�C���[�����ǂ����Ŕ��肷��
        if (Application.isMobilePlatform)
        {
            // �X�}�z��^�u���b�g�̃u���E�U�Ȃ�\��
            mobileUIObject.SetActive(true);
        }
        else
        {
            // PC�̃u���E�U�Ȃ��\��
            mobileUIObject.SetActive(false);
        }
#else
        // Android�A�v����iOS�A�v���Ƃ��ăr���h�����ꍇ�͕\��
        mobileUIObject.SetActive(true);
#endif

        // --- ���C�������܂Ł� ---
    }
}