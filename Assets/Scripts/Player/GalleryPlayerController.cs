using UnityEngine;

public class GalleryPlayerController : MonoBehaviour
{
    public PlayerManager playerManager;

    public bool movedGallery;


    private void Start()
    {
        movedGallery = true;
    }

    private void Update()
    {
        if (!movedGallery)
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
                        //�M�������[�̃N���b�N�����}�X�Ɉړ�����
                        if (selected.CompareTag("GalleryTile"))
                        {
                            playerManager.MoveGallery(selected.GetComponent<TileManager>().tileIndex);
                            transform.position = selected.transform.position;
                            movedGallery = true;
                            if (selected.GetComponent<BoxCollider2D>() != null)
                                selected.GetComponent<BoxCollider2D>().enabled = false;
                            if (selected.GetComponent<CircleCollider2D>() != null)
                                selected.GetComponent<CircleCollider2D>().enabled = false;
                        }
                    }
                }
            }
        }


    }
}
