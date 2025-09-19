using TMPro;
using UnityEngine;

public class SelectSeaFish : MonoBehaviour
{
    public GameObject fishNameText;
    public GameObject fishCountText;

    public SeaFish seaFish;

    public int seaIndex;


    private void Start()
    {
        GetComponent<UnityEngine.UI.Image>().color = seaFish.seaFishes[seaIndex].color;
        fishNameText.GetComponent<TextMeshProUGUI>().text = seaFish.seaFishes[seaIndex].fishName;
        fishCountText.GetComponent<TextMeshProUGUI>().text = seaFish.seaFishCounts[seaIndex].ToString();
    }
}
