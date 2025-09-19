using System.Collections.Generic;
using UnityEngine;

public class StoragePanel : MonoBehaviour
{
    public List<FishPiece> storageFishes = new List<FishPiece>();
    [SerializeField] GameObject fishPrefab;
    GameObject storageFish;

    //�X�g���[�W�ɋ����ǉ�����
    public void AddStorage(FishPiece fish)
    {
        storageFishes.Add(fish);
        storageFish = Instantiate(fishPrefab, transform);
        storageFish.transform.localPosition = Vector3.zero;
    }


    //�X�g���[�W���狛�����菜��
    public void RemoveStorageFish(FishPiece fish)
    {
        storageFishes.Remove(fish);
    }
}
