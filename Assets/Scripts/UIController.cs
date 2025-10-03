using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject turnEnd;  //�^�[���G���h�{�^��
    [SerializeField] GameObject cancel; //�L�����Z���{�^��
    [SerializeField] GameObject seaBoard; //�C�{�[�h

    bool isShowSeaBoad = false; //�C�{�[�h���\������Ă��邩�ǂ���

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
