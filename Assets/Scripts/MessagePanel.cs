using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
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
        image.SetNativeSize();
        image.preserveAspect = true;
        image.rectTransform.sizeDelta = new Vector2(300f,300f);
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
