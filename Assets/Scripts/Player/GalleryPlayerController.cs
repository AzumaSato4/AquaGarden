using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GalleryPlayerController : MonoBehaviour
{
    public PlayerManager playerManager;

    public bool movedGallery = true;
    GameObject selected;
    int selectIndex;
    int clickCount;
    float moveTime = 0.3f; //移動アニメーションの時間

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
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, 10f);
            //クリックしたオブジェクトを取得
            if (hit.collider == null) return;
            selected = hit.collider.gameObject;

            //マウスクリックしたら
            if (Input.GetMouseButtonDown(0))
            {
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

    private void Move()
    {
        //ギャラリーのクリックしたマスに移動する
        if (selected.CompareTag("GalleryTile"))
        {
            selectIndex = selected.GetComponent<TileManager>().tileIndex;
            if (selectIndex < playerManager.galleryIndex && 3 < selectIndex)
            {
                Debug.Log("後ろには進めません");
                ShowMessage("後ろには進めません");
                soundManager.PlaySE(SoundManager.SE_Type.ng);
                return;
            }
            else
            {
                int nextIndex = playerManager.galleryIndex + 1;
                //1マスずつ進む
                OneStep(nextIndex);
                movedGallery = true;
                if (selected.GetComponent<BoxCollider2D>() != null)
                    selected.GetComponent<BoxCollider2D>().enabled = false;
                if (selected.GetComponent<CircleCollider2D>() != null)
                    selected.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
    }

    void OneStep(int nextIndex)
    {
        if (nextIndex >= playerManager.galleryBoard.Tiles.Length)
        {
            nextIndex -= playerManager.galleryBoard.Tiles.Length;
        }
        GameObject next = playerManager.galleryBoard.Tiles[nextIndex];
        //DoTweenで移動アニメーション
        transform.DOMove(next.transform.position, moveTime).OnComplete(() =>
        {
            soundManager.PlaySE(SoundManager.SE_Type.click);
            if (transform.position != selected.transform.position)
            {
                nextIndex++;
                OneStep(nextIndex);
            }
            else //移動が完了
            {
                playerManager.MoveGallery(selectIndex, selected.name);
                selected = null;
            }
        });
    }

    void ShowMessage(string message)
    {
        UIController.messageText.text = message;
        UIController.isMessageChanged = true;
    }
}
