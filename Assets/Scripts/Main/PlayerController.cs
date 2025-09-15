using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PieceController piceCon;
    PlayerManager playerManager;
    public TurnManager turnManager;

    public GameObject pPiece;
    public GameObject[] pPieces;

    private void Start()
    {
        pPieces = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            pPieces[i] = (pPiece);
        }
    }

    public void PlayGallery()
    {
        piceCon.startPos = pPieces[turnManager.nowPlayer].transform.position;



    }

    public void EndTrun()
    {
        turnManager.isEnd = true;
    }
}

