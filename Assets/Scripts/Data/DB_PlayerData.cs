using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //プレイヤーデータを格納
    public int playerNum;               //プレイヤー番号
    public string playerName;           //プレイヤー名
    public Sprite gallerySprite;        //ギャラリーのプレイヤー駒画像
    public Sprite aquariumSprite;       //水族館のプレイヤー駒画像
}


[CreateAssetMenu(menuName = "PlayerData")]
public class DB_PlayerData : ScriptableObject
{
    public PlayerData[] playerDatas;
}
