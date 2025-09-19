using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaSlot : MonoBehaviour
{
    SpriteRenderer sr;
    Color defalutColor;
    public Color highliteColor = Color.yellow;

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
    }


    //選択可能ならハイライトする
    public void SetHighlight(bool on)
    {
        sr.color = on ? highliteColor : defalutColor;
    }


    // 仮置き
    public void AddTempFish(FishPiece fish)
    {
        tempFishes.Add(fish);
        fish.transform.SetParent(transform);
        Vector3 localPos = AddFish(fish);
        fish.transform.localPosition = localPos;
    }


    //水槽に魚駒を追加する
    Vector3 AddFish(FishPiece fish)
    {
        fishes.Add(fish);
        fish.transform.SetParent(transform);    //水槽の子オブジェクトにする

        //重ならないように配置
        Vector3 localPos = Vector3.zero;
        int attempt = 0;
        bool validPos = false;

        while (!validPos && attempt < 50)
        {
            //水槽内のランダムな位置に仮置き
            localPos = new Vector3(
                Random.Range(-scattrtRnage.x, scattrtRnage.x),
                Random.Range(-scattrtRnage.y, scattrtRnage.y),
                0
            );

            //他の駒と位置がかぶっていないか判定
            validPos = true;
            foreach ( FishPiece fp in fishes )
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
        return localPos;
    }


    //水槽から魚駒を取り除く
    public void RemoveFish(FishPiece fish)
    {
        fishes.Remove(fish);
    }

    
    //決定ボタンで確定する
    public void ConfirmFishPlacement()
    {
        foreach (FishPiece fish in tempFishes)
        {
            fishes.Add(fish);
        }
        tempFishes.Clear();
    }


    //移動を滑らかにするコルーチン
    IEnumerator MoveCoroutine(Vector2 toPos, FishPiece piece)
    {
        Vector2 startPos = piece.transform.position;
        float elapsed = 0;

        while (elapsed < moveTime)
        {
            piece.transform.position = Vector2.Lerp(startPos, toPos, elapsed / moveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        piece.transform.position = toPos;
    }
}
