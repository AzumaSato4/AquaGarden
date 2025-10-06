using System.Collections.Generic;
using UnityEngine;

public class FishTile : MonoBehaviour
{
    public List<GameObject> pieces = new List<GameObject>();
    [SerializeField] GameObject galleryPiecePrefab;

    public void AddPiece(PieceData pieceData)
    {
        GameObject piece = Instantiate(galleryPiecePrefab, transform.position, Quaternion.identity);
        piece.GetComponent<GalleryPiece>().pieceData = pieceData;
        pieces.Add(piece);

        CheckTile();
    }

    void CheckTile()
    {
        if (pieces.Count == 2)
        {
            pieces[0].transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f);
            pieces[0].transform.localScale = new Vector2(1.2f, 1.2f);
            pieces[1].transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f);
            pieces[1].transform.localScale = new Vector2(1.2f, 1.2f);
        }
        else if (pieces.Count == 3)
        {
            pieces[0].transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f);
            pieces[0].transform.localScale = new Vector2(1.0f, 1.0f);
            pieces[1].transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y + 0.5f);
            pieces[1].transform.localScale = new Vector2(1.0f, 1.0f);
            pieces[2].transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
            pieces[2].transform.localScale = new Vector2(1.0f, 1.0f);
        }
        else if (pieces.Count == 4)
        {
            pieces[0].transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f);
            pieces[0].transform.localScale = new Vector2(0.8f, 0.8f);
            pieces[1].transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y + 0.5f);
            pieces[1].transform.localScale = new Vector2(0.8f, 0.8f);
            pieces[2].transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.5f);
            pieces[2].transform.localScale = new Vector2(0.8f, 0.8f);
            pieces[3].transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f);
            pieces[3].transform.localScale = new Vector2(0.8f, 0.8f);
        }
    }
}
