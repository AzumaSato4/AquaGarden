using System.Collections.Generic;
using UnityEngine;

public class SeaBoard : MonoBehaviour
{
    public Dictionary<string, int> seaAquaPieces = new Dictionary<string, int>();

    [SerializeField] GameObject content;
    [SerializeField] GameObject seaItemPrefab;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    public void Initialize()
    {
        for (int i = 0; i < gameManager.pieceDataCount; i++)
        {
            PieceData data = gameManager.SarchPieceData(i);
            seaAquaPieces.Add(data.pieceName, 0);

            if (data.pieceName == "Seaweed")
            {
                seaAquaPieces["Seaweed"] = 16;
            }
            if (data.pieceName == "Coral")
            {
                seaAquaPieces["Coral"] = 16;
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
