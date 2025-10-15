using NUnit.Framework.Internal;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
        none //非表示
    }

    [SerializeField] GameObject[] panels; //UIボード

    [SerializeField] GameObject seaBoard; //海ボード
    [SerializeField] GameObject advanceBoard; //上級駒ボード
    [SerializeField] GameObject attentionPanel; //警告パネル
    [SerializeField] GameObject messagePanel; //メッセージパネル
    [SerializeField] GameObject message; //メッセージ
    public static TextMeshProUGUI messageText; //メッセージパネルのテキスト
    [SerializeField] GameObject hintPanel; //魚駒の情報一覧のパネル
    [SerializeField] GameObject cardPanel; //マイルストーンと餌やりイベントのカードパネル

    public static bool isActiveUI = false; //UIが表示されているかどうか
    public bool isOK;
    public static bool isMessageChanged = false;

    AquaPieceManager aquaPieceManager;

    private void Start()
    {
        aquaPieceManager = GetComponent<AquaPieceManager>();
        messageText = message.GetComponent<TextMeshProUGUI>();

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
    }

    //パネルを表示
    void ShowPanel(PanelType panel)
    {
        //一度すべて非表示
        for (int i = 0;i < panels.Length;i++)
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
        for(int i = 0;i < panels.Length; i++)
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
                OnSeaBoradButton();
                break;
            case PanelType.none:
                HidePanel();
                break;
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

    //海ボードボタンが押されたらパネルを表示、非表示
    public void OnSeaBoradButton()
    {
        //魚駒が選択中なら海ボードへ移動させるメソッドを実行
        if (AquaPieceManager.selectedPiece != null)
        {
            aquaPieceManager.MoveToSeaBoard();
            return;
        }
    }

    //警告パネル
    public void ShowAttentionPanel(string massage, Sprite sprite)
    {
        isActiveUI = true;
        attentionPanel.GetComponent<AttentionPanel>().ShowMassage(massage, sprite);

        StartCoroutine(OKCoroutine());
    }

    IEnumerator OKCoroutine()
    {
        //警告パネルでの操作が終わるまで待機
        while (attentionPanel.GetComponent<AttentionPanel>().isShow)
        {
            yield return null;
        }

        if (attentionPanel.GetComponent<AttentionPanel>().isOK)
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
    }

    //メッセージパネル
    public void ShowMessagePanel()
    {
        isActiveUI = true;
        panels[(int)PanelType.message].SetActive(true);
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        panels[(int)PanelType.message].SetActive(false);
        isActiveUI = false;
    }
}
