using UnityEngine;
using UnityEngine.EventSystems;

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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!movedGallery && PhaseManager.currentPhase == PhaseManager.Phase.gallery)
        {
            //カーソルの位置を取得
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            //クリックしたオブジェクトを取得
            if (hit.collider != null)
            {
                GameObject selected = hit.collider.gameObject;

                //マウスクリックしたら
                if (Input.GetMouseButtonDown(0))
                {
                    //何かオブジェクトをクリックしたら
                    if (selected != null)
                    {
                        //ギャラリーのクリックしたマスに移動する
                        if (selected.CompareTag("GalleryTile"))
                        {
                            int index = selected.GetComponent<TileManager>().tileIndex;
                            if (index < playerManager.galleryIndex && 3 < index)
                            {
                                Debug.Log("後ろには進めません");
                                ShowMessage("後ろには進めません");
                                return;
                            }
                            else
                            {
                                playerManager.MoveGallery(index, selected.name);
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

    void ShowMessage(string message)
    {
        UIController.messageText.text = message;
        UIController.isMessageChanged = true;
    }
}
