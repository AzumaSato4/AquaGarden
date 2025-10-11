using System.Collections.Generic;
using UnityEngine;

public class AdvanceBoard : MonoBehaviour
{
    public Dictionary<PieceData.PieceName, int> advanceAquaPieces = new Dictionary<PieceData.PieceName, int>();

    [SerializeField] GameObject content;
    [SerializeField] GameObject advanceItemPrefab;
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public void Initialize()
    {
        for (int i = 0; i < gameManager.pieceDataCount; i++)
        {
            PieceData data = gameManager.GetPieceData(i);
            if (advanceAquaPieces.ContainsKey(data.pieceName) || data.pieceType != PieceData.PieceType.advance) continue;
            advanceAquaPieces.Add(data.pieceName, 0);

            if (data.pieceName == PieceData.PieceName.Flapjack)
            {
                advanceAquaPieces[PieceData.PieceName.Flapjack] = 4;
            }
            if (data.pieceName == PieceData.PieceName.Manta)
            {
                advanceAquaPieces[PieceData.PieceName.Manta] = 4;
            }
            if (data.pieceName == PieceData.PieceName.Remora)
            {
                advanceAquaPieces[PieceData.PieceName.Remora] = 4;
            }

            if (advanceAquaPieces[data.pieceName] > 0)
            {
                GameObject advanceItem = Instantiate(advanceItemPrefab, content.transform);
                advanceItem.GetComponent<AdvanceItem>().advanceBoard = this;
                advanceItem.GetComponent<AdvanceItem>().pieceData = data;
            }
        }
    }
}
