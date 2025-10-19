using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    GameManager gameManager;
    SoundManager soundManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        soundManager = SoundManager.instance;
    }

    public void SelectPlayers(int menber)
    {
        soundManager.PlaySE(SoundManager.SE_Type.celebrate);
        GameManager.selectPlayers = menber;
        gameManager.SetPlayers();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(StartLoad(sceneName));
        soundManager.StopBGM();
    }

    IEnumerator StartLoad(string sceneName)
    {
        yield return new WaitForSeconds(2.0f);
        
        switch (sceneName)
        {
            case "Title":
                soundManager.PlayBGM(SoundManager.BGM_Type.title);
                break;
            case "Main":
                soundManager.PlayBGM(SoundManager.BGM_Type.playing);
                break;
            case "Result":
                soundManager.PlayBGM(SoundManager.BGM_Type.result);
                break;
        }

        SceneManager.LoadScene(sceneName);
    }
}
