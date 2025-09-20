using UnityEngine;

public class SeaBoard : MonoBehaviour
{
    public FishData[] seaFishes;
    public int[] seaCounts;

    public SelectSeaFish[] selectSeaFish;

    //�Q�[���J�n���ɏ�����
    public void Initialze(FishData[] fishTypes, int[] counts)
    {
        //����̎��
        //���̎�ނ̌�
        for (int i = 0; i < fishTypes.Length; i++)
        {
            seaFishes[i] = fishTypes[i];
            seaCounts[i] = counts[i];
        }
    }


    //�C�{�[�h�ɋ����ǉ�
    public void AddSeaFish(FishData fish)
    {
        seaCounts[fish.id]++;
        selectSeaFish[fish.id].InFish();
    }


    // �w�肵���C���f�b�N�X�̋������o��
    public FishData TakeFish(int index)
    {
        if (seaCounts[index] > 0)
        {
            gameObject.SetActive(false);
            GameManager.UIActive = false;

            seaCounts[index]--;
            return seaFishes[index];
        }
        else
        {
            Debug.Log("���̋��͂��܂���");
            return null;
        }
    }
}
