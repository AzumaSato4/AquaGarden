using UnityEngine;

public class SeaPanelBackButton : MonoBehaviour
{
    public GameObject board;      //�C�{�[�h
    public SeaPanel seaPanel;   //�p�l������邽�߂ɐݒ�

    public void OnBackButoon()
    {
        board.SetActive(false);
        GameManager.UIActive = false;
    }
}
