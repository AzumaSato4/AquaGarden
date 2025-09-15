using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //プレイする人数
    public static int players;

    public TMP_InputField[] pNameField;
    public TMP_Dropdown[] pColorField;

    public bool isName;
    public bool isColor;

    //プレイヤーの情報を格納
    public static string[] pName;
    public static int[] pColor;

    void Start()
    {
        pName = new string[4];
        pColor = new int[4];
    }

    //プレイ可能かチェック
    public void Check()
    {
        for (int i = 0; i < players; i++)
        {
            pName[i] = pNameField[i].GetComponent<TMP_InputField>().text;
        }

        //名前入力漏れないかチェック
        isName = true;
        for (int i = 0; i < players; i++)
        {
            if (pName[i] == "")
            {
                isName = false;
                return;
            }
        }


        for (int i = 0; i < players; i++)
        {
            pColor[i] = pColorField[i].GetComponent<TMP_Dropdown>().value;
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
