using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraButton : MonoBehaviour
{
    public int index; //ボタン番号
    public Color color = Color.white; //ボタンの色
    [SerializeField] Image image; //ボタンのImage
    [SerializeField] TextMeshProUGUI text; //ボタンのテキスト

    CameraManager cameraManager;

    private void Start()
    {
        image.color = this.color;
        if (index > 0) text.text = (index + "P"); //ギャラリー用は除外（0）

        cameraManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<CameraManager>();
    }

    public void OnCameraButton()
    {
        cameraManager.OnChangeButton(index);
    }
}
