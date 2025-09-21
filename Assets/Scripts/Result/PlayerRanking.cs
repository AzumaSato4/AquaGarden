using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class PlayerRanking : MonoBehaviour
{
    [SerializeField] GameManager manager;

    public Dictionary<PlayerController, int> playersScore;
    Dictionary<FishData.Type, int> slotFish;

    public int goalScore = 3;  //ゴール順による得点数

    public static List<(PlayerData pData,int score)> results = new List<(PlayerData pData,int score)> ();

    //プレイヤーの得点計算
    public void ResultScore()
    {
        //初期化とゴール順による得点計算
        playersScore = new Dictionary<PlayerController, int>();

        foreach (PlayerController p in manager.players)
        {
            //最終ラウンドのゴール順を評価
            playersScore[p] = goalScore;

            goalScore--;
        }

        //一人ずつ得点を計算
        foreach (PlayerController p in manager.players)
        {
            //すべての水槽を評価
            foreach (AquaSlot slot in p.aquaSlots)
            {
                //水槽内の得点計算用に初期化
                slotFish = new Dictionary<FishData.Type, int>();
                foreach (FishData.Type type in System.Enum.GetValues(typeof(FishData.Type)))
                {
                    slotFish[type] = 0;
                }


                //1つの水槽内を評価
                foreach (FishPiece fish in slot.fishes)
                {
                    slotFish[fish.fishData.type]++;
                }

                //小型魚1個と大型魚1個のセット1つにつき3点
                if (slotFish[FishData.Type.SmallFish] > 0 && slotFish[FishData.Type.LargeFish] > 0)
                {
                    while (slotFish[FishData.Type.SmallFish] > 0 && slotFish[FishData.Type.LargeFish] > 0)
                    {
                        playersScore[p] += 2;
                        slotFish[FishData.Type.SmallFish]--;
                        slotFish[FishData.Type.LargeFish]--;
                    }
                }

                //ウミガメ1個につき2点
                if (slotFish[FishData.Type.Seaturtle] > 0)
                {
                    while (slotFish[FishData.Type.Seaturtle] > 0)
                    {
                        playersScore[p] += 2;
                        slotFish[FishData.Type.Seaturtle]--;
                    }
                }

                //サメ1個につき2点
                if (slotFish[FishData.Type.Shark] > 0)
                {
                    while (slotFish[FishData.Type.Shark] > 0)
                    {
                        playersScore[p] += 2;
                        slotFish[FishData.Type.Shark]--;
                    }
                }

                //タツノオトシゴ1個につき2点
                if (slotFish[FishData.Type.Seahorse] > 0)
                {
                    while (slotFish[FishData.Type.Seahorse] > 0)
                    {
                        playersScore[p] += 2;
                        slotFish[FishData.Type.Seahorse]--;
                    }
                }

                //ジンベエザメ1個につき4点
                if (slotFish[FishData.Type.WhaleShark] > 0)
                {
                    while (slotFish[FishData.Type.WhaleShark] > 0)
                    {
                        playersScore[p] += 4;
                        slotFish[FishData.Type.WhaleShark]--;
                    }
                }

                //サンゴ1個につき1点
                if (slotFish[FishData.Type.Coral] > 0)
                {
                    while (slotFish[FishData.Type.Coral] > 0)
                    {
                        playersScore[p] += 1;
                        slotFish[FishData.Type.Coral]--;
                    }
                }
            }

            //余った資金を評価
            int playerMoney = p.pData.money;
            //資金3につき1点
            playersScore[p] += playerMoney / 3;
        }

        //順位リストを作成
        results.Clear();
        foreach (var n in playersScore.OrderByDescending(x => x.Value))
        {
            results.Add((n.Key.pData, n.Value));
        }
    }
}
