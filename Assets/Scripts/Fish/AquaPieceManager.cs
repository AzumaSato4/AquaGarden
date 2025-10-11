using System.Collections;
using UnityEngine;

public class AquaPieceManager : MonoBehaviour
{
    [SerializeField] GameObject piecePrefab;
    public GameObject selectedPiece;
    UIController uiController;
    public PhaseManager phaseManager;
    [SerializeField] SeaBoard seaBoard;

    private void Update()
    {
    }

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
        current.Invoke("SelectSlot", 0.1f);
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

    public void CreatePiece(PieceData pieceData, int amount = 0, bool isFromSea = false)
    {
        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();

        GameObject spot = current.aquariumBoard.storage.GetComponent<Storage>().Instorage();
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
