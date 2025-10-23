using TMPro;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public enum Phase
    {
        gallery,
        aquarium,
        edit,
        adEdit,
        mileEdit,
        ad,
        feeding,
        endEdit,
        end
    }

    public static Phase currentPhase;

    public CameraManager cameraManager;
    [SerializeField] UIController uiController;
    [SerializeField] TextMeshProUGUI headerText;
    bool ischange = false;


    private void LateUpdate()
    {
        if (!ischange) return;

        string name;
        if (TurnManager.currentPlayer.GetComponent<GossManager>())
        {
            name = TurnManager.currentPlayer.GetComponent<GossManager>().player.playerName;
        }
        else
        {
            name = TurnManager.currentPlayer.GetComponent<PlayerManager>().player.playerName;
        }

        if (currentPhase == Phase.gallery)
        {
            headerText.text = $"ラウンド{TurnManager.roundCnt}\n{name}のターン";
            UIController.messageText.text = $"{name}のターン";
            ischange = false;
        }
        UIController.isMessageChanged = true;
    }

    public void StartGallery(Player player)
    {
        cameraManager.ChangeMainCamera(0);
        currentPhase = Phase.gallery;
        ischange = true;
    }

    public void StartAd(Player player)
    {
        currentPhase = Phase.ad;
    }

    public void EndGallery(Player player)
    {
        StartAquarium(player);
    }

    void StartAquarium(Player player)
    {
        uiController.ActiveCameraChangeImage();
        cameraManager.ChangeMainCamera(player.playerNum);
        currentPhase = Phase.aquarium;
    }

    public void StartFeeding()
    {
        currentPhase = Phase.feeding;
    }

    public void StartEdit()
    {
        currentPhase = Phase.edit;
    }

    public void StartAdEdit(Player player)
    {
        uiController.ActiveCameraChangeImage();
        cameraManager.ChangeMainCamera(player.playerNum);
        currentPhase = Phase.adEdit;
    }

    public void StartMileEdit(Player player)
    {
        cameraManager.ChangeMainCamera(player.playerNum);
        currentPhase = Phase.mileEdit;
    }

    public void EndEdit()
    {
        currentPhase = Phase.endEdit;
    }

    public void EndTurn()
    {
        currentPhase = Phase.end;
    }
}
