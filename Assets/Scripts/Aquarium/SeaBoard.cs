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
            if (seaAquaPieces.ContainsKey(data.pieceName)) continue;
            seaAquaPieces.Add(data.pieceName, 0);

            if (data.pieceName == PieceData.PieceName.Seaweed)
            {
                seaAquaPieces[PieceData.PieceName.Seaweed] = 16;
            }
            if (data.pieceName == PieceData.PieceName.Coral)
            {
                seaAquaPieces[PieceData.PieceName.Coral] = 16;
            }

            if (seaAquaPieces[data.pieceName] > 0)
            {
                GameObject seaItem = Instantiate(seaItemPrefab, content.transform);
                seaItem.GetComponent<SeaItem>().seaBoard = this;
                seaItem.GetComponent<SeaItem>().pieceData = data;
            }
        }
    }


    public void ReleasePiece(GameObject selectPiece)
    {
        PieceData pieceData = selectPiece.GetComponent<AquaPiece>().pieceData;

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
