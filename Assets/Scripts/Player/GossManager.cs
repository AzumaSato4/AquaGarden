using UnityEngine;

public class GossManager : MonoBehaviour
{
    public Player player; //プレイヤー情報
    GalleryBoard galleryBoard; //ギャラリータイル情報を取得するための変数

    [SerializeField] GameObject galleryPlayer;   //ギャラリーのプレイヤー駒

    GameObject currentGalleryTile;  //現在のギャラリーマス
    public int galleryIndex;        //現在のギャラリーマス番号

    public TurnManager turnManager;    //TurnManagerを格納するための変数
    public PhaseManager phaseManager;  //PhaseManagerを格納するための変数
    public AquaPieceManager aquaPieceManager;  //PieceManagerを格納するための変数
    SeaBoard seaBoard;
    [SerializeField] GossController gossController;

    public bool isActive = false;   //自分のターンかどうか
    public bool isGoal = false;     //ゴールしたかどうか

    private void Start()
    {
        //プレイヤー駒の画像をセット
        galleryPlayer.GetComponent<SpriteRenderer>().sprite = player.gallerySprite;

        //ギャラリーの情報を取得
        galleryBoard = GameObject.Find("Gallery").GetComponent<GalleryBoard>();

        //TurnManagerとPhaseManagerを取得
        GameObject manager = GameObject.Find("MainManager");
        turnManager = manager.GetComponent<TurnManager>();
        phaseManager = manager.GetComponent<PhaseManager>();
        seaBoard = turnManager.seaBoard;

        //初期位置にセット
        //ギャラリースタート位置にセット
        galleryIndex = player.playerNum - 1;
        currentGalleryTile = galleryBoard.startSpots[galleryIndex];
        galleryPlayer.transform.position = currentGalleryTile.transform.position;
        galleryBoard.isPlayer[galleryIndex] = true;
    }

    public void StartGallery()
    {
        if (!isActive) return;

        int index = CheckTarget();
        GameObject tile;
        if (index > 3)
        {
            if (index == 23)
            {
                index = 0;
                tile = galleryBoard.startSpots[index];
                isGoal = true;
            }
            else
            {
                tile = galleryBoard.galleryTiles[index - galleryBoard.startSpots.Length];
            }
        }
        else
        {
            tile = galleryBoard.startSpots[index];
            isGoal = true;
        }

            Debug.Log(index + "目標");
        Debug.Log(tile.name);
        gossController.MoveStart(index, tile);
    }

    int CheckTarget()
    {
        int target; //移動目標タイル番号
        int headIndex = 0; //先頭にいるプレイヤーのタイル番号

        if (turnManager.loopCnt > GameManager.players)
        {
            //もし誰かがゴールしていたらゴールを目標にする
            if (galleryBoard.isPlayer[1])
            {
                target = 2;
                return target;
            }
            else if (galleryBoard.isPlayer[0])
            {
                target = 1;
                return target;
            }
        }

        //先頭のプレイヤーを探す
        for (int i = galleryBoard.isPlayer.Length - 1; i > 3; i--)
        {
            Debug.Log(i);
            if (galleryBoard.isPlayer[i])
            {
                headIndex = i;
                break;
            }
        }
        target = headIndex + 1;

        //スタート時に全員がスタート地点にいたら最初の魚マスに移動する
        if (target < 4)
        {
            target = 4;
        }

        return target;
    }

    //ギャラリーボードを移動
    public void MoveGallery(int to, string tile)
    {
        galleryPlayer.transform.localScale = Vector2.one;

        galleryBoard.isPlayer[galleryIndex] = false;
        galleryIndex = to;
        galleryBoard.isPlayer[galleryIndex] = true;

        if (0 <= galleryIndex && galleryIndex <= 3)
        {
            isGoal = true;
            //ゴールしたら強制ターンエンド
            turnManager.EndTurn();
            return;
        }

        //魚マス
        if (tile == "FishTile")
        {
            FishTile fishTile = galleryBoard.galleryTiles[to - 4].GetComponent<FishTile>();
            RelesePiece(fishTile);
        }
        turnManager.EndTurn();
    }

    //魚駒を海ボードへ移動させる
    void RelesePiece(FishTile tile)
    {
        for (int i = 0; i < tile.pieces.Count; i++)
        {
            seaBoard.ReleasePiece(tile.pieces[i]);
            Destroy(tile.pieces[i]);
        }
        tile.pieces.Clear();
    }
}

