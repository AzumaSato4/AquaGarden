using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

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
    public Tile[] galleryTiles;

    public StoragePanel[] storagePanel; //各プレイヤーのストレージパネル
    public bool canSelct = false;   //海ボードのゲットボタンが押せるかどうか

    public GameObject galleryCam;   //ギャラリーボードを映すためのカメラ
    public GameObject[] aquariumCams;   //水族館ボードを映すためのカメラ

    [SerializeField] GameObject[] aquaText; //各プレイヤーの水槽スライダー

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

        roundCount = 0;

        StartRound();

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
        finishOrder.Clear();
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

        HighlightMovableTiles(players[currentPlayerIndex].currentGalleryTile, galleryTiles);

        //ギャラリーに魚駒を並べる
        foreach (Tile tile in galleryTiles)
        {
            if (!tile.isAd && !tile.isStart)
            {
                for (int i = 0; i < 30; i++)
                {
                    int rand = Random.Range(2, 8);
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

        Debug.Log("ラウンド" + roundCount);
        Debug.Log(players[currentPlayerIndex].pData.playerName + "のターン");
    }


    //ギャラリーパート
    //マスが選択されたらそのマスへ移動
    public void OnTileClicked(Tile clickedTile)
    {
        //ギャラリーターンでなければ即リターン
        if (currentPart != Part.gallery) return;
        //クリックされたマスが選択不可なら即リターン
        if (!movableTiles.Contains(clickedTile)) return;

        currentPlayer = players[currentPlayerIndex];
        currentPlayer.MoveToGalleryTile(clickedTile);

        //ゴールしたらフラグを立てる
        if (clickedTile.index == 0)
        {
            currentPlayer.isGoal = true;
            finishOrder.Add(currentPlayer);

            //全員がゴールしたらラウンド終了
            int goalCount = 0;
            foreach (PlayerController p in players)
            {
                if (p.isGoal)
                {
                    goalCount++;
                }
            }
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
            GameObject piece = clickedTile.transform.GetChild(0).gameObject;
            FishPiece fish = piece.GetComponent<FishPiece>();

            //魚駒を上に少し飛ばす（ゲットアニメーション）
            Rigidbody2D rbody = piece.GetComponent<Rigidbody2D>();
            rbody.gravityScale = 1.0f;
            rbody.bodyType = RigidbodyType2D.Dynamic;
            rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

            StartCoroutine(MoveStorage(fish, rbody));
        }



        StartCoroutine(ChangeCamera(galleryCam, currentPlayer.aquariumCam));
        StartCoroutine(ChangeAquaSlider(true));

        //水族館ターンへ
        currentPart = Part.aquarium;
        HighlightMovableTiles(currentPlayer.currentAquaTile, currentPlayer.aquariumTiles);
    }

    //ストレージに移動するコルーチン
    IEnumerator MoveStorage(FishPiece fish, Rigidbody2D rbody)
    {
        yield return new WaitForSeconds(1.0f);
        storagePanel[currentPlayerIndex].AddStorage(fish);
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
    }


    //水族館パートの決定ボタン
    public void OnConfirmButton()
    {
        if (currentPlayer.storagePanel.HasFishInStorage())
        {
            // UI に警告を出す
            UIManager.Instance.ShowMessage("ストレージの魚をすべて配置してください！");
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
        StartCoroutine(ChangeAquaSlider(false));
        NextPlayerTurn();
        }
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

        nextPlayer = players[currentPlayerIndex];

        if (nextPlayer.isGoal)
        {
            NextPlayerTurn();
            return;
        }

        //ボタンを押せないように元に戻す
        canSelct = false;
        confirmButton.SetActive(false);

        currentPart = Part.gallery;

        HighlightMovableTiles(nextPlayer.currentGalleryTile, galleryTiles);
        Debug.Log(nextPlayer.pData.playerName + "のターン");
    }

    // 一番後ろのプレイヤーのインデックス番号を返す
    private int GetLastPlayerIndex()
    {
        int minIndex = 0;
        int minTile = 19;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].currentGalleryTile.index == 0)
            {
                continue;
            }
            else if (players[i].currentGalleryTile.index <= minTile)
            {
                minTile = players[i].currentGalleryTile.index;
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
                    foreach (PlayerController p in players)
                    {
                        if (p != players[currentPlayerIndex] && p.currentGalleryTile == t)
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
            if (players[currentPlayerIndex].currentGalleryTile.index >= 1)
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
        Debug.Log("ラウンド終了！");

        for (int i = 0; i < finishOrder.Count; i++)
        {
            Debug.Log((i + 1) + "位" + finishOrder[i].pData.playerName);
        }

        //ゴール順に並び変え
        players = finishOrder.ToArray();

        //指定回数ラウンドを繰り返したらゲーム終了
        if (roundCount >= maxRound)
        {
            Debug.Log("ゲーム終了");
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
    }


    //水槽酸素量を表示する
    IEnumerator ChangeAquaSlider(bool show)
    {
        yield return new WaitForSeconds(moveCamTime);
        if (show)
        {
            aquaText[currentPlayerIndex].SetActive(true);
        }
        else
        {
            for (int i = 0; i < players.Length; i++)
            {
                aquaText[i].SetActive(false);
            }
        }
    }


}
