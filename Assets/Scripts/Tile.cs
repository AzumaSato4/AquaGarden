using UnityEngine;

public class Tile : MonoBehaviour
{
    public int index;       //マス番号
    public bool isAd;       //広告マスかどうか
    public bool uesdTile;   //ほかのプレイヤーが一度でも止まったかどうか

    public GameManager manager;

    public bool isGallery;  //ture = ギャラリー、false = 水族館

    SpriteRenderer spriteRenderer;
    Color defaultcolor;
    public Color highlightColor = Color.white;

    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultcolor = spriteRenderer.color;
    }


    //マスがクリックされた
    private void OnMouseDown()
    {
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
