using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdvanceItem : MonoBehaviour
{
    public AdvanceBoard advanceBoard;
    public PieceData pieceData;
    public TextMeshProUGUI pieceNameText;
    public Image pieceImg;
    public TextMeshProUGUI pieceCountText;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] Button buyButton;
    int pieceCount;

    UIController uiController;
    AquaPieceManager aquaPieceManager;

    private void Start()
    {
        pieceNameText.text = pieceData.pieceName.ToString();
        pieceImg.sprite = pieceData.pieceSprite;
        pieceCount = advanceBoard.advanceAquaPieces[pieceData.pieceName];
        pieceCountText.text = pieceCount.ToString();
        amountText.text = pieceData.amount.ToString();
        aquaPieceManager = GameObject.Find("MainManager").GetComponent<AquaPieceManager>();
        uiController = GameObject.Find("MainManager").GetComponent<UIController>();
    }

    private void Update()
    {
        if (PhaseManager.currentPhase == PhaseManager.Phase.ad)
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable= false;
        }
        pieceCount = advanceBoard.advanceAquaPieces[pieceData.pieceName];
        pieceCountText.GetComponent<TextMeshProUGUI>().text = pieceCount.ToString();
    }

    public void BuyPiece()
    {
        PlayerManager currentManager = TurnManager.currentPlayer.GetComponent<PlayerManager>();
        if (currentManager.money >= pieceData.amount)
        {
            advanceBoard.advanceAquaPieces[pieceData.pieceName]--;

            aquaPieceManager.CreatePiece(pieceData, pieceData.amount, true);
            uiController.ChangeUI(UIController.PanelType.none);
            if (advanceBoard.advanceAquaPieces[pieceData.pieceName] <= 0) Destroy(this.gameObject);

            //購入後水族館編集フェーズへ
            currentManager.AdEditAquarium();
        }
        else
        {
            uiController.ChangeUI(UIController.PanelType.none);
            Debug.Log("資金が足りません！");
            ShowMessage("資金が足りません！");
        }
    }

    void ShowMessage(string message)
    {
        UIController.messageText.text = message;
        UIController.isMessageChanged = true;
    }
}
