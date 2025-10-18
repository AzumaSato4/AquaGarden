using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class AquaPieceController : MonoBehaviour
{
    public PlayerManager playerManager;
    public AquaPieceManager aquaPieceManager;

    AquaPiece aquaPiece;
    SEManager seManager;

    private void Start()
    {
        aquaPiece = GetComponent<AquaPiece>();
        seManager = SEManager.instance;
    }

    private void LateUpdate()
    {
        if (AquaPieceManager.selectedPiece != null)
        {
            gameObject.layer = 2;
        }
        else
        {
            gameObject.layer = 0;
        }

        if (EventSystem.current.IsPointerOverGameObject() || UIController.isActiveUI)
        {
            return;
        }

        if (playerManager.isActive && (PhaseManager.currentPhase == PhaseManager.Phase.edit || PhaseManager.currentPhase == PhaseManager.Phase.adEdit || PhaseManager.currentPhase == PhaseManager.Phase.mileEdit) && AquaPieceManager.selectedPiece == this.gameObject)
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

            //マウスクリックしたら
            if (Input.GetMouseButtonDown(0))
            {
                //カーソルの位置を取得
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 10f);
                //クリックしたオブジェクトを取得
                if (hit.collider != null)
                {
                    GameObject target = hit.collider.gameObject;

                    //何かオブジェクトをクリックしていなければ何もしない
                    if (target == null)
                    {
                        aquaPieceManager.CanselSelect();
                        return;
                    }
                    //クリックは水槽限定
                    if (!target.CompareTag("AquaSlot"))
                    {
                        aquaPieceManager.CanselSelect();
                        return;
                    }

                    GameObject current = aquaPiece.currentPos;
                    if (current != null)
                    {
                        if (current == target)
                        {
                            aquaPieceManager.CanselSelect();
                            return;
                        }
                        else if (current.CompareTag("AquaSlot"))
                        {
                            Debug.Log("水槽から水槽には移動できません");
                            ShowMessage("水槽から水槽には移動できません");
                            aquaPieceManager.CanselSelect();
                            return;
                        }
                    }

                    //空き確認
                    AquaSlot slot = target.GetComponent<AquaSlot>();
                    (GameObject to, int index) = slot.CheckSpot();
                    if (to == null)
                    {
                        Debug.Log("水槽がいっぱいです");
                        ShowMessage("水槽がいっぱいです");
                        aquaPieceManager.CanselSelect();
                        return;
                    }
                    //酸素計算
                    if (!CheckOxygen(slot))
                    {
                        aquaPieceManager.CanselSelect();
                        return;
                    }
                    //相性チェック
                    if (!CheckType(slot))
                    {
                        seManager.PlaySE(SEManager.SE_Type.ng);
                        aquaPieceManager.CanselSelect();
                        return;
                    }
                    //水槽への移動確定
                    seManager.PlaySE(SEManager.SE_Type.click);
                    transform.position = to.transform.position;
                    aquaPiece.currentPos = target;
                    transform.parent = slot.transform;
                    slot.slotPieces.Add(AquaPieceManager.selectedPiece);
                    slot.slotOxygen += aquaPiece.pieceData.oxygen;
                    slot.isPiece[index] = true;
                    aquaPiece.isiFromSea = false;
                    TurnManager.currentPlayer.GetComponent<PlayerManager>().aquariumBoard.storage.GetComponent<Storage>().ReleasePiece();
                    aquaPiece.spotIndex = index;

                    //マイルストーンの特別編集中なら水槽を最初に選んだものに限定する
                    if (PhaseManager.currentPhase == PhaseManager.Phase.mileEdit && !playerManager.isMoveMilestone)
                    {
                        foreach (GameObject aquaSlot in playerManager.aquariumBoard.aquaSlots)
                        {
                            AquaSlot aSlot = aquaSlot.GetComponent<AquaSlot>();
                            aSlot.selectable = false;
                            aSlot.mask.SetActive(true);
                        }
                        slot.selectable = true;
                        slot.mask.SetActive(false);
                        playerManager.isMoveMilestone = true;
                    }
                    aquaPieceManager.CanselSelect();
                }
            }
        }
    }

    //水槽内の駒相性チェック
    bool CheckType(AquaSlot slot)
    {
        bool isOK = false;

        PieceData[] pieces = new PieceData[slot.slotPieces.Count];
        PieceData.PieceType[] types = new PieceData.PieceType[slot.slotPieces.Count];
        PieceData.PieceName[] names = new PieceData.PieceName[slot.slotPieces.Count];
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i] = slot.slotPieces[i].GetComponent<AquaPiece>().pieceData;
            types[i] = pieces[i].pieceType;
            names[i] = pieces[i].pieceName;
        }

        //タイプ指定
        PieceData.PieceType type = aquaPiece.pieceData.pieceType;
        switch (type)
        {
            case PieceData.PieceType.fish:
                if (types.Contains(PieceData.PieceType.shark))
                {
                    Debug.Log("サメがいる水槽には入れられません");
                    ShowMessage("サメがいる水槽には入れられません");
                }
                else
                {
                    isOK = true;
                }
                break;
            case PieceData.PieceType.seaTurtle:
                if (types.Contains(PieceData.PieceType.shark))
                {
                    Debug.Log("サメがいる水槽には入れられません");
                    ShowMessage("サメがいる水槽には入れられません");
                }
                else if (!types.Contains(PieceData.PieceType.seaweed))
                {
                    Debug.Log("海藻がない水槽には入れられません");
                    ShowMessage("海藻がない水槽には入れられません");
                }
                else
                {
                    isOK = true;
                }
                break;
            case PieceData.PieceType.shark:
                if (types.Contains(PieceData.PieceType.fish) ||
                    types.Contains(PieceData.PieceType.seaTurtle)
                    )
                {
                    Debug.Log("サメと一緒に水槽に入れられない生き物がいます");
                    ShowMessage("サメと一緒に水槽に入れられない生き物がいます");
                }
                else
                {
                    isOK = true;
                }
                break;
            case PieceData.PieceType.other:
                isOK = true;
                break;
            case PieceData.PieceType.advance:
                if (types.Contains(PieceData.PieceType.advance))
                {
                    Debug.Log("すでに上級駒があります");
                    ShowMessage("すでに上級駒があります");
                }
                else
                {
                    isOK = true;
                }
                break;
            case PieceData.PieceType.seaweed:
                if (types.Contains(PieceData.PieceType.seaweed))
                {
                    Debug.Log("海藻は水槽に1つまでです");
                    ShowMessage("海藻は水槽に1つまでです");
                }
                else
                {
                    isOK = true;
                }
                break;
            case PieceData.PieceType.coral:
                if (types.Contains(PieceData.PieceType.coral))
                {
                    Debug.Log("サンゴは水槽に1つまでです");
                    ShowMessage("サンゴは水槽に1つまでです");
                }
                else
                {
                    isOK = true;
                }
                break;
        }

        //名前指定
        PieceData.PieceName name = aquaPiece.pieceData.pieceName;
        switch (name)
        {
            case PieceData.PieceName.Remora:
                if (!names.Contains(PieceData.PieceName.Shark) && !names.Contains(PieceData.PieceName.WhaleShark))
                {
                    Debug.Log("水槽内にサメがいません");
                    ShowMessage("水槽内にサメがいません");
                    isOK = false;
                }
                else
                {
                    isOK = true;
                }
                break;
        }

        return isOK;
    }

    //水槽内の酸素量チェック
    bool CheckOxygen(AquaSlot slot)
    {
        bool isOK = false;

        //この魚駒を入れた水槽内の酸素量が0以上ならture
        int oxygen = slot.slotOxygen + aquaPiece.pieceData.oxygen;
        if (oxygen >= 0)
        {
            isOK = true;
        }
        else
        {
            Debug.Log("水槽内の酸素量が限界です");
            ShowMessage("水槽内の酸素量が限界です");
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
