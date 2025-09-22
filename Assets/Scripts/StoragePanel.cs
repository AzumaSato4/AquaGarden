using System.Collections.Generic;
using UnityEngine;

public class StoragePanel : MonoBehaviour
{
    public List<FishPiece> storageFishes = new List<FishPiece>();

    //ストレージ内に置ける最大範囲
    public Vector2 sScattrtRnage = new Vector2(0.5f, 0.5f);
    public int length;
    //魚同士の最小距離
    public float minDistance = 0.2f;

    //ストレージに魚駒を追加する
    public void AddStorage(FishPiece fish)
    {
        storageFishes.Add(fish);
        fish.transform.SetParent(transform);

        fish.transform.localPosition = Vector3.zero;
        //重ならないように配置
        Vector3 localPos = Vector3.zero;
        int attempt = 0;
        bool validPos = false;

        while (!validPos && attempt < 50)
        {
            //水槽内のランダムな位置に配置
            localPos = new Vector3(
                Random.Range(-sScattrtRnage.x, sScattrtRnage.x),
                Random.Range(-sScattrtRnage.y, sScattrtRnage.y),
                0
            );

            //他の駒と位置がかぶっていないか判定
            validPos = true;
            foreach (FishPiece fp in storageFishes)
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

    //範囲表示
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, length);
    }
}
