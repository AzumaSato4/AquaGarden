using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Pieces : MonoBehaviour
{
    public PieceData[] pieces;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        ////布袋
        //Dictionary<PieceType, int> pouch = new Dictionary<PieceType, int>()
        //{
        //    {PieceType.minifish, minifishes},
        //    {PieceType.bigfish, bigfishes},
        //    {PieceType.seaturtle, seaturtles},
        //    {PieceType.seahorse, seahorses},
        //    {PieceType.shark, sharks},
        //    {PieceType.whaleshark, whalesharks},
        //    {PieceType.seaweed, seaweeds},
        //    {PieceType.coral, corals}
        //};

        ////海ボード
        //Dictionary<PieceType, int> sea = new Dictionary<PieceType, int>()
        //{
        //    {PieceType.minifish, 0},
        //    {PieceType.bigfish, 0},
        //    {PieceType.seaturtle, 0},
        //    {PieceType.seahorse, 0},
        //    {PieceType.shark, 0},
        //    {PieceType.whaleshark, 0},
        //    {PieceType.seaweed, 0},
        //    {PieceType.coral, 0}
        //};

        ////魚駒だけ集める
        //PieceType[] fishType = new PieceType[]
        //{
        // PieceType.minifish,
        // PieceType.bigfish,
        // PieceType.seaturtle,
        // PieceType.seahorse,
        // PieceType.shark,
        // PieceType.whaleshark
        //};

        ////袋から魚駒5個海へ
        //Setup(fishType, pouch, sea, 5);



        ////中身の確認（テスト用）
        //foreach (var kp in pouch)
        //{
        //    var key = kp.Key;
        //    var value = kp.Value;
        //    Debug.Log($"{key} / {value}");
        //}
        //foreach (var kp in sea)
        //{
        //    var key = kp.Key;
        //    var value = kp.Value;
        //    Debug.Log($"{key} / {value}");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameMode == "Start")
        {
            //プレイ人数によって個数を変える
            SetPlayer(SelectPlayer.selectPlayer);

            GameController.gameMode = "Playing";
            Debug.Log(GameController.gameMode);

        }

    }

    public void SetPlayer(int players)
    {
        if (players == 1 || players == 4)
        {
            //4人or1人用
            pieces[0].items = 23; //小型魚
            pieces[1].items = 16; //大型魚
            pieces[2].items = 9; //ウミガメ
            pieces[3].items = 5; //タツノオトシゴ
            pieces[4].items = 9; //サメ
            pieces[5].items = 3; //ジンベエザメ
            pieces[6].items = 17; //海藻
            pieces[7].items = 16; //サンゴ
        }
        else
        {
            //3人or2人用
            pieces[0].items = 18; //小型魚
            pieces[1].items = 12; //大型魚
            pieces[2].items = 7; //ウミガメ
            pieces[3].items = 4; //タツノオトシゴ
            pieces[4].items = 7; //サメ
            pieces[5].items = 2; //ジンベエザメ
            pieces[6].items = 17; //海藻
            pieces[7].items = 16; //サンゴ
        }


        foreach (var count in pieces)
        {
            Debug.Log(count.items);
        }
    }


    ////fromからtoへ魚駒を移動させる
    //void Setup(PieceType[] types, Dictionary<PieceType, int> from, Dictionary<PieceType, int> to, int count)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        //残っている駒をすべて集める
    //        List<PieceType> available = new List<PieceType>();
    //        foreach (var t in types)
    //        {
    //            if (from[t] > 0)
    //            {
    //                available.Add(t);
    //            }
    //        }

    //        //ランダムな駒を移動させる
    //        int rnd = Random.Range(0, available.Count);
    //        PieceType select = available[rnd];
    //        from[select]--;
    //        to[select]++;
    //    }
    //}
}
