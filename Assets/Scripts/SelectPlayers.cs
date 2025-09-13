using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectPlayers : MonoBehaviour
{
    public TMP_InputField p3Field;
    //TextMeshProUGUI p3FieldName;

    private void Start()
    {
        //p3FieldName = p3Field.placeholder as TextMeshProUGUI;
    }

    public void SetPlayers(int n)
    {
        PlayerManager.players = n;

        PlayerManager.pName = new string[n];
        PlayerManager.pColor = new int[n];

        //2�l�̎��́uGoss��3p�ɂ���v
        if (n == 2)
        {
            n++;
            PlayerManager.pName = new string[n];
            PlayerManager.pColor = new int[n];

            PlayerManager.pName[2] = "Goss";
            p3Field.text = "Goss";
        }

    }

}
