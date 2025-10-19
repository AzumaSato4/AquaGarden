using System.Collections.Generic;
using UnityEngine;

public class SeaBoard : MonoBehaviour
{
    public Dictionary<PieceData.PieceName, int> seaAquaPieces = new Dictionary<PieceData.PieceName, int>();

    [SerializeField] GameObject content;
    [SerializeField] GameObject seaItemPrefab;
    GameManager gameManager;

    public void Initialize()
    {
        gameManager = GameManager.instance;
        for (int i = 0; i < gameManager.pieceDataCount; i++)
        {
            PieceData data = gameManager.GetPieceData(i);
            PieceData.PieceName name = data.pieceName;
            if (seaAquaPieces.ContainsKey(name)) continue;
            seaAquaPieces.Add(name, 0);

            if (name == PieceData.PieceName.Seaweed)
            {
                seaAquaPieces[name] = 16;
            }
            if (name == PieceData.PieceName.Coral)
            {
                seaAquaPieces[name] = 16;
            }

            if (seaAquaPieces[name] > 0)
            {
                GameObject seaItem = Instantiate(seaItemPrefab, content.transform);
                seaItem.GetComponent<SeaItem>().seaBoard = this;
                seaItem.GetComponent<SeaItem>().pieceData = data;
            }
        }
    }


    public void ReleasePiece(GameObject selectPiece)
    {
        PieceData pieceData;
        if (selectPiece.GetComponent<GalleryPiece>())
        {
            pieceData = selectPiece.GetComponent<GalleryPiece>().pieceData;
        }
        else
        {
            pieceData = selectPiece.GetComponent<AquaPiece>().pieceData;
        }

        AddPiece(pieceData);
    }

    public void AddPiece(PieceData piece)
    {
        if (seaAquaPieces[piece.pieceName] <= 0)
        {
            GameObject seaItem = Instantiate(seaItemPrefab, content.transform);
            seaItem.GetComponent<SeaItem>().seaBoard = this;
            seaItem.GetComponent<SeaItem>().pieceData = piece;
        }
        seaAquaPieces[piece.pieceName]++;
    }
}
