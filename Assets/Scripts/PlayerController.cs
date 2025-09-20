using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerData pData;        //�v���C���[�̊�{���

    //�M�������[�{�[�h
    public GameObject galleryPiece; //�M�������[�{�[�h�p�̋�
    public Tile currentGalleryTile; //���݂̃}�X�ԍ��i�����l�̓X�^�[�g�}�X�j
    public bool isGoal = false;     //�S�[���������ǂ���

    //�����ك{�[�h
    public GameObject aquariumPiece; //�����ك{�[�h�p�̋�
    public Tile[] aquariumTiles;     //�v���C���[���ꂼ�ꂪ�������ك{�[�h
    public Tile currentAquaTile;    //�����ك{�[�h�̌��݂̃}�X�ԍ�
    public GameObject aquariumCam;  //�����ٗp�J����

    public AquaSlot[] aquaSlots;    //�v���C���[���Ƃ̐���
    public StoragePanel storagePanel;   //�v���C���[���Ƃ̃X�g���[�W

    float moveTime = 0.3f; //�v���C���[��ړI�n�܂ł����鎞��


    private void Start()
    {
        //�����̃M�������[��Ɛ����ً���X�^�[�g�ʒu�ɃZ�b�g����
        galleryPiece.transform.position = currentGalleryTile.transform.position;
        aquariumPiece.transform.position = currentAquaTile.transform.position;

        //�����̋�̐F���v���C���[�J���[�ɂ���
        galleryPiece.GetComponent<SpriteRenderer>().color = pData.color;
        aquariumPiece.GetComponent<SpriteRenderer>().color = pData.color;
    }


    //�I�񂾃M�������[�}�X�Ɉړ�����
    public void MoveToGalleryTile(Tile toTile)
    {
        currentGalleryTile = toTile;
        //���̃v���C���[���I��s�ɂ���
        toTile.uesdTile = true;
        currentGalleryTile.Highlight(false);

        StartCoroutine(MoveCoroutine(toTile.transform.position, galleryPiece));
    }


    //�I�񂾐����ك}�X�Ɉړ�����
    public void MoveToAquaTile(Tile toTile)
    {
        currentAquaTile = toTile;
        EnableAquariumSlot(toTile);
        StartCoroutine(MoveCoroutine(toTile.transform.position, aquariumPiece));
    }


    public void EnableAquariumSlot(Tile currentTile)
    {
        foreach (Tile t in aquariumTiles)
        {
            t.Highlight(false);
            t.GetComponent<Collider2D>().enabled = false;
        }

        //�אڂ̐�����L����
        currentTile.leftSlot.SetHighlight(true);
        currentTile.rightSlot.SetHighlight(true);

        currentTile.leftSlot.GetComponent<Collider2D>().enabled = true;
        currentTile.rightSlot.GetComponent<Collider2D>().enabled = true;
    }


    //�ړ������炩�ɂ���R���[�`��
    IEnumerator MoveCoroutine(Vector2 toPos, GameObject piece)
    {
        Vector2 startPos = piece.transform.position;
        float elapsed = 0;

        while (elapsed < moveTime)
        {
            piece.transform.position = Vector2.Lerp(startPos, toPos, elapsed / moveTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        piece.transform.position = toPos;
    }


    // ���݃h���b�O���̋���u�����X���b�g���`�F�b�N
    public AquaSlot GetValidSlotForFish(FishPiece draggingFish)
    {
        foreach (var slot in aquaSlots)
        {
            if (slot.CanAcceptFish(draggingFish))
                return slot;
        }
        return null;
    }

    // ����UI�X���C�_�[���܂Ƃ߂čX�V
    public void UpdateAllOxygenUI()
    {
        foreach (var slot in aquaSlots)
        {
            slot.UpdateOxygenUI();
        }
    }
}
