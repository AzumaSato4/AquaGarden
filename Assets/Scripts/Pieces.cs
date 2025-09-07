using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    //タイプミスを減らすために駒を列挙
    public enum PieceType
    {
        minifish,
        bigfish,
        seaturtle,
        seahorse,
        shark,
        whaleshark,
        seaweed,
        coral
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //4人用
        int minifishes = 23;
        int bigfishes = 16;
        int seaturtles = 9;
        int seahorses = 5;
        int sharks = 9;
        int whalesharks = 3;
        int seaweeds = 17;
        int corals = 16;


        //布袋
        Dictionary<PieceType, int> pouch = new Dictionary<PieceType, int>()
        {
            {PieceType.minifish, minifishes},
            {PieceType.bigfish, bigfishes},
            {PieceType.seaturtle, seaturtles},
            {PieceType.seahorse, seahorses},
            {PieceType.shark, sharks},
            {PieceType.whaleshark, whalesharks},
            {PieceType.seaweed, seaweeds},
            {PieceType.coral, corals}
        };

        //海ボード
        Dictionary<PieceType, int> sea = new Dictionary<PieceType, int>()
        {
            {PieceType.minifish, 0},
            {PieceType.bigfish, 0},
            {PieceType.seaturtle, 0},
            {PieceType.seahorse, 0},
            {PieceType.shark, 0},
            {PieceType.whaleshark, 0},
            {PieceType.seaweed, 0},
            {PieceType.coral, 0}
        };

        //魚駒だけ集める
        PieceType[] fishType = new PieceType[]
        {
         PieceType.minifish,
         PieceType.bigfish,
         PieceType.seaturtle,
         PieceType.seahorse,
         PieceType.shark,
         PieceType.whaleshark
        };

        //袋から魚駒5個海へ
        Setup(fishType, pouch, sea, 5);



        //中身の確認（テスト用）
        foreach (var kp in pouch)
        {
            var key = kp.Key;
            var value = kp.Value;
            Debug.Log($"{key} / {value}");
        }
        foreach (var kp in sea)
        {
            var key = kp.Key;
            var value = kp.Value;
            Debug.Log($"{key} / {value}");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    //fromからtoへ魚駒を移動させる
    void Setup(PieceType[] types, Dictionary<PieceType, int> from, Dictionary<PieceType, int> to, int count)
    {
        for (int i = 0; i < count; i++)
        {
            //残っている駒をすべて集める
            List<PieceType> available = new List<PieceType>();
            foreach (var t in types)
            {
                if (from[t] > 0)
                {
                    available.Add(t);
                }
            }

            //ランダムな駒を移動させる
            int rnd = Random.Range(0, available.Count);
            PieceType select = available[rnd];
            from[select]--;
            to[select]++;
        }
    }
}
