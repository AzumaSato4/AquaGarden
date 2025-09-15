using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public PlayerController playerCon;

    public int nowPlayer;
    static public string turn;
    public bool isEnd;

    void Start()
    {
        nowPlayer = 0;
        isEnd = false;

        turn = "Gallery";
    }

    void Update()
    {
        if (isEnd)
        {
            nowPlayer++;
            PlayTurn();
            isEnd = false;
        }
    }

    public void PlayTurn()
    {
        Debug.Log(turn + nowPlayer);

        playerCon.PlayGallery();
    }
}
