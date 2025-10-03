using UnityEngine;

public class AquaPiece : MonoBehaviour
{
    public PieceData pieceData;
    public GameObject currentPos;
    public AquaPieceController aquaPieceController;

    private void Start()
    {
        aquaPieceController = GetComponent<AquaPieceController>();
        GetComponent<SpriteRenderer>().sprite = pieceData.pieceSprite;
        GetComponent<Animator>().runtimeAnimatorController = pieceData.animation;
        Debug.Log("ê∂ê¨");
    }

    private void OnMouseEnter()
    {
        GetComponent<Animator>().enabled = true;
    }

    private void OnMouseExit()
    {
        if (aquaPieceController.aquaPieceManager.selectedPiece != this.gameObject)
        {
            GetComponent<Animator>().enabled = false;
        }
    }

    private void OnMouseDown()
    {
        if (aquaPieceController.aquaPieceManager.selectedPiece == null && aquaPieceController.playerManager.isActive && aquaPieceController.playerManager.phaseManager.currentPhase == PhaseManager.Phase.edit)
        {
            transform.localScale = new Vector2(2.5f, 2.5f);
            aquaPieceController.aquaPieceManager.SelectedPiece(this.gameObject);
        }
        else return;
    }
}
