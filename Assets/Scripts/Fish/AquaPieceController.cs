using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class AquaPieceController : MonoBehaviour
{
    public PlayerManager playerManager;
    public AquaPieceManager aquaPieceManager;

    AquaPiece aquaPiece;

    private void Start()
    {
        aquaPiece = GetComponent<AquaPiece>();
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() || UIController.isActiveUI)
        {
            return;
        }

        if (playerManager.isActive && (PhaseManager.currentPhase == PhaseManager.Phase.edit || PhaseManager.currentPhase == PhaseManager.Phase.adEdit) && aquaPieceManager.selectedPiece == this.gameObject)
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
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            //クリックしたオブジェクトを取得
            if (hit.collider != null)
            {
                GameObject target = hit.collider.gameObject;

                //マウスクリックしたら
                if (Input.GetMouseButtonDown(0))
                {
                    //何かオブジェクトをクリックしたら
                    if (target != null)
                    {
                        //クリックした水槽に移動する
                        if (target.CompareTag("AquaSlot"))
                        {
                            GameObject current = aquaPiece.currentPos;
                            if (current != null)
                            {
                                if (current == target)
                                {
                                    Debug.Log("今いる水槽です");
                                    ShowMessage("今いる水槽です");
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

                            AquaSlot slot = target.GetComponent<AquaSlot>();
                            GameObject to = slot.CheckSpot();
                            if (to != null)
                            {
                                if (CheckOxygen(slot))
                                {
                                    if (CheckType(slot))
                                    {
                                        transform.position = to.transform.position;
                                        aquaPiece.currentPos = target;
                                        transform.parent = slot.transform;
                                        slot.slotPieces.Add(aquaPieceManager.selectedPiece);
                                        slot.slotOxygen += aquaPiece.pieceData.oxygen;
                                        aquaPiece.isiFromSea = false;
                                    }
                                }
                                else
                                {
                                    Debug.Log("水槽内の酸素量が限界です");
                                    ShowMessage("水槽内の酸素量が限界です");
                                }
                            }
                            else
                            {
                                Debug.Log("水槽がいっぱいです");
                                ShowMessage("水槽がいっぱいです");
                            }
                            aquaPieceManager.CanselSelect();
                        }
                    }
                }
            }
        }
    }

    bool CheckType(AquaSlot slot)
    {
        bool isOK = false;

        PieceData.PieceType[] pieces = new PieceData.PieceType[slot.slotPieces.Count];
        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i] = slot.slotPieces[i].GetComponent<AquaPiece>().pieceData.pieceType;
        }

        PieceData.PieceType type = aquaPiece.pieceData.pieceType;
        switch (type)
        {
            case PieceData.PieceType.fish:
                if (pieces.Contains(PieceData.PieceType.shark))
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
                if (pieces.Contains(PieceData.PieceType.shark))
                {
                    Debug.Log("サメがいる水槽には入れられません");
                    ShowMessage("サメがいる水槽には入れられません");
                }
                else if (!pieces.Contains(PieceData.PieceType.seaweed))
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
                if (pieces.Contains(PieceData.PieceType.fish) ||
                    pieces.Contains(PieceData.PieceType.seaTurtle)
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
                isOK = true;
                break;
            case PieceData.PieceType.seaweed:
                if (pieces.Contains(PieceData.PieceType.seaweed))
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
                if (pieces.Contains(PieceData.PieceType.coral))
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

        return isOK;
    }

    bool CheckOxygen(AquaSlot slot)
    {
        bool isOK = false;

        //この魚駒を入れた水槽内の酸素量が0以上ならture
        int oxygen = slot.slotOxygen + aquaPiece.pieceData.oxygen;
        if (oxygen >= 0)
        {
            isOK = true;
        }

        return isOK;
    }

    void ShowMessage(string message)
    {
        UIController.messageText.text = message;
        UIController.isMessageChanged = true;
    }
}
