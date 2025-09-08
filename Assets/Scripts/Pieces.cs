using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Pieces : MonoBehaviour
{
    public PieceData[] pieces;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        ////�z��
        //Dictionary<PieceType, int> pouch = new Dictionary<PieceType, int>()
        //{
        //    {PieceType.minifish, minifishes},
        //    {PieceType.bigfish, bigfishes},
        //    {PieceType.seaturtle, seaturtles},
        //    {PieceType.seahorse, seahorses},
        //    {PieceType.shark, sharks},
        //    {PieceType.whaleshark, whalesharks},
        //    {PieceType.seaweed, seaweeds},
        //    {PieceType.coral, corals}
        //};

        ////�C�{�[�h
        //Dictionary<PieceType, int> sea = new Dictionary<PieceType, int>()
        //{
        //    {PieceType.minifish, 0},
        //    {PieceType.bigfish, 0},
        //    {PieceType.seaturtle, 0},
        //    {PieceType.seahorse, 0},
        //    {PieceType.shark, 0},
        //    {PieceType.whaleshark, 0},
        //    {PieceType.seaweed, 0},
        //    {PieceType.coral, 0}
        //};

        ////������W�߂�
        //PieceType[] fishType = new PieceType[]
        //{
        // PieceType.minifish,
        // PieceType.bigfish,
        // PieceType.seaturtle,
        // PieceType.seahorse,
        // PieceType.shark,
        // PieceType.whaleshark
        //};

        ////�܂��狛��5�C��
        //Setup(fishType, pouch, sea, 5);



        ////���g�̊m�F�i�e�X�g�p�j
        //foreach (var kp in pouch)
        //{
        //    var key = kp.Key;
        //    var value = kp.Value;
        //    Debug.Log($"{key} / {value}");
        //}
        //foreach (var kp in sea)
        //{
        //    var key = kp.Key;
        //    var value = kp.Value;
        //    Debug.Log($"{key} / {value}");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.gameMode == "Start")
        {
            //�v���C�l���ɂ���Č���ς���
            SetPlayer(SelectPlayer.selectPlayer);

            GameController.gameMode = "Playing";
            Debug.Log(GameController.gameMode);

        }

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


        foreach (var count in pieces)
        {
            Debug.Log(count.items);
        }
    }


    ////from����to�֋�����ړ�������
    //void Setup(PieceType[] types, Dictionary<PieceType, int> from, Dictionary<PieceType, int> to, int count)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        //�c���Ă��������ׂďW�߂�
    //        List<PieceType> available = new List<PieceType>();
    //        foreach (var t in types)
    //        {
    //            if (from[t] > 0)
    //            {
    //                available.Add(t);
    //            }
    //        }

    //        //�����_���ȋ���ړ�������
    //        int rnd = Random.Range(0, available.Count);
    //        PieceType select = available[rnd];
    //        from[select]--;
    //        to[select]++;
    //    }
    //}
}
