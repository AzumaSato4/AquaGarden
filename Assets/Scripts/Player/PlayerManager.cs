using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public PlayerData playerData; //プレイヤー情報
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

    TurnManager turnManager;    //TurnManagerを格納するための変数
    public PhaseManager phaseManager;  //PhaseManagerを格納するための変数
    public AquaPieceManager aquaPieceManager;  //PieceManagerを格納するための変数

    public bool isActive = false;   //自分のターンかどうか
    public bool isGoal = false;     //ゴールしたかどうか

    int another;    //選択可能なもう一つの水槽を記録するための変数


    private void Start()
    {
        //プレイヤー駒の画像をセット
        galleryPlayer.GetComponent<SpriteRenderer>().sprite = playerData.gallerySprite;
        aquariumPlayer.GetComponent<SpriteRenderer>().sprite = playerData.aquariumSprite;

        //playerManagerに自分自身をセット
        galleryPlayer.GetComponent<GalleryPlayerController>().playerManager = this;
        aquariumPlayer.GetComponent<AquariumPlayerController>().playerManager = this;

        //ギャラリーの情報を取得
        galleryBoard = GameObject.Find("Gallery").GetComponent<GalleryBoard>();

        //ターンエンドボタンとキャンセルボタンを最初は選択不可にする
        turnEnd.GetComponent<Button>().interactable = false;
        cancel.GetComponent<Button>().interactable = false;

        //TurnManagerとPhaseManagerを取得
        turnManager = GameObject.Find("MainManager").GetComponent<TurnManager>();
        phaseManager = GameObject.Find("MainManager").GetComponent<PhaseManager>();

        //初期位置にセット
        //ギャラリースタート位置にセット
        galleryIndex = playerData.playerNum - 1;
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

                phaseManager.EndGallery(playerData);
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
        phaseManager.StartAd(playerData);
    }

    public void EndAd()
    {
        phaseManager.EndAd(playerData);
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
            phaseManager.StartFeeding(playerData);
            StartFeeding();
            return;
        }
        else
        {
            EditAquarium();
            phaseManager.MovedAquarium(playerData);
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
            string name = piece.GetComponent<AquaPiece>().pieceData.pieceName;
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
            string name = piece.GetComponent<AquaPiece>().pieceData.pieceName;
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
        phaseManager.EndFeeding(playerData);
    }

    //水族館編集
    void EditAquarium()
    {
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
            aquaPieceManager.CreatePiece(piece);
            yield return null;
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
}
