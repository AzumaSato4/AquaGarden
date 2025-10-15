using System.Collections.Generic;
using UnityEngine;

public class SeaBoard : MonoBehaviour
{
    public Dictionary<PieceData, int> seaAquaPieces = new Dictionary<PieceData, int>();

    [SerializeField] GameObject content;
    [SerializeField] GameObject seaItemPrefab;
    GameManager gameManager;

    public void Initialize()
    {
        gameManager = GameManager.instance;
        for (int i = 0; i < gameManager.pieceDataCount; i++)
        {
            PieceData data = gameManager.GetPieceData(i);
            if (seaAquaPieces.ContainsKey(data)) continue;
            seaAquaPieces.Add(data, 0);

            if (data.pieceName == PieceData.PieceName.Seaweed)
            {
                seaAquaPieces[data] = 16;
            }
            if (data.pieceName == PieceData.PieceName.Coral)
            {
                seaAquaPieces[data] = 16;
            }

            if (seaAquaPieces[data] > 0)
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
        if (seaAquaPieces[piece] <= 0)
        {
            GameObject seaItem = Instantiate(seaItemPrefab, content.transform);
            seaItem.GetComponent<SeaItem>().seaBoard = this;
            seaItem.GetComponent<SeaItem>().pieceData = piece;
        }
        seaAquaPieces[piece]++;
    }
}
