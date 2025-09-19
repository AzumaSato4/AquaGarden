using System.Collections.Generic;
using UnityEngine;

public class SeaBoard : MonoBehaviour
{
    public List<FishData> seaFishes;
    public List<int> seaCounts;

    //ゲーム開始時に初期化
    public void Initialze(List<FishData> fishTypes, List<int> counts)
    {
        //魚駒の種類
        //その種類の個数
        for (int i = 0; i < fishTypes.Count; i++)
        {
            seaFishes[i] = fishTypes[i];
            seaCounts[i] = counts[i];
        }
    }


    //海ボードに魚駒を追加
    public void AddSeaFish(FishPiece fish)
    {
        seaFishes.Add(fish.fishData);
    }


    //海ボードから魚駒を取り除く
    public void RemoveSeaFish(FishPiece fish)
    {
        seaFishes.Remove(fish.fishData);
    }
}
