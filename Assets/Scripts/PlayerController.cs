using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerData pData;        //プレイヤーの基本情報

    //ギャラリーボード
    public GameObject galleryPiece; //ギャラリーボード用の駒
    public Tile currentGalleryTile; //現在のマス番号（初期値はスタートマス）
    public bool isGoal = false;     //ゴールしたかどうか

    //水族館ボード
    public GameObject aquariumPiece; //水族館ボード用の駒
    public Tile[] aquariumTiles;     //プレイヤーそれぞれが持つ水族館ボード
    public Tile currentAquaTile;    //水族館ボードの現在のマス番号
    public GameObject aquariumCam;  //水族館用カメラ

    float moveTime = 0.3f; //プレイヤー駒が目的地までかかる時間


    private void Start()
    {
        //自分のギャラリー駒と水族館駒をスタート位置にセットする
        galleryPiece.transform.position = currentGalleryTile.transform.position;
        aquariumPiece.transform.position = currentAquaTile.transform.position;

        //自分の駒の色をプレイヤーカラーにする
        galleryPiece.GetComponent<SpriteRenderer>().color = pData.color;
        aquariumPiece.GetComponent<SpriteRenderer>().color = pData.color;
    }


    //選んだギャラリーマスに移動する
    public void MoveToGalleryTile(Tile toTile)
    {
        currentGalleryTile = toTile;
        //他のプレイヤーが選択不可にする
        toTile.uesdTile = true;
        currentGalleryTile.Highlight(false);

        StartCoroutine(MoveCoroutine(toTile.transform.position, galleryPiece));
    }


    //選んだ水族館マスに移動する
    public void MoveToAquaTile(Tile toTile)
    {
        currentAquaTile = toTile;
        EnableAquariumSlot(toTile);
        StartCoroutine(MoveCoroutine(toTile.transform.position, aquariumPiece));
    }


    public void EnableAquariumSlot(Tile currentTile)
    {
        foreach (Tile t in aquariumTiles)
        {
            t.Highlight(false);
            t.GetComponent<Collider2D>().enabled = false;
        }

        //隣接の水槽を有効化
        currentTile.leftSlot.SetHighlight(true);
        currentTile.rightSlot.SetHighlight(true);

        currentTile.leftSlot.GetComponent<Collider2D>().enabled = true;
        currentTile.rightSlot.GetComponent<Collider2D>().enabled = true;
    }


    //移動を滑らかにするコルーチン
    IEnumerator MoveCoroutine(Vector2 toPos, GameObject piece)
    {
        Vector2 startPos = piece.transform.position;
        float elapsed = 0;

        while (elapsed < moveTime)
        {
            piece.transform.position = Vector2.Lerp(startPos, toPos, elapsed / moveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        piece.transform.position = toPos;
    }
}
