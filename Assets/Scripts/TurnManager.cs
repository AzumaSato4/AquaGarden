using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject currentPlayer;

    PhaseManager phaseManager;
    public GameObject[] players;

    [SerializeField] int loopCnt = 0;

    GalleryBoard galleryBoard;

    private void Start()
    {
        phaseManager = GetComponent<PhaseManager>();
        galleryBoard = GameObject.Find("Gallery").GetComponent<GalleryBoard>();
        players = new GameObject[GameManager.players];
        players = GameObject.FindGameObjectsWithTag("Player");

        Invoke("StartTrun", 0.1f);
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
        if (currentPlayer.GetComponent<PlayerManager>().EndAquarium())
        {
            currentPlayer.GetComponent<PlayerManager>().isActive = false;
            phaseManager.EndTurn(currentPlayer.GetComponent<PlayerManager>().playerData);
            NextTurn();
        }
    }

    void NextTurn()
    {
        StartTrun();
    }

    void EndRound()
    {
        Debug.Log("ラウンド終了！");
    }
}
