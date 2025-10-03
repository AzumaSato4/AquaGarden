using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject turnEnd;  //ターンエンドボタン
    [SerializeField] GameObject cancel; //キャンセルボタン
    [SerializeField] GameObject seaBoard; //海ボード

    bool isShowSeaBoad = false; //海ボードが表示されているかどうか

    AquaPieceManager aquaPieceManager;

    private void Start()
    {
        aquaPieceManager = GetComponent<AquaPieceManager>();

        turnEnd.GetComponent<Button>().interactable = false;
        cancel.GetComponent<Button>().interactable = false;
        seaBoard.SetActive(false);
    }

    public void AbledTurnEnd(bool isAble)
    {
        turnEnd.GetComponent<Button>().interactable = isAble;
    }

    public void AbledCancel(bool isAble)
    {
        cancel.GetComponent<Button>().interactable= isAble;
    }

    public void OnSeaBoradButton()
    {
        if (aquaPieceManager.selectedPiece != null)
        {
            aquaPieceManager.MoveToSeaBoard();
            return;
        }
        else
        {
            seaBoard.SetActive(!isShowSeaBoad);
            isShowSeaBoad = !isShowSeaBoad;
        }
    }
}
