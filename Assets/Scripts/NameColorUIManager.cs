
using TMPro;
using UnityEngine;

public class NameColorUIManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public GameObject p1Player;
    public GameObject p2Player;
    public GameObject p3Player;
    public GameObject p4Player;

    public TitleUIManager titleUIManager;

    void Start()
    {
        p1Player.SetActive(false);
        p2Player.SetActive(false);
        p3Player.SetActive(false);
        p4Player.SetActive(false);
    }

    void Update()
    {
        if (titleUIManager.mainPanel == "Panel2")
        {
            HideField(PlayerManager.players);
        }


        if (PlayerManager.players == 2) PlayerManager.p3Name = "Goss";
    }

    //プレイ人数に合わせて表示を変える
    public void HideField(int n)
    {
        switch (n)
        {
            case 1:
                p1Player.SetActive(true);
                p2Player.SetActive(false);
                p3Player.SetActive(false);
                p4Player.SetActive(false);
                break;
            case 2:
                p1Player.SetActive(true);
                p2Player.SetActive(true);
                p3Player.SetActive(false);
                p4Player.SetActive(false);
                break;
            case 3:
                p1Player.SetActive(true);
                p2Player.SetActive(true);
                p3Player.SetActive(true);
                p4Player.SetActive(false);
                break;
            case 4:
                p1Player.SetActive(true);
                p2Player.SetActive(true);
                p3Player.SetActive(true);
                p4Player.SetActive(true);
                break;
        }
    }
}
