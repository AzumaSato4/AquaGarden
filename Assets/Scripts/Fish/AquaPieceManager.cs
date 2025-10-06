using System.Collections;
using UnityEngine;

public class AquaPieceManager : MonoBehaviour
{
    [SerializeField] GameObject piecePrefab;
    public GameObject selectedPiece;
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
        uiController.AbledCancel(true);
        TurnManager.currentPlayer.GetComponent<PlayerManager>().Invoke("SelectSlot", 0.1f);
    }

    public void CanselSelect()
    {
        selectedPiece.GetComponent<Animator>().enabled = false;
        selectedPiece.transform.localScale = new Vector2(1.5f, 1.5f);
        selectedPiece = null;
        uiController.AbledCancel(false);
        TurnManager.currentPlayer.GetComponent<PlayerManager>().DontSelectSlot();
        TurnManager.currentPlayer.GetComponent<PlayerManager>().aquariumBoard.storage.GetComponent<Storage>().Invoke("CheckSpotEmpty", 0.1f);
    }

    public void CreatePiece(PieceData pieceData, int amount = 0, bool isFromSea = false)
    {
        GameObject spot = TurnManager.currentPlayer.GetComponent<PlayerManager>().aquariumBoard.storage.GetComponent<Storage>().Instorage();
        if (spot != null)
        {
            GameObject piece = Instantiate(
                piecePrefab,
                spot.transform.position,
                Quaternion.identity
            );

            if (piece != null)
            {
                piece.GetComponent<AquaPiece>().pieceData = pieceData;
                piece.GetComponent<AquaPiece>().isiFromSea = isFromSea;
                piece.GetComponent<AquaPieceController>().playerManager = TurnManager.currentPlayer.GetComponent<PlayerManager>();
                piece.GetComponent<AquaPieceController>().aquaPieceManager = this;
                TurnManager.currentPlayer.GetComponent<PlayerManager>().money -= amount;
            }
        }

    }

    public void MoveToSeaBoard()
    {
        StartCoroutine(SelectCoroutine());
    }

    IEnumerator SelectCoroutine()
    {
        uiController.ShowMassagePanel("海ボードに移動させますか？", selectedPiece.GetComponent<AquaPiece>().pieceData.pieceSprite);

        while (uiController.isActiveUI)
        {
            yield return null;
        }

        if (uiController.isOK)
        {
            GameObject currentPos = selectedPiece.GetComponent<AquaPiece>().currentPos;
            if (currentPos != null && currentPos.CompareTag("AquaSlot"))
            {
                AquaSlot aquaSlot = currentPos.GetComponent<AquaSlot>();
                aquaSlot.slotPieces.Remove(selectedPiece);
                aquaSlot.slotOxygen -= selectedPiece.GetComponent<AquaPiece>().pieceData.oxygen;
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
}
