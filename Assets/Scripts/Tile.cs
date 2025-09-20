using UnityEditor.ShaderGraph;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int index;       //�}�X�ԍ�
    public bool isAd;       //�L���}�X���ǂ���
    public bool isStart;    //�X�^�[�g�}�X���ǂ���
    public bool uesdTile;   //�ق��̃v���C���[����x�ł��~�܂������ǂ���

    public GameManager manager;

    public bool isGallery;  //ture = �M�������[�Afalse = ������

    public AquaSlot leftSlot;   //���ׂ̐���
    public AquaSlot rightSlot;  //�E�ׂ̐���

    SpriteRenderer spriteRenderer;
    Color defaultcolor;
    public Color highlightColor;

    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultcolor = spriteRenderer.color;
    }


    //�}�X���N���b�N���ꂽ
    private void OnMouseDown()
    {
        // UI�̏�ɃJ�[�\������������A���͂��󂯕t���Ȃ�
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


    //�}�X���n�C���C�g
    public void Highlight(bool on)
    {
        spriteRenderer.color = on ? highlightColor : defaultcolor;
    }

}
