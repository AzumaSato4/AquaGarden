using UnityEngine;

public class GalleryPiece : MonoBehaviour
{
    public PieceData pieceData;

    private void Start()
    {
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
        GetComponent<Animator>().enabled = false;
    }
}
