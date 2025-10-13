using System.Collections;
using System.Linq;
using UnityEngine;

public class AquaPieceManager : MonoBehaviour
{
    [SerializeField] GameObject piecePrefab;
    public static GameObject selectedPiece;
    UIController uiController;
    public PhaseManager phaseManager;
    [SerializeField] SeaBoard seaBoard;

    private void Start()
    {
        uiController = GetComponent<UIController>();
        phaseManager = GetComponent<PhaseManager>();
    }

    public void SelectedPiece(GameObject selected)
    {
        selectedPiece = selected;
        selectedPiece.GetComponent<Animator>().enabled = true;

        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();
        current.AbledCancel(true);
        current.SelectSlot();
    }

    public void CanselSelect()
    {
        selectedPiece.GetComponent<Animator>().enabled = false;
        selectedPiece.transform.localScale = new Vector2(1.5f, 1.5f);
        selectedPiece = null;

        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();
        current.AbledCancel(false);
        current.DontSelectSlot();
        current.aquariumBoard.storage.GetComponent<Storage>().Invoke("CheckSpotEmpty", 0.1f);
    }

    //魚駒を現在のプレイヤーのストレージに生成
    public void CreatePiece(PieceData pieceData, int amount = 0, bool isFromSea = false)
    {
        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();

        (GameObject spot, int index) = current.aquariumBoard.storage.GetComponent<Storage>().Instorage();
        if (spot != null)
        {
            GameObject piece = Instantiate(
                piecePrefab,
                spot.transform.position,
                Quaternion.identity
            );

            if (piece != null)
            {
                AquaPiece aquaPiece = piece.GetComponent<AquaPiece>();
                AquaPieceController pieceController = piece.GetComponent<AquaPieceController>();

                aquaPiece.pieceData = pieceData;
                aquaPiece.isiFromSea = isFromSea;
                aquaPiece.storageIndex = index;
                pieceController.playerManager = current;
                pieceController.aquaPieceManager = this;
                current.money -= amount;
            }
        }

    }

    public void MoveToSeaBoard()
    {
        StartCoroutine(SelectCoroutine());
    }

    IEnumerator SelectCoroutine()
    {
        uiController.ShowAttentionPanel("海ボードに移動させますか？", selectedPiece.GetComponent<AquaPiece>().pieceData.pieceSprite);

        while (UIController.isActiveUI)
        {
            yield return null;
        }

        if (uiController.isOK)
        {

            GameObject currentPos = selectedPiece.GetComponent<AquaPiece>().currentPos;
            if (currentPos != null && currentPos.CompareTag("AquaSlot"))
            {
                AquaSlot aquaSlot = currentPos.GetComponent<AquaSlot>();
                PieceData.PieceName name = selectedPiece.GetComponent<AquaPiece>().pieceData.pieceName;
                //サメ、ジンベエザメ
                if (name == PieceData.PieceName.Shark || name == PieceData.PieceName.WhaleShark)
                {
                    PieceData.PieceName[] names = new PieceData.PieceName[aquaSlot.slotPieces.Count];
                    for (int i = 0; i < names.Length; i++)
                    {
                        names[i] = aquaSlot.slotPieces[i].GetComponent<AquaPiece>().pieceData.pieceName;
                    }
                    //コバンザメ
                    if (names.Contains(PieceData.PieceName.Remora))
                    {
                        Debug.Log("水槽内にサメが必要です");
                        ShowMessage("水槽内にサメが必要です");

                        CanselSelect();
                        yield break;
                    }
                }
                //海藻
                if (name == PieceData.PieceName.Seaweed)
                {
                    //海藻を抜いて水槽内の酸素量が0未満ならNG
                    int oxygen = aquaSlot.slotOxygen - selectedPiece.GetComponent<AquaPiece>().pieceData.oxygen;
                    if (oxygen < 0)
                    {
                        Debug.Log("水槽の酸素量が足りなくなります");
                        ShowMessage("水槽の酸素量が足りなくなります");

                        CanselSelect();
                        yield break;
                    }
                }
                aquaSlot.ReleasePiece();
            }

            if (selectedPiece.GetComponent<AquaPiece>().isiFromSea)
            {
                TurnManager.currentPlayer.GetComponent<PlayerManager>().money += 2;
            }
            seaBoard.ReleasePiece(selectedPiece);
            Destroy(selectedPiece);
        }
        CanselSelect();
    }

    void ShowMessage(string message)
    {
        UIController.messageText.text = message;
        UIController.isMessageChanged = true;
    }
}
