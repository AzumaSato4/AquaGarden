using System.Collections;
using UnityEngine;

public class AquaPieceManager : MonoBehaviour
{
    [SerializeField] GameObject piecePrefab;
    public GameObject selectedPiece;
    UIController uiController;
    TurnManager turnManager;
    public PhaseManager phaseManager;
    [SerializeField] SeaBoard seaBoard;

    private void Start()
    {
        uiController = GetComponent<UIController>();
        turnManager = GetComponent<TurnManager>();
        phaseManager = GetComponent<PhaseManager>();
    }

    public void SelectedPiece(GameObject selected)
    {
        selectedPiece = selected;
        selectedPiece.GetComponent<Animator>().enabled = true;
        uiController.AbledCancel(true);
        turnManager.currentPlayer.GetComponent<PlayerManager>().Invoke("SelectSlot", 0.1f);
    }

    public void CanselSelect()
    {
        selectedPiece.GetComponent<Animator>().enabled = false;
        selectedPiece.transform.localScale = new Vector2(1.5f, 1.5f);
        selectedPiece = null;
        uiController.AbledCancel(false);
        turnManager.currentPlayer.GetComponent<PlayerManager>().DontSelectSlot();
        turnManager.currentPlayer.GetComponent<PlayerManager>().aquariumBoard.storage.GetComponent<Storage>().Invoke("CheckSpotEmpty", 0.1f);
    }

    public void CreatePiece(PieceData pieceData)
    {
        GameObject spot = turnManager.currentPlayer.GetComponent<PlayerManager>().aquariumBoard.storage.GetComponent<Storage>().Instorage();
        if (spot != null)
        {
            GameObject piece = Instantiate(
                piecePrefab,
                spot.transform.position,
                Quaternion.identity
            );

            if (piece != null)
            {
                //piece.transform.localScale = new Vector2(0.8f, 0.8f);
                piece.GetComponent<AquaPiece>().pieceData = pieceData;
                piece.GetComponent<AquaPieceController>().playerManager = turnManager.currentPlayer.GetComponent<PlayerManager>();
                piece.GetComponent<AquaPieceController>().aquaPieceManager = this;
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
            seaBoard.ReleasePiece(selectedPiece);
            Destroy(selectedPiece);
        }
        CanselSelect();
    }
}
