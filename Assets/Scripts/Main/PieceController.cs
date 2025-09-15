using UnityEngine;

public class PieceController : MonoBehaviour
{
    public float moveTime = 0;  //�ړ�����

    public Vector2 startPos;        //�����ʒu
    public Vector2 endPos;                 //�ړI�ʒu
    float movep = 0;                //�ړ��⊮�l
    bool isMove = false;            //������t���O

    void Start()
    {

    }

    public void Update()
    {
        if (isMove)
        {
            float distance = Vector2.Distance(startPos, endPos); //�ړ�����
            float ds = distance / moveTime;                      //1�b�̈ړ�����
            float df = ds * Time.deltaTime;                      //1�t���[���̈ړ�����
            movep += df / distance;                              //�ړ��⊮�l

            startPos = Vector2.Lerp(startPos, endPos, movep);
        }
        if (movep >= 1.0f)
        {
            movep = 0;
            isMove = false;
        }

    }

    //��i�������g�j���ړ�������
    public void MovePiece(Transform moveAt)
    {
        //startPos = transform.position;
        endPos = moveAt.position;
        isMove = true;
    }
}
