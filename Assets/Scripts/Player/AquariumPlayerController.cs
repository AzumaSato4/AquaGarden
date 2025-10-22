using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class AquariumPlayerController : MonoBehaviour
{
    public PlayerManager playerManager;

    int clickCount;
    float moveTime = 0.3f; //移動アニメーションの時間
    int currentIndex = 0; //移動完了前の現在のインデックス番号
    int getMoney; //エサやりイベントで得た資金
    bool isGetMoney; //エサやりイベントで得た資金があるかどうか

    GameObject selected;
    SoundManager soundManager;

    private void Start()
    {
        soundManager = SoundManager.instance;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() || UIController.isActiveUI)
        {
            return;
        }

        if (playerManager.isActive && PhaseManager.currentPhase == PhaseManager.Phase.aquarium)
        {
            Vector3 mousePos = Input.mousePosition;
            //マウス座標が無限値・NaNならスキップ
            if (float.IsNaN(mousePos.x) || float.IsNaN(mousePos.y) ||
                float.IsInfinity(mousePos.x) || float.IsInfinity(mousePos.y))
            {
                return;
            }
            //画面外チェック (負の値や画面解像度を超えた場合)
            if (mousePos.x < 0 || mousePos.y < 0 ||
                mousePos.x > Screen.width || mousePos.y > Screen.height)
            {
                return;
            }

            //カーソルの位置を取得
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 1.0f);

            //マウスクリックしたら
            if (Input.GetMouseButtonDown(0))
            {
                //クリックしたオブジェクトを取得
                if (hit.collider == null) return;
                selected = hit.collider.gameObject;
                //何かオブジェクトをクリックしたら
                if (selected == null) return;

                if (UnityEngine.Device.Application.isMobilePlatform)
                {
                    clickCount++;
                    Invoke("CheckDouble", 0.4f);
                }
                else
                {
                    Move();
                }
            }
        }
    }

    void CheckDouble()
    {
        if (clickCount != 2)
        {
            clickCount = 0;
            selected = null;
            return;
        }
        else
        {
            clickCount = 0;
            Move();
        }
    }

    //水族館のクリックしたマスに移動する
    void Move()
    {
        if (!selected.CompareTag("AquariumTile")) return;

        int moveToIndex = selected.GetComponent<TileManager>().tileIndex;
        int nextIndex = currentIndex;

        nextIndex++;
        if (nextIndex >= playerManager.aquariumBoard.aquaTiles.Length)
        {
            nextIndex -= playerManager.aquariumBoard.aquaTiles.Length;
        }

        //1マスずつ移動
        OneStepForward(nextIndex, moveToIndex);
        playerManager.totalSteps += moveToIndex - currentIndex;
        if (playerManager.totalSteps < 0)
        {
            playerManager.totalSteps += playerManager.aquariumBoard.aquaTiles.Length;
        }
    }

    //前に進む
    void OneStepForward(int nextIndex, int moveToIndex)
    {
        playerManager.isMoveing = true;
        GameObject next = playerManager.aquariumBoard.aquaTiles[nextIndex];
        //DoTweenで移動アニメーション
        transform.DOMove(next.transform.position, moveTime).OnComplete(() =>
        {
            soundManager.PlaySE(SoundManager.SE_Type.click);
            //エサやりイベントチェック
            CheckFeeding(nextIndex);

            //移動したら記録する
            currentIndex = nextIndex;
            //目的タイルまで繰り返す
            if (nextIndex != moveToIndex)
            {
                nextIndex++;
                if (nextIndex >= playerManager.aquariumBoard.aquaTiles.Length)
                {
                    nextIndex -= playerManager.aquariumBoard.aquaTiles.Length;
                }
                OneStepForward(nextIndex, moveToIndex);
            }
            else
            {
                playerManager.playerPanel.AbledMoved(true);
                playerManager.playerPanel.AbledCancelMove(true);
                playerManager.isMovedAquarium = true;
                playerManager.isMoveing = false;
            }
        });
    }

    //移動キャンセルボタン
    public void OnMoveBack()
    {
        MoveBack();
    }

    //移動キャンセル
    void MoveBack()
    {
        int moveToIndex = playerManager.aquariumIndex;
        int nextIndex = currentIndex;

        nextIndex--;
        if (nextIndex < 0)
        {
            nextIndex += playerManager.aquariumBoard.aquaTiles.Length;
        }

        playerManager.totalSteps = 0;
        RePayMoveMoney();
        OneStepBack(nextIndex, moveToIndex);
    }

    //追加移動の資金を返金
    void RePayMoveMoney()
    {
        PlusMoveButton plusMoveButton = playerManager.plusMovePanel.GetComponent<PlusMoveButton>();
        switch (playerManager.steps)
        {
            case 4:
                plusMoveButton.MinusMove();
                break;
            case 5:
                plusMoveButton.MinusMove();
                plusMoveButton.MinusMove();
                break;
        }
    }

    //後ろに進む
    void OneStepBack(int nextIndex, int moveToIndex)
    {
        playerManager.isMoveing = true;
        GameObject next = playerManager.aquariumBoard.aquaTiles[nextIndex];
        //DoTweenで移動アニメーション
        transform.DOMove(next.transform.position, moveTime).OnComplete(() =>
        {
            soundManager.PlaySE(SoundManager.SE_Type.click);
            //エサやりイベントチェック
            CheckFeeding(nextIndex);

            //移動したら記録する
            currentIndex = nextIndex;
            //目的タイルまで繰り返す
            if (nextIndex != moveToIndex)
            {
                nextIndex--;
                if (nextIndex < 0)
                {
                    nextIndex += playerManager.aquariumBoard.aquaTiles.Length;
                }
                OneStepBack(nextIndex, moveToIndex);
            }
            else
            {
                playerManager.playerPanel.AbledMoved(false);
                playerManager.playerPanel.AbledCancelMove(false);
                playerManager.isMovedAquarium = false;
                playerManager.isMoveing = false;
            }
        });
    }

    //エサやりイベントチェック
    void CheckFeeding(int nextIndex)
    {
        if (currentIndex == 0 && nextIndex == 1 && !isGetMoney)
        {
            StartFeeding();
        }
        else if (currentIndex == 1 && nextIndex == 0 && isGetMoney)
        {
            CancelFeeding();
        }
    }

    //エサやりイベント実行
    void StartFeeding()
    {
        //無条件で資金を1追加
        getMoney++;

        int countA = 0;
        int countB = 0;
        foreach (GameObject piece in playerManager.aquariumBoard.aquaSlots[0].GetComponent<AquaSlot>().slotPieces)
        {
            PieceData.PieceName name = piece.GetComponent<AquaPiece>().pieceData.pieceName;
            if (name == playerManager.feedingData.nameA)
            {
                countA++;
            }
            if (name == playerManager.feedingData.nameB)
            {
                countB++;
            }
        }
        foreach (GameObject piece in playerManager.aquariumBoard.aquaSlots[1].GetComponent<AquaSlot>().slotPieces)
        {
            PieceData.PieceName name = piece.GetComponent<AquaPiece>().pieceData.pieceName;
            if (name == playerManager.feedingData.nameA)
            {
                countA++;
            }
            if (name == playerManager.feedingData.nameB)
            {
                countB++;
            }
        }

        while (countA > 0 && countB > 0)
        {
            getMoney++;

            countA--;
            countB--;
        }

        playerManager.money += getMoney;
        soundManager.PlaySE(SoundManager.SE_Type.getMoney);
        isGetMoney = true;
    }

    //エサやりイベントをキャンセル
    void CancelFeeding()
    {
        playerManager.money -= getMoney;
        getMoney = 0;
        isGetMoney = false;
    }


    //移動完了ボタン
    public void OnMovedButton()
    {
        int moveTiles = currentIndex - playerManager.aquariumIndex;
        if (moveTiles < 0) moveTiles += 6;
        //進みすぎていないかチェック
        if (!CheckMoved(moveTiles))
        {
            soundManager.PlaySE(SoundManager.SE_Type.ng);
            ShowMessage("そのマスには止まれません");
            return;
        }

        soundManager.PlaySE(SoundManager.SE_Type.click);
        playerManager.MoveAquarium(currentIndex);
        selected = null;
        getMoney = 0;
        isGetMoney = false;
        playerManager.totalSteps = 0;
    }

    //そのマスが止まれるかどうかチェック
    bool CheckMoved(int moveTiles)
    {
        bool isOK = false;

        if (moveTiles <= playerManager.steps)
        {
            isOK = true;
        }

        return isOK;
    }

    //メッセージを表示
    void ShowMessage(string message)
    {
        UIController.messageText.text = message;
        UIController.isMessageChanged = true;
    }
}
