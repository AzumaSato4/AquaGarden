using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab; //プレイヤーのプレハブ
    [SerializeField] GameManager gameManager;

    void Start()
    {
        //参加プレイヤーの数だけ繰り返す
        for (int i = 0; i < GameManager.players; i++)
        {
            //プレイヤーを生成
            GameObject obj = Instantiate(playerPrefab, new Vector3(i * 30, 0, 0), Quaternion.identity);


            //生成したプレイヤーにプレイヤー情報をセット
            Player playerData = new Player()
            {
                playerNum = i + 1,               //プレイヤー番号
                playerName = GameManager.playerName[i],           //プレイヤー名
                gallerySprite = GameManager.galleryColor[i],        //ギャラリーのプレイヤー駒画像
                aquariumSprite = GameManager.aquariumColor[i]       //水族館のプレイヤー駒画像
            };
            PlayerData.players.Add(playerData);

            PlayerManager playerManager = obj.GetComponent<PlayerManager>();
            playerManager.player = playerData;
            playerManager.feedingData = gameManager.GetFeedingData(i);
            playerManager.aquaPieceManager = GetComponent<AquaPieceManager>();
        }
    }
}
