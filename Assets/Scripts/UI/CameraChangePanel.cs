using UnityEngine;
using UnityEngine.UI;

public class CameraChangePanel : MonoBehaviour
{
    [SerializeField] GameObject cameraButtonPrefab;
    [SerializeField] Color[] buttonColors;
    [SerializeField] GridLayoutGroup gridLayoutGroup;

    private void Start()
    {
        if (UnityEngine.Device.Application.isMobilePlatform)
        {
            gridLayoutGroup.constraintCount = 2;
        }

        for (int i = 0; i < GameManager.players; i++)
        {
            GameObject obj = Instantiate(cameraButtonPrefab, transform);
            CameraButton cameraButton = obj.GetComponent<CameraButton>();

            cameraButton.index = i + 1;
            cameraButton.color = buttonColors[i];
        }
    }
}
