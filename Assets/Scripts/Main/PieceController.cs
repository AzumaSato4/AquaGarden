using UnityEngine;

public class PieceController : MonoBehaviour
{
    public float moveTime = 0;  //移動時間

    public Vector2 startPos;        //初期位置
    public Vector2 endPos;                 //目的位置
    float movep = 0;                //移動補完値
    bool isMove = false;            //駒が動くフラグ

    void Start()
    {

    }

    public void Update()
    {
        if (isMove)
        {
            float distance = Vector2.Distance(startPos, endPos); //移動距離
            float ds = distance / moveTime;                      //1秒の移動距離
            float df = ds * Time.deltaTime;                      //1フレームの移動距離
            movep += df / distance;                              //移動補完値

            startPos = Vector2.Lerp(startPos, endPos, movep);
        }
        if (movep >= 1.0f)
        {
            movep = 0;
            isMove = false;
        }

    }

    //駒（自分自身）を移動させる
    public void MovePiece(Transform moveAt)
    {
        //startPos = transform.position;
        endPos = moveAt.position;
        isMove = true;
    }
}
