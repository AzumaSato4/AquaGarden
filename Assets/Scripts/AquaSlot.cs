using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static FishData;

public class AquaSlot : MonoBehaviour
{
    SpriteRenderer sr;
    Color defalutColor;
    public Color highliteColor = Color.yellow;
    public int maxOxygen = 6;  //水槽内の最大酸素量
    public int currentOxygen;  //現在の水槽内酸素量


    public TextMeshProUGUI oxygenText; // 水槽酸素量
    public Color normalColor = Color.black;
    public Color overLimitColor = Color.red;

    //水槽内の魚駒を管理するリスト
    public List<FishPiece> fishes = new List<FishPiece>();
    //仮置きリスト
    public List<FishPiece> tempFishes = new List<FishPiece>();

    //水槽内に置ける最大範囲
    public Vector2 scattrtRnage = new Vector2(0.3f, 0.3f);
    //魚同士の最小距離
    public float minDistance = 0.2f;

    //駒の移動にかかる時間
    public float moveTime;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        defalutColor = sr.color;
        UpdateOxygenUI();
    }


    //選択可能ならハイライトする
    public void SetHighlight(bool on)
    {
        sr.color = on ? highliteColor : defalutColor;
    }


    // 酸素量を計算
    public int GetTotalOxygen(FishPiece draggingFish = null)
    {
        int tempOxygen = currentOxygen + tempFishes.Sum(f => f.fishData.oxygen);
        if (draggingFish != null && !tempFishes.Contains(draggingFish))
        {
            tempOxygen += draggingFish.fishData.oxygen;
        }
        return tempOxygen;
    }


    //酸素量チェック
    public bool IsOxygenValid()
    {
        int oxygen = GetTotalOxygen();
        return oxygen >= 0 && oxygen <= maxOxygen;
    }

    //魚の相性チェック
    public bool CanAcceptFish(FishPiece fish, List<FishPiece> others = null)
    {
        switch (fish.fishData.type)
        {
            //海藻は水槽に1つだけ
            case Type.Seaweed:
                return !others.Any(f => f.fishData.type == Type.Seaweed);

            //サンゴは水槽に1つだけ
            case Type.Coral:
                return !others.Any(f => f.fishData.type == Type.Coral);

            //タツノオトシゴとサメとジンベエザメはどこでもOK
            case Type.Shark:
            case Type.Seahorse:
            case Type.WhaleShark:
                return true;

            //ウミガメは海藻が必要、サメと共存不可
            case Type.Seaturtle:
                return others.Any(f => f.fishData.type == Type.Seaweed) &&
                       !others.Any(f => f.fishData.type == Type.Shark);

            //小型魚と大型魚はサメと共存不可
            case Type.SmallFish:
            case Type.LargeFish:
                return !others.Any(f => f.fishData.type == Type.Shark);

            //それ以外（基本ない）
            default:
                return false;
        }
    }


    //水槽に魚駒を追加する
    public void AddFish(FishPiece fish)
    {
        if (!fishes.Contains(fish)) fishes.Add(fish);
        tempFishes.Remove(fish); // 仮置きリストから削除
        fish.transform.SetParent(transform);    //水槽の子オブジェクトにする

        //重ならないように配置
        Vector3 localPos = Vector3.zero;
        int attempt = 0;
        bool validPos = false;

        while (!validPos && attempt < 50)
        {
            //水槽内のランダムな位置に配置
            localPos = new Vector3(
                Random.Range(-scattrtRnage.x, scattrtRnage.x),
                Random.Range(-scattrtRnage.y, scattrtRnage.y),
                0
            );

            //他の駒と位置がかぶっていないか判定
            validPos = true;
            foreach (FishPiece fp in fishes)
            {
                //自分自身は除外
                if (fp == fish) continue;
                //他の駒と位置が被ったらfalseにする
                if (Vector3.Distance(localPos, fp.transform.localPosition) < minDistance)
                {
                    validPos = false;
                    break;
                }
            }
            attempt++;

        }
        fish.transform.localPosition = localPos;
        currentOxygen += fish.fishData.oxygen;

        UpdateOxygenUI();
    }


    //水槽から魚駒を取り除く
    public void RemoveFish(FishPiece fish)
    {
        tempFishes.Remove(fish);
        fishes.Remove(fish);

        currentOxygen -= fish.fishData.oxygen;

        UpdateOxygenUI();
    }


    // 酸素量表示更新
    public void UpdateOxygenUI(FishPiece draggingFish = null)
    {
        if (oxygenText != null)
        {
            int total = GetTotalOxygen(draggingFish);
            oxygenText.text = total + "/" + maxOxygen;
            oxygenText.color = total > maxOxygen ? overLimitColor : normalColor;
        }
    }
}
