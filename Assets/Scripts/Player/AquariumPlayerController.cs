using UnityEngine;
using UnityEngine.EventSystems;

public class AquariumPlayerController : MonoBehaviour
{
    public PlayerManager playerManager;

    public bool movedAquarium = true;
    int clickCount;
    GameObject selected;

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() || UIController.isActiveUI)
        {
            return;
        }

        if (playerManager.isActive && !movedAquarium && PhaseManager.currentPhase == PhaseManager.Phase.aquarium)
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
                        Invoke("CheckDouble", 0.4f);
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

            //水族館のクリックしたマスに移動する
            if (selected.CompareTag("AquariumTile"))
            {
                playerManager.MoveAquarium(selected.GetComponent<TileManager>().tileIndex);
                transform.position = selected.transform.position;
                movedAquarium = true;
            }
            selected = null;
        }
    }
}
