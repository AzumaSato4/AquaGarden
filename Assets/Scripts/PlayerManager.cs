using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    //�v���C����l��
    public static int players;

    public TMP_InputField[] pNameField;
    public TMP_Dropdown[] pColorField;

    public bool isName;
    public bool isColor;

    //�v���C���[�̏����i�[
    public static string[] pName;
    public static int[] pColor;

    void Start()
    {
        pName = new string[4];
        pColor = new int[4];
    }

    //�v���C�\���`�F�b�N
    public void Check()
    {
        for (int i = 0; i < players; i++)
        {
            pName[i] = pNameField[i].GetComponent<TMP_InputField>().text;
        }

        //���O���͘R��Ȃ����`�F�b�N
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

        //�F���Ԃ肪�Ȃ����`�F�b�N
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
