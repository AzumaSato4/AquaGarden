using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //プレイする人数
    public static int players;

    public GameObject p1PlayerName;
    public GameObject p2PlayerName;
    public GameObject p3PlayerName;
    public GameObject p4PlayerName;

    public GameObject p1PlayerColor;
    public GameObject p2PlayerColor;
    public GameObject p3PlayerColor;
    public GameObject p4PlayerColor;

    public bool isName;
    public bool isColor;

    //プレイヤーの情報を格納
    public static string p1Name;
    public static string p2Name;
    public static string p3Name;
    public static string p4Name;

    public static Color p1Color;
    public static Color p2Color;
    public static Color p3Color;
    public static Color p4Color;

    Color[] pColor;

    void Start()
    {
        p1Name = p1PlayerName.GetComponent<TextMeshProUGUI>().text;
        p2Name = p2PlayerName.GetComponent<TextMeshProUGUI>().text;
        p3Name = p3PlayerName.GetComponent<TextMeshProUGUI>().text;
        p4Name = p4PlayerName.GetComponent<TextMeshProUGUI>().text;

        p1Color = p1PlayerColor.GetComponent<Image>().color;
        p2Color = p2PlayerColor.GetComponent<Image>().color;
        p3Color = p3PlayerColor.GetComponent<Image>().color;
        p4Color = p4PlayerColor.GetComponent<Image>().color;

        pColor = new Color[] { p1Color, p2Color, p3Color, p4Color };
    }

    public void Check()
    {
        //名前入力漏れないかチェック
        isName = true;
        switch (players)
        {
            case 1:
                if (p1Name == "")
                {
                    isName = false;
                    Debug.Log("NFalse");
                    return;
                }
                break;
            case 2:
                if (p1Name == "" ||
                    p2Name == "")
                {
                    isName = false;
                    Debug.Log("NFalse");
                    return;
                }
                break;
            case 3:
                if (p1Name == "" ||
                    p2Name == "" ||
                    p3Name == "")
                {
                    isName = false;
                    Debug.Log("NFalse");
                    return;
                }
                break;
            case 4:
                if (p1Name == "" ||
                    p2Name == "" ||
                    p3Name == "" ||
                    p4Name == "")
                {
                    isName = false;
                    Debug.Log("NFalse");
                    return;
                }
                break;
        }

        //色かぶりがないかチェック
        isColor = true;
        if (isName)
        {
            for (int i = 0; i < players; i++)
            {
                for (int j = i + 1; j < players; j++)
                {
                if (pColor[i] == pColor[j])
                {
                    isColor = false;
                    break;
                }
                }
            }
        }
    }
}
