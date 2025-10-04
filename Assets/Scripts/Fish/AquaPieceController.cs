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
            //�J�[�\���̈ʒu���擾
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            //�N���b�N�����I�u�W�F�N�g���擾
            if (hit.collider != null)
            {
                GameObject target = hit.collider.gameObject;

                //�}�E�X�N���b�N������
                if (Input.GetMouseButtonDown(0))
                {
                    //�����I�u�W�F�N�g���N���b�N������
                    if (target != null)
                    {
                        //�N���b�N���������Ɉړ�����
                        if (target.CompareTag("AquaSlot"))
                        {
                            GameObject current = GetComponent<AquaPiece>().currentPos;
                            if (current != null)
                            {
                                if (current == target)
                                {
                                    Debug.Log("�����鐅���ł�");
                                    aquaPieceManager.CanselSelect();
                                    return;
                                }
                                else if (current.CompareTag("AquaSlot"))
                                {
                                    Debug.Log("�������琅���ɂ͈ړ��ł��܂���");
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
                                Debug.Log("�����ɓ�����܂���");
                            }
                        }
                    }
                }
            }
        }
    }
}
