using UnityEngine;

public class Storage : MonoBehaviour
{
    public GameObject[] pieceSpots;     //魚駒を置くスポット
    bool[] isPiece; //スポットに駒が置かれているかどうか

    private void Start()
    {
        isPiece = new bool[pieceSpots.Length];
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

    public bool CheckSpotEmpty()
    {
        for (int i = 0; i < isPiece.Length; i++)
        {
            if (isPiece[i] == true)
            {
                return false;
            }
        }
        return true;
    }
}
