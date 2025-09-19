using System.Collections.Generic;
using UnityEngine;

public class StoragePanel : MonoBehaviour
{
    public List<FishPiece> storageFishes = new List<FishPiece>();
    [SerializeField] GameObject fishPrefab;
    GameObject storageFish;

    //ストレージに魚駒を追加する
    public void AddStorage(FishPiece fish)
    {
        storageFishes.Add(fish);
        storageFish = Instantiate(fishPrefab, transform);
        storageFish.transform.localPosition = Vector3.zero;
    }


    //ストレージから魚駒を取り除く
    public void RemoveStorageFish(FishPiece fish)
    {
        storageFishes.Remove(fish);
    }
}
