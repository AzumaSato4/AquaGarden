using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    public void SelectPlayers(int menber)
    {
        GameManager.selectPlayers = menber;
        gameManager.SetPlayers();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
