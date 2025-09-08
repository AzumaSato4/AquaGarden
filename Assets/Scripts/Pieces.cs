using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;



public class Pieces : MonoBehaviour
{
    public PieceData[] pieces;
    //�z��
    public List<string> pouch = new List<string>();
    //�C�{�[�h
    public Dictionary<string, int> sea = new Dictionary<string, int>();

    int totalPieces = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�v���C�l���ɂ���Č���ς���
        SetPlayer(SelectPlayer.selectPlayer);

        GameController.gameMode = "Playing";
        Debug.Log(GameController.gameMode);

        //�z��
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i].category == "Fish")
            {
                for (int j = 0; j < pieces[i].items; j++)
                {
                    pouch.Add(pieces[i].pieceName);
                }
            }
        }

        //�C�{�[�h
        //������
        for (int i = 0; i < pieces.Length; i++)
        {
            sea.Add(pieces[i].pieceName, 0);
        }
        //�C���ƃT���S������
        sea["Seaweed"] = pieces[6].items;
        sea["Coral"] = pieces[7].items;



        //�܂��狛��5�C��
        PouchToSea(5);

        //���g�m�F�p
        foreach (KeyValuePair<string, int> kvp in sea)
        {
            Debug.Log(kvp.Key + "=" + kvp.Value);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayer(int players)
    {
        if (players == 1 || players == 4)
        {
            //4�lor1�l�p
            pieces[0].items = 23; //���^��
            pieces[1].items = 16; //��^��
            pieces[2].items = 9; //�E�~�K��
            pieces[3].items = 5; //�^�c�m�I�g�V�S
            pieces[4].items = 9; //�T��
            pieces[5].items = 3; //�W���x�G�U��
            pieces[6].items = 17; //�C��
            pieces[7].items = 16; //�T���S
        }
        else
        {
            //3�lor2�l�p
            pieces[0].items = 18; //���^��
            pieces[1].items = 12; //��^��
            pieces[2].items = 7; //�E�~�K��
            pieces[3].items = 4; //�^�c�m�I�g�V�S
            pieces[4].items = 7; //�T��
            pieces[5].items = 2; //�W���x�G�U��
            pieces[6].items = 17; //�C��
            pieces[7].items = 16; //�T���S
        }

    }


    //pouch����sea�֋�����ړ�������
    void PouchToSea(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //�����_���ȋ���ړ�������
            int rnd = Random.Range(0, pouch.Count);
            string select = pouch[rnd];

            sea[select]++;
            pouch.RemoveAt(rnd);
        }
    }
}
