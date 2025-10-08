using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int players = 4;
    [SerializeField] DB_PieceData pieceData;
    [SerializeField] DB_PlayerData playerData;
    [SerializeField] DB_AdCardData adCardData;
    [SerializeField] DB_FeedingData feedingData;

    public int pieceDataCount;
    public int playerDataLength;
    public int adCardDataCount;
    public int feedingDataCount;

    public static bool[] avalableAdPieces;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // シーンが切り替わっても破棄されないようにする
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = 60;
#if UNITY_WEBGL
        Application.targetFrameRate = 59;
#endif


        pieceDataCount = pieceData.pieceDatas.Count;
        playerDataLength = playerData.playerDatas.Length;
        adCardDataCount = adCardData.adCardDatas.Count;
        feedingDataCount = feedingData.feedingDatas.Count;
    }

    public PieceData SarchPieceData(int id)
    {
        PieceData data = pieceData.pieceDatas[id];
        return data;
    }
    public PlayerData SarchPlayerData(int id)
    {
        PlayerData data = playerData.playerDatas[id];
        return data;
    }
    public AdCardData SarchAdCardData(int id)
    {
        AdCardData data = adCardData.adCardDatas[id];
        return data;
    }
    public FeedingData SarchFeedingData(int id)
    {
        FeedingData data = feedingData.feedingDatas[id];
        return data;
    }
}
