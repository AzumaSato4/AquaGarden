using UnityEngine;

public class AquaSlot : MonoBehaviour
{
    public GameObject[] pieceSpots;     //魚駒を置くスポット
    public bool selectable = false;     //この水槽が選択可能かどうか
    public GameObject mask;   //水槽を暗くするためのオブジェクト

    private void Update()
    {
        if (selectable)
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
}
