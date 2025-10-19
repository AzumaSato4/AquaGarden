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
    [SerializeField] TextMeshProUGUI oxygenText;
    [SerializeField] Button buyButton;
    int pieceCount;

    UIController uiController;
    AquaPieceManager aquaPieceManager;
    SoundManager soundManager;

    private void Start()
    {
        pieceNameText.text = pieceData.pieceName.ToString();
        pieceImg.sprite = pieceData.pieceSprite;
        pieceCount = seaBoard.seaAquaPieces[pieceData.pieceName];
        pieceCountText.text = pieceCount.ToString();
        oxygenText.text = pieceData.oxygen.ToString();
        aquaPieceManager = GameObject.Find("MainManager").GetComponent<AquaPieceManager>();
        uiController = GameObject.Find("MainManager").GetComponent<UIController>();
        soundManager = SoundManager.instance;
    }

    private void Update()
    {
        if (PhaseManager.currentPhase == PhaseManager.Phase.edit || PhaseManager.currentPhase == PhaseManager.Phase.mileEdit)
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

            soundManager.PlaySE(SoundManager.SE_Type.pay);
            aquaPieceManager.CreatePiece(pieceData, 2, true);
            uiController.ChangeUI(UIController.PanelType.none);
            if (seaBoard.seaAquaPieces[pieceData.pieceName] <= 0) Destroy(this.gameObject);
        }
        else
        {
            soundManager.PlaySE(SoundManager.SE_Type.ng);
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
