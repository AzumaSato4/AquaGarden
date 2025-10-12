using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public GameObject[] cameras;
    [SerializeField] GameObject aquariumCameraPrefab;
    [SerializeField] GameObject galleryCamera;
    [SerializeField] Button changeButton;

    public int currentIndex;

    bool isActive = false;

    private void Start()
    {
        cameras = new GameObject[GameManager.players + 1];

        cameras[0] = galleryCamera;
        currentIndex = 0;
    }

    public void ChangeCamera(int index)
    {
        StartCoroutine(ChangeCoroutine(index));
    }

    IEnumerator ChangeCoroutine(int index)
    {
        yield return new WaitForSeconds(1.0f);
        cameras[currentIndex].SetActive(false);
        currentIndex = index;
        cameras[currentIndex].SetActive(true);
    }

    public void OnChangeButton()
    {
        PlayerManager playerManager = TurnManager.currentPlayer.GetComponent<PlayerManager>();
        if (PhaseManager.currentPhase == PhaseManager.Phase.gallery ||
            PhaseManager.currentPhase == PhaseManager.Phase.ad)
        {
            cameras[0].SetActive(isActive);
            cameras[playerManager.player.playerNum].SetActive(!isActive);
            playerManager.aquariumCanvas.SetActive(!isActive);
        }
        else
        {
            cameras[currentIndex].SetActive(isActive);
            playerManager.aquariumCanvas.SetActive(isActive);
            cameras[0].SetActive(!isActive);
        }
        isActive = !isActive;
    }
}
