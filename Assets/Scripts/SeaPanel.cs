using UnityEngine;

public class SeaPanel : MonoBehaviour
{
    public GameObject seaboard;        //海ボード


    //マウスでクリックされたら表示する
    public void OnMouseDown()
    {
        // UIの上にカーソルがあったら、入力を受け付けない
        if (GameManager.UIActive) return;
        seaboard.SetActive(true);
        GameManager.UIActive = true;
    }
}
