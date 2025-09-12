using UnityEngine;

public class ChangePanelManager : MonoBehaviour
{
    public GameObject currentPanel; //現在のパネル
    public GameObject nextPanel;    //次のパネル
    public GameObject prePanel;     //前のパネル
    public GameObject stopPanel;    //確認パネル

    public TitleUIManager titleUIManager;
    public PlayerManager playerManager;

    //次のパネルに切り替える
    public void MoveNextPanel()
    {
        currentPanel.SetActive(false);
        nextPanel.SetActive(true);
        titleUIManager.mainPanel = nextPanel.name;
    }

    //前のパネルに切り替える
    public void MovePrePanel()
    {
        currentPanel.SetActive(false);
        prePanel.SetActive(true);
        titleUIManager.mainPanel = prePanel.name;
    }
}
