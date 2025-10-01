using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    GameObject currentPlayer;
    GameObject nextPlayer;

    GalleryTileManager gTileManager;
    PhaseManager phaseManager;
    [SerializeField] GameObject[] players;

    bool isSecondLoop = false;

    private void Start()
    {
        gTileManager = GameObject.Find("Gallery").GetComponent<GalleryTileManager>();
        phaseManager = GetComponent<PhaseManager>();
        players = new GameObject[4];
        players = GameObject.FindGameObjectsWithTag("Player");

        StartTrun();
    }

    void StartTrun()
    {
        if (!isSecondLoop)
        {
            for (int i = 0; i < players.Length; i++)
            {
                currentPlayer = players[i];
                PlayerController currentCnt = currentPlayer.GetComponent<PlayerController>();
                phaseManager.StartGallery(currentCnt.playerData.playerName);
            }
        }
        else
        {

        }

    }

    public void NextTurn()
    {

    }
}
