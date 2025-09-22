using TMPro;
using UnityEngine;

public class TitleUIManager : MonoBehaviour
{
    public bool uiActive;

    [Header("���b�Z�[�W�\���p")]
    [SerializeField] GameObject messagePanel;
    [SerializeField] TextMeshProUGUI messageText;

    [SerializeField] GameObject[] panels;

    private void Awake()
    {
        //UI�͍ŏ���\���ɂ��Ă���
        if (messagePanel != null && panels != null)
            messagePanel.SetActive(false);
    }


    //�C���v�b�g��ʂ�\������
    public void ShowInputMenu()
    {
        panels[0].SetActive(false);
        panels[1].SetActive(true);
        //if (inputField != null)
        //    for (int i = 0; i < totalPlayers.Length; i++)
        //    {
        //        inputField[i].SetActive(true);
        //    }
    }


    //�^�C�g���ɖ߂�
    public void BackTitle()
    {
        panels[1].SetActive(false);
        panels[0].SetActive(true);
    }





    //���b�Z�[�W����ʂɕ\������
    public void ShowMessage(string msg)
    {
        uiActive = true;

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
        uiActive = false;
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}
