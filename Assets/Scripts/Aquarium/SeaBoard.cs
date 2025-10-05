using System.Collections.Generic;
using UnityEngine;

public class SeaBoard : MonoBehaviour
{
    public Dictionary<string, int> seaAquaPieces;

    [SerializeField] GameObject content;
    [SerializeField] GameObject seaItemPrefab;


    private void Start()
    {
        seaAquaPieces = new Dictionary<string, int>();

        for (int i = 0; i < GameManager.aquaPieces.Length; i++)
        {
            if (GameManager.avalablePieces[i])
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

                GameObject seaItem = Instantiate(seaItemPrefab, content.transform);
                seaItem.GetComponent<SeaItem>().seaBoard = this;
                seaItem.GetComponent<SeaItem>().pieceData = GameManager.aquaPieces[i];

            }
        }
    }

    public void ReleasePiece(GameObject selectPiece)
    {
        seaAquaPieces[selectPiece.GetComponent<AquaPiece>().pieceData.pieceName]++;
    }
}
