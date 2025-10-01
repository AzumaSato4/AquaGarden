using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerData[] playerData = new PlayerData[4]; //�v���C���[���
    [SerializeField] GameObject playerPrefab; //�v���C���[�̃v���n�u

    void Start()
    {
        //�Q���v���C���[�̐������J��Ԃ�
        for (int i = 0; i < 4; i++)
        {
            //�v���C���[�𐶐�
            GameObject player = Instantiate(playerPrefab);

            //���������v���C���[�Ƀv���C���[�����Z�b�g
            player.GetComponent<PlayerController>().playerData = playerData[i];
        }
    }
}
