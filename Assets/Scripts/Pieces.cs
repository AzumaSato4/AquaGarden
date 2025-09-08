using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;



public class Pieces : MonoBehaviour
{
    public PieceData[] pieces;
    //布袋
    public List<string> pouch = new List<string>();
    //海ボード
    public Dictionary<string, int> sea = new Dictionary<string, int>();

    int totalPieces = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //プレイ人数によって個数を変える
        SetPlayer(SelectPlayer.selectPlayer);

        GameController.gameMode = "Playing";
        Debug.Log(GameController.gameMode);

        //布袋
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i].category == "Fish")
            {
                for (int j = 0; j < pieces[i].items; j++)
                {
                    pouch.Add(pieces[i].pieceName);
                }
            }
        }

        //海ボード
        //初期化
        for (int i = 0; i < pieces.Length; i++)
        {
            sea.Add(pieces[i].pieceName, 0);
        }
        //海藻とサンゴを入れる
        sea["Seaweed"] = pieces[6].items;
        sea["Coral"] = pieces[7].items;



        //袋から魚駒5個海へ
        PouchToSea(5);

        //中身確認用
        foreach (KeyValuePair<string, int> kvp in sea)
        {
            Debug.Log(kvp.Key + "=" + kvp.Value);
        }
    }

    // Update is called once per frame
    void Update()
    {

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

    }


    //pouchからseaへ魚駒を移動させる
    void PouchToSea(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //ランダムな駒を移動させる
            int rnd = Random.Range(0, pouch.Count);
            string select = pouch[rnd];

            sea[select]++;
            pouch.RemoveAt(rnd);
        }
    }
}
