using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    public void GameStart()
    {
        if (SelectPlayer.selectPlayer != 0 && GameController.gameMode == "TopMenu")
        {
            GameController.gameMode = "Start";
            SceneManager.LoadScene("Main");
        }
    }
}
