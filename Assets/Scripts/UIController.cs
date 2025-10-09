using NUnit.Framework.Internal;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject seaBoard; //海ボード
    [SerializeField] GameObject advanceBoard; //上級駒ボード
    [SerializeField] GameObject attentionPanel; //警告パネル
    [SerializeField] GameObject messagePanel; //メッセージパネル
    [SerializeField] GameObject message; //メッセージ
    public static TextMeshProUGUI messageText; //メッセージパネルのテキスト

    public bool isActiveUI = false; //UIが表示されているかどうか
    public bool isOK;
    public static bool isMessageChanged = false;

    AquaPieceManager aquaPieceManager;

    private void Start()
    {
        aquaPieceManager = GetComponent<AquaPieceManager>();
        messageText = message.GetComponent<TextMeshProUGUI>();

        seaBoard.SetActive(false);
        advanceBoard.SetActive(false);
        attentionPanel.SetActive(false);
        messagePanel.SetActive(false);
    }

    private void Update()
    {
        if (!isMessageChanged) return;

        isMessageChanged = false;
        ShowMessagePanel();
    }

    public void OnSeaBoradButton()
    {
        if (aquaPieceManager.selectedPiece != null)
        {
            aquaPieceManager.MoveToSeaBoard();
            return;
        }
        else
        {
            if (advanceBoard.activeSelf) advanceBoard.SetActive(false);
            seaBoard.SetActive(!isActiveUI);
            isActiveUI = !isActiveUI;
        }
    }

    public void OnAdvanceBoradButton()
    {
        if (aquaPieceManager.selectedPiece != null)
        {
            aquaPieceManager.CanselSelect();
        }

        if (seaBoard.activeSelf) seaBoard.SetActive(false);
        advanceBoard.SetActive(!isActiveUI);
        isActiveUI = !isActiveUI;

    }

    public void ShowAttentionPanel(string massage, Sprite sprite)
    {
        isActiveUI = true;
        attentionPanel.SetActive(true);
        attentionPanel.GetComponent<AttentionPanel>().ShowMassage(massage, sprite);

        StartCoroutine(OKCoroutine());
    }

    IEnumerator OKCoroutine()
    {
        while (attentionPanel.GetComponent<AttentionPanel>().isShow)
        {
            yield return null;
        }
        if (attentionPanel.GetComponent<AttentionPanel>().isOK)
        {
            isActiveUI = false;
            attentionPanel.SetActive(false);
            isOK = true;
        }
        else
        {
            isActiveUI = false;
            attentionPanel.SetActive(false);
            isOK = false;
        }
    }
    public  void ShowMessagePanel()
    {
        messagePanel.SetActive(true);

        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        isActiveUI = false;
        messagePanel.SetActive(false);
    }
}
