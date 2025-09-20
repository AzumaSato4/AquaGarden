using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("���b�Z�[�W�\���p")]
    [SerializeField] GameObject messagePanel;
    [SerializeField] TextMeshProUGUI messageText;

    private void Awake()
    {
        //���ɑ��݂��Ă�����j��
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //�ŏ��͔�\���ɂ��Ă���
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }


    //���b�Z�[�W����ʂɕ\������
    public void ShowMessage(string msg)
    {
        float duration = 2.0f;
        if (messageText != null)
            messageText.text = msg;

        if (messagePanel != null)
            messagePanel.SetActive(true);

        //��莞�Ԍ�ɕ���
        CancelInvoke(nameof(HideMessage));
        Invoke(nameof(HideMessage), duration);
    }


    // ���b�Z�[�W���\���ɂ���
    public void HideMessage()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}
