using UnityEngine;
using UnityEngine.EventSystems;

public class FishPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public FishData fishData;
    Transform originalParent;
    Vector3 originalLocalPos;

    AquaSlot currentSlot; //ドラッグ中に重なっている水槽
    AquaSlot hoverSlot;   //ドラッグ中に一時的に重なっている水槽

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = fishData.icon;
        GetComponent<SpriteRenderer>().color = fishData.color;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        //UIの上にカーソルがあったら、入力を受け付けない
        if (GameManager.UIActive) return;

        originalParent = transform.parent;
        originalLocalPos = transform.localPosition;

        //元の水槽の酸素表示を更新（ドラッグ分を引く）
        var slot = originalParent.GetComponent<AquaSlot>();
        if (slot != null)
        {
            currentSlot = slot;
            currentSlot.UpdateOxygenUI();
        }
        else
        {
            currentSlot = null; // ストレージから出す場合は null
        }

        //最前面に出すために一時的に親を外す
        transform.SetParent(null);
    }


    //ドラッグ中
    public void OnDrag(PointerEventData eventData)
    {
        //スクリーン座標をワールド座標に変換
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;
        transform.position = worldPos;

        //ドラッグ中の受け入れ可能水槽判定
        Collider2D hit = Physics2D.OverlapPoint(worldPos, LayerMask.GetMask("Slot"));
        var slot = hit?.GetComponent<AquaSlot>();
        if (slot != null)
        {
            //新しく水槽に入ったとき
            if (hoverSlot != slot)
            {
                //前の水槽を元に戻す
                hoverSlot?.UpdateOxygenUI();
                //新しい水槽に仮置き酸素量を表示
                slot.UpdateOxygenUI(this);
                hoverSlot = slot;
            }
            else
            {
                // 同じ水槽内でドラッグ中は仮置き表示を維持
                slot.UpdateOxygenUI(this);
            }
        }
        else
        {
            // 水槽から外れたとき → 直前の水槽を元に戻す
            if (hoverSlot != null)
            {
                hoverSlot.UpdateOxygenUI();
                hoverSlot = null;
            }
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Collider2D hit = Physics2D.OverlapPoint(worldPos, LayerMask.GetMask("Slot"));

        //ドロップ先判定
        var slot = hit?.GetComponent<AquaSlot>();
        var sea = hit?.GetComponent<SeaPanel>();

        //ドラッグ元の水槽を保持しておく
        AquaSlot originalSlot = currentSlot;

        //同じオブジェクトなら元に戻して即リターン
        if (hit != null && hit.transform == originalParent)
        {
            transform.SetParent(originalParent);
            transform.localPosition = originalLocalPos;

            // 元の水槽の酸素量を更新
            originalSlot?.UpdateOxygenUI();
            return;
        }

        //水槽に置くなら
        if (slot != null)
        {
            StoragePanel fromStorage = originalParent.GetComponent<StoragePanel>();
            //移動元がストレージならOK
            if (slot.CanAcceptFish(this) && fromStorage != null)
            {
                fromStorage.RemoveStorageFish(this);

                slot.AddFish(this);
                currentSlot = slot;

                slot.UpdateOxygenUI();

            }
            //水槽からならNG
            else
            {
                transform.SetParent(originalParent);
                transform.localPosition = originalLocalPos;
                //元の水槽の酸素量を戻す
                originalSlot?.UpdateOxygenUI();
            }
        }
        else if (sea != null) //海に置くなら
        {
            // 水槽から魚駒を取り除く
            if (currentSlot != null)
            {
                currentSlot.RemoveFish(this);
                currentSlot = null;
            }

            originalParent.GetComponent<StoragePanel>()?.RemoveStorageFish(this);

            sea.seaboard.GetComponent<SeaBoard>().AddSeaFish(this.fishData);
            Destroy(this.gameObject);

        }
        else
        {
            //元の位置に戻す
            transform.SetParent(originalParent);
            transform.localPosition = originalLocalPos;
            //元の水槽の酸素量表示を戻す
            currentSlot?.UpdateOxygenUI();
        }

        //ドラッグ中の仮表示をクリア
        hoverSlot?.UpdateOxygenUI();
    }
}
