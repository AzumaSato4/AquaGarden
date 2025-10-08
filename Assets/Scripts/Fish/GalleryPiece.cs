using UnityEngine;

public class GalleryPiece : MonoBehaviour
{
    public PieceData pieceData;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = pieceData.pieceSprite;
        GetComponent<Animator>().runtimeAnimatorController = pieceData.animationController;
        Debug.Log("生成");
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
