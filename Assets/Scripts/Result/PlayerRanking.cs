using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class PlayerRanking : MonoBehaviour
{
    [SerializeField] GameManager manager;

    public Dictionary<PlayerController, int> playersScore;
    Dictionary<FishData.Type, int> slotFish;

    public int goalScore = 3;  //�S�[�����ɂ�链�_��

    public static List<(PlayerData pData,int score)> results = new List<(PlayerData pData,int score)> ();

    //�v���C���[�̓��_�v�Z
    public void ResultScore()
    {
        //�������ƃS�[�����ɂ�链�_�v�Z
        playersScore = new Dictionary<PlayerController, int>();

        foreach (PlayerController p in manager.players)
        {
            //�ŏI���E���h�̃S�[������]��
            playersScore[p] = goalScore;

            goalScore--;
        }

        //��l�����_���v�Z
        foreach (PlayerController p in manager.players)
        {
            //���ׂĂ̐�����]��
            foreach (AquaSlot slot in p.aquaSlots)
            {
                //�������̓��_�v�Z�p�ɏ�����
                slotFish = new Dictionary<FishData.Type, int>();
                foreach (FishData.Type type in System.Enum.GetValues(typeof(FishData.Type)))
                {
                    slotFish[type] = 0;
                }


                //1�̐�������]��
                foreach (FishPiece fish in slot.fishes)
                {
                    slotFish[fish.fishData.type]++;
                }

                //���^��1�Ƒ�^��1�̃Z�b�g1�ɂ�3�_
                if (slotFish[FishData.Type.SmallFish] > 0 && slotFish[FishData.Type.LargeFish] > 0)
                {
                    while (slotFish[FishData.Type.SmallFish] > 0 && slotFish[FishData.Type.LargeFish] > 0)
                    {
                        playersScore[p] += 2;
                        slotFish[FishData.Type.SmallFish]--;
                        slotFish[FishData.Type.LargeFish]--;
                    }
                }

                //�E�~�K��1�ɂ�2�_
                if (slotFish[FishData.Type.Seaturtle] > 0)
                {
                    while (slotFish[FishData.Type.Seaturtle] > 0)
                    {
                        playersScore[p] += 2;
                        slotFish[FishData.Type.Seaturtle]--;
                    }
                }

                //�T��1�ɂ�2�_
                if (slotFish[FishData.Type.Shark] > 0)
                {
                    while (slotFish[FishData.Type.Shark] > 0)
                    {
                        playersScore[p] += 2;
                        slotFish[FishData.Type.Shark]--;
                    }
                }

                //�^�c�m�I�g�V�S1�ɂ�2�_
                if (slotFish[FishData.Type.Seahorse] > 0)
                {
                    while (slotFish[FishData.Type.Seahorse] > 0)
                    {
                        playersScore[p] += 2;
                        slotFish[FishData.Type.Seahorse]--;
                    }
                }

                //�W���x�G�U��1�ɂ�4�_
                if (slotFish[FishData.Type.WhaleShark] > 0)
                {
                    while (slotFish[FishData.Type.WhaleShark] > 0)
                    {
                        playersScore[p] += 4;
                        slotFish[FishData.Type.WhaleShark]--;
                    }
                }

                //�T���S1�ɂ�1�_
                if (slotFish[FishData.Type.Coral] > 0)
                {
                    while (slotFish[FishData.Type.Coral] > 0)
                    {
                        playersScore[p] += 1;
                        slotFish[FishData.Type.Coral]--;
                    }
                }
            }

            //�]����������]��
            int playerMoney = p.pData.money;
            //����3�ɂ�1�_
            playersScore[p] += playerMoney / 3;
        }

        //���ʃ��X�g���쐬
        results.Clear();
        foreach (var n in playersScore.OrderByDescending(x => x.Value))
        {
            results.Add((n.Key.pData, n.Value));
        }
    }
}
