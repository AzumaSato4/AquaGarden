using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject seaBoard; //海ボード
    [SerializeField] GameObject messagePanel; //メッセージパネル

    public bool isActiveUI = false; //UIが表示されているかどうか
    public bool isOK;

    AquaPieceManager aquaPieceManager;

    private void Start()
    {
        aquaPieceManager = GetComponent<AquaPieceManager>();

        seaBoard.SetActive(false);
        messagePanel.SetActive(false);
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
            seaBoard.SetActive(!isActiveUI);
            isActiveUI = !isActiveUI;
        }
    }

    public void ShowMassagePanel(string massage, Sprite sprite)
    {
        isActiveUI = true;
        messagePanel.SetActive(true);
        messagePanel.GetComponent<MessagePanel>().ShowMassage(massage, sprite);

        StartCoroutine(OKCoroutine());
    }

    IEnumerator OKCoroutine()
    {
        while (messagePanel.GetComponent<MessagePanel>().isShow)
        {
            yield return null;
        }
        if (messagePanel.GetComponent<MessagePanel>().isOK)
        {
            isActiveUI = false;
            messagePanel.SetActive(false);
            isOK = true;
        }
        else
        {
            isActiveUI = false;
            messagePanel.SetActive(false);
            isOK = false;
        }
    }
}
