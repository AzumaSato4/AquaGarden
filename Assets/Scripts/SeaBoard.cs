using System.Collections.Generic;
using UnityEngine;

public class SeaBoard : MonoBehaviour
{
    public List<FishData> seaFishes;
    public List<int> seaCounts;

    //�Q�[���J�n���ɏ�����
    public void Initialze(List<FishData> fishTypes, List<int> counts)
    {
        //����̎��
        //���̎�ނ̌�
        for (int i = 0; i < fishTypes.Count; i++)
        {
            seaFishes[i] = fishTypes[i];
            seaCounts[i] = counts[i];
        }
    }


    //�C�{�[�h�ɋ����ǉ�
    public void AddSeaFish(FishPiece fish)
    {
        seaFishes.Add(fish.fishData);
    }


    //�C�{�[�h���狛�����菜��
    public void RemoveSeaFish(FishPiece fish)
    {
        seaFishes.Remove(fish.fishData);
    }
}
