using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeaItem : MonoBehaviour
{
    public SeaBoard seaBoard;
    public PieceData pieceData;
    public GameObject pieceNameText;
    public Image pieceImg;
    public GameObject pieceCountText;
    int pieceCount;

    UIController uiController;
    AquaPieceManager aquaPieceManager;

    private void Start()
    {
        pieceNameText.GetComponent<TextMeshProUGUI>().text = pieceData.pieceName;
        pieceImg.sprite = pieceData.pieceSprite;
        pieceCount = seaBoard.seaAquaPieces[pieceData.pieceName];
        pieceCountText.GetComponent<TextMeshProUGUI>().text = pieceCount.ToString();
        aquaPieceManager = GameObject.Find("MainManager").GetComponent<AquaPieceManager>();
        uiController = GameObject.Find("MainManager").GetComponent<UIController>();
    }

    private void Update()
    {
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
        }
        else
        {
            Debug.Log("資金が足りません！");
            uiController.OnSeaBoradButton();
        }
    }
}
