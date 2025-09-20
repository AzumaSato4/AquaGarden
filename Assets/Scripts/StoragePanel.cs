using System.Collections.Generic;
using UnityEngine;

public class StoragePanel : MonoBehaviour
{
    public List<FishPiece> storageFishes = new List<FishPiece>();

    //�X�g���[�W�ɋ����ǉ�����
    public void AddStorage(FishPiece fish)
    {
        storageFishes.Add(fish);
        fish.transform.SetParent(transform);
        fish.transform.localPosition = Vector3.zero;
    }


    //�X�g���[�W���狛�����菜��
    public void RemoveStorageFish(FishPiece fish)
    {
        storageFishes.Remove(fish);
    }


    //�X�g���[�W�ɋ���c���Ă�����x��
    public bool HasFishInStorage()
    {
        return storageFishes.Count > 0;
    }
}
