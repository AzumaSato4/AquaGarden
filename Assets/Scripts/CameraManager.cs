using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject[] cameras;
    [SerializeField] GameObject aquariumCameraPrefab;
    [SerializeField] GameObject galleryCamera;

    public int currentIndex;

    private void Start()
    {
        cameras = new GameObject[GameManager.players + 1];

        cameras[0] = galleryCamera;
        currentIndex = 0;

        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i] = Instantiate(aquariumCameraPrefab, new Vector3(i * 30, 0, -10), Quaternion.identity);
        }
    }

    public void ChangeCamera(int index)
    {
        cameras[currentIndex].SetActive(false);
        currentIndex = index;
        cameras[currentIndex].SetActive(true);
    }
}
