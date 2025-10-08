using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab; //プレイヤーのプレハブ
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        //参加プレイヤーの数だけ繰り返す
        for (int i = 0; i < GameManager.players; i++)
        {
            //プレイヤーを生成
            GameObject player = Instantiate(playerPrefab, new Vector3(i * 30, 0, 0), Quaternion.identity);

            //生成したプレイヤーにプレイヤー情報をセット
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            playerManager.playerData = gameManager.SarchPlayerData(i);
            playerManager.feedingData = gameManager.SarchFeedingData(i);
            playerManager.aquaPieceManager = GetComponent<AquaPieceManager>();
        }
    }
}
