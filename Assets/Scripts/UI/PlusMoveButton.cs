using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlusMoveButton : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] Button plusButton;
    [SerializeField] Button minusButton;
    [SerializeField] TextMeshProUGUI stepsText;

    SEManager seManager;

    public int payMoney;

    private void Start()
    {
        seManager = SEManager.instance;
    }

    private void OnEnable()
    {
        payMoney = 0;
    }

    private void Update()
    {
        if (playerManager.money <= 0 || 5 <= playerManager.steps)
        {
            plusButton.interactable = false;
        }
        else
        {
            plusButton.interactable = true;
        }

        if (payMoney <= 0)
        {
            minusButton.interactable = false;
        }
        else
        {
            minusButton.interactable = true;
        }

        stepsText.text = $"移動\n{playerManager.steps}マス";
    }

    //ボタンが押されたら移動距離プラス
    public void PlusMove()
    {
        if (playerManager.money <= 0 || 5 <= playerManager.steps) return;

        seManager.PlaySE(SEManager.SE_Type.pay);
        playerManager.steps++;
        playerManager.money--;
        payMoney++;
    }

    //ボタンが押されたら移動距離マイナス
    public void MinusMove()
    {
        Debug.Log(payMoney);
        if (payMoney <= 0) return;

        playerManager.steps--;
        playerManager.money++;
        payMoney--;
    }
}
