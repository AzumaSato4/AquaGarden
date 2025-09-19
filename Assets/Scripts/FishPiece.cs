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
        // UI�̏�ɃJ�[�\������������A���͂��󂯕t���Ȃ�
        if (GameManager.UIActive) return;

        originalParent = transform.parent;
        originalLocalPos = transform.localPosition;

        // �őO�ʂɏo�����߂Ɉꎞ�I�ɐe���O��
        transform.SetParent(null);
    }


    //�h���b�O��
    public void OnDrag(PointerEventData eventData)
    {
        //�X�N���[�����W�����[���h���W�ɕϊ�
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;
        transform.position = worldPos;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Collider2D hit = Physics2D.OverlapPoint(worldPos, LayerMask.GetMask("Slot"));

        //�h���b�v�攻��
        var slot = hit?.GetComponent<AquaSlot>();
        var sea = hit?.GetComponent<SeaBoard>();
        var storage = hit?.GetComponent<StoragePanel>();


        //�����ɒu���Ȃ�
        if (slot != null)
        {
            //�ړ������X�g���[�W�Ȃ�OK
            if (originalParent.GetComponent<StoragePanel>() != null)
            {
                originalParent.GetComponent<StoragePanel>().RemoveStorageFish(this);
                slot.AddTempFish(this);
            }
            else //�������m��NG�A���̈ʒu�ɖ߂�
            {
                transform.SetParent(originalParent);
                transform.localPosition = originalLocalPos;
            }
        }
        else if (sea != null) //�C�ɒu���Ȃ�
        {
            (originalParent.GetComponent<AquaSlot>())?.RemoveFish(this);
            (originalParent.GetComponent<StoragePanel>())?.RemoveStorageFish(this);
            sea.AddSeaFish(this);
        }
        else
        {
            //���̈ʒu�ɖ߂�
            transform.SetParent(originalParent);
            transform.localPosition = originalLocalPos;
        }

    }
}
