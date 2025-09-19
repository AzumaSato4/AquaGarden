using UnityEngine;

public class SeaPanelBackButton : MonoBehaviour
{
    public GameObject board;      //海ボード
    public SeaPanel seaPanel;   //パネルを閉じるために設定

    public void OnBackButoon()
    {
        board.SetActive(false);
        GameManager.UIActive = false;
    }
}
