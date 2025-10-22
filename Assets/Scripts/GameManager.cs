using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //プレイヤー基本情報
    public static int selectPlayers = 1; //プレイ人数
    public static int players = 1;
    public static string[] playerName;
    public static Sprite[] galleryColor;
    public static Sprite[] aquariumColor;
    public static Sprite[] milestoneColor;
    //データ元
    [SerializeField] DB_PieceData pieceData;
    [SerializeField] DB_AdCardData adCardData;
    [SerializeField] DB_FeedingData feedingData;
    [SerializeField] DB_MilestoneData milestoneData;

    //データの数
    public int pieceDataCount;
    public int adCardDataCount;
    public int feedingDataCount;
    public int milestoneDataCount;

    public static bool[] avalableAdPieces;

    [SerializeField] string[] pName;
    [SerializeField] Sprite[] gColor;
    [SerializeField] Sprite[] aColor;
    [SerializeField] Sprite[] mColor;

    public static bool isSecretMode; //マイルストーン非表示モード

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


        playerName = new string[players];
        galleryColor = new Sprite[players];
        aquariumColor = new Sprite[players];
        milestoneColor = new Sprite[players];
        for (int i = 0; i < players; i++)
        {
            playerName[i] = pName[i];
            galleryColor[i] = gColor[i];
            aquariumColor[i] = aColor[i];
            milestoneColor[i] = mColor[i];
        }


        pieceDataCount = pieceData.pieceDatas.Count;
        adCardDataCount = adCardData.adCardDatas.Count;
        feedingDataCount = feedingData.feedingDatas.Count;
        milestoneDataCount = milestoneData.milestoneDatas.Count;
    }

    public void SetPlayers()
    {
        if (selectPlayers == 2)
        {
            players = 3; //二人プレイ専用ルール
        }
        else
        {
            players = selectPlayers;
        }
    }

    public void ChangeMode(bool isMode)
    {
        isSecretMode = isMode;
        Debug.Log($"シークレットモード{isSecretMode}");
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

    public MilestoneData GetMilestoneData(int id)
    {
        MilestoneData data = milestoneData.milestoneDatas[id];
        return data;
    }
}
