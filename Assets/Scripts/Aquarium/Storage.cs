using UnityEngine;

public class Storage : MonoBehaviour
{
    public GameObject[] pieceSpots;     //�����u���X�|�b�g
    public bool isEmpty = true;     //�X�g���[�W�ɋ���c���Ă��邩�ǂ���
    public GameObject mask;   //�X�g���[�W���Â����邽�߂̃I�u�W�F�N�g

    private void Update()
    {
        if (isEmpty)
        {
            mask.SetActive(true);
        }
        else
        {
            mask.SetActive(false);
        }
    }

    public GameObject Instorage()
    {
        foreach (GameObject spot in pieceSpots)
        {
            if (spot.GetComponent<PieceSpot>().available)
            {
                isEmpty = false;
                return spot;
            }
        }

        return null;
    }

    public void CheckSpotEmpty()
    {
        foreach (GameObject spot in pieceSpots)
        {
            if (!spot.GetComponent<PieceSpot>().available)
            {
                isEmpty = false;
                return;
            }
        }

        isEmpty = true;
        return;
    }


}
