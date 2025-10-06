using System.Collections.Generic;
using UnityEngine;

public class SeaBoard : MonoBehaviour
{
    public Dictionary<string, int> seaAquaPieces = new Dictionary<string, int>();

    [SerializeField] GameObject content;
    [SerializeField] GameObject seaItemPrefab;


    public void Initialize()
    {
        for (int i = 0; i < GameManager.aquaPieces.Length; i++)
        {
            seaAquaPieces.Add(GameManager.aquaPieces[i].pieceName, 0);

            if (GameManager.aquaPieces[i].pieceName == "Seaweed")
            {
                seaAquaPieces["Seaweed"] = 16;
            }
            if (GameManager.aquaPieces[i].pieceName == "Coral")
            {
                seaAquaPieces["Coral"] = 16;
            }

            if (seaAquaPieces[GameManager.aquaPieces[i].pieceName] > 0)
            {
                GameObject seaItem = Instantiate(seaItemPrefab, content.transform);
                seaItem.GetComponent<SeaItem>().seaBoard = this;
                seaItem.GetComponent<SeaItem>().pieceData = GameManager.aquaPieces[i];
            }
        }

        for (int i = 0; i < GameManager.adAquaPieces.Length; i++)
        {
            if (GameManager.avalableAdPieces[i])
            {
                seaAquaPieces.Add(GameManager.adAquaPieces[i].pieceName, 0);
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
        if (seaAquaPieces[piece.pieceName] < 1)
        {
            GameObject seaItem = Instantiate(seaItemPrefab, content.transform);
            seaItem.GetComponent<SeaItem>().seaBoard = this;
            seaItem.GetComponent<SeaItem>().pieceData = piece;
        }
        seaAquaPieces[piece.pieceName]++;
    }
}
