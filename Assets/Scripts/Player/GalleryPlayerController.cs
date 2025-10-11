using UnityEngine;
using UnityEngine.EventSystems;

public class GalleryPlayerController : MonoBehaviour
{
    public PlayerManager playerManager;

    public bool movedGallery = true;
    GameObject selected;
    int clickCount;

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() || UIController.isActiveUI)
        {
            return;
        }

        if (playerManager.isActive && !movedGallery && PhaseManager.currentPhase == PhaseManager.Phase.gallery)
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
                selected = hit.collider.gameObject;

                //マウスクリックしたら
                if (Input.GetMouseButtonDown(0))
                {
                    //何かオブジェクトをクリックしたら
                    if (selected != null)
                    {
                        clickCount++;
                        Invoke("CheckDouble", 0.3f);
                    }
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

        void ShowMessage(string message)
        {
            UIController.messageText.text = message;
            UIController.isMessageChanged = true;
        }
    }
}
