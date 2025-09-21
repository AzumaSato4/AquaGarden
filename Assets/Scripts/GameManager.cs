using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Part
    {
        gallery,
        aquarium
    }

    Part currentPart = Part.gallery;

    public SeaBoard sea;
    public GameObject fishPrefab;

    [Header("StartSeaFish")]
    public FishData[] fishData;
    public int[] startSeaFishCount; //�X�^�[�g���̊C�{�[�h���
    public int[] startFishCount;    //�X�^�[�g���̃M�������[���

    public PlayerController[] players;
    public PlayerController[] playersTurn;  //�X�^�[�g���̃v���C�̏��Ԃ��Ǘ�����z��
    public Tile[] galleryTiles;

    public StoragePanel[] storagePanel; //�e�v���C���[�̃X�g���[�W�p�l��
    public bool canSelct = false;   //�C�{�[�h�̃Q�b�g�{�^���������邩�ǂ���

    public GameObject galleryCam;   //�M�������[�{�[�h���f�����߂̃J����

    [SerializeField] GameObject[] aquaText; //�e�v���C���[�̐����X���C�_�[

    [SerializeField] PlayerRanking playerRanking;

    public float moveCamTime;

    PlayerController currentPlayer;
    PlayerController nextPlayer;

    public int maxRound;
    public GameObject confirmButton;
    public static bool UIActive = false;

    int turnCount;
    int roundCount;
    public int currentPlayerIndex = 0;

    //�ړ��\�ȃ}�X���i�邽�߂̃��X�g
    List<Tile> movableTiles = new List<Tile>();
    //�S�[�������v���C���[���m�F���郊�X�g
    List<PlayerController> finishOrder = new List<PlayerController>();

    private void Start()
    {
        sea.Initialze(fishData, startSeaFishCount);
        playersTurn = new PlayerController[players.Length];
        Array.Copy(players, playersTurn, players.Length);

        finishOrder.Clear();

        foreach (Tile t in playersTurn[currentPlayerIndex].aquariumTiles)
        {
            t.GetComponent<Collider2D>().enabled = false;
        }

        roundCount = 0;

        StartRound();

    }


    private void Update()
    {
        if (currentPart == Part.aquarium)
        {
            if (UIActive)
            {
                playersTurn[currentPlayerIndex].oxygenText.SetActive(false);
            }
            else
            {
                playersTurn[currentPlayerIndex].oxygenText.SetActive(true);
            }
        }
    }


    //���E���h�X�^�[�g
    void StartRound()
    {
        currentPart = Part.gallery;
        galleryCam.SetActive(true);
        canSelct = false;
        confirmButton.SetActive(false);

        turnCount = 0;
        currentPlayerIndex = 0;
        movableTiles.Clear();
        ClearHighlights();

        roundCount++;

        for (int i = 0; i < players.Length; i++)
        {
            players[i].isGoal = false;
            players[i].currentGalleryTile = galleryTiles[i];
        }

        foreach (Tile t in galleryTiles)
        {
            t.uesdTile = false;
        }

        HighlightMovableTiles(playersTurn[currentPlayerIndex].currentGalleryTile, galleryTiles);

        //�M�������[�ɋ������ׂ�
        foreach (Tile tile in galleryTiles)
        {
            if (!tile.isAd && !tile.isStart)
            {
                for (int i = 0; i < 30; i++)
                {
                    int rand = UnityEngine.Random.Range(2, 8);
                    if (startFishCount[rand] > 0)
                    {
                        GameObject fish = Instantiate(fishPrefab, tile.transform);
                        fish.GetComponent<FishPiece>().fishData = fishData[rand];

                        startFishCount[rand]--;
                        break;
                    }
                }
            }
        }

        UIManager.Instance.ShowMessage("���E���h" + roundCount + "\\n" + playersTurn[currentPlayerIndex].pData.playerName + "�̃^�[��");
    }


    //�M�������[�p�[�g
    //�}�X���I�����ꂽ�炻�̃}�X�ֈړ�
    public void OnTileClicked(Tile clickedTile)
    {
        //�M�������[�^�[���łȂ���Α����^�[��
        if (currentPart != Part.gallery) return;
        //�N���b�N���ꂽ�}�X���I��s�Ȃ瑦���^�[��
        if (!movableTiles.Contains(clickedTile)) return;

        currentPlayer = playersTurn[currentPlayerIndex];
        currentPlayer.MoveToGalleryTile(clickedTile);

        //�S�[��������t���O�𗧂Ă�
        if (clickedTile.index == 0)
        {
            currentPlayer.isGoal = true;
            finishOrder.Add(currentPlayer);

            //�S�����S�[���������`�F�b�N
            int goalCount = 0;
            foreach (PlayerController p in players)
            {
                if (p.isGoal)
                {
                    goalCount++;
                }
            }
            //�S�����S�[�������烉�E���h�I��
            if (goalCount == players.Length)
            {
                EndRound();
                return;
            }
            //�^�[���������I��
            NextPlayerTurn();
            return;
        }

        //�}�X��̋���Q�b�g
        if (!clickedTile.isAd)
        {
            while (clickedTile.transform.childCount > 0)
            {
                GameObject piece = clickedTile.transform.GetChild(0).gameObject;
                FishPiece fish = piece.GetComponent<FishPiece>();

                //�������ɏ�����΂��i�Q�b�g�A�j���[�V�����j
                Rigidbody2D rbody = piece.GetComponent<Rigidbody2D>();
                rbody.gravityScale = 1.0f;
                rbody.bodyType = RigidbodyType2D.Dynamic;
                rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

                StartCoroutine(MoveStorage(fish, rbody));

                piece.transform.SetParent(null);
            }
        }




        //�����ك^�[����
        StartCoroutine(ChangeCamera(galleryCam, currentPlayer.aquariumCam));
        HighlightMovableTiles(currentPlayer.currentAquaTile, currentPlayer.aquariumTiles);
        foreach (Tile t in playersTurn[currentPlayerIndex].aquariumTiles)
        {
            t.GetComponent<Collider2D>().enabled = true;
        }
    }

    //�X�g���[�W�Ɉړ�����R���[�`��
    IEnumerator MoveStorage(FishPiece fish, Rigidbody2D rbody)
    {
        yield return new WaitForSeconds(1.0f);
        playersTurn[currentPlayerIndex].storagePanel.AddStorage(fish);
        rbody.gravityScale = 0;
        rbody.bodyType = RigidbodyType2D.Static;
    }


    //�����كp�[�g
    //�}�X���I�����ꂽ�炻�̃}�X�ֈړ�
    public void OnAquaTileClicked(Tile clickedTile)
    {
        if (currentPart != Part.aquarium) return;
        if (!movableTiles.Contains(clickedTile)) return;

        currentPlayer.MoveToAquaTile(clickedTile);
        confirmButton.SetActive(true);

        //�ړ����I�������C�{�[�h�̃Q�b�g�{�^���𕜊�������
        canSelct = true;

        foreach (Tile t in playersTurn[currentPlayerIndex].aquariumTiles)
        {
            t.GetComponent<Collider2D>().enabled = false;
        }
    }


    //�����كp�[�g�̌���{�^��
    public void OnConfirmButton()
    {
        if (currentPlayer.storagePanel.HasFishInStorage())
        {
            //�x�����o��
            UIManager.Instance.ShowMessage("�X�g���[�W�̋������ׂĔz�u���Ă��������I");
            return;
        }
        if (!AreAllAquariumsValid(currentPlayer))
        {
            //�x�����o��
            UIManager.Instance.ShowMessage("�����̋����m�F���Ă��������I");
            return;
        }
        else
        {
            //�����̃n�C���C�g�������i���ɖ߂��j
            currentPlayer.currentAquaTile.leftSlot.SetHighlight(false);
            currentPlayer.currentAquaTile.rightSlot.SetHighlight(false);

            //�����̃R���C�_�[������
            currentPlayer.currentAquaTile.leftSlot.GetComponent<Collider2D>().enabled = false;
            currentPlayer.currentAquaTile.rightSlot.GetComponent<Collider2D>().enabled = false;


            //���̃v���C���[�̃^�[����
            StartCoroutine(ChangeCamera(currentPlayer.aquariumCam, galleryCam));
            NextPlayerTurn();
        }
    }


    //�v���C���[�������ׂĂ̐��������[��������Ă��邩
    bool AreAllAquariumsValid(PlayerController player)
    {
        foreach (var slot in player.aquaSlots)
        {
            if (!slot.IsOxygenValid()) //�_�f�ʃI�[�o�[���Ƀ`�F�b�N
                return false;

            foreach (var fish in slot.fishes)
            {
                //�������ꎞ�I�Ƀ��X�g����O���Ĕ���
                var others = slot.fishes.Where(f => f != fish).ToList();

                if (!slot.CanAcceptFish(fish, others)) //�������`�F�b�N
                    return false;
            }
        }
        return true;
    }



    //���̃v���C���[�̃^�[����
    public void NextPlayerTurn()
    {
        //�ړ����I������}�X�̃n�C���C�g��߂��Ď��̃^�[����
        ClearHighlights();
        if (turnCount < players.Length - 1)
        {
            turnCount++;
            currentPlayerIndex++;
        }
        else
        {
            currentPlayerIndex = GetLastPlayerIndex();
        }

        nextPlayer = playersTurn[currentPlayerIndex];

        if (nextPlayer.isGoal)
        {
            NextPlayerTurn();
            return;
        }

        //�{�^���������Ȃ��悤�Ɍ��ɖ߂�
        canSelct = false;
        confirmButton.SetActive(false);

        HighlightMovableTiles(nextPlayer.currentGalleryTile, galleryTiles);
        UIManager.Instance.ShowMessage("���E���h" + roundCount + "\n" + nextPlayer.pData.playerName + "�̃^�[��");
    }

    // ��Ԍ��̃v���C���[�̃C���f�b�N�X�ԍ���Ԃ�
    private int GetLastPlayerIndex()
    {
        int minIndex = 0;
        int minTile = 19;

        for (int i = 0; i < players.Length; i++)
        {
            if (playersTurn[i].currentGalleryTile.index == 0)
            {
                continue;
            }
            else if (playersTurn[i].currentGalleryTile.index <= minTile)
            {
                minTile = playersTurn[i].currentGalleryTile.index;
                minIndex = i;
            }
        }
        return minIndex;
    }




    //�ړ��\���ǂ�������
    void HighlightMovableTiles(Tile from, Tile[] boradTiles)
    {
        movableTiles.Clear();

        if (boradTiles == galleryTiles)
        {
            foreach (Tile t in boradTiles)
            {
                //�������̃}�X���ǂ���
                if (from.index < t.index)
                {
                    //�ق��̃v���C���[�����Ȃ����ǂ���
                    bool occupied = false;
                    foreach (PlayerController p in playersTurn)
                    {
                        if (p != playersTurn[currentPlayerIndex] && p.currentGalleryTile == t)
                        {
                            //�ق��̃v���C���[��������true�ɂ��ď��O
                            occupied = true;
                            break;
                        }
                    }

                    if (occupied) continue;

                    if (t.uesdTile) continue;

                    movableTiles.Add(t);
                    t.Highlight(true);
                }

            }
            if (playersTurn[currentPlayerIndex].currentGalleryTile.index >= 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (!galleryTiles[i].uesdTile)
                    {
                        movableTiles.Add(galleryTiles[i]);
                        galleryTiles[i].Highlight(true);
                        break;
                    }
                }
            }
        }
        else
        {
            int fromIndex = from.index;

            for (int step = 0; step < 3; step++)
            {
                int targetIndex = (fromIndex + step) % boradTiles.Length;
                Tile targetTile = boradTiles[targetIndex];

                movableTiles.Add(targetTile);
                targetTile.Highlight(true);
            }
        }


    }


    //�}�X�̃n�C���C�g�����ɖ߂�
    void ClearHighlights()
    {
        foreach (Tile t in galleryTiles)
        {
            t.Highlight(false);
        }
        foreach (PlayerController p in players)
        {
            foreach (Tile t in p.aquariumTiles)
            {
                t.Highlight(false);
                t.GetComponent<Collider2D>().enabled = true;
            }
        }
    }

    //���E���h�I��
    void EndRound()
    {
        UIManager.Instance.ShowMessage("���E���h�I���I");

        for (int i = 0; i < finishOrder.Count; i++)
        {
            Debug.Log((i + 1) + "��" + finishOrder[i].pData.playerName);
        }

        playersTurn = finishOrder.ToArray();

        //�w��񐔃��E���h���J��Ԃ�����Q�[���I��
        if (roundCount >= maxRound)
        {
            UIManager.Instance.ShowMessage("�Q�[���I���I");
            playerRanking.ResultScore();
            SceneManager.LoadScene("Result");
            return;
        }
        else
        {
            //���̃��E���h��
            StartRound();
            return;
        }
    }


    //�J������moveCamTime�b��ɐ؂�ւ�
    IEnumerator ChangeCamera(GameObject from, GameObject to)
    {
        yield return new WaitForSeconds(moveCamTime);
        from.SetActive(false);
        to.SetActive(true);
        if (currentPart == Part.gallery)
        {
            currentPart = Part.aquarium;
        }
        else
        {
            currentPart = Part.gallery;
            foreach (PlayerController p in players)
            {
                p.oxygenText.SetActive(false);
            }
        }
    }
}
