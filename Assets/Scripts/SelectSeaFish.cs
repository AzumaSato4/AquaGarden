using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSeaFish : MonoBehaviour
{
    public GameManager manager;

    public GameObject fishNameText;
    public GameObject fishCountText;
    public Button selectBtn;

    public SeaBoard sea;
    public int seaIndex;

    public GameObject fishPrefab;     //魚駒のPrefab



    private void Start()
    {
        GetComponent<Image>().color = sea.seaFishes[seaIndex].color;
        fishNameText.GetComponent<TextMeshProUGUI>().text = sea.seaFishes[seaIndex].type.ToString();
        fishCountText.GetComponent<TextMeshProUGUI>().text = sea.seaCounts[seaIndex].ToString();
    }


    private void Update()
    {
        if (manager.canSelct)
        {
            selectBtn.interactable = true;
        }
        else
        {
            selectBtn.interactable = false;
        }
    }

    //海からストレージへ
    public void OnSelectFish()
    {
        FishData fish = sea.TakeFish(seaIndex);
        if (fish != null)
        {
            // ストレージに魚駒を生成
            GameObject newFish = Instantiate(fishPrefab, manager.playersTurn[manager.currentPlayerIndex].storagePanel.transform);
            newFish.transform.localScale = new Vector3(3, 2, 1);
            FishPiece fp = newFish.GetComponent<FishPiece>();
            fp.fishData = fish;

            manager.playersTurn[manager.currentPlayerIndex].storagePanel.AddStorage(fp);

            // カウントをUIに反映
            fishCountText.GetComponent<TextMeshProUGUI>().text = sea.seaCounts[seaIndex].ToString();
        }
    }

    //海ボードに魚駒を追加
    public void InFish()
    {
        // カウントをUIに反映
        fishCountText.GetComponent<TextMeshProUGUI>().text = sea.seaCounts[seaIndex].ToString();
    }
}
