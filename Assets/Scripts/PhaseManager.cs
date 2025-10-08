using TMPro;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public enum Phase
    {
        gallery,
        aquarium,
        edit,
        ad,
        feeding,
        end
    }

    public static Phase currentPhase;

    [SerializeField] TextMeshProUGUI phaseText;
    bool ischange = false;


    private void LateUpdate()
    {
        if (!ischange) return;

        string headerText = null;
        string name = TurnManager.currentPlayer.GetComponent<PlayerManager>().playerData.playerName;
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
            case Phase.ad:
                headerText = ($"広告　（{name}のターン）");
                ischange = false;
                break;
        }

        phaseText.text = headerText;
    }

    public void StartGallery(PlayerData player)
    {
        GetComponent<CameraManager>().ChangeCamera(0);
        currentPhase = Phase.gallery;
        Debug.Log($"ギャラリー（{player.playerName}のターン）開始");
        ischange = true;
    }

    public void StartAd(PlayerData player)
    {
        currentPhase = Phase.ad;
        Debug.Log($"広告イベント（{player.playerName}のターン）開始");
        ischange = true;
    }

    public void EndAd(PlayerData player)
    {
        Debug.Log($"広告イベント（{player.playerName}のターン）終了");
    }

    public void EndGallery(PlayerData player)
    {
        Debug.Log($"ギャラリー（{player.playerName}のターン）終了");
        StartAquarium(player);
    }

    void StartAquarium(PlayerData player)
    {
        GetComponent<CameraManager>().ChangeCamera(player.playerNum);
        currentPhase = Phase.aquarium;
        Debug.Log($"水族館（{player.playerName}のターン）開始");
        ischange = true;
    }

    public void StartFeeding(PlayerData player)
    {
        currentPhase = Phase.feeding;
        Debug.Log($"餌やりイベント（{player.playerName}のターン）開始");
    }

    public void EndFeeding(PlayerData player)
    {
        Debug.Log($"餌やりイベント（{player.playerName}のターン）終了");
        currentPhase = Phase.aquarium;
        MovedAquarium(player);
    }

    public void MovedAquarium(PlayerData player)
    {
        StartEdit(player);
    }

    void StartEdit(PlayerData player)
    {
        currentPhase = Phase.edit;
        Debug.Log($"レイアウト変更（{player.playerName}のターン）開始");
        ischange= true;
    }

    public void EndTurn(PlayerData player)
    {
        currentPhase = Phase.end;
        Debug.Log($"{player.playerName}のターン終了");
    }
}
