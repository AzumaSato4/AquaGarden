using UnityEngine;
using UnityEngine.EventSystems;

public class FishPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public FishData fishData;
    Transform originalParent;
    Vector3 originalLocalPos;

    AquaSlot currentSlot; //�h���b�O���ɏd�Ȃ��Ă��鐅��
    AquaSlot hoverSlot;   //�h���b�O���Ɉꎞ�I�ɏd�Ȃ��Ă��鐅��

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = fishData.icon;
        GetComponent<SpriteRenderer>().color = fishData.color;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        //UI�̏�ɃJ�[�\������������A���͂��󂯕t���Ȃ�
        if (GameManager.UIActive) return;

        originalParent = transform.parent;
        originalLocalPos = transform.localPosition;

        //���̐����̎_�f�\�����X�V�i�h���b�O���������j
        var slot = originalParent.GetComponent<AquaSlot>();
        if (slot != null)
        {
            currentSlot = slot;
            currentSlot.UpdateOxygenUI();
        }
        else
        {
            currentSlot = null; // �X�g���[�W����o���ꍇ�� null
        }

        //�őO�ʂɏo�����߂Ɉꎞ�I�ɐe���O��
        transform.SetParent(null);
    }


    //�h���b�O��
    public void OnDrag(PointerEventData eventData)
    {
        //�X�N���[�����W�����[���h���W�ɕϊ�
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPos.z = 0;
        transform.position = worldPos;

        //�h���b�O���̎󂯓���\��������
        Collider2D hit = Physics2D.OverlapPoint(worldPos, LayerMask.GetMask("Slot"));
        var slot = hit?.GetComponent<AquaSlot>();
        if (slot != null)
        {
            //�V���������ɓ������Ƃ�
            if (hoverSlot != slot)
            {
                //�O�̐��������ɖ߂�
                hoverSlot?.UpdateOxygenUI();
                //�V���������ɉ��u���_�f�ʂ�\��
                slot.UpdateOxygenUI(this);
                hoverSlot = slot;
            }
            else
            {
                // �����������Ńh���b�O���͉��u���\�����ێ�
                slot.UpdateOxygenUI(this);
            }
        }
        else
        {
            // ��������O�ꂽ�Ƃ� �� ���O�̐��������ɖ߂�
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

        //�h���b�v�攻��
        var slot = hit?.GetComponent<AquaSlot>();
        var sea = hit?.GetComponent<SeaPanel>();

        //�h���b�O���̐�����ێ����Ă���
        AquaSlot originalSlot = currentSlot;

        //�����I�u�W�F�N�g�Ȃ猳�ɖ߂��đ����^�[��
        if (hit != null && hit.transform == originalParent)
        {
            transform.SetParent(originalParent);
            transform.localPosition = originalLocalPos;

            // ���̐����̎_�f�ʂ��X�V
            originalSlot?.UpdateOxygenUI();
            return;
        }

        //�����ɒu���Ȃ�
        if (slot != null)
        {
            StoragePanel fromStorage = originalParent.GetComponent<StoragePanel>();
            //�ړ������X�g���[�W�Ȃ�OK
            if (slot.CanAcceptFish(this) && fromStorage != null)
            {
                fromStorage.RemoveStorageFish(this);

                slot.AddFish(this);
                currentSlot = slot;

                slot.UpdateOxygenUI();

            }
            //��������Ȃ�NG
            else
            {
                transform.SetParent(originalParent);
                transform.localPosition = originalLocalPos;
                //���̐����̎_�f�ʂ�߂�
                originalSlot?.UpdateOxygenUI();
            }
        }
        else if (sea != null) //�C�ɒu���Ȃ�
        {
            // �������狛�����菜��
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
            //���̈ʒu�ɖ߂�
            transform.SetParent(originalParent);
            transform.localPosition = originalLocalPos;
            //���̐����̎_�f�ʕ\����߂�
            currentSlot?.UpdateOxygenUI();
        }

        //�h���b�O���̉��\�����N���A
        hoverSlot?.UpdateOxygenUI();
    }
}
