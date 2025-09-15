
using TMPro;
using UnityEngine;

public class NameColorUIManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public TitleUIManager titleUIManager;

    public TMP_InputField player3Name;
    public TMP_Dropdown player3Color;

    void Start()
    {
        //初期値は非表示
    }

    public void SetNameColor()
    {
        if (titleUIManager.mainPanel == "Panel2")
        {
            player3Name.interactable = true;
            HideField(PlayerManager.players);

            //2人プレイの場合は「Goss」が3pに強制的になる
            if (PlayerManager.players == 2)
            {
                player3.SetActive(true);
                player3Name.interactable = false;
                player3Color.interactable = true;

                PlayerManager.pName[2] = "Goss";
                player3Name.text = "Goss";
            }
        }
    }

    //プレイ人数に合わせて表示を変える
    public void HideField(int n)
    {
        switch (n)
        {
            case 1:
                player1.SetActive(true);
                player2.SetActive(false);
                player3.SetActive(false);
                player4.SetActive(false);
                break;
            case 2:
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(false);
                player4.SetActive(false);
                break;
            case 3:
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(true);
                player4.SetActive(false);
                break;
            case 4:
                player1.SetActive(true);
                player2.SetActive(true);
                player3.SetActive(true);
                player4.SetActive(true);
                break;
        }
    }
}
