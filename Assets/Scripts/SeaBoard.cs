using UnityEngine;

public class SeaBoard : MonoBehaviour
{
    public FishData[] seaFishes;
    public int[] seaCounts;

    public SelectSeaFish[] selectSeaFish;

    //ゲーム開始時に初期化
    public void Initialze(FishData[] fishTypes, int[] counts)
    {
        //魚駒の種類
        //その種類の個数
        for (int i = 0; i < fishTypes.Length; i++)
        {
            seaFishes[i] = fishTypes[i];
            seaCounts[i] = counts[i];
        }
    }


    //海ボードに魚駒を追加
    public void AddSeaFish(FishData fish)
    {
        seaCounts[fish.id]++;
        selectSeaFish[fish.id].InFish();
    }


    // 指定したインデックスの魚を取り出す
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
            Debug.Log("その魚はいません");
            return null;
        }
    }
}
