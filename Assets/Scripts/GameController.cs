using UnityEngine;

public class GameController : MonoBehaviour
{
    public static string gameMode;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameMode = "TopMenu";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        if (SelectPlayer.selectPlayer != 0 && gameMode == "TopMenu")
        {
            gameMode = "Start";
        }
    }
}
