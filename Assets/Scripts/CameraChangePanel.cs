using UnityEngine;

public class CameraChangePanel : MonoBehaviour
{
    [SerializeField] GameObject cameraButtonPrefab;
    [SerializeField] Color[] buttonColors;

    private void Start()
    {
        for (int i = 0; i < GameManager.players; i++)
        {
            GameObject obj = Instantiate(cameraButtonPrefab, transform);
            CameraButton cameraButton = obj.GetComponent<CameraButton>();

            cameraButton.index = i + 1;
            cameraButton.color = buttonColors[i];
        }
    }
}
