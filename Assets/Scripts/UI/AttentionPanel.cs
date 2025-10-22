using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttentionPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] Image image;
    public bool isShow = false;
    public bool isOK = false;

    public void ShowMassage(string message, Sprite sprite)
    {
        isShow = true;
        messageText.text = message;
        image.sprite = sprite;
        image.preserveAspect = true;
    }

    public void OnOKButton()
    {
        isOK = true;
        isShow = false;
    }

    public void OnCancelButton()
    {
        isOK = false;
        isShow = false;
    }
}
