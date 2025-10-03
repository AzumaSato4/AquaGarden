using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerData playerData; //プレイヤー情報
    GalleryBoard galleryBoard; //ギャラリータイル情報を取得するための変数
    public AquariumBoard aquariumBoard; //水族館ボード情報を取得するための変数
    public int money; //所持資金

    GameObject aquarium;        //水族館ボード
    GameObject galleryPlayer;   //ギャラリーのプレイヤー駒
    GameObject aquariumPlayer;  //水族館のプレイヤー駒

    GameObject currentGalleryTile;  //現在のギャラリーマス
    public int galleryIndex;        //現在のギャラリーマス番号
    GameObject currentAquariumTile; //現在の水族館マス
    int aquariumIndex;              //現在の水族館マス番号

    TurnManager turnManager;    //TurnManagerを格納するための変数
    public PhaseManager phaseManager;  //PhaseManagerを格納するための変数
    public AquaPieceManager aquaPieceManager;  //PieceManagerを格納するための変数

    public bool isActive = false;   //自分のターンかどうか
    public bool isGoal = false;     //ゴールしたかどうか

    int another;    //選択可能なもう一つの水槽を記録するための変数

    //テスト用
    [SerializeField] PieceData pieceData;

    private void Start()
    {
        //プレイヤーの駒と水族館ボードを生成
        aquarium = Instantiate(playerData.aquarium, new Vector3(playerData.playerNum * 20, 0, 0), Quaternion.identity);
        galleryPlayer = Instantiate(playerData.galleryPlayer);
        aquariumPlayer = Instantiate(playerData.aquariumPlayer);

        //プレイヤー駒の画像をセット
        galleryPlayer.GetComponent<SpriteRenderer>().sprite = playerData.gallerySprite;
        aquariumPlayer.GetComponent<SpriteRenderer>().sprite = playerData.aquariumSprite;

        //playerManagerに自分自身をセット
        galleryPlayer.GetComponent<GalleryPlayerController>().playerManager = this;
        aquariumPlayer.GetComponent<AquariumPlayerController>().playerManager = this;

        //ギャラリーと水族館の情報を取得
        galleryBoard = GameObject.Find("Gallery").GetComponent<GalleryBoard>();
        aquariumBoard = aquarium.GetComponent<AquariumBoard>();

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
        aquariumBoard.coin.transform.position = aquariumBoard.CoinSpots[money].transform.position;
    }

    public void StartGallery()
    {
        if (isActive)
        {
            galleryPlayer.GetComponent<GalleryPlayerController>().movedGallery = false;
        }
    }

    public void MoveGallery(int to)
    {
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

        phaseManager.EndGallery(playerData);
        StartAquarium();
    }

    public void StartAquarium()
    {
        if (isActive)
        {
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
            GetPiece();
        }
    }

    public void MoveAquarium(int to)
    {
        aquariumBoard.isPlayer[aquariumIndex] = false;
        aquariumIndex = to;
        aquariumBoard.isPlayer[aquariumIndex] = true;

        for (int i = 0; i < 6; i++)
        {
            aquariumBoard.aquaTiles[i].GetComponent<PolygonCollider2D>().enabled = false;
        }

        EditAquarium();
        phaseManager.MovedAquarium(playerData);
    }

    void EditAquarium()
    {
        another = aquariumIndex - 1;
        if (another < 0)
        {
            another = 5;
            Debug.Log("マイナス！");
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
        foreach (GameObject slot in aquariumBoard.aquaSlots)
        {
            slot.GetComponent<AquaSlot>().selectable = false;
            slot.GetComponent<AquaSlot>().mask.SetActive(false);
        }
    }

    public void GetPiece()
    {
        for (int i = 0; i < 8; i++)
        {
            aquaPieceManager.CreatePiece(this,pieceData, aquariumBoard.storage);
        }
    }
}
