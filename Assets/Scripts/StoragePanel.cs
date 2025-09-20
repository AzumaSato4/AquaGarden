using System.Collections.Generic;
using UnityEngine;

public class StoragePanel : MonoBehaviour
{
    public List<FishPiece> storageFishes = new List<FishPiece>();

    //ストレージに魚駒を追加する
    public void AddStorage(FishPiece fish)
    {
        storageFishes.Add(fish);
        fish.transform.SetParent(transform);
        fish.transform.localPosition = Vector3.zero;
    }


    //ストレージから魚駒を取り除く
    public void RemoveStorageFish(FishPiece fish)
    {
        storageFishes.Remove(fish);
    }


    //ストレージに魚駒が残っていたら警告
    public bool HasFishInStorage()
    {
        return storageFishes.Count > 0;
    }
}
