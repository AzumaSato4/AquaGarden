using System.Collections.Generic;
using UnityEngine;

public class Player
{
    //プレイヤーデータを格納
    public int playerNum;               //プレイヤー番号
    public string playerName;           //プレイヤー名
    public Sprite gallerySprite;        //ギャラリーのプレイヤー駒画像
    public Sprite aquariumSprite;       //水族館のプレイヤー駒画像
    public Sprite milestoneChecker;     //マイルストーン達成駒画像
}


public  class PlayerData : MonoBehaviour
{
    public static List<Player> players = new List<Player>();
}
