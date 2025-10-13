using UnityEngine;

public class Storage : MonoBehaviour
{
    public GameObject[] pieceSpots;     //魚駒を置くスポット
    bool[] isPiece; //スポットに駒が置かれているかどうか
    public bool isEmpty = true;     //ストレージに魚駒が残っているかどうか
    public GameObject mask;   //ストレージを暗くするためのオブジェクト

    private void Start()
    {
        isPiece = new bool[pieceSpots.Length];
    }

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

    public (GameObject,int) Instorage()
    {
        for (int i = 0; i < isPiece.Length; i++)
        {
            if (isPiece[i] == false)
            {
                isPiece[i] = true;
                return (pieceSpots[i], i);
            }
        }
        //どこも空いてなかったら一番左
        return (pieceSpots[0], 0);
    }

    public void ReleasePiece()
    {
        AquaPiece piece = AquaPieceManager.selectedPiece.GetComponent<AquaPiece>();
        isPiece[piece.storageIndex] = false;
    }

    public void CheckSpotEmpty()
    {
        for (int i = 0; i < isPiece.Length; i++)
        {
            if (isPiece[i] == true)
            {
                isEmpty = false;
                return;
            }
        }
        isEmpty = true;
        return;
    }
}
