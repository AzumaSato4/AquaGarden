using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static GameObject currentPlayer;

    PhaseManager phaseManager;
    public GameObject[] players;

    int loopCnt = 0;
    int roundCnt = 0;

    [SerializeField] GalleryBoard galleryBoard;
    [SerializeField] SeaBoard seaBoard;

    [SerializeField] GalleryPieceManager gPieceManager;
    [SerializeField] GameObject roundPiece;

    private void Start()
    {
        phaseManager = GetComponent<PhaseManager>();
        players = new GameObject[GameManager.players];
        players = GameObject.FindGameObjectsWithTag("Player");

        seaBoard.Invoke("Initialize", 0.1f);
        gPieceManager.Invoke("Initialize", 0.1f);

        Invoke("StartRound", 0.1f);
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
            if (players[i].GetComponent<PlayerManager>().isGoal)
            {
                continue;
            }

            if ((minIndex - players[i].GetComponent<PlayerManager>().galleryIndex) > 0)
            {
                minIndex = players[i].GetComponent<PlayerManager>().galleryIndex;
                currentPlayer = players[i];
            }
        }

        if (loopCnt == players.Length)
        {
            galleryBoard.startSpots[0].GetComponent<CircleCollider2D>().enabled = true;
        }
        else if (loopCnt > players.Length)
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
                galleryBoard.startSpots[1].GetComponent<CircleCollider2D>().enabled = true;
                Debug.Log("ゴール2解放");
            }
            else if (galleryBoard.isPlayer[1] && goalplayers == 2)
            {
                galleryBoard.startSpots[2].GetComponent<CircleCollider2D>().enabled = true;
                Debug.Log("ゴール3解放");
            }
            else if (galleryBoard.isPlayer[2] && goalplayers == 3)
            {
                galleryBoard.startSpots[3].GetComponent<CircleCollider2D>().enabled = true;
                Debug.Log("ゴール4解放");
            }
        }

        //プレイヤーを行動可能にし移動履歴を初期化
        currentPlayer.GetComponent<PlayerManager>().isActive = true;
        currentPlayer.GetComponent<PlayerManager>().StartGallery();
        phaseManager.StartGallery(currentPlayer.GetComponent<PlayerManager>().playerData);
    }

    public void EndTurn()
    {
        currentPlayer.GetComponent<PlayerManager>().isActive = false;
        phaseManager.EndTurn(currentPlayer.GetComponent<PlayerManager>().playerData);
        NextTurn();
    }

    void NextTurn()
    {
        StartTrun();
    }

    void EndRound()
    {
        Debug.Log("ラウンド終了！");
        StartRound();
    }

    void EndGame()
    {
        Debug.Log("ゲーム終了！");
    }
}
