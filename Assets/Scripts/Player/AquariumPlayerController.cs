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
                        //水族館のクリックしたマスに移動する
                        if (selected.CompareTag("AquariumTile"))
                        {
                            playerManager.MoveAquarium(selected.GetComponent<TileManager>().tileIndex);
                            transform.position = selected.transform.position;
                            movedAquarium = true;
                        }
                        if (selected.CompareTag("AquaSlot"))
                        {
                            Debug.Log("水槽！");
                        }
                    }
                }
            }
        }


    }
}
