using System.Collections.Generic;
using UnityEngine;

public class GalleryPieceManager : MonoBehaviour
{
    [SerializeField] List<PieceData> pieceDatas = new List<PieceData>();
    int rand;

    [SerializeField] GalleryBoard galleryBoard;
    [SerializeField] SeaBoard seaBoard;

    [SerializeField] GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    public void Initialize()
    {
        if (GameManager.players == 1 || GameManager.players == 4)
        {
            for (int i = 0; i < 23; i++)
            {
                pieceDatas.Add(gameManager.SarchPieceData(0));
            }
            for (int i = 0; i < 16; i++)
            {
                pieceDatas.Add(gameManager.SarchPieceData(1));
            }
            for (int i = 0; i < 9; i++)
            {
                pieceDatas.Add(gameManager.SarchPieceData(2));
            }
            for (int i = 0; i < 5; i++)
            {
                pieceDatas.Add(gameManager.SarchPieceData(3));
            }
            for (int i = 0; i < 9; i++)
            {
                pieceDatas.Add(gameManager.SarchPieceData(4));
            }
            for (int i = 0; i < 3; i++)
            {
                pieceDatas.Add(gameManager.SarchPieceData(5));
            }
        }
        else if (GameManager.players == 2 || GameManager.players == 3)
        {
            for (int i = 0; i < 18; i++)
            {
                pieceDatas.Add(gameManager.SarchPieceData(0));
            }
            for (int i = 0; i < 12; i++)
            {
                pieceDatas.Add(gameManager.SarchPieceData(1));
            }
            for (int i = 0; i < 7; i++)
            {
                pieceDatas.Add(gameManager.SarchPieceData(2));
            }
            for (int i = 0; i < 4; i++)
            {
                pieceDatas.Add(gameManager.SarchPieceData(3));
            }
            for (int i = 0; i < 7; i++)
            {
                pieceDatas.Add(gameManager.SarchPieceData(4));
            }
            for (int i = 0; i < 2; i++)
            {
                pieceDatas.Add(gameManager.SarchPieceData(5));
            }
        }

        for (int i = 0;i < 5;i++)
        {
            rand = Random.Range(0, pieceDatas.Count);
            seaBoard.AddPiece(pieceDatas[rand]);
            pieceDatas.RemoveAt(rand);
        }
    }

    public void SetPiece()
    {
        for (int i = 0; i < 19; i++)
        {
            if (galleryBoard.galleryTiles[i].name == "FishTile")
            {
                rand = Random.Range(0, pieceDatas.Count);
                galleryBoard.galleryTiles[i].GetComponent<FishTile>().AddPiece(pieceDatas[rand]);
                pieceDatas.RemoveAt(rand);
            }
        }
    }
}
