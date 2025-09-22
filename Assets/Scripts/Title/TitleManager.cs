using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public static string[] pName;

    [Header("�v���C���[�����͗p")]
    [SerializeField] TMP_InputField[] inputField;


    public void StartGame()
    {
        pName = new string[inputField.Length];
        for (int i = 0; i < inputField.Length; i++)
        {
            pName[i] = inputField[i].text;
        }

        SceneManager.LoadScene("Main");
    }
}
