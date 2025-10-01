using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerData playerData; //プレイヤー情報
    GalleryTileManager gTileManager; //ギャラリータイル情報を取得するための変数
    AquariumBoard aquariumBoard; //水族館ボード情報を取得するための変数
    public int money; //所持資金

    GameObject aquarium;        //水族館ボード
    GameObject galleryPlayer;   //ギャラリーのプレイヤー駒
    GameObject aquariumPlayer;  //水族館のプレイヤー駒

    private void Start()
    {
        //プレイヤーの駒と水族館ボードを生成
        aquarium = Instantiate(playerData.aquarium, new Vector3(playerData.playerNum * 20, 0, 0), Quaternion.identity);
        galleryPlayer = Instantiate(playerData.galleryPlayer);
        aquariumPlayer = Instantiate(playerData.aquariumPlayer);
        
        //プレイヤー駒の画像をセット
        galleryPlayer.GetComponent<SpriteRenderer>().sprite = playerData.gallerySprite;
        aquariumPlayer.GetComponent<SpriteRenderer>().sprite = playerData.aquariumSprite;

        //ギャラリーと水族館の情報を取得
        gTileManager = GameObject.Find("Gallery").GetComponent<GalleryTileManager>();
        aquariumBoard = aquarium.GetComponent<AquariumBoard>();

        //初期位置にセット
        galleryPlayer.transform.position = gTileManager.StartSpots[playerData.playerNum - 1].transform.position; //ギャラリースタート位置にセット
        aquariumPlayer.transform.position = aquariumBoard.aquaTiles[0].transform.position; //水族館スタート位置にセット
        aquariumBoard.coin.transform.position = aquariumBoard.CoinSpots[2].transform.position; //コインの初期位置は2
    }

    public void ActGallery()
    {

    }

}
