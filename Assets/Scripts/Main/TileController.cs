using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    [SerializeField] Button[] tiles;
    int preNum; //�O��̃^�[���Ői�񂾃}�X

    void Start()
    {
        preNum = 0;
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].interactable = true;
        }
        preNum = 0;
    }

    //�i�񂾃}�X�̌��������Ȃ�����
    public void UseTile(int tileNum)
    {
        //�O��̃^�[���ŉ����Ȃ��Ȃ����}�X�͏��O���ď���
        for (int i = 0 + preNum; i < tileNum; i++)
        {
            tiles[i].interactable = false;
        }
        preNum = tileNum;
    }

    //�}�X�̎g�p�����Z�b�g
    public void ResetUseTile()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].interactable = true;
        }
        preNum = 0;
    }
}
