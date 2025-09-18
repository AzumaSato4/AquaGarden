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

    public Part currentPart = Part.gallery;


    public PlayerController[] players;
    public Tile[] galleryTiles;

    public GameObject galleryCam;   //ギャラリーボードを映すためのカメラ
    public float moveCamTime;

    PlayerController currentPlayer;
    PlayerController nextPlayer;

    public int maxRound;

    int turnCount;
    int roundCount;
    public int currentPlayerIndex = 0;

    //移動可能なマスを絞るためのリスト
    List<Tile> movableTiles = new List<Tile>();
    //ゴールしたプレイヤーを確認するリスト
    List<PlayerController> finishOrder = new List<PlayerController>();

    private void Start()
    {
        roundCount = 0;

        StartRound();

    }


    //ラウンドスタート
    void StartRound()
    {
        currentPart = Part.gallery;
        galleryCam.SetActive(true);

        turnCount = 0;
        currentPlayerIndex = 0;
        finishOrder.Clear();
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


        StartCoroutine(ChangeCamera(galleryCam, currentPlayer.aquariumCam));
        //水族館ターンへ
        currentPart = Part.aquarium;
        HighlightMovableTiles(currentPlayer.currentAquaTile, currentPlayer.aquariumTiles);
    }


    //水族館パート
    //マスが選択されたらそのマスへ移動
    public void OnAquaTileClicked(Tile clickedTile)
    {
        if (currentPart != Part.aquarium) return;
        if (!movableTiles.Contains(clickedTile)) return;

        currentPlayer.MoveToAquaTile(clickedTile);


        //次のプレイヤーのターンへ
        NextPlayerTurn();

    }


    //次のターンへ
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

        currentPart = Part.gallery;
        StartCoroutine(ChangeCamera(currentPlayer.aquariumCam, galleryCam));

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


    IEnumerator ChangeCamera(GameObject from, GameObject to)
    {
        yield return new WaitForSeconds(moveCamTime);
        from.SetActive(false);
        to.SetActive(true);
    }


}
