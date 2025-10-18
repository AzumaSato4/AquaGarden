using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    GameManager gameManager;

    public static GameObject currentPlayer;

    [SerializeField] PhaseManager phaseManager;
    public GameObject[] players;

    public int loopCnt = 0;
    public static int roundCnt = 0;

    [SerializeField] GalleryBoard galleryBoard;
    [SerializeField] AdvanceBoard advanceBoard;
    public SeaBoard seaBoard;

    [SerializeField] GalleryPieceManager gPieceManager;
    [SerializeField] GameObject roundPiece;

    [SerializeField] MilestonePanel milestonePanel;
    public static MilestoneData[] milestones;
    List<MilestoneData.MilestoneType> mileTypes;
    //マイルストーン達成状況（0：未達成　1：達成済み）
    public int[,] achivements;

    public static List<int> scores = new List<int>();

    private void Awake()
    {
        if (SceneManager.sceneCount == 0)
        {
            foreach (GameObject obj in players)
            {
                Destroy(obj);
            }
        }
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        milestones = new MilestoneData[4];
        mileTypes = new List<MilestoneData.MilestoneType>();
        achivements = new int[milestones.Length, GameManager.players];
        //今回のゲームのマイルストーンを設定
        for (int i = 0; i < milestones.Length; i++)
        {
            //マイルストーンの種類がかぶらないようにする
            int rand;
            do
            {
                rand = Random.Range(0, gameManager.milestoneDataCount);
            } while (mileTypes.Contains(gameManager.GetMilestoneData(rand).type));
            //かぶっていなければ確定
            milestones[i] = gameManager.GetMilestoneData(rand);
            mileTypes.Add(milestones[i].type);

            //テスト用
            Debug.Log(milestones[i]);

            //マイルストーン達成状況をリセット
            for (int j = 0; j < GameManager.players; j++)
            {
                achivements[i, j] = 0;
            }
        }
        milestonePanel.Initialize();
        //動作を安定させるために遅らせる
        Invoke("Initialize", 0.1f);
    }

    void Initialize()
    {
        players = new GameObject[GameManager.players];
        players = GameObject.FindGameObjectsWithTag("Player");

        seaBoard.Initialize();
        advanceBoard.Initialize();
        gPieceManager.Initialize();

        StartRound();
    }

    void StartRound()
    {
        roundCnt++;

        if (players.Length == 4)
        {
            if (roundCnt > 4)
            {
                EndGame();
                return;
            }
        }
        else
        {
            if (roundCnt > 3)
            {
                EndGame();
                return;
            }
        }

        gPieceManager.SetPiece();

        loopCnt = 0;
        if (GameManager.selectPlayers == 2)
        {
            players[0].GetComponent<PlayerManager>().isGoal = false;
            players[1].GetComponent<PlayerManager>().isGoal = false;
            players[2].GetComponent<GossManager>().isGoal = false;
        }
        else
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerManager>().isGoal = false;
            }
        }
        galleryBoard.ResetTile();
        roundPiece.transform.position = galleryBoard.roundSpots[roundCnt - 1].transform.position;

        Debug.Log("ラウンドスタート！");
        StartTrun();

    }

    void StartTrun()
    {
        loopCnt++;

        int minIndex = 23;
        PlayerManager pManager;

        for (int i = 0; i < GameManager.selectPlayers; i++)
        {
            pManager = players[i].GetComponent<PlayerManager>();

            if (pManager.isGoal)
            {
                continue;
            }

            if ((minIndex - pManager.galleryIndex) > 0)
            {
                minIndex = pManager.galleryIndex;
                currentPlayer = players[i];
            }
        }

        //2人プレイ専用ルール（ゴスの順番を考慮）
        if (GameManager.selectPlayers == 2)
        {
            GossManager gossManager = players[2].GetComponent<GossManager>();
            if ((minIndex - gossManager.galleryIndex) > 0 && !gossManager.isGoal)
            {
                currentPlayer = players[2];
            }
        }

        int goalplayers = 0;
        if (loopCnt > players.Length)
        {
            for (int i = 0; i < GameManager.selectPlayers; i++)
            {
                if (players[i].GetComponent<PlayerManager>().isGoal)
                {
                    goalplayers++;
                }
            }
            if (GameManager.selectPlayers == 2)
            {
                if (players[2].GetComponent<GossManager>().isGoal)
                {
                    goalplayers++;
                }
            }

            //全プレイヤーがゴールしたらラウンド終了
            if (goalplayers == players.Length)
            {
                EndRound();
                return;
            }

            if (!galleryBoard.isPlayer[0] && goalplayers == 0)
            {
                galleryBoard.startSpots[0].GetComponent<BoxCollider2D>().enabled = true;
                Debug.Log("ゴール1解放");
            }
            else if (galleryBoard.isPlayer[0] && goalplayers == 1)
            {
                galleryBoard.startSpots[0].GetComponent<BoxCollider2D>().enabled = false;
                galleryBoard.startSpots[1].GetComponent<BoxCollider2D>().enabled = true;
                Debug.Log("ゴール2解放");
            }
            else if (galleryBoard.isPlayer[1] && goalplayers == 2)
            {
                galleryBoard.startSpots[1].GetComponent<BoxCollider2D>().enabled = false;
                galleryBoard.startSpots[2].GetComponent<BoxCollider2D>().enabled = true;
                Debug.Log("ゴール3解放");
            }
            else if (galleryBoard.isPlayer[2] && goalplayers == 3)
            {
                galleryBoard.startSpots[2].GetComponent<BoxCollider2D>().enabled = false;
                galleryBoard.startSpots[3].GetComponent<BoxCollider2D>().enabled = true;
                Debug.Log("ゴール4解放");
            }
        }

        //プレイヤーを行動可能にし移動履歴を初期化
        if (GameManager.selectPlayers == 2 && currentPlayer == players[2])
        {
            GossManager gossManager = currentPlayer.GetComponent<GossManager>();

            gossManager.isActive = true;
            gossManager.Invoke("StartGallery", 2.0f);
            phaseManager.StartGallery(gossManager.player);
        }
        else
        {
            PlayerManager currentPlayerManager = currentPlayer.GetComponent<PlayerManager>();

            currentPlayerManager.isActive = true;
            currentPlayerManager.StartGallery();
            phaseManager.StartGallery(currentPlayerManager.player);
        }
    }

    public void EndTurn()
    {
        if (currentPlayer.GetComponent<GossManager>())
        {
            currentPlayer.GetComponent<GossManager>().isActive = false;
        }
        else
        {
            currentPlayer.GetComponent<PlayerManager>().isActive = false;
        }
        phaseManager.EndTurn();
        NextTurn();
    }

    void NextTurn()
    {
        Invoke(nameof(StartTrun), 0.1f);
    }

    void EndRound()
    {
        Debug.Log("ラウンド終了！");
        StartRound();
    }

    void EndGame()
    {
        Debug.Log("ゲーム終了！");
        GetResult();
    }

    void GetResult()
    {
        for (int i = 0; i < GameManager.selectPlayers; i++)
        {
            scores.Add(players[i].GetComponent<PlayerManager>().GetScore());
        }

        foreach (int i in scores)
        {
            Debug.Log(i);
        }

        SceneManager.LoadScene("Result");
    }
}
