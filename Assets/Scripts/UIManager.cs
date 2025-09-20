using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("メッセージ表示用")]
    [SerializeField] GameObject messagePanel;
    [SerializeField] TextMeshProUGUI messageText;

    private void Awake()
    {
        //既に存在していたら破棄
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //最初は非表示にしておく
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }


    //メッセージを画面に表示する
    public void ShowMessage(string msg)
    {
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
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}
