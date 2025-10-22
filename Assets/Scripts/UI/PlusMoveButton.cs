using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlusMoveButton : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] Button plusButton;
    [SerializeField] Button minusButton;
    [SerializeField] TextMeshProUGUI stepsText;

    SoundManager soundManager;

    public int payMoney;

    private void Start()
    {
        soundManager = SoundManager.instance;
    }

    private void OnEnable()
    {
        payMoney = 0;
    }

    private void Update()
    {
        if (playerManager.isMoveing)
        {
            plusButton.interactable = false;
            minusButton.interactable = false;
            return;
        }

        if (playerManager.money <= 0 || 5 <= playerManager.steps)
        {
            plusButton.interactable = false;
        }
        else
        {
            plusButton.interactable = true;
        }

        if (payMoney <= 0 || playerManager.steps <= 3 || playerManager.isMovedAquarium)
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

        soundManager.PlaySE(SoundManager.SE_Type.pay);
        playerManager.steps++;
        playerManager.money--;
        payMoney++;
    }

    //ボタンが押されたら移動距離マイナス
    public void MinusMove()
    {
        if (payMoney <= 0 || playerManager.steps <= 3) return;

        playerManager.steps--;
        playerManager.money++;
        payMoney--;
    }
}
