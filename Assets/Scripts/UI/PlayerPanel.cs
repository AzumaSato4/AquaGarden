using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    public GameObject editPanel;
    public GameObject movedPanel;
    public Button cancel;
    public Button turnEnd;
    public Button cancelMove;
    public Button moved;

    PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    //ターンエンドを押せるかどうか変更
    public void AbledTurnEnd(bool isAble)
    {
        turnEnd.interactable = isAble;
    }

    //キャンセルボタンを押せるかどうか変更
    public void AbledCancel(bool isAble)
    {
        cancel.interactable = isAble;
    }

    //移動キャンセルボタンを押せるかどうか変更
    public void AbledCancelMove(bool isAble)
    {
        cancelMove.interactable = isAble;
    }

    //移動完了ボタンを押せるかどうか変更
    public void AbledMoved(bool isAble)
    {
        moved.interactable = isAble;
    }

    //キャンセルボタンを押した
    public void OnCancelButton()
    {
        playerManager.aquaPieceManager.CanselSelect();
    }

    //表示したいパネルに切り替える
    public void ChengePanel(GameObject select)
    {
        movedPanel.SetActive(false);
        cancelMove.interactable = false;
        moved.interactable = false;
        editPanel.SetActive(false);
        cancel.interactable = false;
        turnEnd.interactable = false;

        if (select == movedPanel)
        {
            movedPanel.SetActive(true);
        }
        else if (select == editPanel)
        {
            editPanel.SetActive(true);
            turnEnd.interactable = true;
        }
    }
}
