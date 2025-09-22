using System.Collections.Generic;
using UnityEngine;

public class StoragePanel : MonoBehaviour
{
    public List<FishPiece> storageFishes = new List<FishPiece>();

    //�X�g���[�W���ɒu����ő�͈�
    public Vector2 sScattrtRnage = new Vector2(0.5f, 0.5f);
    public int length;
    //�����m�̍ŏ�����
    public float minDistance = 0.2f;

    //�X�g���[�W�ɋ����ǉ�����
    public void AddStorage(FishPiece fish)
    {
        storageFishes.Add(fish);
        fish.transform.SetParent(transform);

        fish.transform.localPosition = Vector3.zero;
        //�d�Ȃ�Ȃ��悤�ɔz�u
        Vector3 localPos = Vector3.zero;
        int attempt = 0;
        bool validPos = false;

        while (!validPos && attempt < 50)
        {
            //�������̃����_���Ȉʒu�ɔz�u
            localPos = new Vector3(
                Random.Range(-sScattrtRnage.x, sScattrtRnage.x),
                Random.Range(-sScattrtRnage.y, sScattrtRnage.y),
                0
            );

            //���̋�ƈʒu�����Ԃ��Ă��Ȃ�������
            validPos = true;
            foreach (FishPiece fp in storageFishes)
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
    }


    //�X�g���[�W���狛�����菜��
    public void RemoveStorageFish(FishPiece fish)
    {
        storageFishes.Remove(fish);
    }


    //�X�g���[�W�ɋ���c���Ă�����x��
    public bool HasFishInStorage()
    {
        return storageFishes.Count > 0;
    }

    //�͈͕\��
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, length);
    }
}
