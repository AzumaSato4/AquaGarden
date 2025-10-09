using UnityEngine;
using UnityEngine.EventSystems;

public class AquaPiece : MonoBehaviour
{
    public PieceData pieceData; //駒データ
    public GameObject currentPos; //今いる場所
    public bool isiFromSea; //海から購入したかどうか
    public AquaPieceController aquaPieceController; //駒の動きを制御

    private void Start()
    {
        //駒が生成されたら必要なコンポーネントを代入
        aquaPieceController = GetComponent<AquaPieceController>();
        GetComponent<SpriteRenderer>().sprite = pieceData.pieceSprite;
        GetComponent<Animator>().runtimeAnimatorController = pieceData.animationController;
    }

    //マウスを重ねたらアニメーション開始
    private void OnMouseEnter()
    {
        GetComponent<Animator>().enabled = true;
    }

    //マウスが離れたらアニメーション停止
    private void OnMouseExit()
    {
        //この駒が選ばれていたらアニメーションを止めない
        if (aquaPieceController.aquaPieceManager.selectedPiece != this.gameObject)
        {
            GetComponent<Animator>().enabled = false;
        }
    }

    //押したら選択中にする
    private void OnMouseDown()
    {
        //UIが表示中は反応しない
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //他の駒が選択されていない、自分の番、編集フェーズならこの駒を選択中にする
        if (aquaPieceController.aquaPieceManager.selectedPiece == null && aquaPieceController.playerManager.isActive && (PhaseManager.currentPhase == PhaseManager.Phase.edit || PhaseManager.currentPhase == PhaseManager.Phase.adEdit))
        {
            transform.localScale = new Vector2(2.5f, 2.5f);
            aquaPieceController.aquaPieceManager.SelectedPiece(this.gameObject);
        }
    }
}
