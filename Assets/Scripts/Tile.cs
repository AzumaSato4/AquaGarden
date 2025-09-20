using UnityEditor.ShaderGraph;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int index;       //マス番号
    public bool isAd;       //広告マスかどうか
    public bool isStart;    //スタートマスかどうか
    public bool uesdTile;   //ほかのプレイヤーが一度でも止まったかどうか

    public GameManager manager;

    public bool isGallery;  //ture = ギャラリー、false = 水族館

    public AquaSlot leftSlot;   //左隣の水槽
    public AquaSlot rightSlot;  //右隣の水槽

    SpriteRenderer spriteRenderer;
    Color defaultcolor;
    public Color highlightColor;

    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultcolor = spriteRenderer.color;
    }


    //マスがクリックされた
    private void OnMouseDown()
    {
        // UIの上にカーソルがあったら、入力を受け付けない
        if (GameManager.UIActive) return;

        if (isGallery)
        {
            manager.OnTileClicked(this);
        }
        else
        {
            manager.OnAquaTileClicked(this);
        }
    }


    //マスをハイライト
    public void Highlight(bool on)
    {
        spriteRenderer.color = on ? highlightColor : defaultcolor;
    }

}
