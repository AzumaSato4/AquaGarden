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
        if (pieceCount > 0)
        {
            seaBoard.seaAquaPieces[pieceData.pieceName]--;

            aquaPieceManager.CreatePiece(pieceData);
            uiController.OnSeaBoradButton();
        }
        else
        {
            Debug.Log(pieceData.pieceName + "ÇÕÇ¢Ç‹ÇπÇÒÅI");
        }
    }
}
