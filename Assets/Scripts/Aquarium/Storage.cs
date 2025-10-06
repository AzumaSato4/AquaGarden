using UnityEngine;

public class Storage : MonoBehaviour
{
    public GameObject[] pieceSpots;     //魚駒を置くスポット
    public bool isEmpty = true;     //ストレージに魚駒が残っているかどうか
    public GameObject mask;   //ストレージを暗くするためのオブジェクト

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

        return pieceSpots[0];
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
