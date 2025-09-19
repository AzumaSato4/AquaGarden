using UnityEngine;
using UnityEngine.EventSystems;

public class FishPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public FishData fishData;
    Transform originalParent;
    Vector3 originalLocalPos;


    public SeaBoard sea;



    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = fishData.icon;
        GetComponent<SpriteRenderer>().color = fishData.color;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        // UIの上にカーソルがあったら、入力を受け付けない
        if (GameManager.UIActive) return;

        originalParent = transform.parent;
        originalLocalPos = transform.localPosition;

        // 最前面に出すために一時的に親を外す
        transform.SetParent(null);
    }


    //ドラッグ中
    public void OnDrag(PointerEventData eventData)
    {
        //スクリーン座標をワールド座標に変換
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;
        transform.position = worldPos;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Collider2D hit = Physics2D.OverlapPoint(worldPos, LayerMask.GetMask("Slot"));

        //ドロップ先判定
        var slot = hit?.GetComponent<AquaSlot>();
        var sea = hit?.GetComponent<SeaBoard>();
        var storage = hit?.GetComponent<StoragePanel>();


        //水槽に置くなら
        if (slot != null)
        {
            //移動元がストレージならOK
            if (originalParent.GetComponent<StoragePanel>() != null)
            {
                originalParent.GetComponent<StoragePanel>().RemoveStorageFish(this);
                slot.AddTempFish(this);
            }
            else //水槽同士はNG、元の位置に戻す
            {
                transform.SetParent(originalParent);
                transform.localPosition = originalLocalPos;
            }
        }
        else if (sea != null) //海に置くなら
        {
            (originalParent.GetComponent<AquaSlot>())?.RemoveFish(this);
            (originalParent.GetComponent<StoragePanel>())?.RemoveStorageFish(this);
            sea.AddSeaFish(this);
        }
        else
        {
            //元の位置に戻す
            transform.SetParent(originalParent);
            transform.localPosition = originalLocalPos;
        }

    }
}
