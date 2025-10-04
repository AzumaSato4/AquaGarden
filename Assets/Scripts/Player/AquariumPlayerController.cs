using UnityEngine;
using UnityEngine.EventSystems;

public class AquariumPlayerController : MonoBehaviour
{
    public PlayerManager playerManager;

    public bool movedAquarium;

    private void Start()
    {
        movedAquarium = true;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!movedAquarium)
        {
            //�J�[�\���̈ʒu���擾
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            //�N���b�N�����I�u�W�F�N�g���擾
            if (hit.collider != null)
            {
                GameObject selected = hit.collider.gameObject;

                //�}�E�X�N���b�N������
                if (Input.GetMouseButtonDown(0))
                {
                    //�����I�u�W�F�N�g���N���b�N������
                    if (selected != null)
                    {
                        //�����ق̃N���b�N�����}�X�Ɉړ�����
                        if (selected.CompareTag("AquariumTile"))
                        {
                            playerManager.MoveAquarium(selected.GetComponent<TileManager>().tileIndex);
                            transform.position = selected.transform.position;
                            movedAquarium = true;
                        }
                        if (selected.CompareTag("AquaSlot"))
                        {
                            Debug.Log("�����I");
                        }
                    }
                }
            }
        }


    }
}
