using UnityEngine;

public class AquaSlot : MonoBehaviour
{
    public GameObject[] pieceSpots;     //�����u���X�|�b�g
    public bool selectable = false;     //���̐������I���\���ǂ���
    public GameObject mask;   //�������Â����邽�߂̃I�u�W�F�N�g

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
