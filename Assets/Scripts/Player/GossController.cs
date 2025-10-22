using DG.Tweening;
using UnityEngine;

public class GossController : MonoBehaviour
{
    public GossManager gossManager;

    int index;
    GameObject selected;
    float moveTime = 0.3f; //移動アニメーションの時間

    SoundManager soundManager;

    private void Start()
    {
        soundManager = SoundManager.instance;
    }

    public void MoveStart(int index, GameObject selcted)
    {
        this.index = index;
        this.selected = selcted;
        Invoke("MoveGoss", 1.0f);
    }

    public void MoveGoss()
    {
        int nextIndex = gossManager.galleryIndex + 1;
        //1マスずつ進む
        OneStep(nextIndex);
        if (selected.GetComponent<BoxCollider2D>() != null)
            selected.GetComponent<BoxCollider2D>().enabled = false;
        if (selected.GetComponent<CircleCollider2D>() != null)
            selected.GetComponent<CircleCollider2D>().enabled = false;
    }

    void OneStep(int nextIndex)
    {
        if (nextIndex >= gossManager.galleryBoard.Tiles.Length)
        {
            nextIndex -= gossManager.galleryBoard.Tiles.Length;
        }
        Debug.Log(nextIndex);
        GameObject next = gossManager.galleryBoard.Tiles[nextIndex];
        //DoTweenで移動アニメーション
        transform.DOMove(next.transform.position, moveTime).OnComplete(() =>
        {
            soundManager.PlaySE(SoundManager.SE_Type.click);
            if (transform.position != selected.transform.position)
            {
                nextIndex++;
                OneStep(nextIndex);
            }
            else //移動が完了
            {
                gossManager.MoveGallery(index, selected.name);
            }
        });
    }
}
