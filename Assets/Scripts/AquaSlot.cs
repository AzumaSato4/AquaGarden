using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static FishData;

public class AquaSlot : MonoBehaviour
{
    SpriteRenderer sr;
    Color defalutColor;
    public Color highliteColor = Color.yellow;
    public int maxOxygen = 6;  //�������̍ő�_�f��
    public int currentOxygen;  //���݂̐������_�f��


    public TextMeshProUGUI oxygenText; // �����_�f��
    public Color normalColor = Color.black;
    public Color overLimitColor = Color.red;

    //�������̋�����Ǘ����郊�X�g
    public List<FishPiece> fishes = new List<FishPiece>();
    //���u�����X�g
    public List<FishPiece> tempFishes = new List<FishPiece>();

    //�������ɒu����ő�͈�
    public Vector2 scattrtRnage = new Vector2(0.3f, 0.3f);
    //�����m�̍ŏ�����
    public float minDistance = 0.2f;

    //��̈ړ��ɂ����鎞��
    public float moveTime;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        defalutColor = sr.color;
        UpdateOxygenUI();
    }


    //�I���\�Ȃ�n�C���C�g����
    public void SetHighlight(bool on)
    {
        sr.color = on ? highliteColor : defalutColor;
    }


    // �_�f�ʂ��v�Z
    public int GetTotalOxygen(FishPiece draggingFish = null)
    {
        int tempOxygen = currentOxygen + tempFishes.Sum(f => f.fishData.oxygen);
        if (draggingFish != null && !tempFishes.Contains(draggingFish))
        {
            tempOxygen += draggingFish.fishData.oxygen;
        }
        return tempOxygen;
    }


    //�_�f�ʃ`�F�b�N
    public bool IsOxygenValid()
    {
        int oxygen = GetTotalOxygen();
        return oxygen >= 0 && oxygen <= maxOxygen;
    }

    //���̑����`�F�b�N
    public bool CanAcceptFish(FishPiece fish, List<FishPiece> others = null)
    {
        switch (fish.fishData.type)
        {
            //�C���͐�����1����
            case Type.Seaweed:
                return !others.Any(f => f.fishData.type == Type.Seaweed);

            //�T���S�͐�����1����
            case Type.Coral:
                return !others.Any(f => f.fishData.type == Type.Coral);

            //�^�c�m�I�g�V�S�ƃT���ƃW���x�G�U���͂ǂ��ł�OK
            case Type.Shark:
            case Type.Seahorse:
            case Type.WhaleShark:
                return true;

            //�E�~�K���͊C�����K�v�A�T���Ƌ����s��
            case Type.Seaturtle:
                return others.Any(f => f.fishData.type == Type.Seaweed) &&
                       !others.Any(f => f.fishData.type == Type.Shark);

            //���^���Ƒ�^���̓T���Ƌ����s��
            case Type.SmallFish:
            case Type.LargeFish:
                return !others.Any(f => f.fishData.type == Type.Shark);

            //����ȊO�i��{�Ȃ��j
            default:
                return false;
        }
    }


    //�����ɋ����ǉ�����
    public void AddFish(FishPiece fish)
    {
        if (!fishes.Contains(fish)) fishes.Add(fish);
        tempFishes.Remove(fish); // ���u�����X�g����폜
        fish.transform.SetParent(transform);    //�����̎q�I�u�W�F�N�g�ɂ���

        //�d�Ȃ�Ȃ��悤�ɔz�u
        Vector3 localPos = Vector3.zero;
        int attempt = 0;
        bool validPos = false;

        while (!validPos && attempt < 50)
        {
            //�������̃����_���Ȉʒu�ɔz�u
            localPos = new Vector3(
                Random.Range(-scattrtRnage.x, scattrtRnage.x),
                Random.Range(-scattrtRnage.y, scattrtRnage.y),
                0
            );

            //���̋�ƈʒu�����Ԃ��Ă��Ȃ�������
            validPos = true;
            foreach (FishPiece fp in fishes)
            {
                //�������g�͏��O
                if (fp == fish) continue;
                //���̋�ƈʒu���������false�ɂ���
                if (Vector3.Distance(localPos, fp.transform.localPosition) < minDistance)
                {
                    validPos = false;
                    break;
                }
            }
            attempt++;

        }
        fish.transform.localPosition = localPos;
        currentOxygen += fish.fishData.oxygen;

        UpdateOxygenUI();
    }


    //�������狛�����菜��
    public void RemoveFish(FishPiece fish)
    {
        tempFishes.Remove(fish);
        fishes.Remove(fish);

        currentOxygen -= fish.fishData.oxygen;

        UpdateOxygenUI();
    }


    // �_�f�ʕ\���X�V
    public void UpdateOxygenUI(FishPiece draggingFish = null)
    {
        if (oxygenText != null)
        {
            int total = GetTotalOxygen(draggingFish);
            oxygenText.text = total + "/" + maxOxygen;
            oxygenText.color = total > maxOxygen ? overLimitColor : normalColor;
        }
    }
}
