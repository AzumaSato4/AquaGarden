using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerData playerData; //�v���C���[���
    GalleryTileManager gTileManager; //�M�������[�^�C�������擾���邽�߂̕ϐ�
    AquariumBoard aquariumBoard; //�����ك{�[�h�����擾���邽�߂̕ϐ�
    public int money; //��������

    GameObject aquarium;        //�����ك{�[�h
    GameObject galleryPlayer;   //�M�������[�̃v���C���[��
    GameObject aquariumPlayer;  //�����ق̃v���C���[��

    private void Start()
    {
        //�v���C���[�̋�Ɛ����ك{�[�h�𐶐�
        aquarium = Instantiate(playerData.aquarium, new Vector3(playerData.playerNum * 20, 0, 0), Quaternion.identity);
        galleryPlayer = Instantiate(playerData.galleryPlayer);
        aquariumPlayer = Instantiate(playerData.aquariumPlayer);
        
        //�v���C���[��̉摜���Z�b�g
        galleryPlayer.GetComponent<SpriteRenderer>().sprite = playerData.gallerySprite;
        aquariumPlayer.GetComponent<SpriteRenderer>().sprite = playerData.aquariumSprite;

        //�M�������[�Ɛ����ق̏����擾
        gTileManager = GameObject.Find("Gallery").GetComponent<GalleryTileManager>();
        aquariumBoard = aquarium.GetComponent<AquariumBoard>();

        //�����ʒu�ɃZ�b�g
        galleryPlayer.transform.position = gTileManager.StartSpots[playerData.playerNum - 1].transform.position; //�M�������[�X�^�[�g�ʒu�ɃZ�b�g
        aquariumPlayer.transform.position = aquariumBoard.aquaTiles[0].transform.position; //�����كX�^�[�g�ʒu�ɃZ�b�g
        aquariumBoard.coin.transform.position = aquariumBoard.CoinSpots[2].transform.position; //�R�C���̏����ʒu��2
    }

    public void ActGallery()
    {

    }

}
