using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Part
    {
        gallery,
        aquarium
    }

    Part currentPart = Part.gallery;

    public SeaBoard sea;
    public GameObject fishPrefab;

    [Header("StartSeaFish")]
    public FishData[] fishData;
    public int[] startSeaFishCount; //スタート時の海ボード魚駒数
    public int[] startFishCount;    //スタート時のギャラリー魚駒数

    public PlayerController[] players;
    public PlayerController[] playersTurn;  //スタート時のプレイの順番を管理する配列
    public Tile[] galleryTiles;

    public StoragePanel[] storagePanel; //各プレイヤーのストレージパネル
    public bool canSelct = false;   //海ボードのゲットボタンが押せるかどうか

    public GameObject galleryCam;   //ギャラリーボードを映すためのカメラ

    [SerializeField] GameObject[] aquaText; //各プレイヤーの水槽スライダー

    [SerializeField] PlayerRanking playerRanking;

    public float moveCamTime;

    PlayerController currentPlayer;
    PlayerController nextPlayer;

    public int maxRound;
    public GameObject confirmButton;
    public static bool UIActive = false;

    int turnCount;
    int roundCount;
    public int currentPlayerIndex = 0;

    //移動可能なマスを絞るためのリスト
    List<Tile> movableTiles = new List<Tile>();
    //ゴールしたプレイヤーを確認するリスト
    List<PlayerController> finishOrder = new List<PlayerController>();

    private void Start()
    {
        sea.Initialze(fishData, startSeaFishCount);
        playersTurn = new PlayerController[players.Length];
        Array.Copy(players, playersTurn, players.Length);

        finishOrder.Clear();

        foreach (Tile t in playersTurn[currentPlayerIndex].aquariumTiles)
        {
            t.GetComponent<Collider2D>().enabled = false;
        }

        roundCount = 0;

        StartRound();

    }


    private void Update()
    {
        if (currentPart == Part.aquarium)
        {
            if (UIActive)
            {
                playersTurn[currentPlayerIndex].oxygenText.SetActive(false);
            }
            else
            {
                playersTurn[currentPlayerIndex].oxygenText.SetActive(true);
            }
        }
    }


    //ラウンドスタート
    void StartRound()
    {
        currentPart = Part.gallery;
        galleryCam.SetActive(true);
        canSelct = false;
        confirmButton.SetActive(false);

        turnCount = 0;
        currentPlayerIndex = 0;
        movableTiles.Clear();
        ClearHighlights();

        roundCount++;

        for (int i = 0; i < players.Length; i++)
        {
            players[i].isGoal = false;
            players[i].currentGalleryTile = galleryTiles[i];
        }

        foreach (Tile t in galleryTiles)
        {
            t.uesdTile = false;
        }

        HighlightMovableTiles(playersTurn[currentPlayerIndex].currentGalleryTile, galleryTiles);

        //ギャラリーに魚駒を並べる
        foreach (Tile tile in galleryTiles)
        {
            if (!tile.isAd && !tile.isStart)
            {
                for (int i = 0; i < 30; i++)
                {
                    int rand = UnityEngine.Random.Range(2, 8);
                    if (startFishCount[rand] > 0)
                    {
                        GameObject fish = Instantiate(fishPrefab, tile.transform);
                        fish.GetComponent<FishPiece>().fishData = fishData[rand];

                        startFishCount[rand]--;
                        break;
                    }
                }
            }
        }

        UIManager.Instance.ShowMessage("ラウンド" + roundCount + "\\n" + playersTurn[currentPlayerIndex].pData.playerName + "のターン");
    }


    //ギャラリーパート
    //マスが選択されたらそのマスへ移動
    public void OnTileClicked(Tile clickedTile)
    {
        //ギャラリーターンでなければ即リターン
        if (currentPart != Part.gallery) return;
        //クリックされたマスが選択不可なら即リターン
        if (!movableTiles.Contains(clickedTile)) return;

        currentPlayer = playersTurn[currentPlayerIndex];
        currentPlayer.MoveToGalleryTile(clickedTile);

        //ゴールしたらフラグを立てる
        if (clickedTile.index == 0)
        {
            currentPlayer.isGoal = true;
            finishOrder.Add(currentPlayer);

            //全員がゴールしたかチェック
            int goalCount = 0;
            foreach (PlayerController p in players)
            {
                if (p.isGoal)
                {
                    goalCount++;
                }
            }
            //全員がゴールしたらラウンド終了
            if (goalCount == players.Length)
            {
                EndRound();
                return;
            }
            //ターンを強制終了
            NextPlayerTurn();
            return;
        }

        //マス上の魚駒ゲット
        if (!clickedTile.isAd)
        {
            while (clickedTile.transform.childCount > 0)
            {
                GameObject piece = clickedTile.transform.GetChild(0).gameObject;
                FishPiece fish = piece.GetComponent<FishPiece>();

                //魚駒を上に少し飛ばす（ゲットアニメーション）
                Rigidbody2D rbody = piece.GetComponent<Rigidbody2D>();
                rbody.gravityScale = 1.0f;
                rbody.bodyType = RigidbodyType2D.Dynamic;
                rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

                StartCoroutine(MoveStorage(fish, rbody));

                piece.transform.SetParent(null);
            }
        }




        //水族館ターンへ
        StartCoroutine(ChangeCamera(galleryCam, currentPlayer.aquariumCam));
        HighlightMovableTiles(currentPlayer.currentAquaTile, currentPlayer.aquariumTiles);
        foreach (Tile t in playersTurn[currentPlayerIndex].aquariumTiles)
        {
            t.GetComponent<Collider2D>().enabled = true;
        }
    }

    //ストレージに移動するコルーチン
    IEnumerator MoveStorage(FishPiece fish, Rigidbody2D rbody)
    {
        yield return new WaitForSeconds(1.0f);
        playersTurn[currentPlayerIndex].storagePanel.AddStorage(fish);
        rbody.gravityScale = 0;
        rbody.bodyType = RigidbodyType2D.Static;
    }


    //水族館パート
    //マスが選択されたらそのマスへ移動
    public void OnAquaTileClicked(Tile clickedTile)
    {
        if (currentPart != Part.aquarium) return;
        if (!movableTiles.Contains(clickedTile)) return;

        currentPlayer.MoveToAquaTile(clickedTile);
        confirmButton.SetActive(true);

        //移動が終わったら海ボードのゲットボタンを復活させる
        canSelct = true;

        foreach (Tile t in playersTurn[currentPlayerIndex].aquariumTiles)
        {
            t.GetComponent<Collider2D>().enabled = false;
        }
    }


    //水族館パートの決定ボタン
    public void OnConfirmButton()
    {
        if (currentPlayer.storagePanel.HasFishInStorage())
        {
            //警告を出す
            UIManager.Instance.ShowMessage("ストレージの魚をすべて配置してください！");
            return;
        }
        if (!AreAllAquariumsValid(currentPlayer))
        {
            //警告を出す
            UIManager.Instance.ShowMessage("水槽の魚を確認してください！");
            return;
        }
        else
        {
            //水槽のハイライトを消す（元に戻す）
            currentPlayer.currentAquaTile.leftSlot.SetHighlight(false);
            currentPlayer.currentAquaTile.rightSlot.SetHighlight(false);

            //水槽のコライダーを消す
            currentPlayer.currentAquaTile.leftSlot.GetComponent<Collider2D>().enabled = false;
            currentPlayer.currentAquaTile.rightSlot.GetComponent<Collider2D>().enabled = false;


            //次のプレイヤーのターンへ
            StartCoroutine(ChangeCamera(currentPlayer.aquariumCam, galleryCam));
            NextPlayerTurn();
        }
    }


    //プレイヤーが持つすべての水槽がルールを守っているか
    bool AreAllAquariumsValid(PlayerController player)
    {
        foreach (var slot in player.aquaSlots)
        {
            if (!slot.IsOxygenValid()) //酸素量オーバーを先にチェック
                return false;

            foreach (var fish in slot.fishes)
            {
                //自分を一時的にリストから外して判定
                var others = slot.fishes.Where(f => f != fish).ToList();

                if (!slot.CanAcceptFish(fish, others)) //相性をチェック
                    return false;
            }
        }
        return true;
    }



    //次のプレイヤーのターンへ
    public void NextPlayerTurn()
    {
        //移動を終えたらマスのハイライトを戻して次のターンへ
        ClearHighlights();
        if (turnCount < players.Length - 1)
        {
            turnCount++;
            currentPlayerIndex++;
        }
        else
        {
            currentPlayerIndex = GetLastPlayerIndex();
        }

        nextPlayer = playersTurn[currentPlayerIndex];

        if (nextPlayer.isGoal)
        {
            NextPlayerTurn();
            return;
        }

        //ボタンを押せないように元に戻す
        canSelct = false;
        confirmButton.SetActive(false);

        HighlightMovableTiles(nextPlayer.currentGalleryTile, galleryTiles);
        UIManager.Instance.ShowMessage("ラウンド" + roundCount + "\n" + nextPlayer.pData.playerName + "のターン");
    }

    // 一番後ろのプレイヤーのインデックス番号を返す
    private int GetLastPlayerIndex()
    {
        int minIndex = 0;
        int minTile = 19;

        for (int i = 0; i < players.Length; i++)
        {
            if (playersTurn[i].currentGalleryTile.index == 0)
            {
                continue;
            }
            else if (playersTurn[i].currentGalleryTile.index <= minTile)
            {
                minTile = playersTurn[i].currentGalleryTile.index;
                minIndex = i;
            }
        }
        return minIndex;
    }




    //移動可能かどうか判定
    void HighlightMovableTiles(Tile from, Tile[] boradTiles)
    {
        movableTiles.Clear();

        if (boradTiles == galleryTiles)
        {
            foreach (Tile t in boradTiles)
            {
                //今より後ろのマスかどうか
                if (from.index < t.index)
                {
                    //ほかのプレイヤーがいないかどうか
                    bool occupied = false;
                    foreach (PlayerController p in playersTurn)
                    {
                        if (p != playersTurn[currentPlayerIndex] && p.currentGalleryTile == t)
                        {
                            //ほかのプレイヤーがいたらtrueにして除外
                            occupied = true;
                            break;
                        }
                    }

                    if (occupied) continue;

                    if (t.uesdTile) continue;

                    movableTiles.Add(t);
                    t.Highlight(true);
                }

            }
            if (playersTurn[currentPlayerIndex].currentGalleryTile.index >= 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (!galleryTiles[i].uesdTile)
                    {
                        movableTiles.Add(galleryTiles[i]);
                        galleryTiles[i].Highlight(true);
                        break;
                    }
                }
            }
        }
        else
        {
            int fromIndex = from.index;

            for (int step = 0; step < 3; step++)
            {
                int targetIndex = (fromIndex + step) % boradTiles.Length;
                Tile targetTile = boradTiles[targetIndex];

                movableTiles.Add(targetTile);
                targetTile.Highlight(true);
            }
        }


    }


    //マスのハイライトを元に戻す
    void ClearHighlights()
    {
        foreach (Tile t in galleryTiles)
        {
            t.Highlight(false);
        }
        foreach (PlayerController p in players)
        {
            foreach (Tile t in p.aquariumTiles)
            {
                t.Highlight(false);
                t.GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    //ラウンド終了
    void EndRound()
    {
        UIManager.Instance.ShowMessage("ラウンド終了！");

        for (int i = 0; i < finishOrder.Count; i++)
        {
            Debug.Log((i + 1) + "位" + finishOrder[i].pData.playerName);
        }

        playersTurn = finishOrder.ToArray();

        //指定回数ラウンドを繰り返したらゲーム終了
        if (roundCount >= maxRound)
        {
            UIManager.Instance.ShowMessage("ゲーム終了！");
            playerRanking.ResultScore();
            SceneManager.LoadScene("Result");
            return;
        }
        else
        {
            //次のラウンドへ
            StartRound();
            return;
        }
    }


    //カメラをmoveCamTime秒後に切り替え
    IEnumerator ChangeCamera(GameObject from, GameObject to)
    {
        yield return new WaitForSeconds(moveCamTime);
        from.SetActive(false);
        to.SetActive(true);
        if (currentPart == Part.gallery)
        {
            currentPart = Part.aquarium;
        }
        else
        {
            currentPart = Part.gallery;
            foreach (PlayerController p in players)
            {
                p.oxygenText.SetActive(false);
            }
        }
    }
}
