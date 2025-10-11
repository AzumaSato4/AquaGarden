using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    public static GameObject currentPlayer;

    [SerializeField] PhaseManager phaseManager;
    public GameObject[] players;

    int loopCnt = 0;
    int roundCnt = 0;

    [SerializeField] GalleryBoard galleryBoard;
    [SerializeField] SeaBoard seaBoard;
    [SerializeField] AdvanceBoard advanceBoard;

    [SerializeField] GalleryPieceManager gPieceManager;
    [SerializeField] GameObject roundPiece;

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
        StartCoroutine(Initialize()); //動作を安定させるために1フレーム待つコルーチン
    }

    IEnumerator Initialize()
    {
        yield return null;
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
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<PlayerManager>().isGoal = false;
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
        for (int i = 0; i < players.Length; i++)
        {
            PlayerManager pManager = players[i].GetComponent<PlayerManager>();
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

        if (loopCnt == players.Length + 1)
        {
            galleryBoard.startSpots[0].GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (loopCnt > players.Length + 1)
        {
            int goalplayers = 0;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<PlayerManager>().isGoal)
                {
                    goalplayers++;
                }
                if (goalplayers == players.Length)
                {
                    EndRound();
                    return;
                }
            }

            if (galleryBoard.isPlayer[0] && goalplayers == 1)
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
        PlayerManager currentPlayerManager = currentPlayer.GetComponent<PlayerManager>();

        currentPlayerManager.isActive = true;
        currentPlayerManager.StartGallery();
        phaseManager.StartGallery(currentPlayerManager.player);
    }

    public void EndTurn()
    {
        currentPlayer.GetComponent<PlayerManager>().isActive = false;
        phaseManager.EndTurn(currentPlayer.GetComponent<PlayerManager>().player);
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
        for (int i = 0; i < players.Length; i++)
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
