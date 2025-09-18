using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum Part
    {
        gallery,
        aquarium
    }

    public Part currentPart = Part.gallery;


    public PlayerController[] players;
    public Tile[] galleryTiles;

    public GameObject galleryCam;   //�M�������[�{�[�h���f�����߂̃J����
    public float moveCamTime;

    PlayerController currentPlayer;
    PlayerController nextPlayer;

    public int maxRound;

    int turnCount;
    int roundCount;
    public int currentPlayerIndex = 0;

    //�ړ��\�ȃ}�X���i�邽�߂̃��X�g
    List<Tile> movableTiles = new List<Tile>();
    //�S�[�������v���C���[���m�F���郊�X�g
    List<PlayerController> finishOrder = new List<PlayerController>();

    private void Start()
    {
        roundCount = 0;

        StartRound();

    }


    //���E���h�X�^�[�g
    void StartRound()
    {
        currentPart = Part.gallery;
        galleryCam.SetActive(true);

        turnCount = 0;
        currentPlayerIndex = 0;
        finishOrder.Clear();
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

        HighlightMovableTiles(players[currentPlayerIndex].currentGalleryTile, galleryTiles);
        Debug.Log("���E���h" + roundCount);
        Debug.Log(players[currentPlayerIndex].pData.playerName + "�̃^�[��");
    }


    //�M�������[�p�[�g
    //�}�X���I�����ꂽ�炻�̃}�X�ֈړ�
    public void OnTileClicked(Tile clickedTile)
    {
        //�M�������[�^�[���łȂ���Α����^�[��
        if (currentPart != Part.gallery) return;
        //�N���b�N���ꂽ�}�X���I��s�Ȃ瑦���^�[��
        if (!movableTiles.Contains(clickedTile)) return;

        currentPlayer = players[currentPlayerIndex];
        currentPlayer.MoveToGalleryTile(clickedTile);

        //�S�[��������t���O�𗧂Ă�
        if (clickedTile.index == 0)
        {
            currentPlayer.isGoal = true;
            finishOrder.Add(currentPlayer);

            //�S�����S�[�������烉�E���h�I��
            int goalCount = 0;
            foreach (PlayerController p in players)
            {
                if (p.isGoal)
                {
                    goalCount++;
                }
            }
            if (goalCount == players.Length)
            {
                EndRound();
                return;
            }
            //�^�[���������I��
            NextPlayerTurn();
            return;
        }


        StartCoroutine(ChangeCamera(galleryCam, currentPlayer.aquariumCam));
        //�����ك^�[����
        currentPart = Part.aquarium;
        HighlightMovableTiles(currentPlayer.currentAquaTile, currentPlayer.aquariumTiles);
    }


    //�����كp�[�g
    //�}�X���I�����ꂽ�炻�̃}�X�ֈړ�
    public void OnAquaTileClicked(Tile clickedTile)
    {
        if (currentPart != Part.aquarium) return;
        if (!movableTiles.Contains(clickedTile)) return;

        currentPlayer.MoveToAquaTile(clickedTile);


        //���̃v���C���[�̃^�[����
        NextPlayerTurn();

    }


    //���̃^�[����
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

        nextPlayer = players[currentPlayerIndex];

        if (nextPlayer.isGoal)
        {
            NextPlayerTurn();
            return;
        }

        currentPart = Part.gallery;
        StartCoroutine(ChangeCamera(currentPlayer.aquariumCam, galleryCam));

        HighlightMovableTiles(nextPlayer.currentGalleryTile, galleryTiles);
        Debug.Log(nextPlayer.pData.playerName + "�̃^�[��");
    }

    // ��Ԍ��̃v���C���[�̃C���f�b�N�X�ԍ���Ԃ�
    private int GetLastPlayerIndex()
    {
        int minIndex = 0;
        int minTile = 19;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].currentGalleryTile.index == 0)
            {
                continue;
            }
            else if (players[i].currentGalleryTile.index <= minTile)
            {
                minTile = players[i].currentGalleryTile.index;
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
                    foreach (PlayerController p in players)
                    {
                        if (p != players[currentPlayerIndex] && p.currentGalleryTile == t)
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
            if (players[currentPlayerIndex].currentGalleryTile.index >= 1)
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
            }
        }
    }

    //���E���h�I��
    void EndRound()
    {
        Debug.Log("���E���h�I���I");

        for (int i = 0; i < finishOrder.Count; i++)
        {
            Debug.Log((i + 1) + "��" + finishOrder[i].pData.playerName);
        }

        //�S�[�����ɕ��ѕς�
        players = finishOrder.ToArray();

        //�w��񐔃��E���h���J��Ԃ�����Q�[���I��
        if (roundCount >= maxRound)
        {
            Debug.Log("�Q�[���I��");
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


    IEnumerator ChangeCamera(GameObject from, GameObject to)
    {
        yield return new WaitForSeconds(moveCamTime);
        from.SetActive(false);
        to.SetActive(true);
    }


}
