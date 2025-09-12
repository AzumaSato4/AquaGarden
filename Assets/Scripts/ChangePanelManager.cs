using UnityEngine;

public class ChangePanelManager : MonoBehaviour
{
    public GameObject currentPanel; //���݂̃p�l��
    public GameObject nextPanel;    //���̃p�l��
    public GameObject prePanel;     //�O�̃p�l��
    public GameObject stopPanel;    //�m�F�p�l��

    public TitleUIManager titleUIManager;
    public PlayerManager playerManager;

    //���̃p�l���ɐ؂�ւ���
    public void MoveNextPanel()
    {
        currentPanel.SetActive(false);
        nextPanel.SetActive(true);
        titleUIManager.mainPanel = nextPanel.name;
    }

    //�O�̃p�l���ɐ؂�ւ���
    public void MovePrePanel()
    {
        currentPanel.SetActive(false);
        prePanel.SetActive(true);
        titleUIManager.mainPanel = prePanel.name;
    }
}
