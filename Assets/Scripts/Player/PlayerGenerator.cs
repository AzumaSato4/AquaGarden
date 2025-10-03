using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public PlayerData[] playerData; //プレイヤー情報
    [SerializeField] GameObject playerPrefab; //プレイヤーのプレハブ

    void Awake()
    {
        //参加プレイヤーの数だけ繰り返す
        for (int i = 0; i < GameManager.players; i++)
        {
            //プレイヤーを生成
            GameObject player = Instantiate(playerPrefab);

            //生成したプレイヤーにプレイヤー情報をセット
            player.GetComponent<PlayerManager>().playerData = playerData[i];
            player.GetComponent<PlayerManager>().aquaPieceManager = GetComponent<AquaPieceManager>();
        }
    }
}
