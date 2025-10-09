using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeaItem : MonoBehaviour
{
    public SeaBoard seaBoard;
    public PieceData pieceData;
    public TextMeshProUGUI pieceNameText;
    public Image pieceImg;
    public TextMeshProUGUI pieceCountText;
    [SerializeField] Button buyButton;
    int pieceCount;

    UIController uiController;
    AquaPieceManager aquaPieceManager;

    private void Start()
    {
        pieceNameText.text = pieceData.pieceName.ToString();
        pieceImg.sprite = pieceData.pieceSprite;
        pieceCount = seaBoard.seaAquaPieces[pieceData.pieceName];
        pieceCountText.text = pieceCount.ToString();
        aquaPieceManager = GameObject.Find("MainManager").GetComponent<AquaPieceManager>();
        uiController = GameObject.Find("MainManager").GetComponent<UIController>();
    }

    private void Update()
    {
        if (PhaseManager.currentPhase == PhaseManager.Phase.edit)
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }
        pieceCount = seaBoard.seaAquaPieces[pieceData.pieceName];
        pieceCountText.GetComponent<TextMeshProUGUI>().text = pieceCount.ToString();
    }

    public void BuyPiece()
    {
        if (TurnManager.currentPlayer.GetComponent<PlayerManager>().money >= 2)
        {
            seaBoard.seaAquaPieces[pieceData.pieceName]--;

            aquaPieceManager.CreatePiece(pieceData, 2, true);
            uiController.OnSeaBoradButton();
            if (seaBoard.seaAquaPieces[pieceData.pieceName] <= 0) Destroy(this.gameObject);
        }
        else
        {
            uiController.OnSeaBoradButton();
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
