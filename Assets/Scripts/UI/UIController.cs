using System.Collections;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public enum PanelType
    {
        sea,
        advance,
        attention,
        message,
        hint,
        card,
        achieve,
        none //非表示
    }

    [SerializeField] GameObject[] panels; //UIボード

    [SerializeField] GameObject cameraChangeImage;
    public static TextMeshProUGUI messageText; //メッセージパネルのテキスト

    public static bool isActiveUI = false; //UIが表示されているかどうか
    public bool isOK;
    public static bool isMessageChanged = false;
    public static bool isAchieved; //マイルストーンを達成したかどうか

    AquaPieceManager aquaPieceManager;
    AttentionPanel attentionPanel;

    private void Start()
    {
        aquaPieceManager = GetComponent<AquaPieceManager>();
        messageText = panels[(int)PanelType.message].GetComponentInChildren<TextMeshProUGUI>();
        attentionPanel = panels[(int)PanelType.attention].GetComponent<AttentionPanel>();

        //パネルは最初すべて非表示
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }

    private void Update()
    {
        //メッセージが変更されたらパネルを表示
        if (isMessageChanged)
        {
            isMessageChanged = false;
            ShowMessagePanel();
        }

        if (isAchieved)
        {
            isAchieved = false;
            ShowAchievePanel();
        }

    }

    //パネルを表示
    void ShowPanel(PanelType panel)
    {
        //一度すべて非表示
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        //表示したいパネルだけ表示
        panels[(int)panel].SetActive(true);
        isActiveUI = true;
    }

    //パネルを非表示
    void HidePanel()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        isActiveUI = false;
    }

    public void ChangeUI(PanelType panel)
    {
        switch (panel)
        {
            case PanelType.sea:
                //魚駒が選択中なら海ボードへ移動させるメソッドを実行
                if (AquaPieceManager.selectedPiece != null)
                {
                    aquaPieceManager.MoveToSeaBoard();
                    return;
                }
                break;
            case PanelType.none:
                HidePanel();
                return;
        }

        //表示中なら非表示に
        if (panels[(int)panel].activeSelf)
        {
            HidePanel();
        }
        else
        {
            ShowPanel(panel);
        }
    }

    //警告パネル
    public void ShowAttentionPanel(string massage, Sprite sprite)
    {
        isActiveUI = true;
        ShowPanel(PanelType.attention);
        attentionPanel.ShowMassage(massage, sprite);

        StartCoroutine(OKCoroutine());
    }

    IEnumerator OKCoroutine()
    {

        //警告パネルでの操作が終わるまで待機
        while (attentionPanel.isShow)
        {
            yield return null;
        }

        if (attentionPanel.isOK)
        {
            //OKが選ばれたら
            isActiveUI = false;
            isOK = true;
        }
        else
        {
            isActiveUI = false;
            isOK = false;
        }
        HidePanel();
    }

    //メッセージパネル
    public void ShowMessagePanel()
    {
        ShowPanel(PanelType.message);
        Invoke("HidePanel", 2.0f);
    }

    void ShowAchievePanel()
    {
        ShowPanel(PanelType.achieve);
        Invoke("HidePanel", 2.0f);
    }

    //カメラ変更時の画面遷移画像
    public void ActiveCameraChangeImage()
    {
        cameraChangeImage.SetActive(true);
        Invoke("HideCameraChangeImage", 1.8f);
    }

    void HideCameraChangeImage()
    {
        cameraChangeImage.SetActive(false);
    }
}
