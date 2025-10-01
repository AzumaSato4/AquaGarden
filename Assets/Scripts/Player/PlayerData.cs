using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    //プレイヤーデータを格納
    public int playerNum;               //プレイヤー番号
    public string playerName;           //プレイヤー名
    public GameObject galleryPlayer;    //ギャラリーのプレイヤー駒
    public Sprite gallerySprite;        //ギャラリーのプレイヤー駒画像
    public GameObject aquariumPlayer;   //水族館のプレイヤー駒
    public Sprite aquariumSprite;       //水族館のプレイヤー駒画像
    public GameObject aquarium;         //水族館ボード
}
