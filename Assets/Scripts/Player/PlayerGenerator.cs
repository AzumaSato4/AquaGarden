using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    GameManager gameManager;
    [SerializeField] FeedingCardPanel feedingCardPanel;

    public List<int> feedingDatas = new List<int>(); //餌やりカードがかぶらないようにするために記録

    void Start()
    {
        gameManager = GameManager.instance;
        feedingDatas.Clear();

        //参加プレイヤーの数だけ繰り返す
        for (int i = 0; i < GameManager.players; i++)
        {
            //プレイヤーを生成
            GameObject obj = Instantiate(playerPrefab, new Vector3(i * 40, 0, 0), Quaternion.identity);


            //生成したプレイヤーにプレイヤー情報をセット
            Player playerData = new Player()
            {
                playerNum = i + 1,  //プレイヤー番号
                playerName = GameManager.playerName[i], //プレイヤー名
                gallerySprite = GameManager.galleryColor[i],    //ギャラリーのプレイヤー駒画像
                aquariumSprite = GameManager.aquariumColor[i],   //水族館のプレイヤー駒画像
                milestoneChecker = GameManager.milestoneColor[i]   //マイルストーンのプレイヤー駒画像
            };
            PlayerData.players.Add(playerData);

            PlayerManager playerManager = obj.GetComponent<PlayerManager>();
            playerManager.player = playerData;
            playerManager.aquaPieceManager = GetComponent<AquaPieceManager>();

            int rand;
            do
            {
                rand = Random.Range(0, gameManager.feedingDataCount);
            } while (feedingDatas.Contains(rand));
            FeedingData data = gameManager.GetFeedingData(rand);
            playerManager.feedingData = data;
            feedingDatas.Add(rand);

            feedingCardPanel.Initialize(data, GameManager.playerName[i]);
        }
    }
}
