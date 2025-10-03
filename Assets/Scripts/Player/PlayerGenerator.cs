using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public PlayerData[] playerData; //�v���C���[���
    [SerializeField] GameObject playerPrefab; //�v���C���[�̃v���n�u

    void Awake()
    {
        //�Q���v���C���[�̐������J��Ԃ�
        for (int i = 0; i < GameManager.players; i++)
        {
            //�v���C���[�𐶐�
            GameObject player = Instantiate(playerPrefab);

            //���������v���C���[�Ƀv���C���[�����Z�b�g
            player.GetComponent<PlayerManager>().playerData = playerData[i];
            player.GetComponent<PlayerManager>().aquaPieceManager = GetComponent<AquaPieceManager>();
        }
    }
}
