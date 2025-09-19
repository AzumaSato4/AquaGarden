using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquaSlot : MonoBehaviour
{
    SpriteRenderer sr;
    Color defalutColor;
    public Color highliteColor = Color.yellow;

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
    }


    //�I���\�Ȃ�n�C���C�g����
    public void SetHighlight(bool on)
    {
        sr.color = on ? highliteColor : defalutColor;
    }


    // ���u��
    public void AddTempFish(FishPiece fish)
    {
        tempFishes.Add(fish);
        fish.transform.SetParent(transform);
        Vector3 localPos = AddFish(fish);
        fish.transform.localPosition = localPos;
    }


    //�����ɋ����ǉ�����
    Vector3 AddFish(FishPiece fish)
    {
        fishes.Add(fish);
        fish.transform.SetParent(transform);    //�����̎q�I�u�W�F�N�g�ɂ���

        //�d�Ȃ�Ȃ��悤�ɔz�u
        Vector3 localPos = Vector3.zero;
        int attempt = 0;
        bool validPos = false;

        while (!validPos && attempt < 50)
        {
            //�������̃����_���Ȉʒu�ɉ��u��
            localPos = new Vector3(
                Random.Range(-scattrtRnage.x, scattrtRnage.x),
                Random.Range(-scattrtRnage.y, scattrtRnage.y),
                0
            );

            //���̋�ƈʒu�����Ԃ��Ă��Ȃ�������
            validPos = true;
            foreach ( FishPiece fp in fishes )
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
        return localPos;
    }


    //�������狛�����菜��
    public void RemoveFish(FishPiece fish)
    {
        fishes.Remove(fish);
    }

    
    //����{�^���Ŋm�肷��
    public void ConfirmFishPlacement()
    {
        foreach (FishPiece fish in tempFishes)
        {
            fishes.Add(fish);
        }
        tempFishes.Clear();
    }


    //�ړ������炩�ɂ���R���[�`��
    IEnumerator MoveCoroutine(Vector2 toPos, FishPiece piece)
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
}
