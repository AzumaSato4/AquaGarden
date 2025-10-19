using UnityEngine;
using UnityEngine.EventSystems;

public class AquaPiece : MonoBehaviour
{
    public PieceData pieceData; //駒データ
    public GameObject currentPos; //今いる場所
    public bool isiFromSea; //海から購入したかどうか
    public bool iscurrentTurn; //このターンに手に入れたかどうか（マイルストーン用）
    public AquaPieceController aquaPieceController; //駒の動きを制御
    public int spotIndex; //水槽内のスポット番号を保存するための変数
    public int storageIndex; //ストレージのスポット番号を保存するための変数

    private void Start()
    {
        //駒が生成されたら必要なコンポーネントを代入
        aquaPieceController = GetComponent<AquaPieceController>();
        GetComponent<SpriteRenderer>().sprite = pieceData.pieceSprite;
        GetComponent<Animator>().runtimeAnimatorController = pieceData.animationController;
    }

    private void Update()
    {
        if (PhaseManager.currentPhase == PhaseManager.Phase.end)
        {
            iscurrentTurn = false;
        }
    }

    //押したら選択中にする
    private void OnMouseDown()
    {
        //UIが表示中は反応しない
        if (EventSystem.current.IsPointerOverGameObject() || UIController.isActiveUI)
        {
            return;
        }

        //もし水槽内にいてその水槽が選択不可なら反応しない
        if (currentPos && currentPos.GetComponent<AquaSlot>())
        {
            if(!currentPos.GetComponent<AquaSlot>().selectable)
            {
                return;
            }
        }

        //他の駒が選択されていない、自分の番、編集フェーズならこの駒を選択中にする
        if (AquaPieceManager.selectedPiece == null && aquaPieceController.playerManager.isActive && (PhaseManager.currentPhase == PhaseManager.Phase.edit || PhaseManager.currentPhase == PhaseManager.Phase.adEdit || PhaseManager.currentPhase == PhaseManager.Phase.mileEdit))
        {
            transform.localScale = new Vector2(2.5f, 2.5f);
            aquaPieceController.aquaPieceManager.SelectedPiece(this.gameObject);
        }
    }
}
