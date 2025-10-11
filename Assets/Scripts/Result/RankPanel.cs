using TMPro;
using UnityEngine;

public class RankPanel : MonoBehaviour
{
    public int index;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;
    public ResultCameraController cameraController;

    public void OnRankPanel()
    {
        cameraController.ChengeCamera(index);
    }
}
