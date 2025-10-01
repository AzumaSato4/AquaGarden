using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerData[] playerData = new PlayerData[4]; //プレイヤー情報
    [SerializeField] GameObject playerPrefab; //プレイヤーのプレハブ

    void Start()
    {
        //参加プレイヤーの数だけ繰り返す
        for (int i = 0; i < 4; i++)
        {
            //プレイヤーを生成
            GameObject player = Instantiate(playerPrefab);

            //生成したプレイヤーにプレイヤー情報をセット
            player.GetComponent<PlayerController>().playerData = playerData[i];
        }
    }
}
