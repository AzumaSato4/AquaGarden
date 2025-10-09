using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int players = 4;
    public static string[] playerName;
    public static Sprite[] galleryColor;
    public static Sprite[] aquariumColor;
    [SerializeField] DB_PieceData pieceData;
    [SerializeField] DB_AdCardData adCardData;
    [SerializeField] DB_FeedingData feedingData;

    public int pieceDataCount;
    public int adCardDataCount;
    public int feedingDataCount;

    public static bool[] avalableAdPieces;

    //テスト用
    [SerializeField] string[] pName;
    [SerializeField] Sprite[] gColor;
    [SerializeField] Sprite[] aColor;

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

        instance = this;

        Application.targetFrameRate = 60;
#if UNITY_WEBGL
        Application.targetFrameRate = 59;
#endif

        //テスト用
        playerName = new string[players];
        galleryColor = new Sprite[players];
        aquariumColor = new Sprite[players];
        for (int i = 0; i < players; i++)
        {
            playerName[i] = pName[i];
            galleryColor[i] = gColor[i];
            aquariumColor[i] = aColor[i];
        }


        pieceDataCount = pieceData.pieceDatas.Count;
        adCardDataCount = adCardData.adCardDatas.Count;
        feedingDataCount = feedingData.feedingDatas.Count;
    }

    public PieceData GetPieceData(int id)
    {
        PieceData data = pieceData.pieceDatas[id];
        return data;
    }

    public AdCardData GetAdCardData(int id)
    {
        AdCardData data = adCardData.adCardDatas[id];
        return data;
    }
    public FeedingData GetFeedingData(int id)
    {
        FeedingData data = feedingData.feedingDatas[id];
        return data;
    }
}
