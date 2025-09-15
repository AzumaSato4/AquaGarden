using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    [SerializeField] Button[] tiles;
    int preNum; //前回のターンで進んだマス

    void Start()
    {
        preNum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].interactable = true;
        }
        preNum = 0;
    }

    //進んだマスの後ろを押せなくする
    public void UseTile(int tileNum)
    {
        //前回のターンで押せなくなったマスは除外して処理
        for (int i = 0 + preNum; i < tileNum; i++)
        {
            tiles[i].interactable = false;
        }
        preNum = tileNum;
    }

    //マスの使用をリセット
    public void ResetUseTile()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].interactable = true;
        }
        preNum = 0;
    }
}
