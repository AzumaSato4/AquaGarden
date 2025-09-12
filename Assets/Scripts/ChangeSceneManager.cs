using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class ChangeSceneManager : MonoBehaviour
{
    public string SceneName;    //Ø‚è‘Ö‚¦‚½‚¢ƒV[ƒ“–¼‚ğİ’è

    public GameManager gameManager;
    public PlayerManager playerManager;
    public ChangePanelManager cPManager;
    public TitleUIManager titleUIManager;

    public void CheckPlayer()
    {
        playerManager.Check();
        if (playerManager.isName && playerManager.isColor) ChangeScene();
        else
        {
            cPManager.stopPanel.SetActive(true);
            titleUIManager.mainPanel = cPManager.stopPanel.name;
        }
    }

    public void ChangeScene()
    {
        if (GameManager.gameState == "Title")
        {
            GameManager.gameState = "Play";
        }

        SceneManager.LoadScene(SceneName);
    }
}
