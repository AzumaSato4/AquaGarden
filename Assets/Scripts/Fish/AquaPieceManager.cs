using UnityEngine;

public class AquaPieceManager : MonoBehaviour
{
    [SerializeField] GameObject piecePrefab;
    public GameObject selectedPiece;
    UIController uiController;
    TurnManager turnManager;
    public PhaseManager phaseManager;

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
    }

    public void CreatePiece(PlayerManager player,PieceData pieceData, GameObject to)
    {
        GameObject piece = Instantiate(piecePrefab, to.transform.position, Quaternion.identity);
        piece.GetComponent<AquaPiece>().pieceData = pieceData;
        piece.GetComponent<AquaPieceController>().playerManager = player;
        piece.GetComponent<AquaPieceController>().aquaPieceManager = this;
    }

    public void MoveToSeaBoard()
    {
        Destroy(selectedPiece);
        selectedPiece = null;
        uiController.AbledCancel(false);
        turnManager.currentPlayer.GetComponent<PlayerManager>().DontSelectSlot();
    }
}
