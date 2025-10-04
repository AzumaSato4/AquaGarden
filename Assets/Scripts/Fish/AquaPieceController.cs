using UnityEngine;
using UnityEngine.EventSystems;

public class AquaPieceController : MonoBehaviour
{
    public PlayerManager playerManager;
    public AquaPieceManager aquaPieceManager;

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (playerManager.isActive && playerManager.phaseManager.currentPhase == PhaseManager.Phase.edit && aquaPieceManager.selectedPiece == this.gameObject)
        {
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
                            GameObject current = GetComponent<AquaPiece>().currentPos;
                            if (current != null)
                            {
                                if (current == target)
                                {
                                    Debug.Log("今いる水槽です");
                                    aquaPieceManager.CanselSelect();
                                    return;
                                }
                                else if (current.CompareTag("AquaSlot"))
                                {
                                    Debug.Log("水槽から水槽には移動できません");
                                    aquaPieceManager.CanselSelect();
                                    return;
                                }
                            }

                            GameObject to = target.GetComponent<AquaSlot>().CheckSpot();
                            if (to != null)
                            {
                                transform.position = to.transform.position;
                                GetComponent<AquaPiece>().currentPos = target;
                                aquaPieceManager.CanselSelect();
                            }
                            else
                            {
                                Debug.Log("水槽に入れられません");
                            }
                        }
                    }
                }
            }
        }
    }
}
