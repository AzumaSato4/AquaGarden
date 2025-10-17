using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AquaSlot : MonoBehaviour
{
    public GameObject[] pieceSpots;     //魚駒を置くスポット
    public bool[] isPiece; //スポットに駒が置かれているかどうか
    public bool selectable = false;     //この水槽が選択可能かどうか
    public GameObject mask;   //水槽を暗くするためのオブジェクト

    public List<GameObject> slotPieces; //水槽内の駒情報を管理
    public int slotOxygen = 4; //水槽内の酸素量
    //テスト用
    [SerializeField] TextMeshProUGUI oxygenText; //水槽内の酸素量を表示するテキスト

    private void Start()
    {
        isPiece = new bool[pieceSpots.Length];
    }

    private void Update()
    {
        oxygenText.text = slotOxygen.ToString();

        //魚駒が選択中、この水槽が選択可能ならレイキャストに反応するようにレイヤー変更
        if (AquaPieceManager.selectedPiece != null && selectable)
        {
            Invoke("ChengeLayer", 0.5f);
        }
        else
        {
            gameObject.layer = 2;
        }
    }

    void ChengeLayer()
    {
        gameObject.layer = 0;
    }

    public (GameObject,int) CheckSpot()
    {
        for (int i = 0; i < isPiece.Length; i++)
        {
            if (isPiece[i] == false)
            {
                return (pieceSpots[i], i);
            }
        }
        return (null, 0);
    }

    public void ReleasePiece()
    {
        slotPieces.Remove(AquaPieceManager.selectedPiece);
        AquaPiece piece = AquaPieceManager.selectedPiece.GetComponent<AquaPiece>();
        slotOxygen -= piece.pieceData.oxygen;
        isPiece[piece.spotIndex] = false;
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
