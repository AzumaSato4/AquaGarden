using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AquaSlot : MonoBehaviour
{
    public GameObject[] pieceSpots;     //魚駒を置くスポット
    public bool selectable = false;     //この水槽が選択可能かどうか
    public GameObject mask;   //水槽を暗くするためのオブジェクト

    public List<GameObject> slotPieces; //水槽内の駒情報を管理
    public int slotOxygen = 4; //水槽内の酸素量
    //テスト用
    [SerializeField] TextMeshProUGUI oxygenText; //水槽内の酸素量を表示するテキスト

    private void Update()
    {
        oxygenText.text = slotOxygen.ToString();

        if (selectable && AquaPieceManager.selectedPiece != null)
        {
            GetComponent<PolygonCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<PolygonCollider2D>().enabled = false;
        }
    }

    public GameObject CheckSpot()
    {
        foreach (GameObject spot in pieceSpots)
        {
            if (spot.GetComponent<PieceSpot>().available)
            {
                return spot;
            }
        }

        return null;
    }

    public List<PieceData.PieceName> GetSlotPiece()
    {
        List<PieceData.PieceName> pieces = new List<PieceData.PieceName>();
        foreach (GameObject piece in slotPieces)
        {
            pieces.Add(piece.GetComponent<AquaPiece>().pieceData.pieceName);
        }

        return pieces;
    }
}
