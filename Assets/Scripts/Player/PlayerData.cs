using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    //�v���C���[�f�[�^���i�[
    public int playerNum;               //�v���C���[�ԍ�
    public string playerName;           //�v���C���[��
    public GameObject galleryPlayer;    //�M�������[�̃v���C���[��
    public Sprite gallerySprite;        //�M�������[�̃v���C���[��摜
    public GameObject aquariumPlayer;   //�����ق̃v���C���[��
    public Sprite aquariumSprite;       //�����ق̃v���C���[��摜
    public GameObject aquarium;         //�����ك{�[�h
}
