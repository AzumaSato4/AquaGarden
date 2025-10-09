using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Player player; //プレイヤー情報
    GalleryBoard galleryBoard; //ギャラリータイル情報を取得するための変数
    public AquariumBoard aquariumBoard; //水族館ボード情報を取得するための変数
    public int money; //所持資金
    int preMoney; //ひとつ前時点の所持資金
    public FeedingData feedingData; //餌やりイベント情報

    [SerializeField] GameObject galleryPlayer;   //ギャラリーのプレイヤー駒
    [SerializeField] GameObject aquariumPlayer;  //水族館のプレイヤー駒

    GameObject currentGalleryTile;  //現在のギャラリーマス
    public int galleryIndex;        //現在のギャラリーマス番号
    GameObject currentAquariumTile; //現在の水族館マス
    int aquariumIndex;              //現在の水族館マス番号
    [SerializeField] GameObject aquariumCanvas; //自分の水族館専用キャンバス
    [SerializeField] GameObject turnEnd;  //ターンエンドボタン
    [SerializeField] GameObject cancel; //キャンセルボタン
    TextMeshProUGUI moneyCountText; //所持資金テキスト

    TurnManager turnManager;    //TurnManagerを格納するための変数
    public PhaseManager phaseManager;  //PhaseManagerを格納するための変数
    public AquaPieceManager aquaPieceManager;  //PieceManagerを格納するための変数

    public bool isActive = false;   //自分のターンかどうか
    public bool isGoal = false;     //ゴールしたかどうか

    int another;    //選択可能なもう一つの水槽を記録するための変数


    private void Start()
    {
        //プレイヤー駒の画像をセット
        galleryPlayer.GetComponent<SpriteRenderer>().sprite = player.gallerySprite;
        aquariumPlayer.GetComponent<SpriteRenderer>().sprite = player.aquariumSprite;

        //playerManagerに自分自身をセット
        galleryPlayer.GetComponent<GalleryPlayerController>().playerManager = this;
        aquariumPlayer.GetComponent<AquariumPlayerController>().playerManager = this;

        //ギャラリーの情報を取得
        galleryBoard = GameObject.Find("Gallery").GetComponent<GalleryBoard>();

        //ターンエンドボタンとキャンセルボタンを最初は選択不可にする
        turnEnd.GetComponent<Button>().interactable = false;
        cancel.GetComponent<Button>().interactable = false;

        //TurnManagerとPhaseManagerを取得
        GameObject manager = GameObject.Find("MainManager");
        turnManager = manager.GetComponent<TurnManager>();
        phaseManager = manager.GetComponent<PhaseManager>();

        //初期位置にセット
        //ギャラリースタート位置にセット
        galleryIndex = player.playerNum - 1;
        currentGalleryTile = galleryBoard.startSpots[galleryIndex];
        galleryPlayer.transform.position = currentGalleryTile.transform.position;
        galleryBoard.isPlayer[galleryIndex] = true;
        //水族館スタート位置にセット
        aquariumIndex = 0;
        currentAquariumTile = aquariumBoard.aquaTiles[aquariumIndex];
        aquariumPlayer.transform.position = currentAquariumTile.transform.position;
        aquariumBoard.isPlayer[aquariumIndex] = true;
        //コインの初期位置は2
        money = 2;
        preMoney = 2;
        aquariumBoard.coin.transform.position = aquariumBoard.CoinSpots[money].transform.position;
        moneyCountText = GameObject.Find("MoneyCountText").GetComponent<TextMeshProUGUI>();
        //UIは最初は消す
        aquariumCanvas.SetActive(false);
    }

    private void Update()
    {
        if (preMoney != money)
        {
            if (money > 15) money = 15;
            aquariumBoard.coin.transform.position = aquariumBoard.CoinSpots[money].transform.position;
            preMoney = money;
        }

        if (isActive) moneyCountText.text = money.ToString();
    }

    public void StartGallery()
    {
        if (isActive)
        {
            galleryPlayer.GetComponent<GalleryPlayerController>().movedGallery = false;
            galleryPlayer.GetComponent<Animator>().enabled = true;
        }
    }

    public void MoveGallery(int to, string tile)
    {
        galleryPlayer.GetComponent<Animator>().enabled = false;
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

        if (tile != null)
        {
            if (tile == "FishTile")
            {
                Debug.Log("魚マス");

                FishTile fishTile = galleryBoard.galleryTiles[to - 4].GetComponent<FishTile>();
                StartCoroutine(GetPieceCoroutine(fishTile));

                phaseManager.EndGallery(player);
                StartAquarium();
            }
            else if (tile == "AdTile")
            {
                Debug.Log("広告マス");
                StartAd();
            }
        }
    }

    void StartAd()
    {
        phaseManager.StartAd(player);
    }

    public void EndAd()
    {
        phaseManager.EndAd(player);
        turnManager.EndTurn();
    }

    public void StartAquarium()
    {
        if (isActive)
        {
            aquariumCanvas.SetActive(true);

            aquariumPlayer.GetComponent<Animator>().enabled = true;
            aquariumPlayer.GetComponent<AquariumPlayerController>().movedAquarium = false;
            for (int i = 1; i < 4; i++)
            {
                int movableTiles = aquariumIndex + i;
                if (movableTiles > 5)
                {
                    movableTiles -= 6;
                }
                aquariumBoard.aquaTiles[movableTiles].GetComponent<PolygonCollider2D>().enabled = true;
            }
        }
    }

    public void MoveAquarium(int to)
    {
        aquariumPlayer.GetComponent<Animator>().enabled = false;
        aquariumPlayer.transform.localScale = Vector2.one;

        bool isFeeding = false;

        int moveTiles = to - aquariumIndex;
        if (moveTiles < 0) moveTiles += 6;

        for (int i = 0; i < moveTiles; i++)
        {
            int indexA = aquariumIndex + i;
            if (indexA > 5) indexA -= 6;
            int indexB = indexA + 1;

            if (indexA == 0 && indexB == 1)
            {
                isFeeding = true;
            }
        }

        aquariumBoard.isPlayer[aquariumIndex] = false;
        aquariumIndex = to;
        aquariumBoard.isPlayer[aquariumIndex] = true;

        for (int i = 0; i < 6; i++)
        {
            aquariumBoard.aquaTiles[i].GetComponent<PolygonCollider2D>().enabled = false;
        }

        if (isFeeding)
        {
            phaseManager.StartFeeding(player);
            StartFeeding();
            return;
        }
        else
        {
            EditAquarium();
            return;
        }
    }

    //餌やりイベント
    void StartFeeding()
    {
        //無条件で資金を1追加
        money++;

        int countA = 0;
        int countB = 0;
        foreach (GameObject piece in aquariumBoard.aquaSlots[0].GetComponent<AquaSlot>().slotPieces)
        {
            PieceData.PieceName name = piece.GetComponent<AquaPiece>().pieceData.pieceName;
            if (name == feedingData.nameA)
            {
                Debug.Log("発見A");
                countA++;
            }
            if (name == feedingData.nameB)
            {
                Debug.Log("発見B");
                countB++;
            }
        }
        foreach (GameObject piece in aquariumBoard.aquaSlots[1].GetComponent<AquaSlot>().slotPieces)
        {
            PieceData.PieceName name = piece.GetComponent<AquaPiece>().pieceData.pieceName;
            if (name == feedingData.nameA)
            {
                Debug.Log("発見A2");
                countA++;
            }
            if (name == feedingData.nameB)
            {
                Debug.Log("発見B2");
                countB++;
            }
        }

        while (countA > 0 && countB > 0)
        {
            money++;

            countA--;
            countB--;
        }

        Debug.Log("終了");
        EditAquarium();
        phaseManager.EndFeeding(player);
    }

    //水族館編集
    void EditAquarium()
    {
        phaseManager.MovedAquarium(player);
        AbledTurnEnd(true);

        another = aquariumIndex - 1;
        if (another < 0)
        {
            another = 5;
        }
        for (int i = 0; i < 6; i++)
        {
            if (i == aquariumIndex || i == another)
            {
                aquariumBoard.aquaSlots[i].GetComponent<AquaSlot>().mask.SetActive(false);
            }
            else
            {
                aquariumBoard.aquaSlots[i].GetComponent<AquaSlot>().mask.SetActive(true);
            }
        }
    }

    public void AdEditAquarium()
    {
        phaseManager.StartAdEdit(player);
        AbledTurnEnd(true);

        if (isActive)
        {
            for (int i = 0; i < 6; i++)
            {
                AquaSlot aquaSlot = aquariumBoard.aquaSlots[i].GetComponent<AquaSlot>();
                aquaSlot.mask.SetActive(false);
                aquaSlot.selectable = true;
            }
            aquariumCanvas.SetActive(true);
        }
    }

    public void SelectSlot()
    {
        aquariumBoard.aquaSlots[aquariumIndex].GetComponent<AquaSlot>().selectable = true;
        aquariumBoard.aquaSlots[another].GetComponent<AquaSlot>().selectable = true;
    }

    public void DontSelectSlot()
    {
        aquariumBoard.aquaSlots[aquariumIndex].GetComponent<AquaSlot>().selectable = false;
        aquariumBoard.aquaSlots[another].GetComponent<AquaSlot>().selectable = false;
    }

    public void EndAquarium()
    {
        if (!aquariumBoard.storage.GetComponent<Storage>().isEmpty)
        {
            Debug.Log("ストレージを空にしてください！");
            UIController.messageText.text = "ストレージを空にしてください！";
            UIController.isMessageChanged = true;
            return;
        }

        foreach (GameObject slot in aquariumBoard.aquaSlots)
        {
            slot.GetComponent<AquaSlot>().selectable = false;
            slot.GetComponent<AquaSlot>().mask.SetActive(false);
        }

        AbledTurnEnd(false);
        AbledCancel(false);
        aquariumCanvas.SetActive(false);
        turnManager.EndTurn();
    }

    IEnumerator GetPieceCoroutine(FishTile tile)
    {
        for (int i = 0; i < tile.pieces.Count; i++)
        {
            PieceData piece = tile.pieces[i].GetComponent<GalleryPiece>().pieceData;
            Destroy(tile.pieces[i]);
            yield return new WaitForSeconds(0.1f);
            aquaPieceManager.CreatePiece(piece);
        }
        tile.pieces.Clear();
    }

    public void OnCancelButton()
    {
        aquaPieceManager.CanselSelect();
    }

    public void AbledTurnEnd(bool isAble)
    {
        turnEnd.GetComponent<Button>().interactable = isAble;
    }

    public void AbledCancel(bool isAble)
    {
        cancel.GetComponent<Button>().interactable = isAble;
    }

    public int GetScore()
    {
        int score = 0;
        List<PieceData.PieceName> totalPiece = new List<PieceData.PieceName>();

        for (int i = 0; i < aquariumBoard.aquaSlots.Length; i++)
        {
            //1つの水槽内のすべての魚駒のリスト
            List<PieceData.PieceName> pieces = new List<PieceData.PieceName>(aquariumBoard.aquaSlots[i].GetComponent<AquaSlot>().GetSlotPiece());
            //1つの水槽内の魚駒の種類
            List<PieceData.PieceName> type = new List<PieceData.PieceName>(pieces.Distinct());
            //海藻とサンゴは種類から除外
            if (type.Contains(PieceData.PieceName.Seaweed)) type.Remove(PieceData.PieceName.Seaweed);
            if (type.Contains(PieceData.PieceName.Coral)) type.Remove(PieceData.PieceName.Coral);

            //すべての水槽内の魚駒リストに追加
            totalPiece.AddRange(pieces);

            //水槽別で計算
            //小型魚と大型魚
            if (pieces.Contains(PieceData.PieceName.SmallFish))
            {
                if (pieces.Contains(PieceData.PieceName.LargeFish))
                {
                    int countS = pieces.Count(item => item == PieceData.PieceName.SmallFish);
                    int countL = pieces.Count(item => item == PieceData.PieceName.LargeFish);

                    while (countS > 0 && countL > 0)
                    {
                        score += 3;
                        countS--;
                        countL--;
                    }
                }
            }
            //ウミガメ
            if (pieces.Contains(PieceData.PieceName.SeaTurtle))
            {
                int count = pieces.Count(item => item == PieceData.PieceName.SeaTurtle);

                score += (count * 2);
            }
            //タツノオトシゴ
            if (pieces.Contains(PieceData.PieceName.Seahorse))
            {
                int count = pieces.Count(item => item == PieceData.PieceName.Seahorse);

                score += (count * 2);
            }
            //サメ
            if (pieces.Contains(PieceData.PieceName.Shark))
            {
                int count = pieces.Count(item => item == PieceData.PieceName.Shark);

                score += (count * 2);
            }
            //ジンベエザメ
            if (pieces.Contains(PieceData.PieceName.WhaleShark))
            {
                int count = pieces.Count(item => item == PieceData.PieceName.WhaleShark);

                score += (count * 4);
            }
            //サンゴ
            if (pieces.Contains(PieceData.PieceName.Coral))
            {
                int count = pieces.Count(item => item == PieceData.PieceName.Coral);

                score += (count * 1);
            }
            //メンダコ
            if (pieces.Contains(PieceData.PieceName.Flapjack))
            {
                int count = pieces.Count(item => item == PieceData.PieceName.Flapjack);
                score += type.Count * count;
            }
            //コバンザメ
            if (pieces.Contains(PieceData.PieceName.Remora))
            {
                int count = pieces.Count(item => item == PieceData.PieceName.Remora);
                int countS = pieces.Count(item => item == PieceData.PieceName.Shark);
                int countW = pieces.Count(item => item == PieceData.PieceName.WhaleShark);

                if (countS > 0 && countW > 0) score += 4 * count;
                else if (countS > 0 || countW > 0) score += 2 * count;
            }
        }

        //水槽全体で計算
        //マンタ
        if (totalPiece.Contains(PieceData.PieceName.Manta))
        {
            int count = totalPiece.Count(item => item == PieceData.PieceName.Manta);
            //各要素の出現回数をカウントする,出現回数が最大の要素を取得する,最頻値を取得する
            int modeCount = totalPiece.GroupBy(x => x).OrderByDescending(g => g.Count()).First().Count();
            score += modeCount * count;
        }

        //資金3つで1点
        score += money / 3;

        //最終ラウンドでのゴール順によって得点
        switch (galleryIndex)
        {
            case 0:
                score += 3;
                break;
            case 1:
                score += 2;
                break;
            case 2:
                score += 1;
                break;
            case 3:
                score += 0;
                break;
        }

        return score;
    }
}
