using TMPro;
using UnityEngine;

public class TitleUIManager : MonoBehaviour
{
    public bool uiActive;

    [Header("メッセージ表示用")]
    [SerializeField] GameObject messagePanel;
    [SerializeField] TextMeshProUGUI messageText;

    [SerializeField] GameObject[] panels;

    private void Awake()
    {
        //UIは最初非表示にしておく
        if (messagePanel != null && panels != null)
            messagePanel.SetActive(false);
    }


    //インプット画面を表示する
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


    //タイトルに戻る
    public void BackTitle()
    {
        panels[1].SetActive(false);
        panels[0].SetActive(true);
    }





    //メッセージを画面に表示する
    public void ShowMessage(string msg)
    {
        uiActive = true;

        float duration = 2.0f;
        if (messageText != null)
            messageText.text = msg;

        if (messagePanel != null)
            messagePanel.SetActive(true);

        //一定時間後に閉じる
        CancelInvoke(nameof(HideMessage));
        Invoke(nameof(HideMessage), duration);
    }


    // メッセージを非表示にする
    public void HideMessage()
    {
        uiActive = false;
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}
