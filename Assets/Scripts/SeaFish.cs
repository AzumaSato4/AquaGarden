using System.Collections.Generic;
using UnityEngine;

public class SeaFish : MonoBehaviour
{
    public GameManager manager;

    public SeaBoard sea;
    public List<FishData> seaFishes;
    public List<int> seaFishCounts;


    //�Q�[���J�n���ɏ�����
    public void Initialze(List<FishData> fishTypes, List<int> counts)
    {
        //����̎��
        //���̎�ނ̌�
        for (int i = 0; i < fishTypes.Count; i++)
        {
            seaFishes[i] = fishTypes[i];
            seaFishCounts[i] = counts[i];
        }
    }
}
