using System.Collections;
using System.Linq;
using UnityEngine;

public class AquaPieceManager : MonoBehaviour
{
    [SerializeField] GameObject piecePrefab;
    [SerializeField] UIController uiController;
    [SerializeField] SeaBoard seaBoard;
    public static GameObject selectedPiece;
    public PhaseManager phaseManager;
    SoundManager soundManager;

    private void Start()
    {
        soundManager = SoundManager.instance;
    }

    public void SelectedPiece(GameObject selected)
    {
        selectedPiece = selected;

        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();
        current.playerPanel.AbledCancel(true);
    }

    public void CanselSelect()
    {
        selectedPiece.transform.localScale = new Vector2(1.5f, 1.5f);
        selectedPiece = null;

        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();
        current.playerPanel.AbledCancel(false);
        current.aquariumBoard.storage.GetComponent<Storage>().Invoke("CheckSpotEmpty", 0.1f);
    }

    //魚駒を現在のプレイヤーのストレージに生成
    public void CreatePiece(PieceData pieceData, int amount = 0, bool isFromSea = false)
    {
        PlayerManager current = TurnManager.currentPlayer.GetComponent<PlayerManager>();

        (GameObject spot, int index) = current.aquariumBoard.storage.GetComponent<Storage>().Instorage();
        if (spot != null)
        {
            GameObject piece = Instantiate(
                piecePrefab,
                spot.transform.position,
                Quaternion.identity
            );

            if (piece != null)
            {
                AquaPiece aquaPiece = piece.GetComponent<AquaPiece>();
                AquaPieceController pieceController = piece.GetComponent<AquaPieceController>();

                aquaPiece.pieceData = pieceData;
                aquaPiece.isiFromSea = isFromSea;
                aquaPiece.storageIndex = index;
                aquaPiece.iscurrentTurn = true;
                pieceController.playerManager = current;
                pieceController.aquaPieceManager = this;
                current.money -= amount;
            }
        }

    }

    //海ボードへの移動
    public void MoveToSeaBoard()
    {
        StartCoroutine(SelectCoroutine());
    }

    //確定ボタンを押すまで停止
    IEnumerator SelectCoroutine()
    {
        uiController.ShowAttentionPanel("海ボードに移動させますか？", selectedPiece.GetComponent<AquaPiece>().pieceData.pieceSprite);

        while (UIController.isActiveUI)
        {
            yield return null;
        }

        //確定ボタンが押されたら
        if (uiController.isOK)
        {
            PlayerManager player = TurnManager.currentPlayer.GetComponent<PlayerManager>();
            GameObject currentPos = selectedPiece.GetComponent<AquaPiece>().currentPos;
            //水槽からだったら
            if (currentPos != null && currentPos.CompareTag("AquaSlot"))
            {
                AquaSlot aquaSlot = currentPos.GetComponent<AquaSlot>();
                PieceData.PieceName name = selectedPiece.GetComponent<AquaPiece>().pieceData.pieceName;
                PieceData.PieceName[] names = new PieceData.PieceName[aquaSlot.slotPieces.Count];
                for (int i = 0; i < names.Length; i++)
                {
                    names[i] = aquaSlot.slotPieces[i].GetComponent<AquaPiece>().pieceData.pieceName;
                }
                //サメ、ジンベエザメ
                if (name == PieceData.PieceName.Shark || name == PieceData.PieceName.WhaleShark)
                {
                    //コバンザメ
                    if (names.Contains(PieceData.PieceName.Remora))
                    {
                        Debug.Log("水槽内にサメが必要です");
                        ShowMessage("水槽内にサメが必要です");
                        soundManager.PlaySE(SoundManager.SE_Type.ng);

                        CanselSelect();
                        yield break;
                    }
                }
                //海藻
                if (name == PieceData.PieceName.Seaweed)
                {
                    //海藻を抜いて水槽内の酸素量が0未満ならNG
                    int oxygen = aquaSlot.slotOxygen - selectedPiece.GetComponent<AquaPiece>().pieceData.oxygen;
                    if (oxygen < 0)
                    {
                        Debug.Log("水槽の酸素量が足りなくなります");
                        ShowMessage("水槽の酸素量が足りなくなります");
                        soundManager.PlaySE(SoundManager.SE_Type.ng);

                        CanselSelect();
                        yield break;
                    }
                    //ウミガメ
                    if (names.Contains(PieceData.PieceName.SeaTurtle))
                    {
                        Debug.Log("水槽内に海藻が必要です");
                        ShowMessage("水槽内に海藻が必要です");
                        soundManager.PlaySE(SoundManager.SE_Type.ng);

                        CanselSelect();
                        yield break;
                    }
                }
                aquaSlot.ReleasePiece();
            }
            //ストレージからだったら
            else
            {
                player.aquariumBoard.storage.GetComponent<Storage>().ReleasePiece();
            }
            //海からだったら
            if (selectedPiece.GetComponent<AquaPiece>().isiFromSea)
            {
                player.money += 2;
            }

            soundManager.PlaySE(SoundManager.SE_Type.click);
            seaBoard.ReleasePiece(selectedPiece);
            Destroy(selectedPiece);
        }
        CanselSelect();
    }

    void ShowMessage(string message)
    {
        UIController.messageText.text = message;
        UIController.isMessageChanged = true;
    }
}
