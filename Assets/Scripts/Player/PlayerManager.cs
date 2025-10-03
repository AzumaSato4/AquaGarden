using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerData playerData; //�v���C���[���
    GalleryBoard galleryBoard; //�M�������[�^�C�������擾���邽�߂̕ϐ�
    public AquariumBoard aquariumBoard; //�����ك{�[�h�����擾���邽�߂̕ϐ�
    public int money; //��������

    GameObject aquarium;        //�����ك{�[�h
    GameObject galleryPlayer;   //�M�������[�̃v���C���[��
    GameObject aquariumPlayer;  //�����ق̃v���C���[��

    GameObject currentGalleryTile;  //���݂̃M�������[�}�X
    public int galleryIndex;        //���݂̃M�������[�}�X�ԍ�
    GameObject currentAquariumTile; //���݂̐����ك}�X
    int aquariumIndex;              //���݂̐����ك}�X�ԍ�

    TurnManager turnManager;    //TurnManager���i�[���邽�߂̕ϐ�
    public PhaseManager phaseManager;  //PhaseManager���i�[���邽�߂̕ϐ�
    public AquaPieceManager aquaPieceManager;  //PieceManager���i�[���邽�߂̕ϐ�

    public bool isActive = false;   //�����̃^�[�����ǂ���
    public bool isGoal = false;     //�S�[���������ǂ���

    int another;    //�I���\�Ȃ�����̐������L�^���邽�߂̕ϐ�

    //�e�X�g�p
    [SerializeField] PieceData pieceData;

    private void Start()
    {
        //�v���C���[�̋�Ɛ����ك{�[�h�𐶐�
        aquarium = Instantiate(playerData.aquarium, new Vector3(playerData.playerNum * 20, 0, 0), Quaternion.identity);
        galleryPlayer = Instantiate(playerData.galleryPlayer);
        aquariumPlayer = Instantiate(playerData.aquariumPlayer);

        //�v���C���[��̉摜���Z�b�g
        galleryPlayer.GetComponent<SpriteRenderer>().sprite = playerData.gallerySprite;
        aquariumPlayer.GetComponent<SpriteRenderer>().sprite = playerData.aquariumSprite;

        //playerManager�Ɏ������g���Z�b�g
        galleryPlayer.GetComponent<GalleryPlayerController>().playerManager = this;
        aquariumPlayer.GetComponent<AquariumPlayerController>().playerManager = this;

        //�M�������[�Ɛ����ق̏����擾
        galleryBoard = GameObject.Find("Gallery").GetComponent<GalleryBoard>();
        aquariumBoard = aquarium.GetComponent<AquariumBoard>();

        //TurnManager��PhaseManager���擾
        turnManager = GameObject.Find("MainManager").GetComponent<TurnManager>();
        phaseManager = GameObject.Find("MainManager").GetComponent<PhaseManager>();

        //�����ʒu�ɃZ�b�g
        //�M�������[�X�^�[�g�ʒu�ɃZ�b�g
        galleryIndex = playerData.playerNum - 1;
        currentGalleryTile = galleryBoard.startSpots[galleryIndex];
        galleryPlayer.transform.position = currentGalleryTile.transform.position;
        galleryBoard.isPlayer[galleryIndex] = true;
        //�����كX�^�[�g�ʒu�ɃZ�b�g
        aquariumIndex = 0;
        currentAquariumTile = aquariumBoard.aquaTiles[aquariumIndex];
        aquariumPlayer.transform.position = currentAquariumTile.transform.position;
        aquariumBoard.isPlayer[aquariumIndex] = true;
        //�R�C���̏����ʒu��2
        money = 2;
        aquariumBoard.coin.transform.position = aquariumBoard.CoinSpots[money].transform.position;
    }

    public void StartGallery()
    {
        if (isActive)
        {
            galleryPlayer.GetComponent<GalleryPlayerController>().movedGallery = false;
        }
    }

    public void MoveGallery(int to)
    {
        galleryBoard.isPlayer[galleryIndex] = false;
        galleryIndex = to;
        galleryBoard.isPlayer[galleryIndex] = true;

        if (0 <= galleryIndex && galleryIndex <= 3)
        {
            isGoal = true;
            //�S�[�������狭���^�[���G���h
            turnManager.EndTurn();
            return;
        }

        phaseManager.EndGallery(playerData);
        StartAquarium();
    }

    public void StartAquarium()
    {
        if (isActive)
        {
            aquariumPlayer.GetComponent<AquariumPlayerController>().movedAquarium = false;
            for (int i = 1; i < 4; i++)
            {
                int movableTiles = aquariumIndex + i;
                if (movableTiles > 5)
                {
                    movableTiles -= 6;
                }
                aquariumBoard.aquaTiles[movableTiles].GetComponent<PolygonCollider2D>().enabled = true;
            }
            GetPiece();
        }
    }

    public void MoveAquarium(int to)
    {
        aquariumBoard.isPlayer[aquariumIndex] = false;
        aquariumIndex = to;
        aquariumBoard.isPlayer[aquariumIndex] = true;

        for (int i = 0; i < 6; i++)
        {
            aquariumBoard.aquaTiles[i].GetComponent<PolygonCollider2D>().enabled = false;
        }

        EditAquarium();
        phaseManager.MovedAquarium(playerData);
    }

    void EditAquarium()
    {
        another = aquariumIndex - 1;
        if (another < 0)
        {
            another = 5;
            Debug.Log("�}�C�i�X�I");
        }
        for (int i = 0; i < 6; i++)
        {
            if (i == aquariumIndex || i == another)
            {
                aquariumBoard.aquaSlots[i].GetComponent<AquaSlot>().mask.SetActive(false);
            }
            else
            {
                aquariumBoard.aquaSlots[i].GetComponent<AquaSlot>().mask.SetActive(true);
            }
        }
    }

    public void SelectSlot()
    {
        aquariumBoard.aquaSlots[aquariumIndex].GetComponent<AquaSlot>().selectable = true;
        aquariumBoard.aquaSlots[another].GetComponent<AquaSlot>().selectable = true;
    }

    public void DontSelectSlot()
    {
        aquariumBoard.aquaSlots[aquariumIndex].GetComponent<AquaSlot>().selectable = false;
        aquariumBoard.aquaSlots[another].GetComponent<AquaSlot>().selectable = false;
    }

    public void EndAquarium()
    {
        foreach (GameObject slot in aquariumBoard.aquaSlots)
        {
            slot.GetComponent<AquaSlot>().selectable = false;
            slot.GetComponent<AquaSlot>().mask.SetActive(false);
        }
    }

    public void GetPiece()
    {
        for (int i = 0; i < 8; i++)
        {
            aquaPieceManager.CreatePiece(this,pieceData, aquariumBoard.storage);
        }
    }
}
