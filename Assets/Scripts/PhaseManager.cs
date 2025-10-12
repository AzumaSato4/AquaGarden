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
        end
    }

    public static Phase currentPhase;

    public CameraManager cameraManager;
    [SerializeField] TextMeshProUGUI phaseText;
    bool ischange = false;


    private void LateUpdate()
    {
        if (!ischange) return;

        string headerText = null;
        string name = TurnManager.currentPlayer.GetComponent<PlayerManager>().player.playerName;
        switch (currentPhase)
        {
            case Phase.gallery:
                headerText = ($"ギャラリー　（{name}のターン）");
                ischange = false;
                break;
            case Phase.aquarium:
                headerText = ($"水族館　（{name}のターン）");
                ischange = false;
                break;
            case Phase.edit:
                headerText = ($"水族館編集　（{name}のターン）");
                ischange = false;
                break;
            case Phase.adEdit:
                headerText = ($"水族館特別編集　（{name}のターン）");
                ischange = false;
                break;
            case Phase.mileEdit:
                headerText = ($"水族館特別編集　（{name}のターン）");
                ischange = false;
                break;
            case Phase.ad:
                headerText = ($"広告選択　（{name}のターン）");
                ischange = false;
                break;
            case Phase.feeding:
                headerText = ($"餌やりイベント　（{name}のターン）");
                ischange = false;
                break;
        }

        phaseText.text = headerText;
        UIController.messageText.text = headerText;
        UIController.isMessageChanged = true;
    }

    public void StartGallery(Player player)
    {
        cameraManager.ChangeCamera(0);
        currentPhase = Phase.gallery;
        Debug.Log($"ギャラリー（{player.playerName}のターン）開始");
        ischange = true;
    }

    public void StartAd(Player player)
    {
        currentPhase = Phase.ad;
        Debug.Log($"広告イベント（{player.playerName}のターン）開始");
        ischange = true;
    }

    public void EndAd(Player player)
    {
        Debug.Log($"広告イベント（{player.playerName}のターン）終了");
    }

    public void EndGallery(Player player)
    {
        Debug.Log($"ギャラリー（{player.playerName}のターン）終了");
        StartAquarium(player);
    }

    void StartAquarium(Player player)
    {
        cameraManager.ChangeCamera(player.playerNum);
        currentPhase = Phase.aquarium;
        Debug.Log($"水族館（{player.playerName}のターン）開始");
        ischange = true;
    }

    public void StartFeeding(Player player)
    {
        currentPhase = Phase.feeding;
        Debug.Log($"餌やりイベント（{player.playerName}のターン）開始");
    }

    public void EndFeeding(Player player)
    {
        Debug.Log($"餌やりイベント（{player.playerName}のターン）終了");
        currentPhase = Phase.aquarium;
        MovedAquarium(player);
    }

    public void MovedAquarium(Player player)
    {
        StartEdit(player);
    }

    void StartEdit(Player player)
    {
        currentPhase = Phase.edit;
        Debug.Log($"レイアウト変更（{player.playerName}のターン）開始");
        ischange= true;
    }
    
    public void StartAdEdit(Player player)
    {
        cameraManager.ChangeCamera(player.playerNum);
        currentPhase = Phase.adEdit;
        Debug.Log($"特別レイアウト変更（{player.playerName}のターン）開始");
        ischange= true;
    }

    public void StartMileEdit(Player player)
    {
        cameraManager.ChangeCamera(player.playerNum);
        currentPhase = Phase.mileEdit;
        Debug.Log($"特別レイアウト変更（{player.playerName}のターン）開始");
        ischange= true;
    }


    public void EndTurn(Player player)
    {
        currentPhase = Phase.end;
        Debug.Log($"{player.playerName}のターン終了");
    }
}
