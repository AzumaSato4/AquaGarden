using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    GameManager gameManager;
    BGMManager bgmManager;
    SEManager seManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        bgmManager = BGMManager.instance;
        seManager = SEManager.instance;
    }

    public void SelectPlayers(int menber)
    {
        seManager.PlaySE(SEManager.SE_Type.celebrate);
        GameManager.selectPlayers = menber;
        gameManager.SetPlayers();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(StartLoad(sceneName));
        bgmManager.StopBGM();
    }

    IEnumerator StartLoad(string sceneName)
    {
        yield return new WaitForSeconds(2.0f);
        
        switch (sceneName)
        {
            case "Title":
                bgmManager.PlayBGM(BGMManager.BGM_Type.title);
                break;
            case "Main":
                bgmManager.PlayBGM(BGMManager.BGM_Type.playing);
                break;
            case "Result":
                bgmManager.PlayBGM(BGMManager.BGM_Type.result);
                break;
        }

        SceneManager.LoadScene(sceneName);
    }
}
