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

    public Phase currentPhase;

    UIController uiController; //UIManager���i�[���邽�߂̕ϐ�


    private void Start()
    {
        uiController = GetComponent<UIController>();
    }

    public void StartGallery(PlayerData player)
    {
        GetComponent<CameraManager>().ChangeCamera(0);
        //�^�[���G���h�{�^���������Ȃ��悤�ɂ���
        uiController.AbledTurnEnd(false);
        currentPhase = Phase.gallery;
        Debug.Log($"�M�������[�i{player.playerName}�̃^�[���j�J�n");
    }

    public void EndGallery(PlayerData player)
    {
        Debug.Log($"�M�������[�i{player.playerName}�̃^�[���j�I��");
        StartAquarium(player);
    }

    void StartAquarium(PlayerData player)
    {
        GetComponent<CameraManager>().ChangeCamera(player.playerNum);
        currentPhase = Phase.aquarium;
        Debug.Log($"�����فi{player.playerName}�̃^�[���j�J�n");
    }

    public void MovedAquarium(PlayerData player)
    {
        //�^�[���G���h�{�^����������悤�ɂ���
        uiController.AbledTurnEnd(true);
        StartEdit(player);
    }

    void StartEdit(PlayerData player)
    {
        currentPhase = Phase.edit;
        Debug.Log($"���C�A�E�g�ύX�i{player.playerName}�̃^�[���j�J�n");
    }

    public void EndTurn(PlayerData player)
    {
        currentPhase = Phase.end;
        Debug.Log($"{player.playerName}�̃^�[���I��");
    }
}
