using UnityEngine;

public class SeaPanel : MonoBehaviour
{
    public GameObject seaboard;        //�C�{�[�h


    //�}�E�X�ŃN���b�N���ꂽ��\���A��\���ɂ���
    public void OnMouseDown()
    {
        // UI�̏�ɃJ�[�\������������A���͂��󂯕t���Ȃ�
        if (GameManager.UIActive) return;
        seaboard.SetActive(true);
        GameManager.UIActive = true;
    }
}
