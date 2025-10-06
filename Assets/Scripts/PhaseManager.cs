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

    UIController uiController; //UIManagerを格納するための変数


    private void Start()
    {
        uiController = GetComponent<UIController>();
    }

    public void StartGallery(PlayerData player)
    {
        GetComponent<CameraManager>().ChangeCamera(0);
        //ターンエンドボタンを押せないようにする
        uiController.AbledTurnEnd(false);
        currentPhase = Phase.gallery;
        Debug.Log($"ギャラリー（{player.playerName}のターン）開始");
    }

    public void StartAd(PlayerData player)
    {
        currentPhase = Phase.ad;
        Debug.Log($"広告イベント（{player.playerName}のターン）開始");
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
        //ターンエンドボタンを押せるようにする
        uiController.AbledTurnEnd(true);
        StartEdit(player);
    }

    void StartEdit(PlayerData player)
    {
        currentPhase = Phase.edit;
        Debug.Log($"レイアウト変更（{player.playerName}のターン）開始");
    }

    public void EndTurn(PlayerData player)
    {
        currentPhase = Phase.end;
        Debug.Log($"{player.playerName}のターン終了");
    }
}
