using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject[] cameras;
    public GameObject[] canvases; //それぞれのキャンバスを格納
    [SerializeField] GameObject aquariumCameraPrefab;
    [SerializeField] GameObject galleryCamera;

    public int currentIndex;
    [SerializeField] GameObject maskPanel; //他のカメラからの操作をさせないためのパネル

    private void Start()
    {
        cameras = new GameObject[GameManager.selectPlayers + 1];
        canvases = new GameObject[GameManager.selectPlayers];

        cameras[0] = galleryCamera;
        currentIndex = 0;
    }

    private void Update()
    {
        //メインカメラに戻ったらマスクを非表示
        if (cameras[currentIndex].activeSelf) maskPanel.SetActive(false);
    }

    public void ChangeMainCamera(int index)
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

    //カメラチェンジボタンが押されたらカメラを変更
    public void OnChangeButton(int index)
    {
        //画面操作できないようにマスクを表示
        maskPanel.SetActive(true);

        //今と別のカメラを選んだら
        if (!cameras[index].activeSelf)
        {
            //すべてオフにする
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].SetActive(false);
                if (i < GameManager.selectPlayers) canvases[i].SetActive(false);
            }

            //indexのカメラだけをオンにする
            //キャンバスもオンにする
            cameras[index].SetActive(true);
            canvases[index - 1].SetActive(true);
            return;
        }

        //もしすでにオンなら
        //メインカメラがindex番号のプレイヤーカメラならギャラリーを映す
        if (currentIndex == index)
        {
            ChangeCamera(true);
        }
        //メインカメラがindex番号のプレイヤーカメラでないならメインカメラに戻す
        else
        {
            ChangeCamera(false);
        }
    }

    public void ChangeCamera(bool isCurrentPlayerCam)
    {
        //すべてオフにする
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);
            if (i < GameManager.selectPlayers) canvases[i].SetActive(false);
        }
        //今のメインカメラがどのプレイヤーかで戻るカメラを変える
        if (isCurrentPlayerCam)
        {
            cameras[0].SetActive(true); //メインプレイヤーのカメラにする
        }
        else
        {
            cameras[currentIndex].SetActive(true); //メインプレイヤーのカメラにする
            if (currentIndex != 0)
            {
                canvases[currentIndex - 1].SetActive(true); //キャンバスもオン
            }
        }
    }
}
