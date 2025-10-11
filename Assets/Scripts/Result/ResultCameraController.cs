using UnityEngine;

public class ResultCameraController : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject[] cameras;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject backPanel;

    GameObject currentCamera;

    private void Start()
    {
        currentCamera = mainCamera;
        cameras = new GameObject[GameManager.players + 1];
        cameras[0] = mainCamera;
        GameObject[] players = new GameObject[GameManager.players];
        players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < GameManager.players; i++)
        {
            cameras[i + 1] = players[i].GetComponent<PlayerManager>().myCamera;
        }
    }

    public void ChengeCamera(int to)
    {
        currentCamera.SetActive(false);
        currentCamera = cameras[to];
        currentCamera.SetActive(true);
        mainPanel.SetActive(false);
        backPanel.SetActive(true);
    }

    public void BackMainCamera()
    {
        currentCamera.SetActive(false);
        currentCamera = cameras[0];
        currentCamera.SetActive(true);
        mainPanel.SetActive(true);
        backPanel.SetActive(false);
    }
}
